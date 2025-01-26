using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameScript : MonoBehaviour
{

    public float gameOverDelay = 2.0f;

    private bool isMinigameActive = false;
    private bool isGameOver = false;
    public GameObject clickimage;


    [Header("ClicsControl")]
    [SerializeField]
    private int clicksToWin = 10;
    private int currentClickCount = 0;

    [SerializeField]
    private float gameOverTimer = 0f;


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
        isMinigameActive = true;
        isGameOver = false;
        gameOverTimer = 0f;
        currentClickCount = 0;
        clickimage.SetActive(true);
    }

    void Start()
    {

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
                print("Objeto clicado: " + hit.collider.name);
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
    {
        print("GameOver");
        isMinigameActive = false;
        isGameOver = false;
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
        GameObject[] minigameObjects = GameObject.FindGameObjectsWithTag("MinigameObject");
        foreach (GameObject obj in minigameObjects)
        { gameObject.SetActive(false); }
        clickimage.SetActive(false);
    }
}
