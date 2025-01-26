using UnityEngine;

public class MinijuegoPilas : MonoBehaviour
{
    public GameObject objetoIzquierdo; // Objeto que está a la izquierda
    public GameObject objetoDerecho; // Objeto que está a la derecha
    public GameObject zonaObjetivo; // Zona donde debe estar el objeto derecho
    public GameObject panelMinijuego; // Panel del minijuego para activarlo/desactivarlo

    public bool derechoFueraDeZona = false; // Detecta si el objeto derecho salió de la zona objetivo
    public bool izquierdoEnZona = false; // Detecta si el objeto izquierdo tocó la zona objetivo al menos una vez

    private Collider2D zonaCollider; // Collider de la zona objetivo
    private Collider2D izquierdoCollider; // Collider del objeto izquierdo
    private bool minijuegoTerminado = false; // Para evitar repetir la lógica de ganar

    void Start()
    {
        // Obtener los colliders necesarios
        zonaCollider = zonaObjetivo.GetComponent<Collider2D>();
        izquierdoCollider = objetoIzquierdo.GetComponent<Collider2D>();

        // Desactivar el collider del objeto izquierdo al inicio
        izquierdoCollider.enabled = false;

        // Asegurarnos de que el minijuego esté desactivado al inicio
        panelMinijuego.SetActive(false);
    }

    void Update()
    {
        if (minijuegoTerminado) return; // Salir si ya terminó el minijuego

        // Control del arrastre del objeto derecho
        if (Input.GetMouseButton(0))
        {
            Vector3 posicionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posicionMouse.z = 0;

            Collider2D colisionDerecho = Physics2D.OverlapPoint(posicionMouse);
            if (colisionDerecho != null && colisionDerecho.gameObject == objetoDerecho)
            {
                objetoDerecho.transform.position = posicionMouse;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Actualizar el estado de "derechoFueraDeZona"
            derechoFueraDeZona = !zonaCollider.bounds.Contains(objetoDerecho.transform.position);

            // Activar el collider del objeto izquierdo si el derecho está fuera de la zona
            if (derechoFueraDeZona)
            {
                izquierdoCollider.enabled = true;
            }
        }

        // Control del arrastre del objeto izquierdo
        if (izquierdoCollider.enabled && Input.GetMouseButton(0))
        {
            Vector3 posicionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posicionMouse.z = 0;

            Collider2D colisionIzquierdo = Physics2D.OverlapPoint(posicionMouse);
            if (colisionIzquierdo != null && colisionIzquierdo.gameObject == objetoIzquierdo)
            {
                objetoIzquierdo.transform.position = posicionMouse;
            }
        }

        // Detectar si el objeto izquierdo tocó la zona objetivo
        if (izquierdoCollider.enabled && zonaCollider.bounds.Contains(objetoIzquierdo.transform.position))
        {
            izquierdoEnZona = true;
            TerminarMinijuego();
        }
    }

    public void IniciarMinijuego()
    {
        panelMinijuego.SetActive(true);
    }

    private void TerminarMinijuego()
    {
        if (!minijuegoTerminado)
        {
            minijuegoTerminado = true; // Evitar que se repita la lógica de ganar
            Debug.Log("¡Minijuego completado!");
            panelMinijuego.SetActive(false); // Desactivar el panel del minijuego
        }
    }
}
