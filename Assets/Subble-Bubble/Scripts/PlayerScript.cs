using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Image _cooldown;

    [Header("Movement")]
   
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
    [SerializeField] public int _DownVelocity;

    [Header("Oxygen")]
    [SerializeField] public int Oxygen;
    [SerializeField] private int OxygenMax;
    [SerializeField] private OxygenManager oxygenManager;

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
    [SerializeField] public GameObject luz;

    [SerializeField] public bool FlashPermiso = false;

    int countflah = 0;

    void Start()
    {
        if (_origenRaycast == null)
            _origenRaycast = transform;

       
    }

    void Update()
    {
        HundirBurbuja();
        PlayerMovement();
        RotarRaycast();
        LanzarRaycast();

        transform.Translate(Vector2.up * -2.5f * Time.deltaTime);

    }

    void HundirBurbuja()
    {
        float movement = _DownVelocity * Time.deltaTime;
        
    }

    void PlayerMovement()
    {
       

       
    }

    void RotarRaycast()
    {
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
            //Debug.Log("Impactó con: " + hit.collider.name);

            if (hit.collider.CompareTag("Izquierda1"))
            {
                //Debug.Log("Izquierda");
                transform.Translate(Vector2.right * 2.5f * Time.deltaTime);
            }
            else if (hit.collider.CompareTag("Derecha1"))
            {
                //Debug.Log("Derecha1");
                transform.Translate(Vector2.left * 2.5f * Time.deltaTime);
            }

            if (hit.collider != null)
            {
                if ((hit.collider.CompareTag("OxigenoIzquierda") || hit.collider.CompareTag("OxigenoDerecha")) && Input.GetMouseButton(0))
                {
                    Debug.Log("Incrementando oxígeno...");
                    oxygenManager.IncreaseOxygen();
                    
                }


                if (hit.collider.CompareTag("FlashIzquierda"))
                {
                    if (Input.GetMouseButtonDown(0) && FlashPermiso == false)
                    {
                        Debug.Log("FlashIzq");
                        FlashIzq.SetActive(true);
                        
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        FlashIzq.SetActive(false);
                        ChangeFlash();
                        luz.SetActive(false);
                        FlashPermiso = false;
                    }

                }
                else
                {
                    FlashIzq.SetActive(false);
                }

                if (hit.collider.CompareTag("FlashDerecha"))
                {
                    if (Input.GetMouseButtonDown(0) && FlashPermiso == false)
                    {
                        Debug.Log("FlashDer");
                        FlashDer.SetActive(true);
                        
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        FlashDer.SetActive(false);
                        ChangeFlash(); luz.SetActive(false);
                        FlashPermiso = false;
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
                        transform.Translate(Vector2.down * -2.5f * Time.deltaTime);
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
            FlashPermiso = true;
            _changeFlashButton.SetActive(true);
            
            //if (countflah == 1)
            //{
            //    _changeFlashButton.SetActive(true);
            //}
        }

       


        //void OnDrawGizmos()
        //{
        //    if (_origenRaycast == null) return;


        //    Gizmos.color = Color.red;
        //    Gizmos.DrawRay(_origenRaycast.position, _raycastDireccion * _raycastDistancia);
        //}

    }

}
