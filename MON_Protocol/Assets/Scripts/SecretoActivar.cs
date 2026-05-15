using UnityEngine;
using UnityEngine.UI;

public class SecretoAcitvar : MonoBehaviour
{
    private int contador = 0;
    public int metaDeClics = 20;

    public GameObject btnSecreto;
    
    public void IncrementarContador()
    {
        contador++;
        
        // Opcional: Debug para ver el progreso actual
        //Debug.Log("Clics actuales: " + contador);
        
        if (contador >= metaDeClics) 
        {
            Debug.Log("¡Se han alcanzado los clics! Funciona correctamente.");
            btnSecreto.SetActive(true);
            // Si quieres que se reinicie después de avisar:
            // contador = 0;
        }
    }
}
