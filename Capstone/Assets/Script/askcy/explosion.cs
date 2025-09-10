using UnityEngine;

public class explosion : MonoBehaviour
{
    public float explosionForce = 10f;     // 弹飞力度
    public float duration = 0.2f;          // 爆炸存在时间
    public LayerMask affectedLayer;        // 受影响的对象层

    private void Start()
    {
        // 延迟销毁，保证触发完
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只影响指定层
        if (((1 << other.gameObject.layer) & affectedLayer) != 0)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = (other.transform.position - transform.position).normalized;
                rb.AddForce(dir * explosionForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, col.radius);
        }
    }
}
