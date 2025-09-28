using SkateGame;
using UnityEngine;

public class ParallexManager : MonoBehaviour
{
    Rigidbody2D rb;
    Rigidbody2D playerRb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRb = FindFirstObjectByType<InputController>().GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 linearVelocity = playerRb.linearVelocity;
        rb.linearVelocity = new Vector2(linearVelocity.x / 1.20f, linearVelocity.y / 1.20f);
    }
}
