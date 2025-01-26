using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombillaCollider : MonoBehaviour
{
    public GameObject Minijuego;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto izquierdo entra en la zona objetivo que tiene el trigger
        if (other.gameObject.CompareTag("CircleWin"))
        {
            Minijuego.SetActive(false);
        }
    }
}
