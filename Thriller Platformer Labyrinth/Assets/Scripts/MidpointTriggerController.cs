using Unity.VisualScripting;
using UnityEngine;

public class MidpointTriggerController : MonoBehaviour
{
    private Labyrinth labirynth;
    private Rigidbody2D second;
    public bool wasReset = false;

    void Awake()
    {
        labirynth = GetComponentInParent<Labyrinth>();
        second = GameObject.FindWithTag("Second").GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Player")) && (!wasReset))
        {
            wasReset = true;
            labirynth.ResetLabyrinth();
            second.GetComponent<SecondController>().enabled = true;
            FearManager secondFear = second.GetComponentInChildren<FearManager>();
            if (secondFear != null)
            {
                secondFear.enabled = true;
            }
        }
    }
}
