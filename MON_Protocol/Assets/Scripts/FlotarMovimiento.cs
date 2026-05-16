using UnityEngine;

public partial class HoverEffect : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float amplitud = 0.5f;  // Qué tan arriba/abajo llega
    public float frecuencia = 1f;  // Qué tan rápido flota

    [Header("Ajustes de Rotación")]
    public float velocidadGiro = 20f; // Para que rote sobre su eje (opcional)

    private Vector3 posicionInicial;

    void Start()
    {
        // Guardamos la posición donde pusiste el nanobot en la escena
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        // Calculamos la nueva posición en el eje Y usando la función Sin
        float nuevaY = Mathf.Sin(Time.time * frecuencia) * amplitud;
        
        // Aplicamos el movimiento respecto a la posición inicial
        transform.localPosition = posicionInicial + new Vector3(0, nuevaY, 0);

        // Opcional: Hace que el nanobot gire lentamente para darle más vida
        transform.Rotate(Vector3.up, velocidadGiro * Time.deltaTime);
    }
}
