using UnityEngine.Networking;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        // Solo permite que exista un GameManager en todo el juego
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var tmp = GameObject.Find("usernameText");
        if (tmp != null)
            usernameText = tmp.GetComponent<TMP_Text>();

        if (usernameText != null && !string.IsNullOrEmpty(usuario))
            usernameText.text = GetDisplayName();
    }

    private string authToken;
    private string usuario;

    public TMP_Text usernameText;

    // Angular llama a este m�todo via SendMessage
    [System.Serializable]
    public class AuthPayload
    {
        public string token;
        public string username;
    }

    public void SetAuthToken(string json)
    {
        AuthPayload data = JsonUtility.FromJson<AuthPayload>(json);

        authToken = data.token;
        usuario = data.username;

        Debug.Log("Token recibido: " + authToken);
        Debug.Log("Usuario actual: " + usuario);
        
        
        if (usernameText != null)
        {
            usernameText.text = GetDisplayName();
        }
        
    }

    public void LoadLevel(string levelName)
    {
        Debug.Log("Cargando nivel: " + levelName);
        SceneManager.LoadScene(levelName);
    }

    public string GetDisplayName()
    {
        if (usuario.Length >= 11)
        {
            return usuario.Substring(0, 11) + "...";
        }
        
        return usuario;
    }
    
    public void PausarAudio(string value)
    {
        if (AudioManager.instancia != null)
        {
            AudioManager.instancia.PausarMusica(value);
        }
    }

    public void DetenerAudio(string value)
    {
        if (AudioManager.instancia != null)
        {
            AudioManager.instancia.DetenerMusica(value);
        }
    }
    
    


    // Ejemplo: guardar score al acabar un nivel
   /* public void SaveScore(int levelId, int moves, int timeMs)
    {
        StartCoroutine(PostScore(levelId, moves, timeMs));
    } 

    private IEnumerator PostScore(int levelId, int moves, int timeMs)
    {
        var body = new ScorePayload { level_id = levelId, moves = moves, time_ms = timeMs };
        string json = JsonUtility.ToJson(body);

        using var request = new UnityWebRequest("https://tu-api.com/scores", "POST");
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken); // JWT

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Score guardado: " + request.downloadHandler.text);
        else
            Debug.LogError("Error: " + request.error);
    } */
}

[System.Serializable]
public class ScorePayload
{
    public int level_id;
    public int moves;
    public int time_ms;
} 


[System.Serializable]
public class AuthResponse
{
    public string msg;
    public string token;
    public User user;
}

[System.Serializable]
public class User
{
    public int id;
    public string name;
    public string email;
} 