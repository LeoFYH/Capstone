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
        public bool allowDoubleJump = true;

        [Header("移动设置")]
        public float moveSpeed = 5f;
        public float airControlForce = 5f;
        public float maxAirHorizontalSpeed = 10f;
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

        [Header("Trick相关")]
        public float trickADuration = 1.5f;
        public int trickAScore = 20;
        public float trickBDuration = 1.5f;
        public int trickBScore = 20;

        [Header("瞄准设置")]
        public float maxAimTime = 3f;
    }
}
