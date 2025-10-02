using UnityEngine;
using Unity.Cinemachine;

public class CM_Control : MonoBehaviour
{
    [Header("Cinemachine设置")]
    public CinemachineCamera virtualCamera;
    public CinemachinePositionComposer positionComposer;
    
    [Header("镜头跟随设置")]
    public float followSpeed = 2f; // 镜头跟随速度
    public float maxOffset = 3f; // 最大偏移距离
    public float speedMultiplier = 0.5f; // 速度倍数，控制镜头移动的敏感度
    
    [Header("平滑设置")]
    public float smoothTime = 0.3f; // 平滑时间
    
    private Transform player;
    private Rigidbody2D playerRb;
    private Vector2 originalTargetOffset;
    private Vector2 targetOffset;
    private Vector2 velocity = Vector2.zero;
    
    void Start()
    {
        // 获取玩家引用
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody2D>();
        }
        
        // 记录原始的target offset
        if (positionComposer != null)
        {
            originalTargetOffset = positionComposer.TargetOffset;
        }
    }

    void Update()
    {
        if (player == null || positionComposer == null || playerRb == null) return;
        
        // 计算镜头偏移
        CalculateCameraOffset();
        
        // 应用镜头偏移
        ApplyCameraOffset();
    }
    
    void CalculateCameraOffset()
    {
        // 获取玩家水平速度
        float horizontalVelocity = playerRb.linearVelocity.x;
        
        // 根据速度计算目标偏移
        // 玩家往左移动时，镜头往右拉（负速度对应正偏移）
        // 玩家往右移动时，镜头往左拉（正速度对应负偏移）
        float targetXOffset = originalTargetOffset.x + (horizontalVelocity * speedMultiplier);
        
        // 限制偏移范围
        targetXOffset = Mathf.Clamp(targetXOffset, originalTargetOffset.x - maxOffset, originalTargetOffset.x + maxOffset);
        
        // 设置目标偏移
        targetOffset = new Vector2(targetXOffset, originalTargetOffset.y);
    }
    
    void ApplyCameraOffset()
    {
        // 平滑移动到目标偏移
        Vector2 currentOffset = positionComposer.TargetOffset;
        Vector2 newOffset = Vector2.SmoothDamp(currentOffset, targetOffset, ref velocity, smoothTime);
        
        // 应用新的偏移
        positionComposer.TargetOffset = newOffset;
    }
    
    // 调试用：在Scene视图中显示偏移信息
    void OnDrawGizmosSelected()
    {
        if (player != null && positionComposer != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(player.position + (Vector3)originalTargetOffset, Vector3.one * 0.5f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(player.position + (Vector3)targetOffset, Vector3.one * 0.3f);
        }
    }
}
