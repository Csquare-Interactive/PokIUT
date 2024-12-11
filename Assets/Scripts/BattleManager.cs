using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Image = UnityEngine.UI.Image;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public PokeIUTData playerPokeIUT;
    public PokeIUTData enemyPokeIUT;

    public Text playerNameText;
    public Text playerLevelText;
    public Text playerHealthText;
    public Image playerIcon;

    public Text enemyNameText;
    public Text enemyLevelText;
    public Text enemyHealthText;
    public Image enemyIcon;

    public Button fightButton;
    public Button bagButton;
    public Button pokeiutButton;
    public Button runButton;

    public Button[] capaciteButtons;

    private int initialEnemyHealth;
    private int initialPlayerHealth;
    private int[] initialPlayerCapacitePP;

    private bool playerTurn;


    void Start()
    {
        SetupBattle();
    }

    void SetupBattle()
    {
        if (playerPokeIUT == null) Debug.LogError("playerPokeIUT is not assigned");
        if (enemyPokeIUT == null) Debug.LogError("enemyPokeIUT is not assigned");
        if (playerNameText == null) Debug.LogError("playerNameText is not assigned");
        if (playerLevelText == null) Debug.LogError("playerLevelText is not assigned");
        if (playerHealthText == null) Debug.LogError("playerHealthText is not assigned");
        if (playerIcon == null) Debug.LogError("playerIcon is not assigned");
        if (enemyNameText == null) Debug.LogError("enemyNameText is not assigned");
        if (enemyLevelText == null) Debug.LogError("enemyLevelText is not assigned");
        if (enemyHealthText == null) Debug.LogError("enemyHealthText is not assigned");
        if (enemyIcon == null) Debug.LogError("enemyIcon is not assigned");

        if (playerPokeIUT == null || enemyPokeIUT == null || playerNameText == null || playerLevelText == null || playerHealthText == null || playerIcon == null || enemyNameText == null || enemyLevelText == null || enemyHealthText == null || enemyIcon == null)
        {
            return;
        }




        playerTurn = playerPokeIUT.speed >= enemyPokeIUT.speed;

        Debug.Log(playerTurn ? "Player goes first!" : "Enemy goes first!");

        // Store initial health and power points
        initialEnemyHealth = enemyPokeIUT.health;
        initialPlayerHealth = playerPokeIUT.health;

  

        playerNameText.text = playerPokeIUT.pokeiutName;
        playerLevelText.text = "Lvl " + playerPokeIUT.level;
        playerHealthText.text = "HP " + playerPokeIUT.health;
        playerIcon.sprite = playerPokeIUT.icon;

        enemyNameText.text = enemyPokeIUT.pokeiutName;
        enemyLevelText.text = "Lvl " + enemyPokeIUT.level;
        enemyHealthText.text = "HP " + enemyPokeIUT.health;
        enemyIcon.sprite = enemyPokeIUT.icon;

        // Initialisation des points de puissance (PP) de base
        initialPlayerCapacitePP = new int[playerPokeIUT.capacites.Count];
        for (int i = 0; i < playerPokeIUT.capacites.Count; i++)
        {
            initialPlayerCapacitePP[i] = playerPokeIUT.capacites[i].powerPoints; // Sauvegarde des PP
        }

        ClonePlayerCapacites();
        CloneEnemyCapacites();

        // Configuration des textes des boutons
        for (int i = 0; i < playerPokeIUT.capacites.Count; i++)
        {
            CapaciteData capacite = playerPokeIUT.capacites[i];
            capaciteButtons[i].GetComponentInChildren<Text>().text = $"{capacite.capaciteName} ({capacite.powerPoints})";
            capaciteButtons[i].gameObject.SetActive(false);
            int index = i; // Capture the current value of i
            capaciteButtons[i].onClick.AddListener(() => UseCapacite(index));
        }
    }

    public void OnFightButton()
    {
        fightButton.gameObject.SetActive(false);
        bagButton.gameObject.SetActive(false);
        pokeiutButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(false);

        for (int i = 0; i < playerPokeIUT.capacites.Count; i++)
        {
            capaciteButtons[i].gameObject.SetActive(true);
        }
    }

    public void UseCapacite(int index)
    {
        CapaciteData capacite = playerPokeIUT.capacites[index];

        if (capacite.powerPoints <= 0)
        {
            Debug.Log("No more power points for this capacity!");
            return;
        }

        capacite.powerPoints --;
        Debug.Log($"Player uses {capacite.capaciteName}!");
        enemyPokeIUT.health -= capacite.damage;

        if (enemyPokeIUT.health <=0)
        {
            enemyPokeIUT.health = 0;
            Debug.Log("Enemy defeated!");
            EndBattle();
            return;
        }

         // Mettez à jour l'affichage
        enemyHealthText.text = "HP " + enemyPokeIUT.health;
        capaciteButtons[index].GetComponentInChildren<Text>().text = $"{capacite.capaciteName} ({capacite.powerPoints})";


        // Cacher les boutons de capacités
        foreach (var button in capaciteButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Passez au tour de l'ennemi
        playerTurn = false;
        NextTurn();
    }

    void NextTurn()
    {
        if (playerTurn)
        {
            UpdateCapaciteButtonsVisibility(false); // Masquer après l'attaque
            fightButton.gameObject.SetActive(true);
            bagButton.gameObject.SetActive(true);
            pokeiutButton.gameObject.SetActive(true);
            runButton.gameObject.SetActive(true);
        }
        else
        {
            EnemyTurn();
        }
    }

    void UpdateCapaciteButtonsVisibility(bool isVisible)
    {
        for (int i = 0; i < capaciteButtons.Length; i++)
        {
            capaciteButtons[i].gameObject.SetActive(isVisible && i < playerPokeIUT.capacites.Count);
            if (isVisible && i < playerPokeIUT.capacites.Count)
            {
                capaciteButtons[i].GetComponentInChildren<Text>().text = 
                    $"{playerPokeIUT.capacites[i].capaciteName} ({playerPokeIUT.capacites[i].powerPoints})";
            }
        }
    }

    void EnablePlayerActions()
    {
        fightButton.gameObject.SetActive(true);
        bagButton.gameObject.SetActive(true);
        pokeiutButton.gameObject.SetActive(true);
        runButton.gameObject.SetActive(true);

        for (int i = 0; i < playerPokeIUT.capacites.Count; i++)
        {
            capaciteButtons[i].gameObject.SetActive(true);
        }
    }

    void EnemyTurn()
    {
        if (enemyPokeIUT.capacites == null || enemyPokeIUT.capacites.Count == 0)
        {
            Debug.LogError("Enemy has no capacities assigned!");
            return; // Ne continuez pas si l'ennemi n'a pas de capacités
        }
    
        // Sélectionner une capacité valide de l'ennemi
        CapaciteData chosenCapacite = null;
    
        // Trouver une capacité valide avec des PP restants
        List<CapaciteData> validCapacites = new List<CapaciteData>();
    
        foreach (var capacite in enemyPokeIUT.capacites)
        {
            if (capacite.powerPoints > 0)  // Si la capacité a encore des PP
            {
                validCapacites.Add(capacite); // Ajouter à la liste des capacités valides
            }
        }
    
        if (validCapacites.Count == 0)
        {
            Debug.Log("Enemy has no capacities with PP left! Turn skipped.");
            playerTurn = true;
            NextTurn();
            return;
        }
    
        // Choisir une capacité au hasard parmi les capacités valides
        chosenCapacite = validCapacites[UnityEngine.Random.Range(0, validCapacites.Count)];
    
        // L'ennemi utilise la capacité
        chosenCapacite.powerPoints--;  // Consommer un PP de l'ennemi
    
        Debug.Log($"Enemy uses {chosenCapacite.capaciteName}!");
    
        // Appliquer les dégâts au joueur
        playerPokeIUT.health -= chosenCapacite.damage;
        if (playerPokeIUT.health <= 0)
        {
            playerPokeIUT.health = 0;
            Debug.Log("Player defeated!");
            EndBattle();
            return;
        }
    
        // Mettre à jour l'affichage de la santé du joueur
        playerHealthText.text = "HP " + playerPokeIUT.health;
    
        // Passez au tour du joueur
        playerTurn = true;
        NextTurn();
    }
    
    

    void EndBattle()
    {
        foreach (var button in capaciteButtons)
        {
            if (button != null)
            {
                button.gameObject.SetActive(true);
            }
        }
        
        ResetCapacitePP(); // Réinitialisez les PP
        Debug.Log("Battle has ended!");
        UpdateCapaciteButtonsVisibility(false);
        fightButton.gameObject.SetActive(true);
        bagButton.gameObject.SetActive(true);
        pokeiutButton.gameObject.SetActive(true);
        runButton.gameObject.SetActive(true);


    }

    

    void ResetCapacitePP()
    {
        for (int i = 0; i < playerPokeIUT.capacites.Count; i++)
        {
            playerPokeIUT.capacites[i].powerPoints = initialPlayerCapacitePP[i];

            // Vérifie si le bouton existe avant de modifier le texte
            if (capaciteButtons[i] != null && capaciteButtons[i].gameObject.activeInHierarchy)
            {
                capaciteButtons[i].GetComponentInChildren<Text>().text = 
                    $"{playerPokeIUT.capacites[i].capaciteName} ({playerPokeIUT.capacites[i].powerPoints})";
            }
            else
            {
                Debug.LogWarning($"Button at index {i} is missing or inactive.");
            }
        }
    }



 void ClonePlayerCapacites()
{
    // Nouvelle liste de capacités clonées pour le joueur
    var clonedCapacites = new List<CapaciteData>();

    foreach (var capacite in playerPokeIUT.capacites)
    {
        // Crée une nouvelle instance de chaque capacité pour le joueur
        CapaciteData cloned = ScriptableObject.Instantiate(capacite);
        cloned.name = capacite.name; // Garde le nom original
        cloned.powerPoints = capacite.powerPoints; // Assure une copie des PP
        clonedCapacites.Add(cloned);
    }

    // Assigne la nouvelle liste clonée des capacités du joueur
    playerPokeIUT.capacites = clonedCapacites;
}

void CloneEnemyCapacites()
{
    // Nouvelle liste de capacités clonées pour l'ennemi
    var clonedCapacites = new List<CapaciteData>();

    foreach (var capacite in enemyPokeIUT.capacites)
    {
        // Crée une nouvelle instance de chaque capacité pour l'ennemi
        CapaciteData cloned = ScriptableObject.Instantiate(capacite);
        cloned.name = capacite.name; // Garde le nom original
        cloned.powerPoints = capacite.powerPoints; // Assure une copie des PP
        clonedCapacites.Add(cloned);
    }

    // Assigne la nouvelle liste clonée des capacités de l'ennemi
    enemyPokeIUT.capacites = clonedCapacites;
}

    void OnDisable()
    {
        if (enemyPokeIUT == null) Debug.LogError("enemyPokeIUT is not assigned");
        if (playerPokeIUT == null) Debug.LogError("playerPokeIUT is not assigned");

        // Restore initial health and power points
        if (enemyPokeIUT != null)
        {
            enemyPokeIUT.health = initialEnemyHealth;
        }

        if (playerPokeIUT != null)
        {
            playerPokeIUT.health = initialPlayerHealth;
        }

        ResetCapacitePP();

        
    }


}