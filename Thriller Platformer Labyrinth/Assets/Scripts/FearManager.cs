using UnityEngine;

public class FearManager : MonoBehaviour
{
    public float currentFear = 0f;
    public float maxFear = 100f;

    // Bazowe tempo w neutralnej strefie (np. powolny wzrost w zwyk³ym mroku)
    public float baseFearRate = 5f;

    // Aktualna suma modyfikatorów ze wszystkich stref
    private float currentZoneModifier = 0f;

    void Update()
    {
        // Jeœli modifier jest ujemny (œwiat³o/bezpieczeñstwo), strach spada. 
        // Jeœli dodatni lub zero (mrok), strach roœnie w tempie bazowym.
        float finalFearRate = baseFearRate - currentZoneModifier;

        currentFear += finalFearRate * Time.deltaTime;
        currentFear = Mathf.Clamp(currentFear, 0, maxFear);
    }

    public void AddInstantFear(float amount)
    {
        currentFear = Mathf.Clamp(currentFear + amount, 0, maxFear);
    }

    // Wywo³ywane przez strefy przy wejœciu/wyjœciu
    public void AddZoneModifier(float modifierAmount)
    {
        currentZoneModifier += modifierAmount;
    }
}