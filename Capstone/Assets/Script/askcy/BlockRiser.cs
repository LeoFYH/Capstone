using UnityEngine;

public class BlockRiser : MonoBehaviour
{
  private float riseSpeed;
    private float riseHeight;
    private float launchForce;
    private float launchRadius;
    private float lifetime;
    
    private Vector3 startPosition;
    private bool hasLaunched = false;
    private float currentHeight = 0f;
    
    public void Initialize(float speed, float height, float force, float radius, float life)
    {
        riseSpeed = speed;
        riseHeight = height;
        launchForce = force;
        launchRadius = radius;
        lifetime = life;
        
        startPosition = transform.position;
        
        // 设置生命周期
        Destroy(gameObject, lifetime);
    }
    
    void Update()
    {
        if (currentHeight < riseHeight)
        {
            // 继续升起
            currentHeight += riseSpeed * Time.deltaTime;
            transform.position = startPosition + Vector3.up * currentHeight;
            
            // 检查是否到达目标高度
            if (currentHeight >= riseHeight && !hasLaunched)
            {
                LaunchObjectsAbove();
                hasLaunched = true;
            }
        }
    }
    
    void LaunchObjectsAbove()
    {
        // 检测上方的物体并顶飞
        Collider2D[] objectsAbove = Physics2D.OverlapCircleAll(transform.position, launchRadius);
        
        foreach (Collider2D obj in objectsAbove)
        {
            if (obj.gameObject != gameObject && obj.gameObject.tag == "Player")
            {
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // 计算顶飞方向（向上）
                    Vector2 launchDirection = Vector2.up;
                    rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
                }
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, launchRadius);
    }
}
