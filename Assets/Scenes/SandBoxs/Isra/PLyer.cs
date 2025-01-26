using UnityEngine;

public class BurbujaScript : MonoBehaviour
{
    public float velocidadHundimiento = 2f; // Velocidad de hundimiento de la burbuja
    public float raycastDistancia = 10f; // Distancia del raycast
    public float velocidadRotacion = 5f; // Velocidad de rotación del raycast (basado en el mouse)
    public LayerMask capaInteractuable; // Capa que el raycast puede impactar (por ejemplo, objetos que interactúan con la burbuja)

    public Transform origenRaycast; // El Transform del origen del raycast (puedes asignar cualquier GameObject aquí)
    private RaycastHit hitInfo; // Información del raycast
    private Vector3 raycastDireccion; // Dirección del raycast

    // Velocidades específicas para cada dirección
    public float velocidadIzquierda1 = 3f;
    public float velocidadIzquierda2 = 5f;
    public float velocidadDerecha1 = 3f;
    public float velocidadDerecha2 = 5f;

    private bool moviendoIzquierda = false;
    private bool moviendoDerecha = false;
    private float velocidadActual = 0f;

    void Start()
    {
        // Si no se asignó un origen del raycast, usamos el centro de la burbuja (el mismo GameObject)
        if (origenRaycast == null)
        {
            origenRaycast = transform;
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

        // Mover la burbuja en la dirección asignada
        MoverBurbuja();
    }

    // Función para hacer que la burbuja se hunda
    void HundirBurbuja()
    {
        transform.position += Vector3.down * velocidadHundimiento * Time.deltaTime;
    }

    // Función para rotar el raycast en función de la posición del mouse
    void RotarRaycast()
    {
        // Obtener la posición del mouse en el mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // Asegurarnos de que el raycast solo se mueva en el plano XY (2D)

        // Obtener la dirección hacia el mouse
        raycastDireccion = (mousePos - origenRaycast.position).normalized;

        // Rotar el origenRaycast para que apunte hacia el mouse
        float angle = Mathf.Atan2(raycastDireccion.y, raycastDireccion.x) * Mathf.Rad2Deg;
        origenRaycast.rotation = Quaternion.Slerp(origenRaycast.rotation, Quaternion.Euler(0f, 0f, angle), velocidadRotacion * Time.deltaTime);
    }

    // Función para lanzar el raycast y detectar colisiones
    void LanzarRaycast()
    {
        // Dibujar el raycast para ver la dirección en la escena (utilizando Gizmos)
        Debug.DrawRay(origenRaycast.position, raycastDireccion * raycastDistancia, Color.red);

        // Lanzar el raycast
        if (Physics.Raycast(origenRaycast.position, raycastDireccion, out hitInfo, raycastDistancia, capaInteractuable))
        {
            // Si el raycast golpea algo, mostrar la información
            Debug.Log("Raycast impactó con: " + hitInfo.collider.name);

            // Identificar colisiones específicas para mover la burbuja
            switch (hitInfo.collider.tag)
            {
                case "Izquierda1":
                    AsignarMovimiento(true, velocidadIzquierda1);
                    break;

                case "Izquierda2":
                    AsignarMovimiento(true, velocidadIzquierda2);
                    break;

                case "Derecha1":
                    AsignarMovimiento(false, velocidadDerecha1);
                    break;

                case "Derecha2":
                    AsignarMovimiento(false, velocidadDerecha2);
                    break;

                default:
                    DetenerMovimiento();
                    break;
            }
        }
    }

    // Asigna el movimiento según la dirección
    void AsignarMovimiento(bool izquierda, float velocidad)
    {
        moviendoIzquierda = izquierda;
        moviendoDerecha = !izquierda;
        velocidadActual = velocidad;
    }

    // Detener el movimiento
    void DetenerMovimiento()
    {
        moviendoIzquierda = false;
        moviendoDerecha = false;
        velocidadActual = 0f;
    }

    // Mover la burbuja hacia la izquierda o derecha
    void MoverBurbuja()
    {
        if (moviendoIzquierda)
        {
            transform.position += Vector3.left * velocidadActual * Time.deltaTime;
        }
        else if (moviendoDerecha)
        {
            transform.position += Vector3.right * velocidadActual * Time.deltaTime;
        }
    }

    // Dibujar el raycast con Gizmos en la vista de la escena
    void OnDrawGizmos()
    {
        if (origenRaycast == null) return;

        // Dibujar el raycast en Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origenRaycast.position, raycastDireccion * raycastDistancia);
    }
}
