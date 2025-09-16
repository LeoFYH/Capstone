using UnityEngine;
using QFramework;

namespace SkateGame
{
    /// <summary>
    /// 子弹基类
    /// 提供通用的子弹功能，包括碰撞检测、爆炸逻辑等
    /// </summary>
    public abstract class Bullet : ViewerControllerBase
    {
        [Header("子弹基础设置")]
        public float lifeTime = 3f;                    // 子弹生命周期
        public bool destroyOnHit = true;              // 碰撞时是否销毁
        public LayerMask hitLayers = -1;              // 可碰撞的层级
        
        [Header("爆炸效果")]
        public GameObject explosionPrefab;            // 爆炸特效预制体
        
        protected float spawnTime;                    // 生成时间
        protected bool isDestroyed = false;           // 是否已销毁
        
        protected virtual void Start()
        {
            base.Start();
            spawnTime = Time.time;
        }
        
        protected override void OnRealTimeUpdate()
        {
            // 检查生命周期
            if (Time.time - spawnTime >= lifeTime)
            {
                OnLifeTimeExpired();
            }
        }
        
        /// <summary>
        /// 碰撞检测 - 子类可以重写以添加特殊逻辑
        /// </summary>
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (isDestroyed) return;
            
            // 检查是否在可碰撞层级中
            if (!IsInHitLayer(other.gameObject.layer)) return;
            
            // 检查标签过滤
            if (!ShouldHitTarget(other)) return;
            
            // 执行碰撞逻辑
            OnHitTarget(other);
        }
        
        /// <summary>
        /// 检查目标是否在可碰撞层级中
        /// </summary>
        protected virtual bool IsInHitLayer(int layer)
        {
            return (hitLayers.value & (1 << layer)) != 0;
        }
        
        /// <summary>
        /// 检查是否应该击中目标 - 子类可重写
        /// </summary>
        protected virtual bool ShouldHitTarget(Collider2D other)
        {
            // 默认不击中玩家
            return !other.CompareTag("Player");
        }
        
        /// <summary>
        /// 击中目标时的处理 - 子类必须实现
        /// </summary>
        protected abstract void OnHitTarget(Collider2D other);
        
        /// <summary>
        /// 爆炸方法 - 子类可重写
        /// </summary>
        protected virtual void Explode()
        {
            if (isDestroyed) return;
            
            // 播放爆炸特效
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            
            // 发送爆炸事件
            this.SendEvent(new BulletExplodedEvent(this));
            
            // 销毁子弹
            if (destroyOnHit)
            {
                DestroyBullet();
            }
        }
        
        /// <summary>
        /// 生命周期到期时的处理
        /// </summary>
        protected virtual void OnLifeTimeExpired()
        {
            if (isDestroyed) return;
            
            Explode();
        }
        
        /// <summary>
        /// 销毁子弹
        /// </summary>
        protected virtual void DestroyBullet()
        {
            if (isDestroyed) return;
            
            isDestroyed = true;
            Destroy(gameObject);
        }
        
        /// <summary>
        /// 设置子弹方向 - 子类可重写
        /// </summary>
        public virtual void SetDirection(Vector2 direction)
        {
            // 默认实现，子类可重写
        }
        
        /// <summary>
        /// 设置子弹速度 - 子类可重写
        /// </summary>
        public virtual void SetSpeed(float speed)
        {
            // 默认实现，子类可重写
        }
    }
    
    /// <summary>
    /// 子弹爆炸事件
    /// </summary>
    public struct BulletExplodedEvent
    {
        public Bullet bullet;
        
        public BulletExplodedEvent(Bullet bullet)
        {
            this.bullet = bullet;
        }
    }
}
