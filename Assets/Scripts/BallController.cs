using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private Vector2 initVelocity;
    [SerializeField]
    private Vector3 initPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initPosition = transform.position;
    }

    private void Start()
    {
        rb.velocity = initVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);
        transform.position = initPosition;
        rb.velocity = Vector2.zero;
    }
}
