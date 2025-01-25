using UnityEngine;

public class BurbujaScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _fallScale;
    [SerializeField] private float _breakeScale;

    public float _aceleraccionHundimiento; // Velocidad de hundimiento de la burbuja
    public float _raycastDistancia = 10f; // Distancia del raycast
    public float _velocidadRotacion = 5f; // Velocidad de rotaci�n del raycast (basado en el mouse)
    public LayerMask _capaInteractuable; // Capa que el raycast puede impactar (por ejemplo, objetos que interact�an con la burbuja)

    public Transform _origenRaycast; // El Transform del origen del raycast (puedes asignar cualquier GameObject aqu�)
    private RaycastHit _hitInfo; // Informaci�n del raycast
    private Vector2 _raycastDireccion; // Direcci�n del raycast

    [SerializeField] private int MaxSpeed; // The mas speed the capsule can axelerate to.

    void Start()
    {
        // Si no se asign� un origen del raycast, usamos el centro de la burbuja (el mismo GameObject)
        while (_origenRaycast == null)
        {
            _origenRaycast = transform;
        }

        while (_rb == null) 
        {
            _rb = this.gameObject.GetComponent<Rigidbody2D>();
        }

        _rb.gravityScale = _fallScale;
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


    // Funci�n para hacer que la burbuja se hunda
    void HundirBurbuja()
    {
        if(_rb != null)
        {
            if(_rb.velocity.y >= MaxSpeed)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, MaxSpeed);
            }
        }

        //transform.position += Vector3.down * _velocidadHundimiento * Time.deltaTime;
    }

    // Funci�n para rotar el raycast en funci�n de la posici�n del mouse
    void RotarRaycast()
    {
        // Obtener la posici�n del mouse en el mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // Asegurarnos de que el raycast solo se mueva en el plano XY (2D)

        // Obtener la direcci�n hacia el mouse
        _raycastDireccion = (mousePos - _origenRaycast.position).normalized;

        // Rotar el origenRaycast para que apunte hacia el mouse
        float angle = Mathf.Atan2(_raycastDireccion.y, _raycastDireccion.x) * Mathf.Rad2Deg;
        _origenRaycast.rotation = Quaternion.Slerp(_origenRaycast.rotation, Quaternion.Euler(0f, 0f, angle), _velocidadRotacion * Time.deltaTime);
    }

    // Funci�n para lanzar el raycast y detectar colisiones
    void LanzarRaycast()
    {
        // Dibujar el raycast para ver la direcci�n en la escena (utilizando Gizmos)
        Debug.DrawRay(_origenRaycast.position, _raycastDireccion * _raycastDistancia, Color.red);

        // Lanzar el raycast
        if (Physics.Raycast(_origenRaycast.position, _raycastDireccion, out _hitInfo, _raycastDistancia, _capaInteractuable))
        {
            // Si el raycast golpea algo, mostrar la informaci�n
            Debug.Log("Raycast impact� con: " + _hitInfo.collider.name);
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