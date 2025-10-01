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
            if (IsWall(other))
            {
                // 撞击墙壁 - 横向冒出方块
                CreateHorizontalBlock();
            }
            else
            {
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
        // 检查碰撞点的法向量来判断是墙壁还是地面
        Vector2 hitPoint = collider.ClosestPoint(transform.position);
        Vector2 hitNormal = ((Vector2)transform.position - hitPoint).normalized;
        
        // 如果法向量主要水平（X轴分量大于Y轴分量），认为是墙壁
        if (Mathf.Abs(hitNormal.x) > Mathf.Abs(hitNormal.y))
        {
            return true;
        }
        
        // 也可以检查是否有Wall组件作为额外判断
        Wall wall = collider.gameObject.GetComponent<Wall>();
        if (wall != null)
        {
            return true;
        }
        
        return false;
    }
    
    void CreateVerticalBlock()
    {
        if (blockPrefab == null) return;
        
        // 在子弹位置创建方块
        Vector3 blockPosition = transform.position;
        GameObject block = Instantiate(blockPrefab, blockPosition, Quaternion.identity);
        
        // 设置方块大小
        block.transform.localScale = Vector3.one * blockSize;
        
        // 添加方块升起行为
        BlockRiser riser = block.AddComponent<BlockRiser>();
        riser.Initialize(blockRiseSpeed, blockRiseHeight, launchForce, launchRadius, blockLifetime);
    }
    
    void CreateHorizontalBlock()
    {
        if (blockPrefab == null) return;
        
        // 计算横向冒出方向（与飞行方向X轴相反）
        Vector3 blockDirection = new Vector3(-direction.x, 0, 0).normalized;
        
        // 在子弹位置创建方块
        Vector3 blockPosition = transform.position;
        GameObject block = Instantiate(blockPrefab, blockPosition, Quaternion.identity);
        
        // 设置方块大小
        block.transform.localScale = Vector3.one * blockSize;
        
        // 添加方块横向移动行为
        BlockLauncher launcher = block.AddComponent<BlockLauncher>();
        launcher.Initialize(blockDirection, blockHorizontalSpeed, launchForce, launchRadius, blockLifetime);
    }
}
