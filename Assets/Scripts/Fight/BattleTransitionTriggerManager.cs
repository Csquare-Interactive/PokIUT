using UnityEngine;

public class BattleTransitionTriggerManager : MonoBehaviour
{
     private Collider battleCollider; 

    private void Start()
    {
        battleCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered battle trigger");
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
