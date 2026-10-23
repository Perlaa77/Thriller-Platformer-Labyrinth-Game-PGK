using UnityEngine;

public class Room : MonoBehaviour
{
    [field: SerializeField] public int row { get; private set; }
    [field: SerializeField] public int column { get; private set; }

    [field: SerializeField] public bool connectTop { get; set; }
    [field: SerializeField] public bool connectRight { get; set; }
    [field: SerializeField] public bool connectBottom { get; set; }
    [field: SerializeField] public bool connectLeft { get; set; }

    public int parity => (row + column) % 2;



    public void Init(int r, int c)
    {
        this.row = r;
        this.column = c;
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
    }

    public void DisconnectAll()
    {
        this.connectTop = false;
        this.connectRight = false;
        this.connectBottom = false;
        this.connectLeft = false;
    }

    //public void AddDoors()
    //{

    //}
}
