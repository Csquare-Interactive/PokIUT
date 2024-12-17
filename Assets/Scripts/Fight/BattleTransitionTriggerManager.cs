using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTransitionTriggerManager : MonoBehaviour
{
     private Collider battleCollider;
     public string combatSceneName = "CombatScene";

    private void Start()
    {
        battleCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerData playerData = GameManager.Instance.playerData;
            playerData.lastPosition = other.transform.position;
            SceneManager.LoadScene(combatSceneName, LoadSceneMode.Additive);
            DisableCollider();
        }
    }

    public void DisableCollider()
    {
        if (battleCollider != null)
        {
            battleCollider.enabled = false;
        }
    }

    public void EnableCollider()
    {
        if (battleCollider != null)
        {
            battleCollider.enabled = true;
        }
    }
}
