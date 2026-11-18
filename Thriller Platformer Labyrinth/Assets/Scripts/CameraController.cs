using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float transitionTime = 0.4f;
    private Vector3 targetPosition; // Docelowa pozycja
    private Vector3 velocity; // Prêdkoœæ przemieszczenia kamery
    private Vector2 roomSpacingBase = new(19.2f, 10.8f); // Ustalone podstawowe odleg³oœci przesuniêcia
    private Vector2 roomSpacing; // Odleg³oœæ przesuniêcia kamery, z wliczon¹ przestrzeni¹ pomiêdzy pokojami

    void Start()
    {
        targetPosition = transform.position;
    }

    public void SetSpacing(float padding)
    {
        roomSpacing.x = roomSpacingBase.x + padding;
        roomSpacing.y = roomSpacingBase.y + padding;
    }

    // Wychodzenie z gry
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if (UNITY_EDITOR)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
            #else
            {
                Application.Quit();
            }
            #endif
        }
    }

    // Aktywne przesuwanie kamery do wyznaczonego celu
    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, transitionTime);
    }

    // Wyznaczenie nowego celu za pomoc¹ kierunku
    public void MoveCamera(Vector2Int direction)
    {
        targetPosition += (Vector3)Vector2.Scale(direction, roomSpacing);
    }

    // Nag³e przeniesienie kamery
    public void SnapCamera(Vector3 pos)
    {
        Vector3 position = new Vector3(pos.x, pos.y, transform.position.z);
        targetPosition = position;
        transform.position = position;
        velocity = Vector3.zero;
    }
}
