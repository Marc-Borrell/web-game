using TMPro;
using UnityEngine;

public class SceneUIInitializer : MonoBehaviour
{
    public TMP_Text localUsernameText;

    void Start()
    {
        // Buscamos al GameManager persistente
        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        
        if (gm != null && localUsernameText != null)
        {
            gm.RegisterTextElement(localUsernameText);
        }
    }
}
