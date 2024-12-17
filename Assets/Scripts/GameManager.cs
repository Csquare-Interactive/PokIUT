using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerData playerData;
    
    private AudioListener audioListener;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject); // Don't destroy the GameManager when loading a new scene

            audioListener = gameObject.GetComponent<AudioListener>();
            if (audioListener == null)
            {
                audioListener = gameObject.AddComponent<AudioListener>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
