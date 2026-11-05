using UnityEngine;

public class MidpointTriggerController : MonoBehaviour
{
    private Labyrinth labirynth;
    private bool wasReset = false;

    void Awake()
    {
        labirynth = GetComponentInParent<Labyrinth>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        UnityEngine.Debug.Log("Trigger hit by " + other.name + ", tag " + other.tag);
        if ((other.CompareTag("Player")) && (!wasReset))
        {
            wasReset = true;
            labirynth.ResetLabyrinth();
        }
    }
}
