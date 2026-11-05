using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    [field: SerializeField] public int row { get; private set; }
    [field: SerializeField] public int column { get; private set; }

    // Informaje o po³¹czeniach z s¹siadami
    [field: SerializeField] public bool connectTop { get; set; }
    [field: SerializeField] public bool connectRight { get; set; }
    [field: SerializeField] public bool connectBottom { get; set; }
    [field: SerializeField] public bool connectLeft { get; set; }

    public int parity = 0; // Parzystoœæ dla poprawnego uk³adania drzwi
    private float roomPadding; // Przestrzeñ pomiêdzy pokojami

    [SerializeField] private Door doorPrefab;
    private List<Door> doors = new(); // Lista drzwi w pokoju
    private static readonly Vector2[] doorPositions = // Ustalone miejsca na drzwi
    {
        new(-8f, 1.8f), //top-left
        new(8f, 1.8f), //top-right
        new(8f, -5.3f), //bottom-right
        new(-8f, -5.3f) //bottom-left
    };

    [SerializeField] private LightZone spotlightPrefab;
    [SerializeField] private LightZone torchPrefab;
    [SerializeField] private int chanceForLight = 3; // Szansa na œwiat³o: jeden na ile
    private LightZone chosenLightPrefab; // Wybrany prefab œwiat³a
    private List<LightZone> lights = new(); // Lista œwiate³ w pokoju
    int guaranteedLightPosition = 0; // Gwarantowane œwiat³o
    private Vector2[] lightPositions; // Wybrana lista œwiate³
    private static readonly Vector2[] spotlightPositions = // Ustalone miejsca na œwiat³a typu spotlight
    {
        new(-8f, 5.2f),
        new(8f, 5.2f),
        new(-5f, -2.74f),
        new(5f, -2.74f)
    };
    private static readonly Vector2[] torchPositions = // Ustalone miejsca na œwiat³a typu torch
    {
        new(-2.5f, 1.5f),
        new(2.5f, 1.5f),
        new(-7.3f, -1f),
        new(7.3f, -1f),
        new(0f, -3.4f),
    };


    public void Init(int c, int r, float p, bool special = false)
    {
        this.row = r;
        this.column = c;
        this.roomPadding = p;
        parity = (row + column) % 2;

        if (!special )
        {
            SetLighting();
        }
        else
        {
            if (column == -1) // start room
            {
                LightZone light = Instantiate(spotlightPrefab, transform);
                light.transform.localPosition = new Vector2(6.1f, 1.4f);
                lights.Add(light);

                LightZone bigLight = Instantiate(spotlightPrefab, transform);
                bigLight.transform.localPosition = new Vector2(-9.3f, 5.2f);
                bigLight.transform.localRotation = Quaternion.Euler(0f, 0f, 209f);
                lights.Add(bigLight);
            }
            else // midpoint room
            {
                LightZone light = Instantiate(spotlightPrefab, transform);
                light.transform.localPosition = new Vector2(-6.3f, 1.3f);
                lights.Add(light);

                LightZone bigLight = Instantiate(spotlightPrefab, transform);
                bigLight.transform.localPosition = new Vector2(9.3f, 5.2f);
                bigLight.transform.localRotation = Quaternion.Euler(0f, 0f, 149f);
                lights.Add(bigLight);
            }
        }
    }

    // Dodanie œwiate³ do pokoju
    void SetLighting()
    {
        if (Random.Range(0, 2) == 0)
        {
            lightPositions = spotlightPositions;
            chosenLightPrefab = spotlightPrefab;
        }
        else
        {
            lightPositions = torchPositions;
            chosenLightPrefab = torchPrefab;
        }

        guaranteedLightPosition = Random.Range(0, lightPositions.Length);

        for (int i = 0; i < lightPositions.Length; i++)
        {
            if ((i != guaranteedLightPosition) && (Random.Range(0, chanceForLight) != 0))
            {
                continue;
            }

            LightZone light = Instantiate(chosenLightPrefab, transform);
            light.transform.localPosition = lightPositions[i];
            lights.Add(light);
        }
    }

    // Dodanie drzwi w odpowiednim miejscu w pokoju, zale¿nie od kierunku po³¹czenia
    public void Connect(int direction)
    {
        switch(direction)
        {
            case 0: connectTop = true; break;
            case 1: connectRight = true; break;
            case 2: connectBottom = true; break;
            case 3: connectLeft = true; break;
        }

        Door door = Instantiate(doorPrefab, transform);
        door.transform.localPosition = doorPositions[(direction + parity) % 4];
        door.Init(parity, direction, roomPadding);
        doors.Add(door);
    }

    // Funkcja do usuniêcia wszystkich drzwi z pokoju
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
