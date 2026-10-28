using UnityEngine;

public class Door : MonoBehaviour
{
    private CameraController cameraController;
    private Rigidbody2D player;

    private Vector2 direction = new(1, 0);
    private static readonly float[] doorDistances =
    {
        3.7f, //top<->bottom
        3.2f //right<->left
    };
    private float distance = 0;
    public int roomParity = 0;
    private static readonly Vector2Int[] doorDirections =
    {
        new(0, 1), //top
        new(1, 0), //right
        new(0, -1), //bottom
        new(-1, 0) //left
    };

    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    public void Init(int parity, int dir, float padding)
    {
        this.roomParity = parity;
        this.direction = doorDirections[dir];
        this.distance = doorDistances[dir % 2] + padding;
    }

    void Update()
    {
        if ((player != null) && Input.GetKeyDown(KeyCode.DownArrow))
        {
            player.linearVelocity = Vector2.zero;
            player.position = (Vector2)transform.position + direction.normalized * distance;
            cameraController.MoveCamera(Vector2Int.RoundToInt(direction));
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
