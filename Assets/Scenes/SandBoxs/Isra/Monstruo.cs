using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float phase1Duration = 2f; // Tiempo que dura la fase 1 (sin collider)
    public float phase2Duration = 3f; // Tiempo que dura la fase 2 (con collider)
    public float reactionDuration = 2f; // Tiempo de reacción si recibe un ray hit en fase 2
    public float attackDelay = 1f; // Tiempo antes de atacar en la fase final
    public float moveSpeed = 2f; // Velocidad de movimiento

    private Collider enemyCollider;
    private bool isRayHit = false;

    private enum EnemyState { Phase1, Phase2, Reaction, Attacking }
    private EnemyState currentState;

    private void Start()
    {
        enemyCollider = GetComponent<Collider>();

        if (enemyCollider != null)
            enemyCollider.enabled = false; // El collider está desactivado inicialmente

        currentState = EnemyState.Phase1;
        StartCoroutine(Phase1());
    }

    private void Update()
    {
        // Siempre avanza hacia adelante
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private IEnumerator Phase1()
    {
        // Fase 1: Aparece sin collider
        yield return new WaitForSeconds(phase1Duration);
        currentState = EnemyState.Phase2;
        StartCoroutine(Phase2());
    }

    private IEnumerator Phase2()
    {
        // Fase 2: Activa el collider y continúa avanzando
        if (enemyCollider != null)
            enemyCollider.enabled = true;

        yield return new WaitForSeconds(phase2Duration);

        if (!isRayHit)
        {
            currentState = EnemyState.Attacking;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Reaction()
    {
        // Reacción al ray hit
        moveSpeed = 0; // Detiene el movimiento
        yield return new WaitForSeconds(reactionDuration);

        Destroy(gameObject); // Se elimina después de la reacción
    }

    private IEnumerator Attack()
    {
        // Fase final: Ataca
        moveSpeed = 0; // Detiene el movimiento para atacar
        yield return new WaitForSeconds(attackDelay);

        // Aquí colocas la lógica del ataque
        Debug.Log("Enemy is attacking!");
    }

    private void OnRayHit()
    {
        if (currentState == EnemyState.Phase2)
        {
            isRayHit = true;
            currentState = EnemyState.Reaction;
            StartCoroutine(Reaction());
        }
    }

    // Simulación de un ray hit (para pruebas)
    private void OnMouseDown()
    {
        OnRayHit();
    }
}
