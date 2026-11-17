using UnityEngine;

public class FearManager : MonoBehaviour
{
    [Header("Ustawienia Strachu")]
    public float currentFear = 0f;
    public float maxFear = 100f;
    public float baseFearRate = 5f;

    [Header("Odliczanie do Œmierci")]
    public float maxDeathTime = 5f; // Ile sekund gracz wytrzyma przy pe³nym strachu
    public float currentDeathTimer = 0f;
    public float deathRecoveryRate = 2f; // Tempo cofania siê paska po wejœciu w œwiat³o

    // Zwraca poziom strachu w skali od 0.0 (spokój) do 1.0 (panika)
    public float FearNormalized => maxFear > 0f ? currentFear / maxFear : 0f;

    // Opcjonalnie: postêp odliczania do œmierci w skali od 0.0 do 1.0
    public float DeathNormalized => maxDeathTime > 0f ? currentDeathTimer / maxDeathTime : 0f;

    private float currentZoneModifier = 0f;
    private Labyrinth game;

    void Awake()
    {
        game = GameObject.FindWithTag("Game").GetComponent<Labyrinth>();
    }

    void Update()
    {
        if (game.inMenu || game.fading)
        {
            return;
        }

        float finalFearRate = baseFearRate - currentZoneModifier;

        currentFear += finalFearRate * Time.deltaTime;
        currentFear = Mathf.Clamp(currentFear, 0, maxFear);

        // Odliczanie do œmierci dzia³a tylko przy maksymalnym strachu
        if (currentFear >= maxFear)
        {
            currentDeathTimer += Time.deltaTime;
            if (currentDeathTimer >= maxDeathTime)
            {
                currentFear = 0f;
                currentDeathTimer = 0f;
                if (gameObject.CompareTag("Player")) // œmieræ gracza
                {
                    game.ending = 2;
                }
                else // œmieræ porwanego
                {
                    game.ending = 3;
                }
                game.ResetGame();
            }
        }
        else if (currentDeathTimer > 0f)
        {
            // Cofanie odliczania, gdy strach spadnie poni¿ej maksimum
            currentDeathTimer = Mathf.Max(0f, currentDeathTimer - deathRecoveryRate * Time.deltaTime);
        }
    }

    public void AddInstantFear(float amount)
    {
        currentFear = Mathf.Clamp(currentFear + amount, 0, maxFear);
    }

    public void AddZoneModifier(float modifierAmount)
    {
        currentZoneModifier += modifierAmount;
    }
}