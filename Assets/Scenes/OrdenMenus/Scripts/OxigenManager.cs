using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    [Header("Oxygen Settings")]
    [SerializeField] private float maxOxygen = 1000f; // Máximo nivel de oxígeno
    [SerializeField] private float oxygenDecreaseRate = 10f; // Oxígeno que se reduce por segundo
    [SerializeField] private float oxygenIncreaseRate = 20f; // Oxígeno que se regenera por segundo

    [Header("Health")]
    public Slider sliderhealth;


    public float currentOxygen; // Oxígeno actual
    private bool isGameOver = false; // Estado del juego

    void Start()
    {
        currentOxygen = maxOxygen; // Inicializar el oxígeno al máximo
     
    }

    void Update()
    {
        if (isGameOver) return;

        // Reducir oxígeno con el tiempo
        DecreaseOxygen();

        // Verificar si el oxígeno llega a 0
        if (currentOxygen <= 0f)
        {
            GameOver();
        }

        sliderhealth.value = currentOxygen;
        if (currentOxygen >= 1000)
        {
            currentOxygen = 1000;
        }

        if (currentOxygen <= 0)
        {
            currentOxygen = 0;
        }

    }

    public void IncreaseOxygen()
    {
        if (isGameOver) return;

        currentOxygen += oxygenIncreaseRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
        //currentOxygen++;
    }

    private void DecreaseOxygen()
    {
        currentOxygen -= oxygenDecreaseRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
        //currentOxygen--;
    }

 





    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
        // Lógica adicional para terminar el juego (reinicio, pantalla final, etc.)
    }
}
