using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonSecreto : MonoBehaviour
{
    [Header("Configuración del Secreto")]
    public string nombreEscenaSecreta = "Nivel_Secreto";
    public int idPersonajeEspecial = 99; // Usamos 99 para diferenciarlo de Bunny(0) y Panda(1)

    public void AccederAlNivelSecreto()
    {
        // 1. Forzamos el ID especial en el banco de memoria persistente
        if (SeleccionDatos.instancia != null)
        {
            SeleccionDatos.instancia.personajeSeleccionado = idPersonajeEspecial;
            Debug.Log("¡Acceso concedido! Personaje especial configurado con ID: " + idPersonajeEspecial);
        }
        else
        {
            Debug.LogWarning("No se encontró SeleccionDatos en el menú de inicio.");
        }

        // 2. Cargamos directamente la escena oculta
        SceneManager.LoadScene(nombreEscenaSecreta);
    }
}
