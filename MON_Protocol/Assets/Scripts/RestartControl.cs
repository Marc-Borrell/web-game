using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartControl : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene("");
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ContinueGame()
    {
        // 1. Obtenemos el índice de la escena actual (ej. si es la 1...)
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
    
        // 2. Calculamos el siguiente índice (...sería la 2)
        int nextIndex = currentIndex + 1;

        // 3. Verificamos que esa escena exista en el Build Settings para evitar errores
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("No hay más escenas después de esta en el Build Settings.");
            // Opcional: Volver al menú principal
            // 
        }
    }
}
