using UnityEngine;

public class IceBullet : MonoBehaviour
{
    public float speed = 15f;                  // �ӵ��ٶ�
    public float timeToCreateTrack = 2;     // ����������ɹ�����룩
    public GameObject trackPrefab;             // ���Prefab

    private Vector2 startPos;
    private float launchHeight;                // ����ʱ�߶�
    private Vector2 direction;                 // ���з���
    public float elapsedTime = 0f;            // �ѹ�ʱ��
    public bool spawned;
    public GameObject TempLiftPrefab; // ��ʱ����ƽ̨Prefab
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        startPos = transform.position;
        launchHeight = transform.position.y;   // ��¼����߶�
        elapsedTime = 0f;
    }

    void Update()
    {
        //if (direction == Vector2.zero) return;

        // �����䷽�����
        transform.Translate(direction * speed * Time.deltaTime);

        // �ۼ�ʱ�䣬�ﵽָ��ʱ�������ɹ��
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

        // ˮƽ������ɣ�X ���� = �ӵ���ǰλ�ã�Y ���� = ����߶�
        Vector2 trackPos = new Vector2(transform.position.x, launchHeight);

        Instantiate(trackPrefab, trackPos, Quaternion.identity);

        Debug.Log("���ɹ�� at: " + trackPos);
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
