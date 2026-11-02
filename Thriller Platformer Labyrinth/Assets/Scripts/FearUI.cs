using UnityEngine;
using UnityEngine.UI;

public class FearUI : MonoBehaviour
{
    [Header("Referencje")]
    public Slider fearSlider;
    public FearManager playerFearManager; // Przeci¹gnij tutaj obiekt gracza

    void Start()
    {
        if (fearSlider == null || playerFearManager == null)
        {
            Debug.LogError("Brakuje przypisañ w skrypcie FearUI!");
            return;
        }

        // Dopasuj maksymaln¹ wartoœæ slidera do maksymalnego strachu w mened¿erze
        fearSlider.maxValue = playerFearManager.maxFear;
    }

    void Update()
    {
        // P³ynna aktualizacja wype³nienia
        if (playerFearManager != null && fearSlider != null)
        {
            fearSlider.value = playerFearManager.currentFear;
        }
    }
}