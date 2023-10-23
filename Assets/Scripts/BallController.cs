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
        StartCoroutine(Restart(
            Random.Range(-1, 1) > 0 ? -1 : 1,
            rb.velocity.magnitude
            ));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);
        transform.position = initPosition;
        StartCoroutine(Restart(
            (int)Mathf.Sign(rb.velocity.x), 
            rb.velocity.magnitude
            ));
        rb.velocity = Vector2.zero;
    }

    private IEnumerator Restart(int dir, float speed)
    {
        print("Restart in 3...");
        yield return new WaitForSeconds(1f);
        print("Restart in 2...");
        yield return new WaitForSeconds(1f);
        print("Restart in 1...");
        yield return new WaitForSeconds(1f);
        print("Go");
        rb.velocity = new Vector2(-dir, Random.Range(-1f, 1f)).normalized * speed;
    }
}
