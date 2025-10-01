using UnityEngine;
using MoreMountains.Feedbacks;
public class VineBlock : MonoBehaviour
{

    public float lifetime = 10f;
    public MMF_Player OnSpawnEffect;
    public MMF_Player OnDestroyEffect;
    public PhysicsMaterial2D[] physicsMaterials;
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private BoxCollider2D col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        if (OnSpawnEffect != null)
        {
            OnSpawnEffect.PlayFeedbacks();
        }
        if (lifetime > 0)
        {
            Invoke("DestroyObject", lifetime);
        }
    }


void OnTriggerEnter2D(Collider2D other)
{
    if(other.gameObject.tag == "Ice")
    {
        gameObject.tag = "Ice";
        col.sharedMaterial = physicsMaterials[1];
        spriteRenderer.color = new Color(0,1,1);
    }
}



    void DestroyObject()
    {
        if (OnDestroyEffect != null)
        {
            OnDestroyEffect.PlayFeedbacks();
        }
        Destroy(gameObject);
    }




}
