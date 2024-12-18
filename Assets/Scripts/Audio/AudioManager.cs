using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            AudioListener audioListener = gameObject.GetComponent<AudioListener>();
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
