using UnityEngine;

namespace SkateGame
{
    /// <summary>
    /// 冰子弹
    /// 继承Bullet基类，实现冰子弹的特殊逻辑（生成轨道）
    /// </summary>
    public class IceBullet : Bullet
    {
        [Header("冰子弹特殊设置")]
        public float speed = 15f;                  // 子弹速度
        public float timeToCreateTrack = 2f;       // 多少秒后生成轨道（秒）
        public GameObject trackPrefab;             // 轨道Prefab
        public GameObject TempLiftPrefab;          // 临时升降平台Prefab

        private Vector2 startPos;
        private float launchHeight;                // 发射时高度
        private Vector2 direction;                 // 飞行方向
        private float elapsedTime = 0f;            // 已过时间
        private bool spawned = false;              // 是否已生成轨道

        protected override void Start()
        {
            base.Start();
            startPos = transform.position;
            launchHeight = transform.position.y;   // 记录发射高度
            elapsedTime = 0f;
        }

        protected override void OnRealTimeUpdate()
        {
            base.OnRealTimeUpdate();
            
            // 按方向移动
            if (direction != Vector2.zero)
            {
                transform.Translate(direction * speed * Time.deltaTime);
            }

            // 累计时间，达到指定时间后生成轨道
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeToCreateTrack && !spawned)
            {
                CreateTrack();
                spawned = true;
            }
        }

        /// <summary>
        /// 击中目标时的处理
        /// </summary>
        protected override void OnHitTarget(Collider2D other)
        {
            // 冰子弹击中目标时直接销毁，不爆炸
            DestroyBullet();
        }

        /// <summary>
        /// 设置子弹方向
        /// </summary>
        public override void SetDirection(Vector2 dir)
        {
            direction = dir.normalized;
        }

        /// <summary>
        /// 设置子弹速度
        /// </summary>
        public override void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        /// <summary>
        /// 创建轨道
        /// </summary>
        private void CreateTrack()
        {
            if (trackPrefab == null) return;

            // 水平轨道生成，X 坐标 = 子弹当前位置，Y 坐标 = 发射高度
            Vector2 trackPos = new Vector2(transform.position.x, launchHeight);

            Instantiate(trackPrefab, trackPos, Quaternion.identity);

            Debug.Log("生成轨道 at: " + trackPos);
        }

        /// <summary>
        /// 重写爆炸方法，冰子弹不爆炸，只销毁
        /// </summary>
        protected override void Explode()
        {
            // 冰子弹不爆炸，直接销毁
            DestroyBullet();
        }

        /// <summary>
        /// 绘制轨道预览
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (trackPrefab == null) return;

            Vector2 trackPos = new Vector2(transform.position.x, launchHeight);
            Vector2 size = trackPrefab.GetComponent<BoxCollider2D>().size;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(trackPos, size);
        }
    }
}