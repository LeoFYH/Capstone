using UnityEngine;

public class IceBullet : MonoBehaviour
{
    public float speed = 15f;                  // 子弹速度
    public float timeToCreateTrack = 2;     // 发射后多久生成轨道（秒）
    public GameObject trackPrefab;             // 轨道Prefab

    private Vector2 startPos;
    private float launchHeight;                // 发射时高度
    private Vector2 direction;                 // 飞行方向
    public float elapsedTime = 0f;            // 已过时间
    public bool spawned;
    public GameObject TempLiftPrefab; // 临时托升平台Prefab
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        startPos = transform.position;
        launchHeight = transform.position.y;   // 记录发射高度
        elapsedTime = 0f;
    }

    void Update()
    {
        //if (direction == Vector2.zero) return;

        // 按发射方向飞行
        transform.Translate(direction * speed * Time.deltaTime);

        // 累计时间，达到指定时间则生成轨道
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= timeToCreateTrack && spawned == false)
        {

            CreateTrack();
            spawned = true;
        }
    }

    void CreateTrack()
    {
        if (trackPrefab == null) return;

        // 水平轨道生成：X 坐标 = 子弹当前位置，Y 坐标 = 发射高度
        Vector2 trackPos = new Vector2(transform.position.x, launchHeight);

        Instantiate(trackPrefab, trackPos, Quaternion.identity);

        Debug.Log("生成轨道 at: " + trackPos);
    }

    private void OnDrawGizmosSelected()
    {
        if (trackPrefab == null) return;

        Vector2 trackPos = new Vector2(transform.position.x, launchHeight);
        Vector2 size = trackPrefab.GetComponent<BoxCollider2D>().size;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(trackPos, size);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;



        Destroy(gameObject);
    }


}
