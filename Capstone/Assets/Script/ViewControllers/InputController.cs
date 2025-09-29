using UnityEngine;
using QFramework;
using System.Collections;
using MoreMountains.Feedbacks;
using System;

namespace SkateGame
{
    /// <summary>
    /// 玩家控制器
    /// 集成了输入检测、状态机管理和物理控制
    /// 替代原来的PlayerScript
    /// </summary>
    public class InputController : ViewerControllerBase
    {
        public PlayerConfig playerConfig;
        private IPlayerModel playerModel;
        private IInputModel inputModel;
        [Header("状态机")]
        public LayeredStateMachine stateMachine;
        private Rigidbody2D rb;

        [Header("Animation")]
        public Animator animator;
        public SpriteRenderer spriteRenderer;

        [Header("轨道设置")]
        public float grindJumpIgnoreTime = 0.2f;
        public float grindJumpTimer = 0f;

        [Header("空中设置")]
        public bool isInAir = false;
        public float airTime = 0f;
        public int airCombo = 0;

        [Header("Wall Setting")]
        public Wall currentWall;

        [Header("Ground Detection")]
        public LayerMask groundLayer = 1; // 地面层级

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

        public GameObject[] bulletPrefabs;   // 可切换的子弹类型
        public float bulletSpeed = 15f;

        [Header("颜色设置")]
        public SpriteRenderer playerSprite; // 玩家精灵渲染器
        private Color originalColor = Color.white; // 原始颜色

        [Header("瞄准时间奖励")]
        private float _baseMaxAimTime = 3f; // 基础瞄准时间上限

        [Header("MMF效果")]
        public MMF_Player moveEffect;
        public MMF_Player powerGrindEffect;
        public MMF_Player ReverseEffect;
        public MMF_Player GrindEffect;
        public MMF_Player WallRideEffect;
        public MMF_Player TrickAEffect;
        public MMF_Player TrickBEffect;
        public MMF_Player TrickABoostEffect;
        public MMF_Player AirEffect;
        public MMF_Player JumpEffect;
        public MMF_Player GJumpEffect;
        public MMF_Player DoubleJumpEffect;
        public MMF_Player GrabEffect;
        public MMF_Player IdleEffect;
        public MMF_Player powerGrindEffectPlayer; 
        
        [Header("粒子特效容器")]
        public Transform particleEffectContainer; // 粒子特效容器
        private float lastMoveInput = 0f; //上一帧的移动输入
        private bool isFacingRight = true; // 当前面向方向
        protected override void InitializeController()
        {
            // Debug.Log("玩家控制器初始化完成");
            // 获取玩家参数
            playerModel = this.GetModel<IPlayerModel>();
            inputModel = this.GetModel<IInputModel>();
            this.GetSystem<IPlayerAssetSystem>().SetPlayerConfig(playerConfig);
            
            // 获取组件
            rb = GetComponent<Rigidbody2D>();

            // 获取玩家精灵渲染器
            if (playerSprite == null)
            {
                playerSprite = GetComponentInChildren<SpriteRenderer>();
                if (playerSprite == null)
                {
                    playerSprite = GetComponent<SpriteRenderer>();
                }
            }

            // 保存原始颜色
            if (playerSprite != null)
            {
                originalColor = playerSprite.color;
            }

            // 初始化瞄准时间
            playerModel.MaxAimTime.Value = _baseMaxAimTime;

            // 初始化分层状态机
            stateMachine = new LayeredStateMachine();
            
            // Movement Layer
            stateMachine.AddState("Idle", new IdleState(this, rb), StateLayer.Movement);
            stateMachine.AddState("Jump", new JumpState(this, rb), StateLayer.Movement);
            stateMachine.AddState("GJump", new GJumpState(this, rb), StateLayer.Movement);
            stateMachine.AddState("Move", new MoveState(this, rb), StateLayer.Movement);
            stateMachine.AddState("Air", new AirState(this, rb), StateLayer.Movement);
            stateMachine.AddState("DoubleJump", new DoubleJumpState(this, rb), StateLayer.Movement);
            stateMachine.AddState("PowerGrind", new PowerGrindState(this, rb), StateLayer.Movement);
            stateMachine.AddState("Reverse", new ReverseState(this, rb), StateLayer.Movement);
            stateMachine.AddState("Land", new LandState(this, rb), StateLayer.Movement);
            // Action Layer
            stateMachine.AddState("None", new NoActionState(), StateLayer.Action);
            stateMachine.AddState("TrickA", new TrickAState(this, rb), StateLayer.Action);
            stateMachine.AddState("Grind", new GrindState(this, rb), StateLayer.Action);
            stateMachine.AddState("Grab", new GrabbingState(this, rb), StateLayer.Action);
            stateMachine.AddState("WallRide", new WallRideState(this, rb), StateLayer.Action);

            // 初始各层状态
            stateMachine.SwitchState(StateLayer.Movement, "Idle");
            stateMachine.SwitchState(StateLayer.Action, "None");
        }

        protected override void OnRealTimeUpdate()
        {
            IsGrounded();
            /// Warning 待创建一个落地状态

            if(playerModel.WasGrounded.Value)
            {
                playerModel.CanDoubleJump.Value = true;
            }

            // 更新当前State
            stateMachine.UpdateCurrentState();

            /* 移动由OnMoveInput事件处理 */
            float moveInput = inputModel.Move.Value.x;
            this.SendEvent<MoveInputEvent>(new MoveInputEvent { HorizontalInput = moveInput });

            // 更新着地状态
            playerModel.WasGrounded.Value = IsGrounded();

            // 发送移动输入事件
            this.SendEvent<MoveInputEvent>(new MoveInputEvent { HorizontalInput = moveInput });
            // 检测输入并发送事件
            DetectInput();

            HandleAimAndShoot();

            // 检测玩家方向变化并更新粒子特效
            CheckPlayerDirectionChange();
        }

        // 提供给状态机使用的方法
        public void DetectInput()
        {
            Grind();
            SwitchItem();
            Trick();
        }

        private void Grind()
        {
            if (inputModel.Grind.Value)
            {
                this.SendEvent<GrindInputEvent>();
            }
        }
        private void SwitchItem()
        {
            if (inputModel.SwitchItem.Value)
            {
                playerModel.CurrentBulletIndex.Value = (playerModel.CurrentBulletIndex.Value + 1) % bulletPrefabs.Length;
                Debug.Log("当前子弹类型: " + playerModel.CurrentBulletIndex.Value);
            }
        }
        private void Trick()
        {
            if (inputModel.TrickStart.Value && stateMachine.GetMovementStateName() == "Air")
            {
                this.SendEvent<TrickAInputEvent>();
            }
        }

        #region Collision
        public bool IsGrounded()
        {
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
            playerModel.WasGrounded.Value = playerModel.IsGrounded.Value;
            playerModel.IsGrounded.Value = grounded;
            return grounded;
        }
        #endregion

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
                rb.linearVelocity = new Vector2(currentVelocity.x, 10f);
                Debug.Log("奖励跳跃！获得额外跳跃力");
            }
        }

        // 延迟切换状态的协程
        public IEnumerator SwitchToStateDelayed(string stateName)
        {
            yield return new WaitForSeconds(0.1f);
            stateMachine.SwitchState(StateLayer.Action, stateName);
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
                    playerModel.CurrentTrack.Value = track;
                    playerModel.IsNearTrack.Value = true;
                }

                Wall wall = other.GetComponent<Wall>();
                if (wall != null)
                {
                    Debug.Log($"检测到墙壁: {wall.name}");
                    currentWall = wall;
                    playerModel.IsNearWall.Value = true;
                }
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log($"OnTriggerExit2D: {other.name}");

            if (other.isTrigger)
            {
                Track track = other.GetComponent<Track>();
                if (track != null && track == playerModel.CurrentTrack.Value)
                {
                    Debug.Log($"离开滑轨: {track.name}");
                    playerModel.IsNearTrack.Value = false;
                    playerModel.CurrentTrack.Value = null;
                }

                Wall wall = other.GetComponent<Wall>();
                if (wall != null)
                {
                    Debug.Log($"离开墙壁: {wall.name}");
                    currentWall = null;
                    playerModel.IsNearWall.Value = false;
                }
            }
        }

        private void HandleAimAndShoot()
        {
            // 按住R键进入瞄准
            if (Input.GetMouseButtonDown(0))
            {
                playerModel.IsAiming.Value = true;
                playerModel.AimTimer.Value = 0f; // 重置计时器
                Time.timeScale = 0.2f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                if (aimLine != null) aimLine.enabled = true;
            }

            // 松开R键发射子弹
            if (Input.GetMouseButtonUp(0))
            {
                if(playerModel.IsAiming.Value)
                {
                    FireBullet();
                }
                StopAiming();
            }

            if (playerModel.IsAiming.Value)
            {
                // 更新瞄准计时器
                playerModel.AimTimer.Value += Time.unscaledDeltaTime; // 使用unscaledDeltaTime因为时间被放慢了

                // 检查是否超过最大瞄准时间
                if (playerModel.AimTimer.Value >= playerModel.MaxAimTime.Value)
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
            playerModel.IsAiming.Value = false;
            playerModel.AimTimer.Value = 0f;
            Time.timeScale = 1f;
            if (aimLine != null) aimLine.enabled = false;
        }

        private void FireBullet()
        {
            if (bulletPrefabs.Length == 0) return;

            GameObject bulletPrefab = bulletPrefabs[playerModel.CurrentBulletIndex.Value];
            if (bulletPrefab == null) return;

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            var ice = bullet.GetComponent<IceBullet>();
            if (ice != null)
            {
                ice.SetDirection(direction);
            }
            else
            {
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                if (bulletRb != null)
                {
                    bulletRb.linearVelocity = direction * bulletSpeed;
                }
            }

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
            return playerModel.AimTimer.Value;
        }

        // 标记已执行trick（由TrickState调用）
        public void MarkTrickPerformed()
        {
            playerModel.HasPerformedTrickInAir.Value = true;
            Debug.Log("InputController: 标记已执行trick");
        }

        // 处理落地时的瞄准时间奖励
        public void HandleLandingAimTimeBonus()
        {
            if (playerModel.HasPerformedTrickInAir.Value)
            {
                // 增加瞄准时间上限1秒
                playerModel.MaxAimTime.Value += 1f;
                Debug.Log($"InputController: 落地奖励！瞄准时间上限增加1秒，当前上限: {playerModel.MaxAimTime.Value}秒");

                // 重置标志
                playerModel.HasPerformedTrickInAir.Value = false;
            }
        }

        // 重置瞄准时间上限到基础值
        public void ResetAimTimeToBase()
        {
            playerModel.MaxAimTime.Value = _baseMaxAimTime;
            Debug.Log($"InputController: 重置瞄准时间上限到基础值: {playerModel.MaxAimTime.Value}秒");
        }

        // 获取基础瞄准时间上限（供UI使用）
        public float baseMaxAimTime => _baseMaxAimTime;

        // 检测玩家方向并更新例子特效容器
        private void CheckPlayerDirectionChange()
        {
            float currentMoveInput = inputModel.Move.Value.x;
            
            // 检测是否有移动输入
            if(Mathf.Abs(currentMoveInput) > 0.01f)
            {
                bool shouldFaceRight = currentMoveInput > 0;
                // 检测方向是否改变
                if(shouldFaceRight != isFacingRight)
                {
                    // 方向改变，粒子特效容器旋转180度
                    if(particleEffectContainer != null)
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