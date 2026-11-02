using UnityEngine;

public class FearManager : MonoBehaviour
{
    public float currentFear = 0f;
    public float maxFear = 100f;
    public float fearIncreaseRate = 5f; // Wzrost na sekundê w mroku
    public float fearDecreaseRate = 15f; // Spadek na sekundê w œwietle

    private int activeLightZones = 0; // Licznik nak³adaj¹cych siê stref

    void Update()
    {
        if (activeLightZones == 0)
            currentFear += fearIncreaseRate * Time.deltaTime;
        else
            currentFear -= fearDecreaseRate * Time.deltaTime;

        currentFear = Mathf.Clamp(currentFear, 0, maxFear);
    }

    // Do wywo³ywania z zewn¹trz przez eventy (np. nag³y skok strachu)
    public void AddInstantFear(float amount)
    {
        currentFear = Mathf.Clamp(currentFear + amount, 0, maxFear);
    }

    public void ModifyLightZoneCount(int amount)
    {
        activeLightZones += amount;
    }
}