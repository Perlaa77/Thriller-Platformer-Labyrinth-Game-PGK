using UnityEngine;

public class MapManagerController : MonoBehaviour
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    //[SerializeField] private Room roomPrefab;
    //private Room[,] labyrinthGrid;

    void Start()
    {
        //labyrinthGrid = new Room[rows, columns];
        GenerateLabyrinthMap();
        GenerateConnections();
    }

    void Update()
    {
        
    }

    void GenerateLabyrinthMap()
    {
        //Room room = Instantiate(roomPrefab, transform.position, Quaternion.identity, transform);
        //Vector2 roomSize = room.GetComponent<SpriteRenderer>().bounds.size;
    }

    void GenerateConnections()
    {

    }
}
