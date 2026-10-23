using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float distance;

    private Rigidbody2D player;

    public void Init(Vector2 dir, float dist)
    {
        this.direction = dir;
        this.distance = dist;
    }

    void Update()
    {
        if ((player != null) && Input.GetKeyDown(KeyCode.DownArrow))
        {
            player.linearVelocity = Vector2.zero;
            player.position = (Vector2)transform.position + direction.normalized * distance;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.attachedRigidbody;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(direction.normalized * distance));
        Gizmos.DrawWireSphere(transform.position + (Vector3)(direction.normalized * distance), 0.4f);
    }
}
