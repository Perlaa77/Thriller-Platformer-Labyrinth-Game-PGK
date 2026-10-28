using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    [field: SerializeField] public int row { get; private set; }
    [field: SerializeField] public int column { get; private set; }

    [field: SerializeField] public bool connectTop { get; set; }
    [field: SerializeField] public bool connectRight { get; set; }
    [field: SerializeField] public bool connectBottom { get; set; }
    [field: SerializeField] public bool connectLeft { get; set; }

    public int parity => (row + column) % 2;
    private float roomPadding;

    [SerializeField] private Door doorPrefab;
    private List<Door> doors = new();
    private static readonly Vector2[] doorPositions =
    {
        new(-8f, 1.8f), //top-left
        new(8f, 1.8f), //top-right
        new(8f, -5.3f), //bottom-right
        new(-8f, -5.3f) //bottom-left
    };

    public void Init(int c, int r, float p)
    {
        this.row = r;
        this.column = c;
        this.roomPadding = p;
    }

    public void Connect(int direction)
    {
        switch(direction)
        {
            case 0: connectTop = true; break;
            case 1: connectRight = true; break;
            case 2: connectBottom = true; break;
            case 3: connectLeft = true; break;
        }
        Vector2 doorPosition = doorPositions[(direction + parity) % 4];
        Door door = Instantiate(doorPrefab, transform);
        door.transform.localPosition = doorPosition;
        door.Init(parity, direction, roomPadding);
        doors.Add(door);
    }

    public void DisconnectAll()
    {
        this.connectTop = false;
        this.connectRight = false;
        this.connectBottom = false;
        this.connectLeft = false;

        foreach(Door door in doors)
        {
            Destroy(door.gameObject);
        }
        doors.Clear();
    }
}
