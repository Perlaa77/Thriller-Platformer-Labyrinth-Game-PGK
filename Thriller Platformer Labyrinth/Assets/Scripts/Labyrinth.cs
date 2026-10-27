using UnityEngine;

public class Labyrinth : MonoBehaviour
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    private static readonly Vector2 roomSize = new(19.2f, 10.8f);
    [SerializeField] private float padding = 4f;
    [SerializeField] private Room roomPrefab;
    private Room[,] labyrinthGrid;

    private bool[,] alreadyInMaze;
    private int[,] directionOfStepInWalk;

    private static readonly Vector2Int[] connectionDirections =
    {
        new(0, 1), //top
        new(1, 0), //right
        new(0, -1), //bottom
        new(-1, 0) //left
    };

    void Start()
    {
        if ((rows == 0) || (columns == 0))
        {
            Debug.Log($"Either rows or columns are set to 0.");
            return;
        }
            labyrinthGrid = new Room[columns, rows];

        GenerateLabyrinthMap();

        alreadyInMaze = new bool[columns, rows];
        directionOfStepInWalk= new int[columns, rows];
        alreadyInMaze[Random.Range(0, columns), Random.Range(0, rows)] = true;

        GenerateConnections();
    }

    void GenerateLabyrinthMap()
    {
        for(int row = 0; row < rows; row++)
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

    // Wilson's algorithm
    void GenerateConnections()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
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
    }

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
