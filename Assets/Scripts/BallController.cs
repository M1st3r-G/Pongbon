using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float initSpeed;
    private Vector3 initPosition;
    private int[] points;

    private float curSpeed;
    [SerializeField]
    private float speedIncrease = 1.05f;
    [SerializeField]
    private Text p1;
    [SerializeField]
    private Text p2;

    private void Awake()
    {
        points = new int[2];
        curSpeed = initSpeed;
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
            p1.text = points[0].ToString();

        }
        else
        {
            points[1] += 1;
            p2.text = points[1].ToString();
        }
    }

    private IEnumerator Restart(int dir, float speed)
    {
        yield return new WaitForEndOfFrame();
        print("Restart in 3...");
        yield return new WaitForSeconds(1f);
        print("Restart in 2...");
        yield return new WaitForSeconds(1f);
        print("Restart in 1...");
        yield return new WaitForSeconds(1f);
        print("Go");
        rb.velocity = new Vector2(-dir, Random.Range(-1f, 1f)).normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Wall")) return;
        
        curSpeed *= speedIncrease;
        float percent = (transform.position.y - collision.collider.gameObject.transform.position.y) / 1.5f;
        rb.velocity = new Vector2(
            transform.position.x < 0 ? 1f : -1f,
            Mathf.Lerp(-1f, 1f, percent)).normalized * curSpeed;
    }
}
