using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo2D : MonoBehaviour
{
    public float tiempoInmovilInicial = 2f; // Tiempo inicial en el que el enemigo está inmóvil
    public float tiempoInmovilConCollider = 2f; // Tiempo inmóvil después de activar el collider
    public float distanciaMovimiento = 3f; // Distancia que se mueve antes de quedarse inmóvil
    public float velocidadMovimiento = 2f; // Velocidad con la que se mueve
    public float velocidadMovimientoFinal = 5f; // Velocidad con la que se moverá al final
    public float distanciaMovimientoFinal = 10f; // Distancia que recorre al final

    private Collider2D enemigoCollider; // Referencia al collider del enemigo (2D)
    private Vector3 posicionInicial; // Posición inicial del enemigo
    private bool estaMoviendose = false; // Controla si el enemigo está en movimiento
    private int faseActual = 0; // Fase del comportamiento del enemigo
    private float tiempoFase = 0f; // Tiempo que lleva en la fase actual
    private Vector3 direccionMovimiento; // Dirección del movimiento calculada a partir de la rotación
    private Vector3 direccionOriginal; // Dirección inicial en la que se mueve el enemigo

    [SerializeField] public Animator animator;
    private bool IsMordida = false;

    void Start()
    {
        enemigoCollider = GetComponent<Collider2D>();
        enemigoCollider.enabled = false; // Desactivar el collider al inicio
        posicionInicial = transform.localPosition; // Guardar la posición inicial

        // Calcular la dirección del movimiento basándonos en la rotación del objeto
        direccionMovimiento = transform.right; // La dirección será hacia el "lado derecho" del objeto
        direccionOriginal = direccionMovimiento; // Guardar la dirección original
    }

    void Update()
    {
        switch (faseActual)
        {
            case 0: // Fase inicial: enemigo inmóvil
                tiempoFase += Time.deltaTime;
                if (tiempoFase >= tiempoInmovilInicial)
                {
                    tiempoFase = 0f;
                    faseActual = 1; // Pasar a la fase de movimiento inicial
                    estaMoviendose = true;
                }
                break;

            case 1: // Fase de movimiento inicial
                if (estaMoviendose)
                {
                    MoverEnemigo(distanciaMovimiento);
                }
                else
                {
                    tiempoFase += Time.deltaTime;
                    if (tiempoFase >= tiempoInmovilConCollider)
                    {
                        tiempoFase = 0f;
                        faseActual = 2; // Pasar a la fase de movimiento final
                        enemigoCollider.enabled = true; // Activar el collider
                        estaMoviendose = true;
                    }
                }
                break;

            case 2: // Fase de movimiento final
                if (estaMoviendose)
                {
                    MoverEnemigoFinal(distanciaMovimientoFinal);
                }
                break;
        }
    }

    private void MoverEnemigo(float distanciaObjetivo)
    {
        float distanciaRecorrida = velocidadMovimiento * Time.deltaTime;
        transform.localPosition += direccionMovimiento * distanciaRecorrida; // Mover en la dirección calculada

        // Comprobar si se alcanzó la distancia deseada
        float distanciaTotal = Vector3.Distance(posicionInicial, transform.localPosition);
        if (distanciaTotal >= distanciaObjetivo)
        {
            estaMoviendose = false; // Detener el movimiento
            posicionInicial = transform.localPosition; // Actualizar la posición inicial para la siguiente fase
        }
    }

    private void MoverEnemigoFinal(float distanciaObjetivo)
    {
        animator.SetBool("IsMordida", true);
        
        float distanciaRecorrida = velocidadMovimientoFinal * Time.deltaTime;
        transform.localPosition += direccionMovimiento * distanciaRecorrida; // Mover en la dirección calculada

        // Comprobar si se alcanzó la distancia deseada
        float distanciaTotal = Vector3.Distance(posicionInicial, transform.localPosition);
        if (distanciaTotal >= distanciaObjetivo)
        {
            estaMoviendose = false; // Detener el movimiento
            posicionInicial = transform.localPosition; // Actualizar la posición inicial para la siguiente fase
        }
    }

    // Detectar la colisión con el objeto "Flash"
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Flash"))
        {
            //// Detener el movimiento actual
            estaMoviendose = false;

            //// Detener al enemigo por cierto tiempo
            StartCoroutine(DetenidoPorTiempo());
        }
    }

    // Coroutine para detener al enemigo por cierto tiempo y luego cambiar la dirección
    private IEnumerator DetenidoPorTiempo()
    {
        // El enemigo se detiene por el tiempo que se desee
        yield return new WaitForSeconds(2f); // 2 segundos de espera (ajustable)

        // Cambiar la dirección de movimiento
        direccionMovimiento = -direccionOriginal; // Moverse en dirección contraria

        // Continuar con el movimiento
        estaMoviendose = true;
        IsMordida = true;
    }
}
