using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    //OuterComponentRefrence
    [SerializeField] private TextMeshProUGUI p1;
    [SerializeField] private TextMeshProUGUI p2;
    //InnerComponentRefrence
    private Rigidbody2D rb;

    //OuterParams
    [SerializeField] private float initSpeed;
    [SerializeField] float pushForce = 1;
    [SerializeField] private float speedIncrease = 1.05f;
    //InnerParams
    private Vector3 initPosition;
    //Temps
    private int[] points;
    private float curSpeed;

    // Unitys Awake-Funktion, die bei Erstellung des Objektes aufgerufen wird.
    private void Awake()
    {
        //Initialisieren von InnerComponents und Temps
        points = new int[2];
        curSpeed = initSpeed;
        rb = GetComponent<Rigidbody2D>();
        initPosition = transform.position;
    }

    // Unitys Start-Funktion die vor dem ersten Frame aufgerufen wird.
    private void Start()
    {
        // Startet das Spiel durch aufrufen der Restart Coroutine mit start Geschwindikeit und (ca.) 50% für die Startseite
        StartCoroutine(Restart(
            Random.Range(-1, 1) > 0 ? -1 : 1,       // wenn zufallzähl > 0, Ball nach links
            initSpeed                               // Start geschwindigkeit
            ));
    }

    // Beim erreichen eines Triggers (dem linken oder rechten Endbereich)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.position = initPosition;          // Reset Position
        int dir = rb.velocity.x > 0 ? 1 : -1;       // Ermittelt die Richtung
        StartCoroutine(
            Restart(-dir, rb.velocity.magnitude));  // Restart mit gleicher geschwingikeit aber umgedrehter richtung
        
        // Fügt dem entsprechenden Spieler Punkte hinzu und Aktualisiert die Anzeige
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

    // Bei einer Kollison mit einem anderen Objekt
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Wenn es eine Wand ist
        if (collision.collider.gameObject.CompareTag("Wall"))
        {
            // Mehr Geschwundikeit nach unten, damit der sich nicht lange aufhält
            rb.velocity = (rb.velocity + pushForce * (transform.position.y > 0 ? Vector2.down : Vector2.up)).normalized * curSpeed;
            return;
        }
        
        // Wenn es keine Wand (sondern ein Spieler ist)
        curSpeed *= speedIncrease;                      // Geschwindigkeit steigt Prozentual an
        float diff = (transform.position.y -            // Berechnet die differez der Y Koordinaten
            collision.collider.gameObject.transform.position.y);
        float percent = (diff + 1.5f) / 3f;             // Mappt die Koordinaten auf prozente von 0 (-1.5f) über 50 (0f) zu 100 (1.5f)
        rb.velocity = new Vector2(
            transform.position.x < 0 ? 1f : -1f,        // Ermittelt die (x-)Richtung des Vektors
            Mathf.Lerp(-1f, 1f, percent)).normalized    // Ermittelt den Abprallwinkel aus "percent"
            * curSpeed;                                 // Setzt die Geschwindigkeit auf curSpeed
    }

    /// <summary>
    /// Eine Coroutine, die den Ball anhält, drei Sekunden wartet,
    /// und dann den Ball erneut (zufällig im 90 Grad winkel) abschickt
    /// </summary>
    /// <param name="dir">Die Richtung, in die der Ball fliegt, -1 : links; 1: rechts</param>
    /// <param name="speed">Die Geschwindigkeit mit der der Ball weiterfliegt</param>
    /// <returns></returns>
    private IEnumerator Restart(int dir, float speed)
    {
        if (!(dir == -1 || dir == 1)) return     // Abbrechen, wenn der Input illegal ist
         rb.velocity = Vector2.zero;             // Hält den Ball an
        //Warten und runterzählen
        yield return new WaitForEndOfFrame();
        print("Restart in 3...");
        yield return new WaitForSeconds(1f);
        print("Restart in 2...");
        yield return new WaitForSeconds(1f);
        print("Restart in 1...");
        yield return new WaitForSeconds(1f);
        print("Go");
        curSpeed *= 0.5f;                       // Der Ball verlangsamt sich etwas
        rb.velocity = new Vector2(              // Ein neuer Vektor mit zufälligem Winkel zwischen -45 und 45 Grad
            dir,
            Random.Range(-1f, 1f)).normalized * speed; // Anpassen der Geschwindigkeit
    }
}
