using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float initSpeed;
    private Vector3 initPosition;
    private int[] points;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initPosition = transform.position;
    }

    private void Start()
    {
        StartCoroutine(Restart(
            Random.Range(-1, 1) > 0 ? -1 : 1,
            initSpeed
            )) ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.position = initPosition;
        int dir = (int)Mathf.Sign(rb.velocity.x);
        StartCoroutine(Restart(dir, rb.velocity.magnitude));
        rb.velocity = Vector2.zero;

        if(dir == 1)
        {
            points[0] += 1;
        }
        else
        {
            points[1] += 1;
        }
        print($"Neuer Punktestand: Spieler 1 {points[0]} | Spieler 2 {points[1]}");
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
