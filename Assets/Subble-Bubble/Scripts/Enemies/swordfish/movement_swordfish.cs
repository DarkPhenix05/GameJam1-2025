using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement_swordfish : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform player;
    [SerializeField] private Vector2 relativeOffset = new Vector2(0, -4.5f);

    private float holdTime = 3f;
    private float destructionTime = 3f;
    private bool isMovingStraight = false;

    void Start()
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
                return;
            }
        }

        Vector3 newPosition = player.position + new Vector3(relativeOffset.x, relativeOffset.y, 0);
        transform.position = newPosition;

        StartCoroutine(HandleMovement());
    }

    void Update()
    {
        if (isMovingStraight)
        {
            Move();
        }
        else if (player)
        {
            Vector3 newPosition = player.position + new Vector3(relativeOffset.x, relativeOffset.y, 0);
            transform.position = newPosition;
        }
    }

    private void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private IEnumerator HandleMovement()
    {
        yield return new WaitForSeconds(holdTime);

        isMovingStraight = true;

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
