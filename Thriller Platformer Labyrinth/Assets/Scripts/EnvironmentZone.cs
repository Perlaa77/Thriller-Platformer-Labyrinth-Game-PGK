using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnvironmentZone : MonoBehaviour
{
    [Header("Ustawienia Strefy")]
    [Tooltip("Wartoœci dodatnie zmniejszaj¹ strach (np. œwiat³o = 15). Wartoœci ujemne przyspieszaj¹ strach (np. bardzo mroczna mg³a = -10).")]
    public float safetyMultiplier = 10f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FearManager manager = other.GetComponent<FearManager>();
            if (manager != null)
                manager.AddZoneModifier(safetyMultiplier);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FearManager manager = other.GetComponent<FearManager>();
            if (manager != null)
                manager.AddZoneModifier(-safetyMultiplier); // Odejmujemy wp³yw strefy przy wyjœciu
        }
    }
}