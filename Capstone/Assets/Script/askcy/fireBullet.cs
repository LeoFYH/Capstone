using UnityEngine;

namespace SkateGame
{
    /// <summary>
    /// 火焰子弹
    /// 继承Bullet基类，实现火焰子弹的特殊逻辑
    /// </summary>
    public class fireBullet : Bullet
    {
        [Header("火焰子弹特殊设置")]
        public GameObject BIGexplosionPrefab;        // 大爆炸特效（击中冰块时使用）
        
        /// <summary>
        /// 击中目标时的处理
        /// </summary>
        protected override void OnHitTarget(Collider2D other)
        {
            // 检查是否击中墙壁
            if (other.gameObject.GetComponent<Wall>() == null)
            {
                // 检查是否击中冰块
                if (other.gameObject.transform.tag == "Ice")
                {
                    IceExplode(other.gameObject);
                }
                else
                {
                    // 普通爆炸
                    Explode();
                }
            }
        }
        
        /// <summary>
        /// 击中冰块时的特殊爆炸逻辑
        /// </summary>
        private void IceExplode(GameObject iceObject)
        {
            if (isDestroyed) return;
            
            // 播放大爆炸特效
            if (BIGexplosionPrefab != null)
            {
                Instantiate(BIGexplosionPrefab, transform.position, Quaternion.identity);
            }
            
            // 销毁冰块
            if (iceObject != null)
            {
                Destroy(iceObject);
            }
            
            // 销毁子弹
            DestroyBullet();
        }
        
        /// <summary>
        /// 重写爆炸方法，添加火焰子弹的特殊效果
        /// </summary>
        protected override void Explode()
        {
            // 调用基类的爆炸方法
            base.Explode();
            
            // 可以在这里添加火焰子弹特有的爆炸效果
            // 比如火焰粒子效果、音效等
        }
    }
}
