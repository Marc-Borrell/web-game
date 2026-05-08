using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LogicaNivel : MonoBehaviour
{
    public SecuenciaVictoria secuencia; 
    
    public List<ZonaDeteccion> todasLasZonas;
    
    
    [Header("Variables de Estado")]
    public bool cajasEnPosicion = false; // Se vuelve true cuando todas las cajas están puestas
    private bool nivelFinalizadoTotalmente = false; // Se vuelve true solo al tocar la meta
    
    
    [Header("UI")]
    public TextMeshProUGUI ScoreText;
    
    [Header("Configuración de Puntos")]
    public int puntosPorCaja = 100;
    public float tiempoMaximoBono = 60f; 
    
    private float cronometro = 0f;
    private int puntajeTotal = 0;
    private int cajasContadas = 0;

    void Start()
    {
        // Inicializamos el texto
        if (ScoreText != null) ScoreText.text = "Score: 0";
    }

    void Update()
    {
        // Si ya llegamos a la meta final, no hacemos nada más
        if (nivelFinalizadoTotalmente) return;

        cronometro += Time.deltaTime; 
        
        // Ejecutamos la lógica de revisión una sola vez por frame
        RevisarProgreso();
    }
    
    void RevisarProgreso()
    {
        int zonasOcupadasActuales = 0;

        foreach (ZonaDeteccion zona in todasLasZonas)
        {
            if (zona.estaCajaEnPosicion)
            {
                zonasOcupadasActuales++;

                // SI LA ZONA ESTÁ OCUPADA Y NUNCA DIO PUNTOS:
                if (!zona.yaDioPuntos)
                {
                    zona.yaDioPuntos = true; // La marcamos para que no repita
                    CalcularPuntosCaja();
                }
            }
        }

        // LÓGICA DE ESTADO (CAJAS LISTAS)
        if (zonasOcupadasActuales >= todasLasZonas.Count && todasLasZonas.Count > 0)
        {
            if (!cajasEnPosicion)
            {
                cajasEnPosicion = true;
                NotificarCajasListas();
            }
        }
        else
        {
            cajasEnPosicion = false;
        }
    }

    void CalcularPuntosCaja()
    {
        float multiplicador = Mathf.Max(1f, (tiempoMaximoBono - cronometro) / 10f);
        int puntosGanados = Mathf.RoundToInt(puntosPorCaja * multiplicador);
        
        puntajeTotal += puntosGanados;
        
        if (ScoreText != null) ScoreText.text = "Score: " + puntajeTotal;
        Debug.Log($"<color=cyan>¡Caja colocada! +{puntosGanados} puntos. Total: {puntajeTotal}</color>");
        ScoreText.text = "Score: " + puntajeTotal;
    }

    void NotificarCajasListas()
    {
        Debug.Log("<color=yellow>¡Todas las cajas están en su sitio! Dirígete a la salida.</color>");
        // Aquí podrías activar visualmente la meta (ej: prender una luz)
    }

    // Esta función la llamas desde el script de la META
    public void FinalizarNivelDefinitivo()
    {
        if (cajasEnPosicion && !nivelFinalizadoTotalmente)
        {
            nivelFinalizadoTotalmente = true;
            Debug.Log("<color=green><b>¡VICTORIA DEFINITIVA!</b></color>");
            // Código para cambiar de escena o mostrar menú de victoria
            secuencia.IniciarAnimacionFinal();
        }
    }
    
    public int GetPuntaje()
    {
        return puntajeTotal;
    }
}