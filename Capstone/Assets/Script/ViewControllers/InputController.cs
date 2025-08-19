using UnityEngine;
using QFramework;
using System.Collections;

namespace SkateGame
{
    /// <summary>
    /// 玩家控制器
    /// 集成了输入检测、状态机管理和物理控制
    /// 替代原来的PlayerScript
    /// </summary>
    public class InputController : ViewerControllerBase
    {
        [Header("状态机")]
        public E stateMachine;
        private Rigidbody2D rb;
        
        [Header("跳跃设置")]
        public float maxJumpForce = 14f;
        public float minJumpForce = 2f;
        public float doubleJumpForce = 8f;
        public float maxChargeTime = 2f;

        [Header("移动设置")]
        public float moveSpeed = 5f;
        public float airControlForce = 5f;
        public float maxAirHorizontalSpeed = 10f;
        
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
        
        [Header("Life Setting")]
        public bool canBeHurt;
        
        [Header("Combat Setting")]
        public bool isPowerGrinding;
        private bool isCheckingReverseWindow = false;
        public float reverseInputWindow = 0.2f;
        
        protected override void InitializeController()
        {
            Debug.Log("玩家控制器初始化完成");
            
            // 获取组件
            rb = GetComponent<Rigidbody2D>();
            
            // 初始化状态机
            stateMachine = new E();
            stateMachine.AddState("Idle", new IdleState(this, rb));
            stateMachine.AddState("Jump", new JumpState(this, rb));
            stateMachine.AddState("Move", new MoveState(this, rb));
            stateMachine.AddState("Grind", new GrindState(this, rb));
            stateMachine.AddState("Air", new AirState(this, rb));
            stateMachine.AddState("Trick", new TrickState(this, rb));
            stateMachine.AddState("GJump", new GJumpState(this, rb));
            stateMachine.AddState("Grab", new GrabbingState(this, rb));
            stateMachine.AddState("WallRide", new WallRideState(this, rb));
            stateMachine.AddState("Reverse", new ReverseState(this, rb));
            stateMachine.AddState("PowerGrind", new PowerGrindState(this, rb));
            
            stateMachine.SwitchState("Idle");
        }
        
        protected override void OnRealTimeUpdate()
        {
            // 更新状态机
            stateMachine.UpdateCurrentState();
            
            // 更新轨道跳计时器
            if (grindJumpTimer > 0f)
            {
                grindJumpTimer -= Time.deltaTime;
            }
            
            // 检测输入并发送事件
            DetectInput();
        }
        
        private void DetectInput()
        {
            // 跳跃输入
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                this.SendEvent<JumpInputEvent>();
            }
            
            // 轨道输入
            if (Input.GetKeyDown(KeyCode.E) && !IsGrounded())
            {
                isEHeld = true;
                this.SendEvent<GrindInputEvent>();
            }
            
            if (Input.GetKeyUp(KeyCode.E))
            {
                isEHeld = false;
            }
            
            // 强力轨道输入
            if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
            {
                isWHeld = true;
                
                if (!isCheckingReverseWindow)
                {
                    StartCoroutine(CheckReverseWindow());
                }
            }
            
            if (Input.GetKeyUp(KeyCode.W))
            {
                isWHeld = false;
            }
            
            // 技巧输入
            if (Input.GetKeyDown(KeyCode.A))
            {
                this.SendEvent<TrickAInputEvent>();
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                this.SendEvent<TrickBInputEvent>();
            }
            
            // 移动状态切换
            string currentState = stateMachine.GetCurrentStateName();
            float moveInput = Input.GetAxisRaw("Horizontal");
            
            if (currentState == "Idle" && Mathf.Abs(moveInput) > 0.01f && IsGrounded())
            {
                stateMachine.SwitchState("Move");
            }
            else if (currentState == "Move" && Mathf.Abs(moveInput) <= 0.01f && IsGrounded())
            {
                stateMachine.SwitchState("Idle");
            }
        }
        
        // 提供给状态机使用的方法
        public bool IsGrounded()
        {
            if (rb == null) return false;
            return Mathf.Abs(rb.linearVelocity.y) < 0.01f;
        }
        
        public Rigidbody2D GetRigidbody()
        {
            return rb;
        }
        
        // 延迟切换状态的协程
        public IEnumerator SwitchToStateDelayed(string stateName)
        {
            yield return new WaitForSeconds(0.1f);
            stateMachine.SwitchState(stateName);
        }
        
        // 反向输入检测协程
        private IEnumerator CheckReverseWindow()
        {
            isCheckingReverseWindow = true;

            float timer = 0f;
            bool reverseTriggered = false;
            float originalDirection = Mathf.Sign(rb.linearVelocity.x);

            while (timer < reverseInputWindow)
            {
                float input = Input.GetAxisRaw("Horizontal");

                if (Mathf.Abs(input) > 0.01f && Mathf.Sign(input) != originalDirection)
                {
                    Debug.Log("Reverse");
                    this.SendEvent<ReverseInputEvent>();
                    reverseTriggered = true;
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            if (!reverseTriggered)
            {
                Debug.Log("PG");
                this.SendEvent<PowerGrindInputEvent>();
            }

            isCheckingReverseWindow = false;
        }
        
        // 碰撞检测
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger)
            {
                Track track = other.GetComponent<Track>();
                if (track != null)
                {
                    currentTrack = track;
                    isNearTrack = true;
                }

                Wall wall = other.GetComponent<Wall>();
                if (wall != null)
                { 
                    currentWall = wall;
                    isNearWall = true;
                }
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.isTrigger)
            {
                Track track = other.GetComponent<Track>();
                if (track != null && track == currentTrack)
                {
                    isNearTrack = false;
                    currentTrack = null;
                }

                Wall wall = other.GetComponent<Wall>();
                if (wall != null)
                {
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
    }
}
