using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float interactionDistance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForHealingStation();
        }
    }

    private void CheckForHealingStation()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.green, 2f); 

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Debug.Log("Raycast hit: " + hit.collider.name); 

            if (hit.collider.CompareTag("HealingStation"))
            {
                Debug.Log("HealingStation detected!"); 
                hit.collider.GetComponent<HealingStation>().HealAllPokeIUTs();
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any object."); 
        }
    }
}