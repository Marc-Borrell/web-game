using UnityEngine;

public class FlotacionEspacial : MonoBehaviour
{
    [Header("Ajustes de Rotación")]
    public Vector3 velocidadRotacion = new Vector3(0, 20f, 0); // Rotación sobre sí mismo

    [Header("Ajustes de Balanceo (Flotación)")]
    public float amplitud = 0.5f;  // Qué tanto sube y baja
    public float frecuencia = 1f; // Qué tan rápido sube y baja
    
    // Guardamos la posición inicial para que no se "vaya" volando
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
        
        // Opcional: Desfasar el inicio para que no todos floten al mismo tiempo exacto
        posicionInicial.y += Random.Range(-0.2f, 0.2f);
    }

    void Update()
    {
        // 1. ROTACIÓN CONSTANTE
        // Esto los hace girar sobre su propio eje
        transform.Rotate(velocidadRotacion * Time.deltaTime);

        // 2. MOVIMIENTO DE FLOTACIÓN (Efecto Levitar)
        // Usamos la función Matemática Sinusoidal (Sin) para crear el vaivén
        float nuevoY = posicionInicial.y + Mathf.Sin(Time.time * frecuencia) * amplitud;
        
        transform.position = new Vector3(transform.position.x, nuevoY, transform.position.z);
    }
}