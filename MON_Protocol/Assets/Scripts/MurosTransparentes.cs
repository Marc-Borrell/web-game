using UnityEngine;

public class OcultarMuros : MonoBehaviour
{
    public Transform objetivo; // El Bunny
    public float transparenciaBaja = 0.2f;
    private GameObject ultimoMuro;
    private Color colorOriginal;

    void Update()
    {
        // Lanzamos un rayo desde la cámara hacia el Bunny
        Vector3 direccion = objetivo.position - transform.position;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direccion, out hit))
        {
            if (hit.collider.CompareTag("Muro")) // Asegúrate de que tus muros tengan el Tag "Muro"
            {
                GameObject muroActual = hit.collider.gameObject;
                
                // Si es un muro nuevo, restauramos el anterior y desvanecemos el nuevo
                if (ultimoMuro != muroActual)
                {
                    RestaurarMuro();
                    ultimoMuro = muroActual;
                    Renderer rend = ultimoMuro.GetComponent<Renderer>();
                    colorOriginal = rend.material.color;
                    
                    // Cambiamos a transparencia (necesitas que el material sea Transparent)
                    Color c = colorOriginal;
                    c.a = transparenciaBaja;
                    rend.material.color = c;
                }
            }
            else
            {
                RestaurarMuro();
            }
        }
    }

    void RestaurarMuro()
    {
        if (ultimoMuro != null)
        {
            ultimoMuro.GetComponent<Renderer>().material.color = colorOriginal;
            ultimoMuro = null;
        }
    }
}