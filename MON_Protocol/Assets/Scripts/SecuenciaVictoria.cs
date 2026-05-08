using UnityEngine;
using System.Collections;
using TMPro;

public class SecuenciaVictoria : MonoBehaviour
{
    public Transform elevador;
    public GameObject player; // El conejo (Bunny)
    public GameObject nanobot;
    
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

    public void IniciarAnimacionFinal()
    {
        StartCoroutine(RutinaExtraccion());
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

        // 2. BAJAR EL ELEVADOR (Vacío)
        Debug.Log("Bajando elevador...");
        while (elevador.position.y > alturaFinal + 0.05f)
        {
            elevador.position = Vector3.MoveTowards(elevador.position, 
                new Vector3(elevador.position.x, alturaFinal, elevador.position.z), 
                velocidad * Time.deltaTime);
            yield return null;
        }

        // 3. LA DESAPARICIÓN (Abordaje)
        yield return new WaitForSeconds(esperaParaDesaparecer);
        Debug.Log("¡Extrayendo personajes! (Desactivando objetos)");
        
        // Simplemente los apagamos. Esto quita movimiento, render y sombra.
        if (player != null) player.SetActive(false);
        if (nanobot != null) nanobot.SetActive(false);

        // Pausa para que se note que "ya no están" y sube la plataforma vacía
        yield return new WaitForSeconds(esperaTrasDesaparecer);

        // 4. SUBIR EL ELEVADOR (Vacío)
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
        
        // Aquí conectaremos el Canvas de puntuación en el próximo paso
        
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
            textoPuntosFinal.text = "Puntos: " + manager.GetPuntaje(); 

        // Rellenamos tiempo
        if (timer != null)
        {
            int minutos = Mathf.FloorToInt(timer.tiempoEnSegundos / 60);
            int segundos = Mathf.FloorToInt(timer.tiempoEnSegundos % 60);
            textoTiempoFinal.text = string.Format("Tiempo: {0:00}:{1:00}", minutos, segundos);
        }
        
        // Liberar el cursor para que el jugador pueda hacer clic en el botón
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}