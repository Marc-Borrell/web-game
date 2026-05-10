using UnityEngine;

public class EfectoHolograma : MonoBehaviour
{
    private Material mat;
    private Color colorBase;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        colorBase = mat.GetColor("_EmissionColor");
    }

    void Update()
    {
        // Hace que la intensidad del brillo oscile suavemente
        float parpadeo = 0.8f + Mathf.PingPong(Time.time * 0.5f, 0.4f);
        // Opcional: parpadeo rápido tipo interferencia
        if (Random.value > 0.98f) parpadeo = 0.2f; 

        mat.SetColor("_EmissionColor", colorBase * parpadeo);
    }
}