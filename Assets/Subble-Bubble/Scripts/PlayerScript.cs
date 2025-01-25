using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class BurbujaScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _fallScale;
    [SerializeField] private float _breakeScale;
    [SerializeField] private float _sideMove;

    public float _raycastDistancia = 10f; // Distancia del raycast
    public float _velocidadRotacion = 5f; // Velocidad de rotación del raycast (basado en el mouse)
    public LayerMask _capaInteractuable; // Capa que el raycast puede impactar (por ejemplo, objetos que interactúan con la burbuja)

    public Transform _origenRaycast; // El Transform del origen del raycast (puedes asignar cualquier GameObject aquí)
    private RaycastHit _hitInfo; // Información del raycast
    private Vector2 _raycastDireccion; // Dirección del raycast

    [SerializeField] private int _maxSpeed; // The mas speed the capsule can axelerate to.

    [SerializeField] private Keyboard _keyboard;
    [SerializeField] private Gamepad _gamepad;

    void Start()
    {
        // Si no se asignó un origen del raycast, usamos el centro de la burbuja (el mismo GameObject)
        while (_origenRaycast == null)
        {
            _origenRaycast = transform;
        }

        while (_rb == null) 
        {
            _rb = this.gameObject.GetComponent<Rigidbody2D>();
        }

        _rb.gravityScale = _fallScale;
        _rb.freezeRotation = true;

        if (Keyboard.current != null)
        {
            _keyboard = Keyboard.current;
        }

        else if (Gamepad.current != null)
        {
            _gamepad = Gamepad.current;
        }
    }

    void Update()
    {
        // Hacer que la burbuja se hunda
        HundirBurbuja();

        // MovePlayer
        PlayerMovement();

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
            Debug.Log("Max Speed Reached");
        }

    }

    void PlayerMovement()
    {

        if (_keyboard.aKey.isPressed)
        {
            _rb.AddForce(new Vector2(_sideMove, 0.0f));
            Debug.Log("A");
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
}