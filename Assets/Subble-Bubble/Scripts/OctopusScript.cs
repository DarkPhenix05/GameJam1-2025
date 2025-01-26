using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OctopusScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    public Animator animator;
    [SerializeField]
    public MiniGameScript script;
    public GameObject _buttonUI;
    public GameObject _player;

    [SerializeField]
    Vector2 direction;

    public bool _win = false;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.right;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (script == null)
        {
            script = FindAnyObjectByType<MiniGameScript>();
        }
        if (_buttonUI == null)
        {
            _buttonUI = FindObjectOfType<MiniGameScript>().gameObject;
        }
        //if(_player == null) 
        //{
        //    _player = FindObjectOfType<BurbujaScript>().gameObject;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {       
        //transform.Translate(Vector2.down * nextRowOffset);
        if (collision.collider.CompareTag("RaycastOrigen"))
        {
            //animator.SetTrigger("Atrapado");
            Invoke("StartMinigame", 0f);
        }
        direction = Vector2.zero;
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
        if (!_win)
        {
            Vector3 targetDirection = new Vector3 (0.0f,0.0f,_player.transform.position.z - transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position , (speed * Time.deltaTime));
            Vector2 rot = Vector3.RotateTowards(transform.forward, targetDirection , (speed * Time.deltaTime), 0.0f);

            transform.rotation = Quaternion.LookRotation(rot);
        }
        else
        {
            Vector3 targetDirection = new Vector3 (0.0f,0.0f,_player.transform.position.z - transform.position.z);
            transform.position += new Vector3(-speed * Time.deltaTime, speed * Time.deltaTime, 0.0f);
            Vector3 rot = (Vector3.RotateTowards(transform.right, targetDirection, (speed * Time.deltaTime), 0.0f) - new Vector3(0.0f,0.0f,180.0f));

            transform.rotation = Quaternion.LookRotation(rot);
        }
    }

    public void Victory()
    {
        StartCoroutine(VictoryCorrutine());
    }

    public IEnumerator VictoryCorrutine()
    {
        _win = true;
        _buttonUI.SetActive(false);
        //direction = new Vector2(-6, 0);
        //Se genera pequeña animacion de escpae.

        yield return new WaitForSeconds(5.0f);

        //Se apaga el objeto fuera de camara.

        this.gameObject.SetActive(false);
    }
}