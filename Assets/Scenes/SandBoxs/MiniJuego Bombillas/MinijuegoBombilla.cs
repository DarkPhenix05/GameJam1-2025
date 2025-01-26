using UnityEngine;

public class MinijuegoObjetos : MonoBehaviour
{
    public GameObject objetoDerecho; // Objeto que empieza a la derecha
    public GameObject objetoIzquierdo; // Objeto que empieza a la izquierda
    public GameObject zonaObjetivo; // Zona objetivo (posición inicial del objeto derecho)
    public GameObject panelMinijuego; // Panel o contenedor del minijuego

    private Vector3 posicionInicialDerecho; // Posición inicial del objeto derecho
    private Vector3 posicionInicialIzquierdo; // Posición inicial del objeto izquierdo

    private bool derechoMovido = false; // Indica si el objeto derecho se movió fuera de la zona objetivo
    private bool izquierdoColocado = false; // Indica si el objeto izquierdo se colocó en la zona objetivo

    void Start()
    {
        // Guardar las posiciones iniciales
        posicionInicialDerecho = objetoDerecho.transform.position;
        posicionInicialIzquierdo = objetoIzquierdo.transform.position;

        // Asegurarnos de que el minijuego esté desactivado al inicio
        panelMinijuego.SetActive(false);
    }

    void Update()
    {
        void Update()
        {
            panelMinijuego.transform.position = Camera.main.transform.position;
        }


        // Si ya se completó el minijuego, no hacer nada
        if (izquierdoColocado) return;

        // Movimiento del objeto derecho
        if (Input.GetMouseButton(0) && !derechoMovido)
        {
            MoverObjetoConMouse(objetoDerecho);

            // Verificar si el objeto derecho salió de la zona objetivo
            if (!zonaObjetivo.GetComponent<Collider2D>().bounds.Contains(objetoDerecho.transform.position))
            {
                derechoMovido = true; // Marca que el objeto derecho fue movido
                Debug.Log("¡Objeto derecho movido fuera de la zona!");
            }
        }

        // Movimiento del objeto izquierdo (solo permitido si el derecho ya se movió)
        if (Input.GetMouseButton(0) && derechoMovido)
        {
            MoverObjetoConMouse(objetoIzquierdo);

            // Verificar si el objeto izquierdo llegó a la zona objetivo
            if (zonaObjetivo.GetComponent<Collider2D>().bounds.Contains(objetoIzquierdo.transform.position))
            {
                izquierdoColocado = true; // Marca que el objeto izquierdo fue colocado
                Debug.Log("¡Minijuego completado!");
                TerminarMinijuego();
            }
        }
    }

    // Método para mover un objeto con el mouse
    private void MoverObjetoConMouse(GameObject objeto)
    {
        Vector3 posicionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionMouse.z = 0; // Mantener el objeto en el plano 2D
        objeto.transform.position = Vector3.Lerp(objeto.transform.position, posicionMouse, 0.2f); // Movimiento suave
    }

    // Método para iniciar el minijuego
    public void IniciarMinijuego()
    {
        panelMinijuego.SetActive(true); // Activar el panel del minijuego
        ResetearMinijuego(); // Asegurar que el estado inicial esté configurado
    }

    // Método para terminar el minijuego
    private void TerminarMinijuego()
    {
        // Desactivar todos los objetos del minijuego
        objetoDerecho.SetActive(false);
        objetoIzquierdo.SetActive(false);
        panelMinijuego.SetActive(false);

        // Reiniciar las posiciones después de un breve retraso
        Invoke(nameof(ResetearMinijuego), 1f); // Reinicia después de 1 segundo
    }

    // Método para resetear las posiciones y estados del minijuego
    private void ResetearMinijuego()
    {
        // Restaurar posiciones iniciales
        objetoDerecho.transform.position = posicionInicialDerecho;
        objetoIzquierdo.transform.position = posicionInicialIzquierdo;

        // Restaurar estados
        derechoMovido = false;
        izquierdoColocado = false;

        // Reactivar los objetos del minijuego
        objetoDerecho.SetActive(true);
        objetoIzquierdo.SetActive(true);
    }
}
