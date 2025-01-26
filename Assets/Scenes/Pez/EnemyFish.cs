using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyFish : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]float _speed = 1f;
    [SerializeField] float _speedEscape = 5f;
    [SerializeField] Transform _player;
    [SerializeField] Transform _spawn;
    [SerializeField] float _timeMoving = 4f;
    [SerializeField] float _timeStop = 3f;
    float _totalStop;
    bool _attackMode = false;
    float _temporizador;
    Vector3 _initialPos;

    [Header("Health")]
    public int _healthDamage;
    [SerializeField] int _health;


    [Header("Damage")]
    float _healthBubble = 10;
    [SerializeField]int _damageBubble = 10;

    // Start is called before the first frame update
    void Start()
    {
        _attackMode = true;
        _totalStop = _timeMoving + _timeStop;
        _healthDamage = _health;
        _initialPos = this.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        Temporizador();
        Health();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DamageToBubble(_healthBubble);
        _attackMode = false;
        DamageToFish();
        this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
    }

    void Temporizador()
    {
        if (_attackMode)
        {
            _temporizador += Time.deltaTime;
            if (_temporizador <= _timeMoving)
            {
                Movement();
            }
            if (_temporizador >= _totalStop)
            {
                _temporizador = 0;
                
            }
        }
        else 
        { 
            _temporizador = _timeStop ;
            Movement();
        }

    }

    void Movement()
    {
        float movement = _speed * Time.deltaTime;
        
        if (_attackMode)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.position, movement);
        }
        else 
        {
            _speed = _speedEscape;

            transform.position = Vector3.MoveTowards(transform.position, _spawn.position, movement);
            if (transform.position == _spawn.position)
            {
                this.gameObject.SetActive(false);
                Restart();
                
            }
        }
    }
    

    void Health()
    {
        if (_health <= 0)
        {
            _attackMode = false;
        }
    }



    void DamageToFish()
    {
        _healthDamage--;
    }

    void DamageToBubble(float healthBubble)
    {
        healthBubble = -_damageBubble;
    }

    void Restart()
    {
        this.gameObject.SetActive(true);
        _attackMode = true;
        _temporizador = 0;
        _healthDamage = _health;
        this.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        this.transform.position = _initialPos;
    }

}
