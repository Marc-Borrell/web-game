using UnityEngine;

public class RotacionCamara : MonoBehaviour
{
    [Header("Ajustes de Rotación")]
    public float velocidadGiro = 20f;
    public bool rotarAutomaticamente = false;

    void Update()
    {
        if (rotarAutomaticamente)
        {
            // Gira la cámara sobre su eje Y (horizontal)
            transform.Rotate(Vector3.up * velocidadGiro * Time.deltaTime);
        }
    }

    // Función para activar el giro desde otros scripts
    public void EmpezarGiro()
    {
        rotarAutomaticamente = true;
    }

    public void DetenerGiro()
    {
        rotarAutomaticamente = false;
    }
}
