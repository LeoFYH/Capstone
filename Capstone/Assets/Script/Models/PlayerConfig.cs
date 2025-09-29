using UnityEngine;

namespace SkateGame
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "SkateGame/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("跳跃设置")]
        public float maxJumpForce = 6f;
        public float minJumpForce = 0f;
        public float doubleJumpForce = 8f;
        public float maxChargeTime = 2f;

        [Header("移动设置")]
        public float maxMoveSpeed = 5f;
        public float maxAirHorizontalSpeed = 10f;
        public float airControlForce = 5f;
        public float airAccel = 20f;
        public float airDecel = 10f;
        public float groundAccel = 20f;
        public float groundDecel = 10f;
        public float turnDecel = 40f;

        [Header("Air相关")]
        public float airControlForceConfig = 10f;
        public float maxAirHorizontalSpeedConfig = 8f;

        [Header("Grind相关")]
        public float normalG = 1f;

        [Header("Move相关")]
        public float acceleration = 15f;
        public float moveDeceleration = 20f;
        public float maxSpeed = 5f;

        [Header("Power Grind相关")]
        public float powerGrindDeceleration = 1f;
        public float reverseInputWindow = 2.0f;
        public float grindJumpIgnoreTime = 0.2f;

        [Header("Trick相关")]
        public float trickADuration = 1.5f;
        public int trickAScore = 20;
        public float trickBDuration = 1.5f;
        public int trickBScore = 20;

        [Header("瞄准设置")]
        public float baseMaxAimTime = 3f;
        public GameObject[] bulletPrefabs;   // 可切换的子弹类型
        public float bulletSpeed = 15f;
        
        [Header("Ground Detection")]
        public LayerMask groundLayer; 
    }
}
