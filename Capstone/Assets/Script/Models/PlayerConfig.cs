using UnityEngine;

namespace SkateGame
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "SkateGame/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("========== Action Layer基础参数 ==========")]
        [Header("No Action State")]
        public bool isLoopNoAction = true;
        public Vector2 ignoreMovementLayerDurationNoAction = new Vector2(-1f, -1f);

        [Header("Grind State")]
        public bool isLoopGrind = true;
        public Vector2 ignoreMovementLayerDurationGrind = new Vector2(1f, 1f);

        [Header("Wall Ride State")]
        public bool isLoopWallRide = true;
        public Vector2 ignoreMovementLayerDurationWallRide = new Vector2(-1f, -1f);

        [Header("Grabbing State")]
        public bool isLoopGrab = true;
        public Vector2 ignoreMovementLayerDurationGrab = new Vector2(-1f, -1f);
        [Header("Trick A State")]
        public bool isLoopTrickA = false;
        public float durationTrickA = 0.5f;
        public Vector2 ignoreMovementLayerDurationTrickA = new Vector2(0f, 0f);
        public Vector2 recoveryDurationTrickA = new Vector2(0f, 0.5f);

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
        [Header("Wall Ride相关")]
        public float wallRideCooldown = 1.5f;
        public float wallrideDuration = 0.6f;

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
