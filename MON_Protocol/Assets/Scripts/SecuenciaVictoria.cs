using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine.SceneManagement;

public class SecuenciaVictoria : MonoBehaviour
{
    public Transform elevador;
    private GameObject player; // El conejo (Bunny)
    public GameObject nanobot;
    public TextMeshPro PlayerName;
    
    public LogicaNivel manager;
    
    [Header("Ajustes de Movimiento")]
    public float velocidad = 8f;
    public float alturaInicial = 30f;
    public float alturaFinal = 0.1f;

    [Header("Tiempos de Espera (Cinemática)")]
    public float esperaTrasMeta = 1.0f; // Pausa antes de que baje
    public float esperaParaDesaparecer = 0.5f; // Tiempo tras llegar abajo para apagar personajes
    public float esperaTrasDesaparecer = 1.0f; // Pausa dramática en el suelo vacío
    public float esperaArribaDefinitiva = 1.0f; // Pausa arriba antes de mostrar puntos
    
    [Header("Scripts a Detener")]
    public TimeControl timer; // Arrastra aquí el objeto que tiene el cronómetro
    
    [Header("UI de Victoria")]
    public GameObject panelJuego; 
    public GameObject panelVictoria; 
    public TextMeshProUGUI textoPuntosFinal;
    public TextMeshProUGUI textoTiempoFinal;
    public GameObject btnContinuar;
    
    [Header("Alternancia de Materiales Smooth")]
    public Renderer hologramaRenderer;
    public Material materialTransparente; // Tu material original 'Elevador'
    public Material materialOpaco;        // Tu material duplicado 'Elevador_Opaco'
    public float duracionTransicion = 0.6f; // Cuánto tarda en encenderse/apagarse la luz suavemente
    
    
    private Color colorBaseCian = new Color(0f, 150f / 255f, 191f / 255f);
    private float intensidadMinima = 2.41f; // Brillo original en tu Inspector
    private float intensidadMaxima = 20f;   // Brillo masivo antes de volverse 100% opaco
    
    [DllImport("__Internal")]
    private static extern void SendMessageToAngular(string json);

    [System.Serializable]
    private class GameOverData
    {
        public string type = "GAME_OVER";
        public int level_id;
        public int moves;
        public int time_ms;
    }

    public void IniciarAnimacionFinal()
    {
        player = GameObject.FindWithTag("Player");
        
        if (hologramaRenderer != null && materialTransparente != null)
        {
            // Creamos una instancia única por código para no sobreescribir el archivo original
            materialTransparente = hologramaRenderer.material;
            SetMaterialPropiedades(intensidadMinima, 0.5f); // Estado base inicial translúcido
        }
        
        if (player != null)
        {
            Debug.Log("¡Jugador detectado dinámicamente para la cinemática!: " + player.name);
            StartCoroutine(RutinaExtraccion());
        }
        else
        {
            Debug.LogError("¡ERROR CRÍTICO! No se encontró ningún objeto con el tag 'Player' activo en la escena.");
        }
    }
    
    // Función auxiliar para modificar dinámicamente el brillo y el alfa del material transparente
    void SetMaterialPropiedades(float intensidad, float alfa)
    {
        if (materialTransparente == null) return;
        
        // 1. Modificamos la intensidad de la Emisión HDR
        Color colorEmision = colorBaseCian * intensidad;
        materialTransparente.SetColor("_EmissionColor", colorEmision);
        
        // 2. Modificamos la opacidad de la Base (Base Color)
        Color colorBase = colorBaseCian;
        colorBase.a = alfa;
        materialTransparente.SetColor("_BaseColor", colorBase);

        DynamicGI.SetEmissive(hologramaRenderer, colorEmision);
    }

    // Corrutina interna para suavizar la interpolación de la luz usando Mathf.SmoothStep
    IEnumerator TransicionEnergia(float desdeIntensidad, float hastaIntensidad, float desdeAlfa, float hastaAlfa)
    {
        float tiempo = 0f;
        while (tiempo < duracionTransicion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionTransicion;
            
            // Suavizado en las esquinas (aceleración y desaceleración fluida)
            float tSuave = Mathf.SmoothStep(0f, 1f, t);

            float intActual = Mathf.Lerp(desdeIntensidad, hastaIntensidad, tSuave);
            float alfaActual = Mathf.Lerp(desdeAlfa, hastaAlfa, tSuave);
            
            SetMaterialPropiedades(intActual, alfaActual);
            yield return null;
        }
    }
    
    IEnumerator CargarHolograma()
    {
        // 1. EFECTO SMOOTH: Incrementamos energía y opacidad progresivamente
        Debug.Log("Incrementando energía del holograma de forma suave...");
        yield return StartCoroutine(TransicionEnergia(intensidadMinima, intensidadMaxima, 0.5f, 0.95f));

        // 2. INTERCAMBIO INVISIBLE: Cambiamos al material opaco con Bloom máximo
        Debug.Log("Cambiando a material opaco con Bloom masivo...");
        if (hologramaRenderer != null && materialOpaco != null)
        {
            hologramaRenderer.material = materialOpaco;
        }
        
        // Esperamos el tiempo establecido de abordaje con la pantalla cubierta de luz
        yield return new WaitForSeconds(esperaParaDesaparecer);
        Debug.Log("Desactivando personajes detrás del escudo de luz.");
        
        // 3. LA DESAPARICIÓN FÍSICA (100% oculta tras la luz sólida)
        if (player != null) player.SetActive(false);
        if (nanobot != null) nanobot.SetActive(false);
        if (PlayerName != null) PlayerName.SetText("");

        // 4. RESTAURAR MATERIAL TRANSPARENTE (Vuelve a su punto álgido de brillo)
        if (hologramaRenderer != null && materialTransparente != null)
        {
            hologramaRenderer.material = materialTransparente;
        }

        // 5. EFECTO SMOOTH: Desvanecemos el brillo de vuelta a la normalidad
        Debug.Log("Estabilizando energía y disipando el Bloom de forma suave...");
        yield return StartCoroutine(TransicionEnergia(intensidadMaxima, intensidadMinima, 0.95f, 0.5f));
    }

    IEnumerator RutinaExtraccion()
    {
        if (timer != null) timer.enabled = false;
        
        //BLOQUEO TOTAL DEL JUGADOR
        Debug.Log("Bloqueando movimiento del jugador...");
    
        // Buscamos el componente de movimiento y lo desactivamos
        // Cambia "MovimientoJugador" por el nombre real de tu script
        var scriptMovimiento = player.GetComponent<MovimientoJugador>(); 
        if (scriptMovimiento != null) 
        {
            scriptMovimiento.enabled = false; 
        }
        
        // 1. BLOQUEO VISUAL
        Debug.Log("Pausa pre-ascensor...");
        // Opcional: Si tienes una UI de "Espera...", actívala aquí
        yield return new WaitForSeconds(esperaTrasMeta);

        // 2. BAJAR EL ELEVADOR (Transparente)
        Debug.Log("Bajando elevador...");
        while (elevador.position.y > alturaFinal + 0.05f)
        {
            elevador.position = Vector3.MoveTowards(elevador.position, 
                new Vector3(elevador.position.x, alturaFinal, elevador.position.z), 
                velocidad * Time.deltaTime);
            yield return null;
        }

        // 3. SECUENCIA DE ABORDAJE (Carga smooth, desactiva y descarga smooth)
        yield return StartCoroutine(CargarHolograma());
        yield return new WaitForSeconds(esperaTrasDesaparecer);

        // 4. SUBIR EL ELEVADOR (Vacío y estabilizado)
        Debug.Log("Subiendo elevador...");
        while (elevador.position.y < alturaInicial - 0.05f)
        {
            elevador.position = Vector3.MoveTowards(elevador.position, 
                new Vector3(elevador.position.x, alturaInicial, elevador.position.z), 
                velocidad * Time.deltaTime);
            yield return null;
        }

        // 5. FINALIZAR SECUENCIA
        yield return new WaitForSeconds(esperaArribaDefinitiva);
        Debug.Log("¡Secuencia terminada! Listo para el Canvas.");
        
        ActivarPanelVictoria();
    }
    
    void ActivarPanelVictoria()
    {
        // Desactivamos la UI del juego (cronómetro y score pequeño)
        if (panelJuego != null) panelJuego.SetActive(false);

        // Activamos el panel de victoria
        if (panelVictoria != null) panelVictoria.SetActive(true);

        // Rellenamos puntos
        if (manager != null)
            textoPuntosFinal.text = "score: " + manager.GetPuntaje(); 

        // Rellenamos tiempo
        if (timer != null)
        {
            int minutos = Mathf.FloorToInt(timer.tiempoEnSegundos / 60);
            int segundos = Mathf.FloorToInt(timer.tiempoEnSegundos % 60);
            textoTiempoFinal.text = string.Format("Temps: {0:00}:{1:00}", minutos, segundos);
        }
        
        int totalEscenas = SceneManager.sceneCountInBuildSettings;
        int indiceActual = SceneManager.GetActiveScene().buildIndex;
        
        if (btnContinuar != null) 
        {
            btnContinuar.SetActive(indiceActual < totalEscenas - 2);
        }
        
        // Liberar el cursor para que el jugador pueda hacer clic en el botón
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        EnviarScoreAAngular(indiceActual);
    }
    
    void EnviarScoreAAngular(int levelId)
    {
        int moves = manager != null ? manager.GetPuntaje() : 0;
        int timeMs = timer != null ? Mathf.RoundToInt(timer.tiempoEnSegundos * 1000) : 0;

        GameOverData data = new GameOverData
        {
            level_id = levelId,
            moves = moves,
            time_ms = timeMs
        };

        string json = JsonUtility.ToJson(data);
        Debug.Log("Enviando score: " + json);

#if UNITY_WEBGL && !UNITY_EDITOR
            SendMessageToAngular(json);
#endif
    }
}