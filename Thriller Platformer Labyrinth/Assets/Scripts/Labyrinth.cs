using System.Security.Cryptography;
using UnityEngine;

public class Labyrinth : MonoBehaviour
{
    private CameraController cameraController;
    private Rigidbody2D player;
    private Rigidbody2D second;

    [SerializeField] private int rows;
    [SerializeField] private int columns;
    private static readonly Vector2 roomSize = new(19.2f, 10.8f); // Ustalona wielkoœæ pokoju - zgodna z prefabem i rozdzielczoœci¹ typowego ekranu
    [SerializeField] private float padding = 4f; // Przestrzeñ pomiêdzy pokojami
    [SerializeField] private Room roomPrefab;
    [SerializeField] private Room startRoomPrefab;
    [SerializeField] private Room midpointRoomPrefab;
    private Room[,] labyrinthGrid; // Siatka pokojów

    private bool[,] alreadyInMaze; // Siatka z info czy dany pokój jest ju¿ czêœci¹ labiryntu
    private int[,] directionOfStepInWalk; // Siatka z info w którym kierunku poszed³ algorytm z danego pokoju

    private static readonly Vector2Int[] connectionDirections = // Ustalone info o kierunkach po³¹czeñ
    {
        new(0, 1), //top
        new(1, 0), //right
        new(0, -1), //bottom
        new(-1, 0) //left
    };


    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        second = GameObject.FindWithTag("Second").GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        cameraController.SetSpacing(padding); // Ustawienie odleg³oœci do przesuwania kamery

        if ((rows == 0) || (columns == 0))
        {
            Debug.Log($"Either rows or columns are set to 0.");
            return;
        }

        GenerateLabyrinthMap(); // Wygenerowanie pokojów w labiryncie

        GenerateConnections(); // Ustalenie po³¹czeñ pokojów w labiryncie

        AddSpecialRooms();
    }

    // Wygenerowanie pokojów w labiryncie
    void GenerateLabyrinthMap()
    {
        labyrinthGrid = new Room[columns, rows];

        for (int row = 0; row < rows; row++)
        {
            for(int col = 0; col < columns; col++)
            {
                Vector3 position = transform.position + new Vector3(col * (roomSize.x + padding), row * (roomSize.y + padding), 0);
                Room room = Instantiate(roomPrefab, position, Quaternion.identity, transform);
                room.Init(col, row, padding);
                labyrinthGrid[col, row] = room;
                room.name = $"Room {row * columns + col + 1}: ({col + 1},{row + 1})";
            }
        }
    }

    // Ustalenie po³¹czeñ pokojów w labiryncie - algorytm Wilsona
    void GenerateConnections()
    {
        alreadyInMaze = new bool[columns, rows];
        directionOfStepInWalk = new int[columns, rows];
        alreadyInMaze[Random.Range(0, columns), Random.Range(0, rows)] = true;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Przejœcie teoretyczne - wyznaczenie mo¿liwych po³¹czeñ
                Vector2Int currentRoom = new Vector2Int(col, row);
                while (!alreadyInMaze[currentRoom.x, currentRoom.y])
                {
                    int direction = Random.Range(0, 4);
                    Vector2Int nextRoom = currentRoom + connectionDirections[direction];
                    if ((nextRoom.x < 0) || (nextRoom.y < 0) || (nextRoom.x >= columns) || (nextRoom.y >= rows))
                    {
                        continue;
                    }
                    directionOfStepInWalk[currentRoom.x, currentRoom.y] = direction;
                    currentRoom = nextRoom;

                }

                // Faktyczne po³¹czenie wraz z dodniem drzwi w pokojach
                currentRoom = new Vector2Int(col, row);
                while (!alreadyInMaze[currentRoom.x, currentRoom.y])
                {
                    alreadyInMaze[currentRoom.x, currentRoom.y] = true;
                    int direction = directionOfStepInWalk[currentRoom.x, currentRoom.y];
                    Vector2Int nextRoom = currentRoom + connectionDirections[direction];
                    labyrinthGrid[currentRoom.x, currentRoom.y].Connect(direction);
                    labyrinthGrid[nextRoom.x, nextRoom.y].Connect((direction + 2) % 4);
                    currentRoom = nextRoom;

                }
            }
        }

        // Dodanie po³¹czeñ z labiryntu do pokoi specjalnych
        labyrinthGrid[0, rows - 1].Connect(3);
        labyrinthGrid[columns - 1, 0].Connect(1);
    }

    // Dodanie pokojów specjalnych oraz poprawne ustawienie gracza, porwanego i kamery
    void AddSpecialRooms()
    {
        // Pokój startowy / koñcowy
        Vector3 positionStartRoom = transform.position + new Vector3(-1 * (roomSize.x + padding), (rows - 1) * (roomSize.y + padding), 0);
        Room startRoom = Instantiate(startRoomPrefab, positionStartRoom, Quaternion.identity, transform);
        startRoom.Init(-1, rows - 1, padding, true);
        startRoom.name = $"Starting/Ending Room: ({-1},{rows - 1})";
        startRoom.Connect(1);

        // Pokój z zak³adnikiem
        Vector3 positionMidRoom = transform.position + new Vector3(columns * (roomSize.x + padding), 0, 0);
        Room midRoom = Instantiate(midpointRoomPrefab, positionMidRoom, Quaternion.identity, transform);
        midRoom.Init(columns, 0, padding, true);
        midRoom.name = $"Midpoint Room: ({columns},{0})";
        midRoom.Connect(3);

        player.position = (Vector2)positionStartRoom + new Vector2(-5.6f, -1.3f);
        player.linearVelocity = Vector2.zero;

        second.position = (Vector2)positionMidRoom + new Vector2(5.6f, -1.5f);
        second.linearVelocity = Vector2.zero;

        cameraController.SnapCamera(positionStartRoom);
    }

    // Reset labityntu - nowe po³¹czenia
    public void ResetLabyrinth()
    {
        foreach (Room room in labyrinthGrid)
        {
            room.DisconnectAll();
        }
        GenerateConnections();
    }

    // Wizualizacja po³¹czeñ pokojów w labiryncie
    //private void OnDrawGizmos()
    //{
    //    if (labyrinthGrid == null)
    //    {
    //        return;
    //    }
    //    Gizmos.color = Color.blue;

    //    for (int row = 0; row < rows; row++)
    //    {
    //        for (int col = 0; col < columns; col++)
    //        {
    //            if (labyrinthGrid[col, row].connectTop)
    //            {
    //                Gizmos.DrawLine(labyrinthGrid[col, row].transform.position, labyrinthGrid[col, row + 1].transform.position);
    //            }
    //            if (labyrinthGrid[col, row].connectRight)
    //            {
    //                Gizmos.DrawLine(labyrinthGrid[col, row].transform.position, labyrinthGrid[col + 1, row].transform.position);
    //            }
    //            if (labyrinthGrid[col, row].connectBottom)
    //            {
    //                Gizmos.DrawLine(labyrinthGrid[col, row].transform.position, labyrinthGrid[col, row - 1].transform.position);
    //            }
    //            if (labyrinthGrid[col, row].connectLeft)
    //            {
    //                Gizmos.DrawLine(labyrinthGrid[col, row].transform.position, labyrinthGrid[col - 1, row].transform.position);
    //            }
    //        }
    //    }
    //}
}
