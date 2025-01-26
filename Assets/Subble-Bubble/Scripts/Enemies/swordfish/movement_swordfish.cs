using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement_swordfish : MonoBehaviour
{
    private enum MovementDirection
    {
        Horizontal,
        Vertical
    }

    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform player;
    [SerializeField] public Transform cameraTransform;
    [SerializeField] private Vector2 relativeOffset = new Vector2(0, -4.5f);
    [SerializeField] private MovementDirection movementDirection = MovementDirection.Vertical;

    private Vector2 axisDirection;
    private float holdTime = 3f;
    private float destructionTime = 3f;
    private bool initialTimerIsOver = false;
    private float fixedYPosition;

    void Start()
    {
        InitializeMovement();
        StartCoroutine(HandleTimers());
    }

    void Update()
    {
        if (initialTimerIsOver)
        {
            Move();
        }
        else if (player)
        {
            UpdatePositionRelativeToPlayer();
        }
    }

    private void InitializeMovement()
    {
        SetAxisDirection();
        AssignPlayerReference();
        AssignCameraReference();
        CalculateInitialPosition();
    }

    private void SetAxisDirection()
    {
        axisDirection = movementDirection == MovementDirection.Vertical ? Vector2.up : Vector2.right;
    }

    private void AssignPlayerReference()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag("RaycastOrigen");
            if (foundPlayer)
            {
                player = foundPlayer.transform;
            }
            else
            {
                Debug.LogError("El objeto 'player' no está asignado y no se encontró un objeto con el tag 'RaycastOrigen'.");
                enabled = false;
                return;
            }
        }
    }

    private void AssignCameraReference()
    {
        if (cameraTransform == null)
        {
            cameraTransform = player.GetComponentInChildren<Camera>()?.transform;
            if (cameraTransform == null)
            {
                Debug.LogError("No se encontró una cámara asociada al jugador. Por favor, asigna una cámara en el Inspector.");
                enabled = false;
            }
        }
    }


    private void CalculateInitialPosition()
    {
        if (player == null) return;

        Vector3 playerPosition = player.position;
        Vector3 newPosition = new Vector3(playerPosition.x + relativeOffset.x, playerPosition.y + relativeOffset.y, transform.position.z);
        transform.position = newPosition;

        if (movementDirection == MovementDirection.Horizontal)
        {
            fixedYPosition = transform.position.y - cameraTransform.position.y;
        }
    }

    private void UpdatePositionRelativeToPlayer()
    {
        Vector3 playerPosition = player.position;
        Vector3 newPosition = new Vector3(playerPosition.x + relativeOffset.x, playerPosition.y + relativeOffset.y, transform.position.z);
        transform.position = newPosition;
    }

    private void Move()
    {
        if (movementDirection == MovementDirection.Horizontal)
        {
            float adjustedY = cameraTransform.position.y + fixedYPosition;
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, adjustedY);
        }
        else
        {
            transform.Translate((Vector3)axisDirection * speed * Time.deltaTime);
        }
    }

    private IEnumerator HandleTimers()
    {
        yield return new WaitForSeconds(holdTime);

        initialTimerIsOver = true;

        yield return new WaitForSeconds(destructionTime);

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == player)
        {
            gameObject.SetActive(false);
        }
    }
}
