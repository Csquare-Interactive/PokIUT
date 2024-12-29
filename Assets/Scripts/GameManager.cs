using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerData playerData;
    public Shop shop;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Screen.SetResolution(2560, 1440, FullScreenMode.FullScreenWindow);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            Screen.SetResolution(3840, 2160, FullScreenMode.FullScreenWindow);
        }
    }
}
