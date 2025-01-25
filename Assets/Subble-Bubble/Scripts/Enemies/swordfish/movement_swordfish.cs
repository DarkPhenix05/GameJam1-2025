using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement_swordfish : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    void Start()
    {

    }

    void Update()
    {
        Mover();
    }

    private void Mover()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
