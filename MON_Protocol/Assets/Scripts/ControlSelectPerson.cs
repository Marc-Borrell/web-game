using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement; 

public class ControlSelectPerson : MonoBehaviour
{
    public GameObject panelSelector;
    public GameObject panelPrincipal;

    public void VolverMenu()
    {
        panelSelector.SetActive(false);
        panelPrincipal.SetActive(true); // Opcional: vuelve a mostrar el menú de inicio
    }

    public void SelectPerson()
    {
        panelSelector.SetActive(true);
        panelPrincipal.SetActive(false); // Opcional: oculta el menú principal para ver solo los personajes
    }
    
    public void SeleccionarPersonaje(int idPersonaje)
    {
        // 1. Guardamos la info en el objeto que no se destruye
        if (SeleccionDatos.instancia != null)
        {
            SeleccionDatos.instancia.personajeSeleccionado = idPersonaje;
            Debug.Log("Personaje guardado con ID: " + idPersonaje);
        }

        // 2. Cargamos la escena del laboratorio de juego
        // Pon aquí el nombre exacto de tu escena de nivel entre comillas
        //SceneManager.LoadScene("EscenaLaboratorio"); 
    }
}