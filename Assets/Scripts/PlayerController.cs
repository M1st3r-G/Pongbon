using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //InnerComponentRefrence
    private Rigidbody2D rb;
    //Outer Params
    [SerializeField] private static float speed = 3;    //Static, damit für beide Spieler Gleich
    [SerializeField] private bool isPlayer1;

    private void Awake()
    {
        // Initialisieren der InnerComponentRefrences
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Wenn Spieler 1, Player1 Axe auswerten, sonst Player2 und dementsprechend Bewegen
        rb.velocity = Input.GetAxisRaw(isPlayer1 ? "Player1" : "Player2") * speed * Vector3.up;
    }
}
