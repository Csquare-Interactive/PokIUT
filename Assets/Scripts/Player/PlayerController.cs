using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private BattleManager battleManager;
    public string combatSceneName = "BattleScene";
    [Header("Healing Station")]
    public float interactionDistance = 3f;
    [Header("Grass Encounter")]
    public float encounterRate = 0.03f;
    public float distanceThreshold = 1f;
    private bool isInGrass = false;    
    private Vector3 lastPosition;
    private GameObject currentGrass;

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

    private bool IsMoving()=> Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

    private void TryStartEncounter()
    {
        if (Random.value < encounterRate)
        {
            StartEncounter(currentGrass.GetComponent<TallGrassManager>().GetRandomEnemy());
        }
    }

    private void StartEncounter(EnemyData enemyData)
    {
        Debug.Log("Encounter started!");
        BattleDataManager.Instance.SetEnemyData(enemyData);
        SceneManager.LoadScene(combatSceneName, LoadSceneMode.Additive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grass"))
        {
            isInGrass = true;
            currentGrass = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grass"))
        {
            isInGrass = false;
            currentGrass = null;
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