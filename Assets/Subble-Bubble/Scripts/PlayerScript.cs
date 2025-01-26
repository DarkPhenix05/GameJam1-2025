using UnityEngine;
using UnityEngine.InputSystem;

public class BurbujaScript : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _gravityScale;
    [SerializeField] private Vector2 _sideMove;
    [SerializeField] private float _hopDuration;
    [SerializeField] private float _decelerate;

    public float _raycastDistancia = 10f; // Distancia del raycast
    public float _velocidadRotacion = 5f; // Velocidad de rotación del raycast (basado en el mouse)
    public LayerMask _capaInteractuable; // Capa que el raycast puede impactar (por ejemplo, objetos que interactúan con la burbuja)

    public Transform _origenRaycast; // El Transform del origen del raycast (puedes asignar cualquier GameObject aquí)
    private RaycastHit _hitInfo; // Información del raycast
    private Vector2 _raycastDireccion; // Dirección del raycast

    [SerializeField] private int _maxSpeed; // The max speed the capsule can axelerate to.
    [SerializeField] private int _minSpeed; // The min speed the capsule can axelerate to.

    private Keyboard _keyboard;
    private float _counter = 0.0f;
    private bool _detectInput = true;


    void Start()
    {
        while (_origenRaycast == null)
        {
            _origenRaycast = transform;
        }

        while (_rb == null) 
        {
            _rb = this.gameObject.GetComponent<Rigidbody2D>();
        }

        _rb.gravityScale = _gravityScale;
        _rb.freezeRotation = true;

        if (Keyboard.current != null)
        {
            _keyboard = Keyboard.current;
        }
    }

    void Update()
    {
        // Hacer que la burbuja se hunda
        HundirBurbuja();

        // Rotar el raycast hacia el mouse
        RotarRaycast();

        // Lanzar el raycast para detectar colisiones
        LanzarRaycast();
    }


    // Función para hacer que la burbuja se hunda
    void HundirBurbuja()
    {
        if(_rb.velocity.y >= _maxSpeed)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _maxSpeed);
        }
    }

    void PlayerMovement()
    {
        _counter += Time.deltaTime;
        
        if (_keyboard.aKey.IsPressed() && _detectInput)
        {
            //_detectInput = false;
            _rb.velocity = new Vector2(-_sideMove.x, _sideMove.y);
        }
        else if (_keyboard.dKey.isPressed && _detectInput)
        {
            //_detectInput = false;
            _rb.velocity = new Vector2(_sideMove.x, _sideMove.y);
        }

        if( _keyboard.wKey.isPressed) 
        {
            if(_rb.velocity.y < _minSpeed) 
            {
                _rb.AddForce(new Vector2(0, _decelerate));
                //Debug.Log("BREAKING");
            }
            else
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _minSpeed);
                Debug.Log("MAX - BREAKING");
            }
        }
    }

    // Función para rotar el raycast en función de la posición del mouse
    void RotarRaycast()
    {
        // Obtener la posición del mouse en el mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // Asegurarnos de que el raycast solo se mueva en el plano XY (2D)

        // Obtener la dirección hacia el mouse
        _raycastDireccion = (mousePos - _origenRaycast.position).normalized;

        // Rotar el origenRaycast para que apunte hacia el mouse
        float angle = Mathf.Atan2(_raycastDireccion.y, _raycastDireccion.x) * Mathf.Rad2Deg;
        _origenRaycast.rotation = Quaternion.Slerp(_origenRaycast.rotation, Quaternion.Euler(0f, 0f, angle), _velocidadRotacion * Time.deltaTime);
    }

    // Función para lanzar el raycast y detectar colisiones
    void LanzarRaycast()
    {
        // Dibujar el raycast para ver la dirección en la escena (utilizando Gizmos)
        Debug.DrawRay(_origenRaycast.position, _raycastDireccion * _raycastDistancia, Color.red);

        // Lanzar el raycast
        if (Physics.Raycast(_origenRaycast.position, _raycastDireccion, out _hitInfo, _raycastDistancia, _capaInteractuable))
        {
            // Si el raycast golpea algo, mostrar la información
            Debug.Log("Raycast impactó con: " + _hitInfo.collider.name);
        }
    }

    // Dibujar el raycast con Gizmos en la vista de la escena
    void OnDrawGizmos()
    {
        if (_origenRaycast == null) return;

        // Dibujar el raycast en Gizmos
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