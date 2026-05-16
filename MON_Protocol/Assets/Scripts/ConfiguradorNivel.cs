using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class ConfiguradorNivel : MonoBehaviour
{
    [Header("Configuraciones")]
    public GameObject usernameText;
    
    [Header("Modelos 3D en la escena")]
    public GameObject modeloConejo;
    public GameObject modeloPanda;
    
    [Header("AudioManager")]
    public AudioClip musica;
    [Range(0f, 1f)] public float volumenDePrueba = 0.5f;

    void Start()
    {
        if (musica != null)
        {
            AudioManager.instancia.InicializarMusicaDePrueba(musica, volumenDePrueba);
        }
        
        // Comprobamos si el objeto persistente existe en la memoria
        if (SeleccionDatos.instancia != null)
        {
            int id = SeleccionDatos.instancia.personajeSeleccionado;
            Debug.Log("¡Éxito! El nivel leyó el ID del personaje: " + id);
            
            PositionConstraint constraint = usernameText.GetComponent<PositionConstraint>();
            Transform personajeActivo = null;

            // Activamos el personaje correspondiente
            switch (id)
            {
                case 0:
                    modeloConejo.SetActive(true);
                    modeloPanda.SetActive(false);
                    personajeActivo = modeloConejo.transform;
                    break;
                
                case 1:
                    modeloConejo.SetActive(false);
                    modeloPanda.SetActive(true);
                    personajeActivo = modeloPanda.transform;
                    break;
            }
            
            ConstraintSource nuevaFuente = new ConstraintSource();
            nuevaFuente.sourceTransform = personajeActivo;
            nuevaFuente.weight = 1f;

            constraint.AddSource(nuevaFuente);
            constraint.constraintActive = true;
        }
        else
        {
            PositionConstraint constraint = usernameText.GetComponent<PositionConstraint>();
            Transform personajeActivo = null;
            
            Debug.LogError("No se encontró SeleccionDatos. ¿Iniciaste el juego desde el menú principal?");
            // Por seguridad, si pruebas el nivel solo, activa uno por defecto
            
            modeloConejo.SetActive(true);
            personajeActivo = modeloConejo.transform;
            ConstraintSource nuevaFuente = new ConstraintSource();
            nuevaFuente.sourceTransform = personajeActivo;
            nuevaFuente.weight = 1f;

            constraint.AddSource(nuevaFuente);
            constraint.constraintActive = true;
        }
    }
}