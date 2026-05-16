using UnityEngine;

public class ZonaDeteccion : MonoBehaviour
{
    public LayerMask capaCajas;
    public float distanciaRayo = 1.0f;
    
    [HideInInspector] 
    public bool estaCajaEnPosicion = false; 
    public bool yaDioPuntos = false;

    void Update()
    {
        Vector3 origen = transform.position + Vector3.up * 0.2f;
        RaycastHit hit;

        // Lanzamos el rayo constantemente para verificar presencia
        bool hayCajaAhora = false;

        if (Physics.Raycast(origen, Vector3.up, out hit, distanciaRayo, capaCajas))
        {
            if (hit.collider.CompareTag("Caja"))
            {
                hayCajaAhora = true;
            }
        }

        // --- LÓGICA DE CAMBIO DE ESTADO ---

        // 1. Si la caja ACABA DE ENTRAR
        if (hayCajaAhora && !estaCajaEnPosicion)
        {
            estaCajaEnPosicion = true;
            Debug.Log("<color=yellow>¡Zona " + gameObject.name + " OCUPADA!</color>");
            
          //  GestionarMuro(true);
        }
        // 2. Si la caja ACABA DE SALIR
        else if (!hayCajaAhora && estaCajaEnPosicion)
        {
            estaCajaEnPosicion = false;
            Debug.Log("<color=red>Zona " + gameObject.name + " LIBERADA.</color>");
            
          //  GestionarMuro(false);
        }

        // Visual de depuración (Verde si hay caja, Rojo si está vacía)
        Debug.DrawRay(origen, Vector3.up * distanciaRayo, hayCajaAhora ? Color.green : Color.red);
    }

    /*
    private void GestionarMuro(bool activar)
    {
        Transform tBlock = transform.Find("TransparentBloc");
        if (tBlock != null)
        {
            tBlock.gameObject.SetActive(activar);
        }
    }*/
}