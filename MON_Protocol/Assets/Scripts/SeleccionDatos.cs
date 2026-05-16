using UnityEngine;

public class SeleccionDatos : MonoBehaviour
{
    public static SeleccionDatos instancia;
    public int personajeSeleccionado; // 0 para Conejo, 1 para Robot, etc.

    void Awake()
    {
        // Patrón Singleton: Solo puede existir uno
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // ¡Aquí está el truco!
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
}
    
