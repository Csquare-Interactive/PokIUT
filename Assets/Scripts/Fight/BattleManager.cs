using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Image = UnityEngine.UI.Image;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public BattleUIManager battleUIManager;
    public PokeIUTData playerPokeIUT;
    public PokeIUTData enemyPokeIUT;
    private CombatSystem combatSystem;
    private bool battleOver = false;
    private bool playerHasChosenAction = false;

    void Start()
    {
        if (playerPokeIUT == null) Debug.LogError("playerPokeIUT is not assigned");
        if (enemyPokeIUT == null) Debug.LogError("enemyPokeIUT is not assigned");

        combatSystem = new CombatSystem(playerPokeIUT, enemyPokeIUT);
        combatSystem.OnBattleStart += HandleBattleStart;
        battleUIManager.SetupBattleUI();
        combatSystem.StartBattle();
    }

    public void HandleBattleStart()
    {
        battleUIManager.InitializeFightUI();
        battleUIManager.UpdateUI(combatSystem.Player, combatSystem.Enemy);
    }

    public void HandleBattleLoop()
    {
        while(!battleOver)
        {
            if (combatSystem.IsPlayerTurn)
            {
                battleUIManager.RefreshUI(combatSystem.Player, combatSystem.Enemy);
                yield return new WaitUntil(() => playerHasChosenAction);
                // Définir les actions que le joueur peut effectuer (attaquer)
                // Faire en sorte que ça skip son tour et que le flag playerHasChosenAction soit à false

            }
            else
            {
                combatSystem.EnemyTurn();
                battleUIManager.RefreshUI(combatSystem.Player, combatSystem.Enemy);
            }
        }
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