using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombillaaa : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ESTOY INICIALIZANDO!!!!!");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisiono");
    }
}
