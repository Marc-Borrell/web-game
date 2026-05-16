using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LlavePick : MonoBehaviour
{
    public List<GameObject> Cajas;
    
    // El método debe empezar con Mayúscula obligatoriamente
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Prueba contacto llave detectado por Trigger!");
            gameObject.SetActive(false);
            foreach(GameObject caja in Cajas)   caja.SetActive(false);
        }
    }

    // Esta es la función que llamaríamos desde la Opción 1
    public void DetectarContacto()
    {
        Debug.Log("¡Prueba contacto llave detectado por Raycast!");
    }
    
}
