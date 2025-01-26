using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement_swordfish : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private Transform player;
    [SerializeField] private Vector2 relativeOffset = new Vector2(0, -4.5f);

    private float holdTime = 3f;
    private float destructionTime = 5f;
    private bool isMovingStraight = false;

    void Start()
    {
        if (player)
        {
            Vector3 newPosition = player.position + new Vector3(relativeOffset.x, relativeOffset.y, 0);
            transform.position = newPosition;
        }

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
        if (collision.gameObject.CompareTag("RaycastOrigen"))
        {
            gameObject.SetActive(false);
        }
    }
}
