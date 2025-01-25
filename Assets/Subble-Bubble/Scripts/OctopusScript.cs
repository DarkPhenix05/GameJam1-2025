using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusScript : MonoBehaviour
{

public Animator animator;
    [SerializeField]
    private MiniGameScript script;
    // Start is called before the first frame update
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (script == null)
        {
            script = FindAnyObjectByType<MiniGameScript>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Octopus"))
        {
            animator.SetTrigger("Atrapado");
            Invoke("StartMinigame", 1f);
        }
    }

    void StartMinigame()
    {
        if (script != null) 
        {
            script.StartMinigame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
