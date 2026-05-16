using UnityEngine;

public class Meta : MonoBehaviour
{
    // Aquí es donde arrastras el objeto que tiene el script LogicaNivel
    public LogicaNivel manager; 

    private void OnTriggerEnter(Collider other)
    {
        // Verifica que el objeto tenga el Tag "Player"
        if (other.CompareTag("Player"))
        {
            // Verificamos si el manager existe y si ya detectó todas las cajas
            if (manager != null)
            {
                if (manager.cajasEnPosicion) 
                {
                    // Llamamos a la función de victoria que creamos en LogicaNivel
                    manager.FinalizarNivelDefinitivo();
                }
                else
                {
                    Debug.Log("<color=orange>Meta: Aún no has acomodado todas las cajas.</color>");
                }
            }
        }
    }
}