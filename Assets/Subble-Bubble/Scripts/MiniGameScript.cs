using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameScript : MonoBehaviour
{

    public float gameOverDelay = 2.0f;

    private bool isMinigameActive = false;
    private bool isGameOver = false;
    public GameObject clickimage;
    public GameObject _octopusScript;
    public Animator _gameOverAnimator;

    [Header("ClicsControl")]
    [SerializeField]
    private int clicksToWin = 10;
    private int currentClickCount = 0;

    [SerializeField]
    private float gameOverTimer = 2.0f;


   // Con esto mandamos a llamar el minijuego de esta forma iniciandolo al colisionar con el pulpo.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("El Pulpasio te ha atrapado");
        StartMinigame();
    }


    //Al iniciar el minijuego se nos dara una alerta para escapar de el, teniendo un temporizador.
    public void StartMinigame() 
    {
        print("Escapa del Pulpo");
        isMinigameActive=true;
        isGameOver = false;
        gameOverTimer = 0f;
        currentClickCount = 0;
        clickimage.SetActive(true);
    }

    void Start()
    {
        if (_octopusScript == null)
        {
            _octopusScript = FindObjectOfType<OctopusScript>().gameObject;
        }
    }

    // Aquí se encuentra el funcionamiento del minijuego del minijuego con raycast y detección de click
    void Update()
    {
        if (isMinigameActive) 
        {
        if (Input.GetMouseButtonDown(0))
            {
                print("Clic Detectado");
                Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
                // print("Objeto clicado: " + hit.collider.name);
                if (hit.collider != null && hit.collider.CompareTag("TargetZone"))
                {
                    print("Zona objetivo clicada");
                    currentClickCount++;
                    print("Clics acuales" + currentClickCount);

                    if (currentClickCount >= clicksToWin)
                    {
                        MinigameSuccess();
                    }
                }
            }

        if (isGameOver) 
            {
                gameOverTimer += Time.deltaTime;
                if (gameOverTimer >= gameOverDelay) 
                {
                    EndMinigame();
                }
            }
        }
    }

    void MinigameSuccess()
    {
        print("Desciende");
        isMinigameActive = false;
        DisableMinigameObjects();
    }

    //Esta es la pantalla de GameOver
    void EndMinigame()
    { print("GameOver");
        isMinigameActive = false;
        isGameOver = true;
        if (_gameOverAnimator != null)
        {
            _gameOverAnimator.SetTrigger("GameOver");

        }
        Invoke("RestartGameplay", 2f);
            }

    // Aqui se encuentra la pantalla de victoria del minijuego
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isMinigameActive && other.CompareTag("TargetZone"))
        {
            print("Minijuego superado");
            MinigameSuccess();
        }
    }
    void DisableMinigameObjects()
    {
        _octopusScript.GetComponent<OctopusScript>().Victory();
        /*
         * Se implemento una corrutina en el OctoScript el cual esta en el GO de "pulpo"
         * Este escript esta en la UI, para facilitar que pulpo oobtenga su referencia.
         * 
         * Se quito la logica de TAGS
        */
    }

    void RestartGameplay()
    {
        isMinigameActive= false;
        isGameOver= false;
    }
}
