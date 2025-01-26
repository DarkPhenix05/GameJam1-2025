using UnityEngine;

public class BurbujaScript : MonoBehaviour
{
    public float velocidadHundimiento = 2f; // Velocidad de hundimiento de la burbuja
    public float raycastDistancia = 10f; // Distancia del raycast
    public float velocidadRotacion = 5f; // Velocidad de rotaci�n del raycast (basado en el mouse)
    public LayerMask capaInteractuable; // Capa que el raycast puede impactar (por ejemplo, objetos que interact�an con la burbuja)

    public Transform origenRaycast; // El Transform del origen del raycast (puedes asignar cualquier GameObject aqu�)
    private RaycastHit hitInfo; // Informaci�n del raycast
    private Vector3 raycastDireccion; // Direcci�n del raycast

    void Start()
    {
        // Si no se asign� un origen del raycast, usamos el centro de la burbuja (el mismo GameObject)
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
    }


    // Funci�n para hacer que la burbuja se hunda
    void HundirBurbuja()
    {
        transform.position += Vector3.down * velocidadHundimiento * Time.deltaTime;
    }

    // Funci�n para rotar el raycast en funci�n de la posici�n del mouse
    void RotarRaycast()
    {
        // Obtener la posici�n del mouse en el mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // Asegurarnos de que el raycast solo se mueva en el plano XY (2D)

        // Obtener la direcci�n hacia el mouse
        raycastDireccion = (mousePos - origenRaycast.position).normalized;

        // Rotar el origenRaycast para que apunte hacia el mouse
        float angle = Mathf.Atan2(raycastDireccion.y, raycastDireccion.x) * Mathf.Rad2Deg;
        origenRaycast.rotation = Quaternion.Slerp(origenRaycast.rotation, Quaternion.Euler(0f, 0f, angle), velocidadRotacion * Time.deltaTime);
    }

    // Funci�n para lanzar el raycast y detectar colisiones
    void LanzarRaycast()
    {
        // Dibujar el raycast para ver la direcci�n en la escena (utilizando Gizmos)
        Debug.DrawRay(origenRaycast.position, raycastDireccion * raycastDistancia, Color.red);

        // Lanzar el raycast
        if (Physics.Raycast(origenRaycast.position, raycastDireccion, out hitInfo, raycastDistancia, capaInteractuable))
        {
            // Si el raycast golpea algo, mostrar la informaci�n
            Debug.Log("Raycast impact� con: " + hitInfo.collider.name);
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