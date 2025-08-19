using UnityEngine;
using QFramework;

namespace SkateGame
{
    /// <summary>
    /// 碰撞检测控制器
    /// 负责检测玩家碰撞，发送相应事件
    /// 只负责检测，具体处理在系统层
    /// </summary>
    public class CollisionController : ViewerControllerBase
    {
        [Header("碰撞检测设置")]
        public LayerMask groundLayer = 1;
        public LayerMask trackLayer = 1;
        public LayerMask wallLayer = 1;
        
        private PlayerScript player;
        private Rigidbody2D rb;
        
        protected override void InitializeController()
        {
            Debug.Log("碰撞检测控制器初始化完成");
            
            player = GetComponent<PlayerScript>();
            rb = GetComponent<Rigidbody2D>();
        }
        
        protected override void OnRealTimeUpdate()
        {
            // 检测地面碰撞
            DetectGroundCollision();
            
            // 检测轨道碰撞
            DetectTrackCollision();
            
            // 检测墙壁碰撞
            DetectWallCollision();
        }
        
        private void DetectGroundCollision()
        {
            // 射线检测地面
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
            bool isGrounded = hit.collider != null;
            
            // 获取当前状态
            var playerModel = this.GetModel<IPlayerModel>();
            bool wasGrounded = playerModel.IsGrounded.Value;
            
            // 状态发生变化时发送事件
            if (isGrounded != wasGrounded)
            {
                playerModel.IsGrounded.Value = isGrounded;
                playerModel.IsInAir.Value = !isGrounded;
                
                if (isGrounded && !wasGrounded)
                {
                    // 玩家着地
                    this.SendEvent<PlayerLandedEvent>();
                }
            }
        }
        
        private void DetectTrackCollision()
        {
            // 检测是否靠近轨道
            Collider2D[] tracks = Physics2D.OverlapCircleAll(transform.position, 2f, trackLayer);
            bool isNearTrack = tracks.Length > 0;
            
            var playerModel = this.GetModel<IPlayerModel>();
            playerModel.IsNearTrack.Value = isNearTrack;
        }
        
        private void DetectWallCollision()
        {
            // 检测是否靠近墙壁
            Collider2D[] walls = Physics2D.OverlapCircleAll(transform.position, 1.5f, wallLayer);
            bool isNearWall = walls.Length > 0;
            
            var playerModel = this.GetModel<IPlayerModel>();
            playerModel.IsNearWall.Value = isNearWall;
        }
        
        // 碰撞事件处理
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // 可以根据碰撞对象发送不同事件
            if (collision.gameObject.CompareTag("Track"))
            {
                // 轨道碰撞事件
            }
            else if (collision.gameObject.CompareTag("Wall"))
            {
                // 墙壁碰撞事件
            }
        }
    }
}
