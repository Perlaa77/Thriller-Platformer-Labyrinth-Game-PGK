using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private CameraController cameraController;
    private Labyrinth game;
    private Rigidbody2D player;
    public int ending = 0; // 0 sprawdzenie instrukcji, 1 wyjœcie z porwanym, 2 wyjœcie bez porwanego bo po nim
    // dodaæ obiekt z tekstem i umieœciæ na lewo od startu w kodzie Labyrinth

    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        game = GameObject.FindWithTag("Game").GetComponent<Labyrinth>();
    }

    // Sprawdzenie czy gracz chce przejœæ przez drzwi
    void Update()
    {
        if ((player != null) && Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (game.inMenu)
            {
                game.inMenu = false;
                player.GetComponent<MovementController>().enabled = true;
                cameraController.MoveCamera(Vector2Int.RoundToInt(new Vector2Int(1, 0)));
            }
            else
            {
                game.ResetGame();
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
