using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LightZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<FearManager>().ModifyLightZoneCount(1);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<FearManager>().ModifyLightZoneCount(-1);
    }
}