using UnityEngine;
using QFramework;
using System.Collections;
using MoreMountains.Feedbacks;

namespace SkateGame
{
    /// <summary>
    /// 玩家控制器
    /// 集成了输入检测、状态机管理和物理控制
    /// 替代原来的PlayerScript
    /// </summary>
    public class InputController : ViewerControllerBase
    {
        private IPlayerModel playerModel;

        [Header("状态机")]
        public E stateMachine;
        private Rigidbody2D rb;

        [Header("轨道设置")]
        public Track currentTrack;
        public float grindJumpIgnoreTime = 0.2f;
        public float grindJumpTimer = 0f;

        [Header("空中设置")]
        public bool isInAir = false;
        public float airTime = 0f;
        public int airCombo = 0;
        public bool isNearTrack = false;

        [Header("Wall Setting")]
        public Wall currentWall;
        public bool isNearWall = false;

        [Header("Button Check")]
        public bool isEHeld = false;
        public bool isWHeld = false;
        private bool hasJumpedThisFrame = false; // 防止同一帧重复跳跃

        [Header("Ground Detection")]
        public LayerMask groundLayer = 1; // 地面层级
        private bool wasGrounded = false; // 上一帧是否着地
        private float jumpBufferTime = 0.1f; // 跳跃缓冲时间
        private float jumpBufferTimer = 0f; // 跳跃缓冲计时器

        [Header("Life Setting")]
        public bool canBeHurt;

        [Header("Combat Setting")]
        public bool isPowerGrinding;
        private bool isCheckingReverseWindow = false;
        public float reverseInputWindow = 2.0f;
        private float reverseTimer = 0f;

        [Header("瞄准与射击设置")]
        public LineRenderer aimLine;      // 瞄准线
        public float aimLineLength = 10f; // 瞄准线长度
        public LayerMask shootLayer;
        public float maxAimTime = 3f;     // 最大瞄准时间

        public bool isAiming = false;
        public GameObject[] bulletPrefabs;   // 可切换的子弹类型
        private int currentBulletIndex = 0;
        public float bulletSpeed = 15f;
        private float aimTimer = 0f;      // 瞄准计时器

        [Header("颜色设置")]
        public SpriteRenderer playerSprite; // 玩家精灵渲染器
        private Color originalColor = Color.white; // 原始颜色

        [Header("瞄准时间奖励")]
        private bool hasPerformedTrickInAir = false; // 是否在空中执行了trick
        private float _baseMaxAimTime = 3f; // 基础瞄准时间上限


        [Header("MMF效果")]
        public MMF_Player powerGrindEffect;
        public MMF_Player ReverseEffect;
        public MMF_Player GrindEffect;
        public MMF_Player WallRideEffect;
        public MMF_Player TrickAEffect;
        public MMF_Player TrickBEffect;
        public MMF_Player TrickABoostEffect;
        [Header("粒子特效容器")]
        public Transform particleEffectContainer; // 粒子特效容器
        private float lastMoveInput = 0f; // 上一帧的移动输入
        private bool isFacingRight = true; // 当前面向方向
        protected override void InitializeController()
        {
            // Debug.Log("玩家控制器初始化完成");
            // 获取玩家参数
            playerModel = this.GetModel<IPlayerModel>();
            
            // 获取组件
            rb = GetComponent<Rigidbody2D>();

            // 获取玩家精灵渲染器
            if (playerSprite == null)
            {
                playerSprite = GetComponent<SpriteRenderer>();
            }

            // 保存原始颜色
            if (playerSprite != null)
            {
                originalColor = playerSprite.color;
            }

            // 初始化瞄准时间
            _baseMaxAimTime = maxAimTime;

            // 初始化状态机
            stateMachine = new E();
            stateMachine.AddState("Idle", new IdleState(this, rb));
            stateMachine.AddState("Jump", new JumpState(this, rb));
            stateMachine.AddState("Move", new MoveState(this, rb));
            stateMachine.AddState("Grind", new GrindState(this, rb));
            stateMachine.AddState("Air", new AirState(this, rb));
            stateMachine.AddState("Trick", new TrickState(this, rb));
            stateMachine.AddState("GJump", new GJumpState(this, rb));
            stateMachine.AddState("DoubleJump", new DoubleJumpState(this, rb));
            stateMachine.AddState("Grab", new GrabbingState(this, rb));
            stateMachine.AddState("WallRide", new WallRideState(this, rb));
            stateMachine.AddState("Reverse", new ReverseState(this, rb));
            stateMachine.AddState("PowerGrind", new PowerGrindState(this, rb));

            stateMachine.SwitchState("Idle");
        }

        protected override void OnRealTimeUpdate()
        {
            // 更新状态机
            string currentState = stateMachine.GetCurrentStateName();

            if(wasGrounded){
                this.GetModel<IPlayerModel>().AllowDoubleJump.Value = true;
                
            }
            else{
                // 只有在不是特殊状态时才切换到Air状态
                if (currentState != "Jump" && currentState != "DoubleJump" && 
                    currentState != "WallRide" && currentState != "Trick" && 
                    currentState != "PowerGrind" && currentState != "Reverse")
                {
                    stateMachine.SwitchState("Air");
                    Debug.Log("切换到Air状态");
                }
            }

            stateMachine.UpdateCurrentState();

            // 更新轨道跳计时器
            if (grindJumpTimer > 0f)
            {
                grindJumpTimer -= Time.deltaTime;
            }

            // 更新跳跃缓冲计时器
            if (jumpBufferTimer > 0f)
            {
                jumpBufferTimer -= Time.deltaTime;
            }

            // 更新反向检测计时器
            if (isCheckingReverseWindow)
            {
                reverseTimer += Time.deltaTime;
                if (reverseTimer >= reverseInputWindow)
                {
                    isCheckingReverseWindow = false;
                    reverseTimer = 0f;
                    Debug.Log("反向检测计时窗口结束");
                }
            }

            // 重置跳跃标志
            hasJumpedThisFrame = false;

            // 更新着地状态
            wasGrounded = IsGrounded();

            // 检测输入并发送事件
            DetectInput();

            HandleAimAndShoot();
            
            // 检测玩家方向变化并更新粒子特效
            CheckPlayerDirectionChange();
        }

        private void DetectInput()
        {
            // 获取当前状态和移动输入
            string currentState = stateMachine.GetCurrentStateName();
            float moveInput = Input.GetAxisRaw("Horizontal");

            // 调试信息
            if (Mathf.Abs(moveInput) > 0.01f)
            {
                Debug.Log($"移动输入检测: moveInput={moveInput}, currentState={currentState}, IsGrounded={IsGrounded()}");
            }

            // 跳跃输入检测 - 只负责状态切换
            // 在Air状态下不处理空格键，让AirState自己处理二段跳
            Debug.Log("hasJumpedThisFrame:" + !hasJumpedThisFrame + "currentstate: " + currentState + "wasground" + wasGrounded);
            if (Input.GetKeyDown(KeyCode.Space) && currentState != "Air")
            {
                if (wasGrounded)
                {
                    Debug.Log("Space键按下 - 切换到Jump状态" + "currentState:" + currentState);
                    hasJumpedThisFrame = true; // 设置标志防止重复跳跃
                    stateMachine.SwitchState("Jump");

                    return; // 跳跃后直接返回，不处理其他输入
                }
                //else
                //{
                //    // 如果不在着地状态，启动跳跃缓冲
                //    jumpBufferTimer = jumpBufferTime;
                //     Debug.Log("Space键按下但未着地，启动跳跃缓冲");
                //}
            }

            // 检查跳跃缓冲
            //if (jumpBufferTimer > 0f && wasGrounded && !hasJumpedThisFrame)
            //{
            //    Debug.Log("跳跃缓冲触发 - 切换到Jump状态");
            //    hasJumpedThisFrame = true;
            //    jumpBufferTimer = 0f; // 清除缓冲
            //    stateMachine.SwitchState("Jump");
            //    return;
            //}
            if (Input.GetKeyDown(KeyCode.F))
            {
                currentBulletIndex = (currentBulletIndex + 1) % bulletPrefabs.Length;
                Debug.Log("当前子弹类型: " + currentBulletIndex);
            }


            // 轨道输入 - E键只用于滑轨，不用于技巧
            if (Input.GetKeyDown(KeyCode.E))
            {
                // 在地面上按E键，检查滑轨条件
                Debug.Log($"E键按下 - 检测滑轨条件:");
                Debug.Log($"  - IsGrounded(): {IsGrounded()}");
                Debug.Log($"  - isNearTrack: {isNearTrack}");
                Debug.Log($"  - currentTrack: {(currentTrack != null ? currentTrack.name : "null")}");
                Debug.Log($"  - grindJumpTimer: {grindJumpTimer}");

                isEHeld = true;
                this.SendEvent<GrindInputEvent>();
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                isEHeld = false;
            }

            // 强力轨道输入 - 只有在没有按Space键时才触发
            if (Input.GetKeyDown(KeyCode.W) && IsGrounded() && !Input.GetKey(KeyCode.Space))
            {
                isWHeld = true;

                if (!isCheckingReverseWindow)
                {
                    CheckReverseWindow();
                }

                stateMachine.SwitchState("PowerGrind");
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                isWHeld = false;
            }

            // 技巧输入 - J对应TrickA，K对应TrickB
            if (Input.GetKeyDown(KeyCode.W)&&stateMachine.GetCurrentStateName() == "Air")
            {
                Debug.Log("trickainput");
                this.SendEvent<TrickAInputEvent>();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                this.SendEvent<TrickBInputEvent>();
            }

            // 反向检测 - 在PowerGrind状态下检测反向输入
            if (currentState == "PowerGrind" && isCheckingReverseWindow)
            {
                float currentVelocityX = rb.linearVelocity.x;

                // 如果当前有水平速度且输入方向与速度方向相反
                if (Mathf.Abs(currentVelocityX) > 1f && Mathf.Abs(moveInput) > 0.01f)
                {
                    if (Mathf.Sign(moveInput) != Mathf.Sign(currentVelocityX))
                    {
                        Debug.Log($"PowerGrind状态下检测到反向输入: 当前速度={currentVelocityX}, 输入={moveInput}");
                        stateMachine.SwitchState("Reverse");
                        isCheckingReverseWindow = false;
                        return; // 进入反向状态后直接返回，不处理其他逻辑
                    }
                }
            }

            // 移动状态切换（基于移动输入）
            if (currentState == "Idle" && Mathf.Abs(moveInput) > 0.01f && IsGrounded())
            {
                stateMachine.SwitchState("Move");

            }
            else if (currentState == "Move" && Mathf.Abs(moveInput) <= 0.01f && IsGrounded())
            {
                stateMachine.SwitchState("Idle");
                Debug.Log("2222:" + currentState);
            }



        }

        // 提供给状态机使用的方法
        public bool IsGrounded()
        {
            if (rb == null)
            {
                Debug.LogWarning("IsGrounded: rb为空");
                return false;
            }

            // 使用多个射线检测来提高准确性
            Vector2 rayStart = transform.position;
            Vector2 rayDirection = Vector2.down;
            float rayDistance = 0.35f; // 减少检测距离，避免误判

            // 主射线检测
            RaycastHit2D hit = Physics2D.Raycast(rayStart, rayDirection, rayDistance, groundLayer);

            // 如果主射线没检测到，尝试左右偏移的射线
            if (hit.collider == null)
            {
                Vector2 leftRayStart = rayStart + Vector2.left * 0.3f;
                Vector2 rightRayStart = rayStart + Vector2.right * 0.3f;

                RaycastHit2D leftHit = Physics2D.Raycast(leftRayStart, rayDirection, rayDistance, groundLayer);
                RaycastHit2D rightHit = Physics2D.Raycast(rightRayStart, rayDirection, rayDistance, groundLayer);

                if (leftHit.collider != null)
                {
                    hit = leftHit;
                }
                else if (rightHit.collider != null)
                {
                    hit = rightHit;
                }
            }

            bool grounded = hit.collider != null;

            return grounded;
        }

        public Rigidbody2D GetRigidbody()
        {
            return rb;
        }

        // 奖励跳跃方法 - 用于高等级技巧奖励
        public void RewardJump()
        {
            if (rb != null)
            {
                // 给予一个额外的跳跃力
                Vector2 currentVelocity = rb.linearVelocity;
                rb.linearVelocity = new Vector2(currentVelocity.x, 0);
                Debug.Log("奖励跳跃！获得额外跳跃力");
            }
        }

        // 延迟切换状态的协程
        public IEnumerator SwitchToStateDelayed(string stateName)
        {
            yield return new WaitForSeconds(0.1f);
            stateMachine.SwitchState(stateName);
        }

        // 反向输入检测 - 只负责计时
        private void CheckReverseWindow()
        {
            isCheckingReverseWindow = true;
            reverseTimer = 0f;
            Debug.Log("开始反向检测计时窗口");
        }

        // 碰撞检测
        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"OnTriggerEnter2D: {other.name} (isTrigger: {other.isTrigger})");

            if (other.isTrigger)
            {
                Track track = other.GetComponent<Track>();
                if (track != null)
                {
                    Debug.Log($"检测到滑轨: {track.name}");
                    currentTrack = track;
                    isNearTrack = true;
                }

                Wall wall = other.GetComponent<Wall>();
                if (wall != null)
                {
                    Debug.Log($"检测到墙壁: {wall.name}");
                    currentWall = wall;
                    isNearWall = true;
                }
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log($"OnTriggerExit2D: {other.name}");

            if (other.isTrigger)
            {
                Track track = other.GetComponent<Track>();
                if (track != null && track == currentTrack)
                {
                    Debug.Log($"离开滑轨: {track.name}");
                    isNearTrack = false;
                    currentTrack = null;
                }

                Wall wall = other.GetComponent<Wall>();
                if (wall != null)
                {
                    Debug.Log($"离开墙壁: {wall.name}");
                    currentWall = null;
                    isNearWall = false;
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 1.5f);
        }




        private void HandleAimAndShoot()
        {
            // 按住R键进入瞄准
            if (Input.GetKeyDown(KeyCode.R))
            {
                isAiming = true;
                aimTimer = 0f; // 重置计时器
                Time.timeScale = 0.2f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                if (aimLine != null) aimLine.enabled = true;
                Debug.Log("开始瞄准模式");
            }

            // 松开R键发射子弹
            if (Input.GetKeyUp(KeyCode.R))
            {
                if (isAiming)
                {
                    FireBullet();
                }
                StopAiming();
            }

            if (isAiming)
            {
                // 更新瞄准计时器
                aimTimer += Time.unscaledDeltaTime; // 使用unscaledDeltaTime因为时间被放慢了

                // 检查是否超过最大瞄准时间
                if (aimTimer >= maxAimTime)
                {
                    StopAiming();
                    return;
                }

                // 获取鼠标方向
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

                // 更新瞄准线
                if (aimLine != null)
                {
                    aimLine.SetPosition(0, transform.position);
                    aimLine.SetPosition(1, (Vector2)transform.position + direction * aimLineLength);
                }
            }
        }

        private void StopAiming()
        {
            isAiming = false;
            aimTimer = 0f;
            Time.timeScale = 1f;
            if (aimLine != null) aimLine.enabled = false;
            Debug.Log("结束瞄准模式");
        }

        private void FireBullet()
        {
            if (bulletPrefabs.Length == 0) return;

            GameObject bulletPrefab = bulletPrefabs[currentBulletIndex];
            if (bulletPrefab == null) return;

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = direction * bulletSpeed;
            }

            Debug.Log($"发射子弹！方向: {direction}, 速度: {bulletSpeed}");
            // 如果是轨道子弹，不需要方向参数，轨道始终水平生成
        }

        // 颜色管理方法
        public void ChangePlayerColor(Color newColor)
        {
            if (playerSprite != null)
            {
                playerSprite.color = newColor;
            }
        }

        public void ResetPlayerColor()
        {
            if (playerSprite != null)
            {
                playerSprite.color = originalColor;
            }
        }

        // 获取当前瞄准计时器值（供UI使用）
        public float GetAimTimer()
        {
            return aimTimer;
        }

        // 标记已执行trick（由TrickState调用）
        public void MarkTrickPerformed()
        {
            hasPerformedTrickInAir = true;
            Debug.Log("InputController: 标记已执行trick");
        }

        // 处理落地时的瞄准时间奖励
        public void HandleLandingAimTimeBonus()
        {
            if (hasPerformedTrickInAir)
            {
                // 增加瞄准时间上限1秒
                maxAimTime += 1f;
                Debug.Log($"InputController: 落地奖励！瞄准时间上限增加1秒，当前上限: {maxAimTime}秒");

                // 重置标志
                hasPerformedTrickInAir = false;
            }
        }

        // 重置瞄准时间上限到基础值
        public void ResetAimTimeToBase()
        {
            maxAimTime = _baseMaxAimTime;
            Debug.Log($"InputController: 重置瞄准时间上限到基础值: {maxAimTime}秒");
        }

        // 获取基础瞄准时间上限（供UI使用）
        public float baseMaxAimTime => _baseMaxAimTime;

        // 检测玩家方向变化并更新粒子特效容器
        private void CheckPlayerDirectionChange()
        {
            float currentMoveInput = Input.GetAxisRaw("Horizontal");
            
            // 检查是否有移动输入
            if (Mathf.Abs(currentMoveInput) > 0.01f)
            {
                bool shouldFaceRight = currentMoveInput > 0;
                
                // 检查方向是否改变
                if (isFacingRight != shouldFaceRight)
                {
                    // 方向改变，旋转粒子特效容器180度
                    if (particleEffectContainer != null)
                    {
                        Vector3 currentRotation = particleEffectContainer.localEulerAngles;
                        particleEffectContainer.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y + 180f, currentRotation.z);
                        isFacingRight = shouldFaceRight;
                        
                    }
                }
            }
            
            // 更新上一帧的移动输入
            lastMoveInput = currentMoveInput;
        }
    }


}

