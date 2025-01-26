using UnityEngine;

public class RectanguloRotativo : MonoBehaviour
{
    public Transform origen; // El punto fijo del rectángulo (puedes asignarlo en el editor)

    void Update()
    {
        // Obtener la posición del mouse en el mundo
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Nos aseguramos de que trabajamos en 2D

        // Calcular la dirección del mouse con respecto al punto de origen
        Vector3 direccion = mousePos - origen.position;

        // Obtener el ángulo de rotación de esa dirección
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // Rotar el rectángulo (transform) para que apunte hacia la dirección del mouse
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo));
    }
}
