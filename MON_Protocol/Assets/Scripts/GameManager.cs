using UnityEngine.Networking;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;

public class GameManager : MonoBehaviour
{
    private string authToken;
    private string usuario;

    // Angular llama a este método via SendMessage
    public void SetAuthToken(string token, string username)
    {
        authToken = token;
        usuario = username;
        Debug.Log("Token recibido: " + token);
        Debug.Log("Usuario actual" + username);
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