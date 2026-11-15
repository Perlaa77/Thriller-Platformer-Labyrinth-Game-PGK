using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FearLightEffect : MonoBehaviour
{
    [Header("Referencje")]
    public FearManager fearManager;
    public Light2D globalLight;

    [Header("Ustawienia Jasnoœci")]
    [Tooltip("Jasnoœæ Global Light przy 0% strachu")]
    public float normalIntensity = 0.8f;

    [Tooltip("Jasnoœæ Global Light przy 100% strachu")]
    public float maxFearIntensity = 0.1f;

    [Tooltip("P³ynnoœæ przejœcia jasnoœci")]
    public float smoothSpeed = 5f;

    void Start()
    {
        if (globalLight == null)
        {
            globalLight = GetComponent<Light2D>();
        }
    }

    void Update()
    {
        if (fearManager == null || globalLight == null) return;

        // Oblicza docelow¹ jasnoœæ na podstawie procentowego poziomu strachu (0 -> 1)
        float targetIntensity = Mathf.Lerp(normalIntensity, maxFearIntensity, fearManager.FearNormalized);

        // P³ynnie zmienia intensywnoœæ œwiat³a
        globalLight.intensity = Mathf.Lerp(globalLight.intensity, targetIntensity, Time.deltaTime * smoothSpeed);
    }
}