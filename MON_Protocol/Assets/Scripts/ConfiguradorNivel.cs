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
    public GameObject personajeSecreto;
    
    [Header("AudioManager")]
    public AudioClip musica;
    [Range(0f, 1f)] public float volumenDePrueba = 0.5f;
    
    [Header("¿Es este el Nivel Secreto?")]
    public bool esNivelSecreto = false;

    void Start()
    {
        int id = 0; 
        bool tieneDatos = false;
        
        // Comprobamos si el objeto persistente existe en la memoria
        if (SeleccionDatos.instancia != null)
        {
            id = SeleccionDatos.instancia.personajeSeleccionado;
            tieneDatos = true;
            Debug.Log("¡Éxito! El nivel leyó el ID del personaje: " + id);
            
            if (id == 99 && !esNivelSecreto)
            {
                id = 0;
                SeleccionDatos.instancia.personajeSeleccionado = 0; // Guardamos la corrección
                Debug.LogWarning("¡Desvío web detectado! Cambiando personaje secreto por Conejo en nivel común.");
            }
        }
        else
        {
            Debug.LogError("No se encontró SeleccionDatos. ¿Iniciaste el juego desde el menú principal?");
        }
        
        if (musica != null)
        {
            AudioManager.instancia.CambiarMusicaInteligente(musica, volumenDePrueba, esNivelSecreto);
        }
            
            
        PositionConstraint constraint = usernameText.GetComponent<PositionConstraint>();
        Transform personajeActivo = null;

        if (constraint != null)
        {
            while (constraint.sourceCount > 0)
            {
                constraint.RemoveSource(0);
            }
        }

        if (tieneDatos)
        {
            // Activamos el personaje correspondiente
            switch (id)
            {
                case 0:
                    modeloConejo.SetActive(true);
                    modeloPanda.SetActive(false);
                    if(personajeSecreto != null) personajeSecreto.SetActive(false);
                    personajeActivo = modeloConejo.transform;
                    break;
                
                case 1:
                    modeloConejo.SetActive(false);
                    modeloPanda.SetActive(true);
                    if(personajeSecreto != null) personajeSecreto.SetActive(false);
                    personajeActivo = modeloPanda.transform;
                    break;
                case 99:
                    modeloConejo.SetActive(false);
                    modeloPanda.SetActive(false);
                    if(personajeSecreto != null) personajeSecreto.SetActive(true);
                    personajeActivo = personajeSecreto.transform;
                    break;
            }
        }
        else
        {
            // Por seguridad, si estás probando la escena sola en el editor:
            modeloConejo.SetActive(true);
            modeloPanda.SetActive(false);
            if(personajeSecreto != null) personajeSecreto.SetActive(false);
            personajeActivo = modeloConejo.transform;
        }

        if (constraint != null && personajeActivo != null)
        {
            ConstraintSource nuevaFuente = new ConstraintSource();
            nuevaFuente.sourceTransform = personajeActivo;
            nuevaFuente.weight = 1f;

            constraint.AddSource(nuevaFuente);
            constraint.constraintActive = true;
        }
    }
}