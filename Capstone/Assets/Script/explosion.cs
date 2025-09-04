using UnityEngine;

public class explosion : MonoBehaviour
{
    public float explosionForce = 10f;     // ��������
    public float duration = 0.2f;          // ��ը����ʱ��
    public LayerMask affectedLayer;        // ��Ӱ��Ķ����

    private void Start()
    {
        // �ӳ����٣���֤������
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ֻӰ��ָ����
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
