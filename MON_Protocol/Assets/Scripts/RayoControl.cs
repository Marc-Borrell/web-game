using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RayoControl : MonoBehaviour
{
    public List<GameObject> rayos;
    public float intervalo = 5f;
    
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > intervalo)
        {
            CalcularTiempo();
            timer = 0f;
        }
        
    }

    private void CalcularTiempo()
    {
        foreach (GameObject rayo in rayos)
        {
            if (rayo != null)
            {
                rayo.SetActive(!rayo.activeSelf);
            }
        }
    }

    
}
