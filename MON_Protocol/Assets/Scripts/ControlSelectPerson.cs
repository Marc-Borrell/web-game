using UnityEngine;
using UnityEngine.Rendering;

public class ControlSelectPerson : MonoBehaviour
{
    
    public GameObject panelSelector;
    public GameObject panelPrincipal;

    public void VolverMenu()
    {
        panelSelector.SetActive(false);
    }

    public void SelectPerson()
    {
        panelSelector.SetActive(true);
    }
    
    public void SeleccionarPersonaje(int idPersonaje)
    {
        // 1. Guardamos la info en el objeto que no se destruye
        if (SeleccionDatos.instancia != null)
        {
            SeleccionDatos.instancia.personajeSeleccionado = idPersonaje;
        }
    }
}
