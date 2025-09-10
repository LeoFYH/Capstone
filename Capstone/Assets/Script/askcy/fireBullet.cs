using UnityEngine;

public class fireBullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public GameObject explosionPrefab;
    public GameObject BIGexplosionPrefab;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
        {
            if (other.gameObject.GetComponent<Wall>() == null)
                if (other.gameObject.transform.tag != "Ice")
                    explode();
                else
                    iceExplode(other.gameObject);

        }
    }


    private void explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void iceExplode(GameObject other)
    {
        Instantiate(BIGexplosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        Destroy(other.gameObject);
    }
}
