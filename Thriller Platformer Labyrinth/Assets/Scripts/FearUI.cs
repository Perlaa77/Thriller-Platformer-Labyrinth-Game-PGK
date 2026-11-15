using UnityEngine;
using UnityEngine.UI;

public class FearUI : MonoBehaviour
{
    [Header("Referencje")]
    public Slider fearSlider;
    public Slider deathSlider; // Drugi pasek (odliczanie do œmierci)
    public FearManager playerFearManager;

    void Start()
    {
        if (fearSlider == null || playerFearManager == null)
        {
            Debug.LogError("Brakuje przypisañ w skrypcie FearUI!");
            return;
        }

        fearSlider.maxValue = playerFearManager.maxFear;

        if (deathSlider != null)
        {
            deathSlider.maxValue = playerFearManager.maxDeathTime;
            deathSlider.value = 0f;
            deathSlider.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerFearManager == null) return;

        if (fearSlider != null)
        {
            fearSlider.value = playerFearManager.currentFear;
        }

        if (deathSlider != null)
        {
            deathSlider.value = playerFearManager.currentDeathTimer;

            bool shouldShowDeathBar = playerFearManager.currentDeathTimer > 0f;
            if (deathSlider.gameObject.activeSelf != shouldShowDeathBar)
            {
                deathSlider.gameObject.SetActive(shouldShowDeathBar);
            }
        }
    }
}