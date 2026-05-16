using TMPro;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    public float tiempoEnSegundos = 0f;
    
    [Header("UI")]
    public TextMeshProUGUI textoTemporizador;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tiempoEnSegundos += Time.deltaTime;
        ActualizarTextoUI();
    }
    
    private void ActualizarTextoUI()
    {
        // Formatear a minutos y segundos (ej. 01:30)
        int minutos = Mathf.FloorToInt(tiempoEnSegundos / 60);
        int segundos = Mathf.FloorToInt(tiempoEnSegundos % 60);

        textoTemporizador.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}
