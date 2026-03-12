using UnityEngine;

public class MonsterMoveLeft : MonoBehaviour
{
    public float speed = 3f;
    public float activeDistance = 10f; // khoảng cách để quái bắt đầu chạy

    public Transform player;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= activeDistance)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}