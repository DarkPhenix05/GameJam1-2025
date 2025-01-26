using UnityEditor.PackageManager;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public Image _cooldown;

    [Header("Movement")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _gravityScale;

    [SerializeField] private Vector2 _sideMove;
    [SerializeField] private float _hopDuration;
    [SerializeField] private float _timeBetweenHops;
    public float _midTimeCounter = 0.0f;
    public float _hopTimeCounter = 0.0f;
    public bool _canHop = true;

    [SerializeField] private float _decelerate;
    public bool mousePressed = false;

    [Header("Raycast")]
    [SerializeField] private float _raycastDistancia = 10f;
    [SerializeField] private float _velocidadRotacion = 5f;
    [SerializeField] private LayerMask _capaInteractuable;

    [SerializeField] private Transform _origenRaycast;
    private RaycastHit _hitInfo;
    private Vector2 _raycastDireccion;

    [SerializeField] private int _maxSpeed;
    [SerializeField] private int _minSpeed;

    [Header("Oxygen")]
    [SerializeField] public int Oxygen;
    [SerializeField] private int OxygenMax;

    public float _raycastDistancia = 10f; // Distancia del raycast
    public float _velocidadRotacion = 5f; // Velocidad de rotación del raycast (basado en el mouse)
    public LayerMask _capaInteractuable; // Capa que el raycast puede impactar (por ejemplo, objetos que interactúan con la burbuja)

    public Transform _origenRaycast; // El Transform del origen del raycast (puedes asignar cualquier GameObject aquí)
    private RaycastHit _hitInfo; // Información del raycast
    private Vector2 _raycastDireccion; // Dirección del raycast


    [Header("Horizontal Velocity")]
    // Velocidades modificables para cada tag
    [SerializeField] private float velocidadIzquierda1;
    [SerializeField] private float velocidadIzquierda2;
    [SerializeField] private float velocidadDerecha1;
    [SerializeField] private float velocidadDerecha2;

    [Header("Flash")]
    [SerializeField] public GameObject FlashIzq;
    [SerializeField] public GameObject FlashDer;
    [SerializeField] GameObject _changeFlashButton;
    int countflah = 0;


    void Start()
    {
        while (_origenRaycast == null)
            _origenRaycast = transform;

        if (_rb == null)
            _rb = this.gameObject.GetComponent<Rigidbody2D>();

        _rb.gravityScale = _gravityScale;
        _rb.freezeRotation = true;
    }

    void Update()
    {
        HundirBurbuja();
        PlayerMovement();
        RotarRaycast();
        LanzarRaycast();
    }
    
    
    void HundirBurbuja()
    {
        if (_rb.velocity.y >= _maxSpeed)
            _rb.velocity = new Vector2(_rb.velocity.x, _maxSpeed);
    }

    void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.A) && _canHop)
        {
            _hopTimeCounter += Time.deltaTime;
            if (_hopTimeCounter < _hopDuration)
            {
                _rb.AddForce(new Vector2(-_sideMove.x, _sideMove.y), ForceMode2D.Impulse);
            }

            Debug.Log("Hop L: " + _hopTimeCounter);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            _canHop = false;
            _hopTimeCounter = 0.0f;
        }
        if (Input.GetKey(KeyCode.D) && _canHop)
        {
            _hopTimeCounter += Time.deltaTime;
            if (_hopTimeCounter < _hopDuration)
            {
                _rb.AddForce(new Vector2(_sideMove.x, _sideMove.y), ForceMode2D.Impulse);
            }

            Debug.Log("Hop R: " + _hopTimeCounter);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            _canHop = false;
            _hopTimeCounter = 0.0f;
        }

        if (!_canHop)
        {
            if (_midTimeCounter < _timeBetweenHops)
            {
                _midTimeCounter += Time.deltaTime;

                _cooldown.fillAmount = _timeBetweenHops / _midTimeCounter;
            }
            else
            {
                _midTimeCounter = 0.0f;
                _canHop = true;
            }
        }
    }

    void RotarRaycast()
    {
        // Obtener la posición del mouse en el mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        _raycastDireccion = (mousePos - _origenRaycast.position).normalized;

        float angle = Mathf.Atan2(_raycastDireccion.y, _raycastDireccion.x) * Mathf.Rad2Deg;
        _origenRaycast.rotation = Quaternion.Slerp(_origenRaycast.rotation, Quaternion.Euler(0f, 0f, angle), _velocidadRotacion * Time.deltaTime);
    }

    void LanzarRaycast()
    {
        Debug.DrawRay(_origenRaycast.position, _raycastDireccion * _raycastDistancia, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(_origenRaycast.position, _raycastDireccion, _raycastDistancia, _capaInteractuable);
        
        if (hit.collider != null)
        {
            //Debug.Log("Impact� con: " + hit.collider.name);

            if (hit.collider.CompareTag("Izquierda1"))
            {
                Debug.Log("Izquierda");
                _rb.velocity = new Vector2(velocidadIzquierda1, _rb.velocity.y);
            }
            else if (hit.collider.CompareTag("Derecha1"))
            {
                Debug.Log("Derecha1");
                _rb.velocity = new Vector2(-velocidadDerecha1, _rb.velocity.y);
            }

            if (hit.collider.CompareTag("FlashIzquierda"))
            {
                if(Input.GetMouseButtonDown(0) && countflah == 0) 
                {
                    Debug.Log("FlashIzq");
                    FlashIzq.SetActive(true);
                    countflah++;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    FlashIzq.SetActive(false);
                    ChangeFlash();
                }

            }
            else
            {
                FlashIzq.SetActive(false);
            }
            
            if (hit.collider.CompareTag("FlashDerecha"))
            {
                if(Input.GetMouseButtonDown(0) && countflah == 0)
                {
                Debug.Log("FlashDer");
                FlashDer.SetActive(true);
                countflah++;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    FlashDer.SetActive(false);
                    ChangeFlash();
                }

            }
            else
            {
                FlashDer.SetActive(false);
            }

            if (hit.collider.CompareTag("Freno"))
            {

                if (Input.GetMouseButton(0))
                {
                    Debug.Log("Freno");
                    if (_rb.velocity.y < _minSpeed)
                    {
                        if (_rb.velocity.x > 0)
                            _rb.AddForce(new Vector2(-1, _decelerate));

                        else if (_rb.velocity.x < 0)
                            _rb.AddForce(new Vector2(1, _decelerate));

                        Debug.Log("BREAKING");
                    }
                    else
                    {
                        if (_rb.velocity.x > 0)
                            _rb.velocity = new Vector2(_rb.velocity.x - 1, _minSpeed);

                        else if (_rb.velocity.x < 0)
                            _rb.velocity = new Vector2(_rb.velocity.x + 1, _minSpeed);

                        Debug.Log("MAX - BREAKING");
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("NoFreno");
                }
            }
        }
        
        else
        {
            Debug.Log("NoColisiona");
        }
    }

    void ChangeFlash()
    {
        if(countflah == 1)
        {
            _changeFlashButton.SetActive(true);
=======
            // Si el raycast golpea algo, mostrar la información
            Debug.Log("Raycast impactó con: " + _hitInfo.collider.name);
        }
    }

    void OnDrawGizmos()
    {
        if (_origenRaycast == null) return;


        Gizmos.color = Color.red;
        Gizmos.DrawRay(_origenRaycast.position, _raycastDireccion * _raycastDistancia);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision == null) return;
        else
        {
            Debug.Log(_rb.velocity);
        }
    }
}
