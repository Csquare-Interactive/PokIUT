using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float interactionDistance = 3f;
    private bool isInGrass = false;
    private float encounterRate = 0.03f;
    private Vector3 lastPosition;
    private float distanceThreshold = 1f;
    private BattleManager battleManager;


    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForHealingStation();
        }

       if (isInGrass && IsMoving())
        {
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);
            if (distanceMoved >= distanceThreshold)
            {
                TryStartEncounter();
                lastPosition = transform.position; 
            }
        }
    }

    private bool IsMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private void TryStartEncounter()
    {
        if (Random.value < encounterRate)
        {
            StartEncounter();
        }
    }

    private void StartEncounter()
    {
        Debug.Log("Encounter started!");
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grass"))
        {
            isInGrass = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grass"))
        {
            isInGrass = false;
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