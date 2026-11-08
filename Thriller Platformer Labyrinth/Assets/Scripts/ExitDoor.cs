using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private CameraController cameraController;
    private Rigidbody2D player;
    private bool inMenu = true;

    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    // Sprawdzenie czy gracz chce przejœæ przez drzwi
    void Update()
    {
        if ((player != null) && Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (inMenu)
            {
                inMenu= false;
                player.GetComponent<MovementController>().enabled = true;
                cameraController.MoveCamera(Vector2Int.RoundToInt(new Vector2Int(1, 0)));
            }
            else
            {
                inMenu = true;
                player.GetComponent<MovementController>().enabled = false;
                cameraController.MoveCamera(Vector2Int.RoundToInt(new Vector2Int(-1, 0)));
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
}
