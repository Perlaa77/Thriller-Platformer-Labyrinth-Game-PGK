using UnityEngine;

public class RoomController : MonoBehaviour
{
    [field: SerializeField] public int row { get; private set; }
    [field: SerializeField] public int column { get; private set; }

    [field: SerializeField] public bool connectTop { get; set; }
    [field: SerializeField] public bool connectRight { get; set; }
    [field: SerializeField] public bool connectBotton { get; set; }
    [field: SerializeField] public bool connectLeft { get; set; }

    public int parity => (row + column) % 2;

    public void Init(int r, int c)
    {
        this.row = r;
        this.column = c;
    }

    void Update()
    {
        
    }
}
