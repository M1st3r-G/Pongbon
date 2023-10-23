using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private readonly float speed = 3;
    public bool isPlayer1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Input.GetAxisRaw(isPlayer1 ? "Player1" : "Player2") * speed * Vector3.up;
    }
}
