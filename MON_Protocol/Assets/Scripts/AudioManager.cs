using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instancia;

    // Propiedad pública para acceder al AudioManager desde cualquier lugar
    public static AudioManager instancia
    {
        get
        {
            // Si algún script pide el AudioManager y no existe en la escena...
            if (_instancia == null)
            {
                // Buscamos si hay uno en la escena por si acaso
                _instancia = FindObjectOfType<AudioManager>();

                // Si de verdad no existe (porque iniciamos directamente en el Nivel 1)
                if (_instancia == null)
                {
                    // Creamos un objeto nuevo en tiempo de ejecución
                    GameObject nuevoManager = new GameObject("AudioManager_Autogenerado");
                    _instancia = nuevoManager.AddComponent<AudioManager>();
                    
                    Debug.Log("AudioManager creado dinámicamente porque se inició el juego desde un nivel de pruebas.");
                }
            }
            return _instancia;
        }
    }

    [Header("Configuración de Audio")]
    public AudioClip musicaFondo;
    [Range(0f, 1f)] public float volumen = 0.5f;

    private AudioSource audioSource;

    void Awake()
    {
        // Si ya hay una instancia asignada y no soy yo, me destruyo (Duplicados)
        if (_instancia != null && _instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        _instancia = this;
        DontDestroyOnLoad(gameObject);

        // Si el AudioSource no existe, lo configuramos
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.volume = volumen;
            audioSource.playOnAwake = false;
        }

        // Si ya tenemos un clip asignado (desde el inspector del nivel 0), lo reproducimos
        if (musicaFondo != null && !audioSource.isPlaying)
        {
            audioSource.clip = musicaFondo;
            audioSource.Play();
        }
    }

    // Método para que los niveles de prueba le puedan inyectar la música de forma dinámica
    public void InicializarMusicaDePrueba(AudioClip clipDePrueba, float volumenDePrueba)
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
        }

        if (audioSource.clip == null) // Solo si no hay música sonando ya
        {
            this.volumen = volumenDePrueba;
            audioSource.volume = volumenDePrueba;
            audioSource.clip = clipDePrueba;
            audioSource.Play();
        }
    }
    
    void OnApplicationFocus(bool tieneFoco)
    {
        if (audioSource != null)
        {
            if (tieneFoco)
            {
                // El usuario regresó a la pestaña del juego: reactivamos el volumen
                audioSource.volume = volumen; 
            }
            else
            {
                // El usuario cambió de pestaña: silenciamos por completo
                audioSource.volume = 0f; 
            }
        }
    }
    
    public void PausarMusica(string value)
    {
        if (audioSource != null)
        {
            if (value == "true")
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.volume = volumen;
                audioSource.UnPause();
            }
        }
    }

    public void DetenerMusica(string value)
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}