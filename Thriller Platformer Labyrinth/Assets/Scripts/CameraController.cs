using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float transitionTime = 0.4f;
    private Vector3 targetPosition;
    private Vector3 velocity;
    private Vector2 roomSpacingBase = new(19.2f, 10.8f);
    private Vector2 roomSpacing;

    void Start()
    {
        targetPosition = transform.position;
    }

    public void SetSpacing(float padding)
    {
        roomSpacing.x = roomSpacingBase.x + padding;
        roomSpacing.y = roomSpacingBase.y + padding;
    }

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, transitionTime);
    }

    public void MoveCamera(Vector2Int direction)
    {
        targetPosition += (Vector3)Vector2.Scale(direction, roomSpacing);
    }
}
