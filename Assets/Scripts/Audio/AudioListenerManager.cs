using UnityEngine;

public class AudioListenerManager : MonoBehaviour
{
    private void Awake()
    {
        if (GameManager.Instance != null && GameManager.Instance.GetComponent<AudioListener>() != null)
        {
            AudioListener[] listeners = FindObjectsOfType<AudioListener>();
            foreach (var listener in listeners)
            {
                if (listener.gameObject != GameManager.Instance.gameObject)
                {
                    listener.enabled = false;
                }
            }
        }
    }
}
