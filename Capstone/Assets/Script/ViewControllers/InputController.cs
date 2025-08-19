using UnityEngine;
using QFramework;

namespace SkateGame
{
    /// <summary>
    /// 输入控制器
    /// 负责检测玩家输入，发送相应事件
    /// 只负责检测，具体处理在系统层
    /// </summary>
    public class InputController : ViewerControllerBase
    {
        [Header("输入设置")]
        public KeyCode trickAKey = KeyCode.J;
        public KeyCode trickBKey = KeyCode.K;
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode grindKey = KeyCode.E;
        
        protected override void InitializeController()
        {
            Debug.Log("输入控制器初始化完成");
        }
        
        protected override void OnRealTimeUpdate()
        {
            // 只检测输入，发送事件
            DetectInput();
        }
        
        private void DetectInput()
        {
            // 检测技巧输入
            if (Input.GetKeyDown(trickAKey))
            {
                this.SendEvent<TrickAInputEvent>();
            }
            else if (Input.GetKeyDown(trickBKey))
            {
                this.SendEvent<TrickBInputEvent>();
            }
            
            // 检测跳跃输入
            if (Input.GetKeyDown(jumpKey))
            {
                this.SendEvent<JumpInputEvent>();
            }
            
            // 检测轨道输入
            if (Input.GetKeyDown(grindKey))
            {
                this.SendEvent<GrindInputEvent>();
            }
        }
    }
}
