using System;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class Labyrinth : MonoBehaviour
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private Vector2 roomSize;
    [SerializeField] private Room roomPrefab;
    private Room[,] labyrinthGrid;

    private bool[,] alreadyInMaze;
    private int[,] directionOfStepInWalk;

    private static readonly Vector2Int[] connectionDirections =
    {
        new(-1, 0),
        new(0, 1),
        new(1, 0),
        new(0, -1)
    };

    void Start()
    {
        if ((rows == 0) || (columns == 0))
        {
            UnityEngine.Debug.Log($"Either rows or columns are set to 0.");
            return;
        }
            labyrinthGrid = new Room[rows, columns];

        GenerateLabyrinthMap();

        alreadyInMaze = new bool[rows, columns];
        directionOfStepInWalk= new int[rows, columns];
        alreadyInMaze[UnityEngine.Random.Range(0, rows), UnityEngine.Random.Range(0, columns)] = true;

        GenerateConnections();
    }

    void GenerateLabyrinthMap()
    {
        for(int row = 0; row < rows; row++)
        {
            for(int col = 0; col < columns; col++)
            {
                Vector3 position = transform.position + new Vector3(col*roomSize.x, -row*roomSize.y, 0);
                Room room = Instantiate(roomPrefab, position, Quaternion.identity, transform);
                room.Init(row, col);
                labyrinthGrid[row, col] = room;
                room.name = $"Room {row*columns+col+1}: ({row+1},{col+1})";
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
                Vector2Int currentRoom = new Vector2Int(row, col);
                while (!alreadyInMaze[currentRoom.x, currentRoom.y])
                {
                    int direction = UnityEngine.Random.Range(0, 4);
                    Vector2Int nextRoom = currentRoom + connectionDirections[direction];
                    if ((nextRoom.x < 0) || (nextRoom.y < 0) || (nextRoom.x >= rows) || (nextRoom.y >= columns))
                    {
                        continue;
                    }
                    directionOfStepInWalk[currentRoom.x, currentRoom.y] = direction;
                    currentRoom = nextRoom;

                }

                currentRoom = new Vector2Int(row, col);
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

    private void OnDrawGizmos()
    {
        if (labyrinthGrid == null)
        {
            return;
        }
        Gizmos.color = Color.green;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (labyrinthGrid[row, col].connectTop)
                {
                    Gizmos.DrawLine(labyrinthGrid[row, col].transform.position, labyrinthGrid[row - 1, col].transform.position);
                }
                if (labyrinthGrid[row, col].connectRight)
                {
                    Gizmos.DrawLine(labyrinthGrid[row, col].transform.position, labyrinthGrid[row, col + 1].transform.position);
                }
                if (labyrinthGrid[row, col].connectBottom)
                {
                    Gizmos.DrawLine(labyrinthGrid[row, col].transform.position, labyrinthGrid[row + 1, col].transform.position);
                }
                if (labyrinthGrid[row, col].connectLeft)
                {
                    Gizmos.DrawLine(labyrinthGrid[row, col].transform.position, labyrinthGrid[row, col - 1].transform.position);
                }
            }
        }
    }
}
