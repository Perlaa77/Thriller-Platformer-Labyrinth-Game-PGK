using UnityEngine;

public class Door : MonoBehaviour
{
    private CameraController cameraController;
    private Rigidbody2D player;
    private Rigidbody2D second;

    private Vector2 direction = new(1, 0); // Kierunek, w który gracz bêdzie przeniesiony
    private static readonly float[] doorDistances = // Ustalone odleg³oœci pomiêdzy drzwiami
    {
        3.7f, //top<->bottom
        3.2f //right<->left
    };
    private float distance = 0; // Odleg³oœæ o jak¹ zostanie przeniesiony gracz
    public int roomParity = 0; // S¹siaduj¹ce pokoje maj¹ inne parzystoœci, by drzwi ³¹czy³y siê poprawnie
    private static readonly Vector2Int[] doorDirections = // Ustalone kierunki do wyboru
    {
        new(0, 1), //top
        new(1, 0), //right
        new(0, -1), //bottom
        new(-1, 0) //left
    };

    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        second = GameObject.FindWithTag("Second").GetComponent<Rigidbody2D>();
    }

    public void Init(int parity, int dir, float padding)
    {
        this.roomParity = parity;
        this.direction = doorDirections[dir];
        this.distance = doorDistances[dir % 2] + padding;
    }

    // Sprawdzenie czy gracz chce przejœæ przez drzwi
    void Update()
    {
        if ((player != null) && Input.GetKeyDown(KeyCode.DownArrow))
        {
            player.linearVelocity = Vector2.zero;
            player.position = (Vector2)transform.position + direction.normalized * distance;
            cameraController.MoveCamera(Vector2Int.RoundToInt(direction));

            if (second.GetComponent<SecondController>().enabled)
            {
                second.linearVelocity = Vector2.zero;
                second.position = (Vector2)transform.position + direction.normalized * distance;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.attachedRigidbody;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }

    // Wizualizacja dzia³ania drzwi
    private void OnDrawGizmos()
    {
        if (roomParity == 1)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direction * distance));
        Gizmos.DrawWireSphere(transform.position + (Vector3)(direction * distance), 0.4f);
    }
}