using UnityEngine;

public class VineBullet : MonoBehaviour
{
    [Header("子弹设置")]
    public float speed = 15f;
    public float lifeTime = 5f;
    
    [Header("方块预制体")]
    public GameObject blockPrefab; // 生成的方块预制体
    
    [Header("方块设置")]
    public float blockSize = 10f; // 方块大小
    public float blockRiseSpeed = 5f; // 方块升起速度
    public float blockRiseHeight = 2f; // 方块升起高度
    public float blockHorizontalSpeed = 8f; // 方块横向移动速度
    public float blockLifetime = 10f; // 方块存在时间
    
    [Header("方块生成设置")]
    public float verticalOffset = 0.5f; // 垂直方块向下偏移
    public float horizontalOffset = 0.3f; // 横向方块往墙里偏移
    public float additionalRiseHeight = 1f; // 额外上升高度
    
    [Header("顶飞设置")]
    public float launchForce = 15f; // 顶飞物体的力度
    public float launchRadius = 2f; // 顶飞影响半径
    
    private Vector2 direction;
    private Rigidbody2D rb;
    private float elapsedTime = 100f;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        elapsedTime = 0f;
        // 设置生命周期
        Destroy(gameObject, lifeTime);
    }
    
    void Update()
    {
        elapsedTime += Time.deltaTime;
        
        // 如果没有刚体，使用Transform移动
        if (rb == null)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
    
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") return;
        
        // 检查是否撞击到Ground层（包括地面和墙壁）
        if (IsGroundLayer(other))
        {
            // 判断撞击的是地面还是墙壁
            bool isWall = IsWall(other);

            
            if (isWall)
            {
                Debug.Log("创建横向方块");
                // 撞击墙壁 - 横向冒出方块
                CreateHorizontalBlock();
            }
            else
            {
                Debug.Log("创建垂直方块");
                // 撞击地面 - 升起方块
                CreateVerticalBlock();
            }
        }
        
        // 销毁子弹
        Destroy(gameObject);
    }
    


    bool IsGroundLayer(Collider2D collider)
    {
        // 检查是否是Ground层（包括地面和墙壁）
        return collider.gameObject.layer == LayerMask.NameToLayer("Ground");
    }
    
    bool IsWall(Collider2D collider)
    {
        // 方法4: 通过碰撞点的法向量判断（适用于Tilemap）
        Vector2 hitPoint = collider.ClosestPoint(transform.position);
        Vector2 hitNormal = ((Vector2)transform.position - hitPoint).normalized;
        

        
        // 如果法向量主要水平（X轴分量大于Y轴分量），认为是墙壁
        if (Mathf.Abs(hitNormal.x) > Mathf.Abs(hitNormal.y))
        {

            return true;
        }

        else
        {
            return false;
        }
        
    }
    
 void CreateVerticalBlock()
{
    if (blockPrefab == null) return;
    
    // 在子弹位置下方创建方块（更靠下）
    Vector3 blockPosition = transform.position + Vector3.down * verticalOffset;
    GameObject block = Instantiate(blockPrefab, blockPosition, Quaternion.identity);
    
    // 设置方块大小
    block.transform.localScale = Vector3.one * blockSize;
    
    // 禁用碰撞器（升起过程中没有碰撞）
    Collider2D blockCollider = block.GetComponent<Collider2D>();
    if (blockCollider != null)
    {
        blockCollider.enabled = false;
    }
    
    // 立即顶飞周围的物体（在生成的那一刻）
    LaunchObjectsAround(blockPosition);
    
    // 给方块一个向上的初始力
    Rigidbody2D blockRb = block.GetComponent<Rigidbody2D>();
    if (blockRb != null)
    {
        blockRb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
    }
    
    // 添加方块升起行为（垂直方向）
    BlockRiser riser = block.AddComponent<BlockRiser>();
    riser.Initialize(blockRiseSpeed, blockRiseHeight + additionalRiseHeight, launchForce, launchRadius, blockLifetime);
}

void CreateHorizontalBlock()
{
    if (blockPrefab == null) return;
    
    // 计算横向冒出方向（与飞行方向X轴相反）
    Vector3 blockDirection = new Vector3(-direction.x, 0, 0).normalized;
    
    // 在子弹位置往墙里面创建方块（更靠里）
    Vector3 blockPosition = transform.position + blockDirection * horizontalOffset;
    GameObject block = Instantiate(blockPrefab, blockPosition, Quaternion.identity);
    
    // 设置方块大小
    block.transform.localScale = Vector3.one * blockSize;
    
    // 旋转方块90度（横向）
    block.transform.rotation = Quaternion.Euler(0, 0, 90);
    
    // 禁用碰撞器（升起过程中没有碰撞）
    Collider2D blockCollider = block.GetComponent<Collider2D>();
    if (blockCollider != null)
    {
        blockCollider.enabled = false;
    }
    
    // 立即顶飞周围的物体（在生成的那一刻）
    LaunchObjectsAround(blockPosition, blockDirection);
    
    // 给方块一个横向的初始力（而不是向上）
    Rigidbody2D blockRb = block.GetComponent<Rigidbody2D>();
    if (blockRb != null)
    {
        blockRb.AddForce((Vector2)blockDirection * launchForce, ForceMode2D.Impulse);
    }
    
    // 使用横向移动行为
    BlockRiser riser = block.AddComponent<BlockRiser>();
    riser.Initialize(blockRiseSpeed, blockRiseHeight + additionalRiseHeight, launchForce, launchRadius, blockLifetime);
}

// 新增方法：立即顶飞周围的物体
void LaunchObjectsAround(Vector3 position, Vector3 blockDirection = default)
{
    // 检测周围的物体并顶飞
    Collider2D[] objectsAround = Physics2D.OverlapCircleAll(position, launchRadius);
    
    foreach (Collider2D obj in objectsAround)
    {
        if (obj.gameObject.tag == "Player")
        {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 计算顶飞方向（从方块位置指向物体）
                Vector2 launchDirection = ((Vector2)obj.transform.position - (Vector2)position).normalized;
                
                // 如果是垂直方块（默认），主要向上顶飞
                if (blockDirection == default && Vector3.Dot(launchDirection, Vector2.up) > 0)
                {
                    launchDirection = Vector2.up;
                }
                // 如果是横向方块，保持原来的方向或者使用方块方向
                else if (blockDirection != default)
                {
                    // 保持径向顶飞，或者使用方块移动方向
                    // launchDirection = (Vector2)blockDirection; // 如果想统一横向顶飞
                }
                
                // 完全重设速度，确保物体被正确顶飞
                Vector2 newVelocity = launchDirection * launchForce * 0.1f;
                rb.linearVelocity = newVelocity;
                
                // 应用额外的顶飞力
                rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
            }
        }
    }
}

}
