using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    //OuterComponentRefrence
    [SerializeField] private TextMeshProUGUI p1;
    [SerializeField] private TextMeshProUGUI p2;
    [SerializeField] private TextMeshProUGUI countdown;
    //InnerComponentRefrence
    private Rigidbody2D rb;

    //OuterParams
    [SerializeField] private float initSpeed;
    [SerializeField] private float speedIncrease = 1.05f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float speedPenalty = 3f;
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
        // Startet das Spiel durch aufrufen der Restart Coroutine mit start Geschwindikeit und (ca.) 50% f�r die Startseite
        StartCoroutine(Restart(
            Random.Range(-1, 1) > 0 ? -1 : 1));       // wenn zufallz�hl > 0, Ball nach links
    }

    // Beim erreichen eines Triggers (dem linken oder rechten Endbereich)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.position = initPosition;          // Reset Position
        int dir = rb.velocity.x > 0 ? 1 : -1;       // Ermittelt die Richtung
        StartCoroutine(
            Restart(-dir));  // Restart mit gleicher geschwingikeit aber umgedrehter richtung
        
        // F�gt dem entsprechenden Spieler Punkte hinzu und Aktualisiert die Anzeige
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
        if (collision.collider.gameObject.CompareTag("Wall")) return;
        
        // Wenn es keine Wand (sondern ein Spieler ist)
        curSpeed *= speedIncrease;                      // Geschwindigkeit steigt Prozentual an
        float diff = (transform.position.y -            // Berechnet die differez der Y Koordinaten
            collision.collider.gameObject.transform.position.y);
        float percent = (diff + 1.5f) / 3f;             // Mappt die Koordinaten auf prozente von 0 (-1.5f) �ber 50 (0f) zu 100 (1.5f)
        rb.velocity = new Vector2(
            transform.position.x < 0 ? 1f : -1f,        // Ermittelt die (x-)Richtung des Vektors
            Mathf.Lerp(-1f, 1f, percent)).normalized    // Ermittelt den Abprallwinkel aus "percent"
            * curSpeed;                                 // Setzt die Geschwindigkeit auf curSpeed
    }

    /// <summary>
    /// Eine Coroutine, die den Ball anh�lt, drei Sekunden wartet,
    /// und dann den Ball erneut (zuf�llig im 90 Grad winkel) abschickt
    /// </summary>
    /// <param name="dir">Die Richtung, in die der Ball fliegt, -1 : links; 1: rechts</param>
    /// <param name="speed">Die Geschwindigkeit mit der der Ball weiterfliegt</param>
    /// <returns></returns>
    private IEnumerator Restart(int dir)
    {
        if (!(dir == -1 || dir == 1)) yield break;    // Abbrechen, wenn der Input illegal ist
         rb.velocity = Vector2.zero;             // H�lt den Ball an
        //Warten und runterz�hlen
        countdown.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        countdown.text = "Restart in 3...";
        yield return new WaitForSeconds(1f);
        countdown.text = "Restart in 2...";
        yield return new WaitForSeconds(1f);
        countdown.text = "Restart in 1...";
        yield return new WaitForSeconds(1f);
        countdown.text = "Go";
        Invoke(nameof(RemoveCountdown), 1f);    // Nach einer Sekude den Countdown entfernen.
        curSpeed = Mathf.Clamp(                 // Der Ball verlangsamt sich etwas
            curSpeed - speedPenalty,
            initSpeed,
            maxSpeed);                          
        rb.velocity = new Vector2(              // Ein neuer Vektor mit zuf�lligem Winkel zwischen -45 und 45 Grad
            dir,
            Random.Range(-1f, 1f)).normalized * curSpeed; // Anpassen der Geschwindigkeit
    }

    private void RemoveCountdown()
    {
        countdown.gameObject.SetActive(false);
    }
}
