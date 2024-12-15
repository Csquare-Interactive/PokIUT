using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Image = UnityEngine.UI.Image;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public BattleUIManager battleUIManager;
    public PokeIUTData playerPokeIUT;
    public PokeIUTData enemyPokeIUT;
    private CombatSystem combatSystem;
    private bool battleOver = false;
    private bool playerHasActed = false;

    void Start()
    {
        if (playerPokeIUT == null) Debug.LogError("playerPokeIUT is not assigned");
        if (enemyPokeIUT == null) Debug.LogError("enemyPokeIUT is not assigned");

        combatSystem = new CombatSystem(playerPokeIUT, enemyPokeIUT);
        combatSystem.OnBattleStart += HandleBattleStart;
        combatSystem.OnTurnEnd += HandleTurnEnd;
        combatSystem.OnBattleEnd += HandleBattleEnd;

        battleUIManager.SetupBattleUI();
        battleUIManager.OnFightClicked += HandleFightClicked;
        battleUIManager.OnCapaciteClicked += HandleCapaciteClicked;
        battleUIManager.OnBagClicked += HandleBagClicked;
        battleUIManager.OnPokeiutClicked += HandlePokeiutClicked;
        battleUIManager.OnRunClicked += HandleRunClicked;
        battleUIManager.OnBackClicked += HandleBackClicked;

        combatSystem.StartBattle();
    }

    public void HandleBattleStart()
    {
        battleUIManager.UpdateUI(combatSystem.Player, combatSystem.Enemy);
        StartCoroutine(HandleBattleLoop());
    }

    public IEnumerator HandleBattleLoop()
    {
        while(!battleOver)
        {
            if (combatSystem.IsPlayerTurn)
            {
                battleUIManager.ShowDescription("C'est au tour de {0} !", playerPokeIUT.pokeiutName);
                battleUIManager.ShowActionButtons(true);
                battleUIManager.RefreshUI(combatSystem.Player, combatSystem.Enemy);

                yield return new WaitUntil(() => playerHasActed); // Wait untill player hasn't acted

                playerHasActed = false; // Reset the flag

            }
            else
            {
                battleUIManager.ShowDescription("C'est au tour de {0} !", enemyPokeIUT.pokeiutName);
                combatSystem.EnemyTurn();
                battleUIManager.RefreshUI(combatSystem.Player, combatSystem.Enemy);
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void HandleFightClicked()
    {
        battleUIManager.ShowActionButtons(false);
        battleUIManager.ShowCapaciteButtons(true, playerPokeIUT);
    }

    private void HandleCapaciteClicked(int index)
    {
        battleUIManager.ShowDescription("{0} a utilisé {1}", playerPokeIUT.pokeiutName, playerPokeIUT.capacites[index].name);
        combatSystem.PlayerUseCapacite(index);
        battleUIManager.ShowCapaciteButtons(false, playerPokeIUT);
    }

    private void HandleBagClicked()
    {
        battleUIManager.ShowDescription("{0} a ouvert son sac", playerPokeIUT.pokeiutName);
        playerHasActed = true; // Not implemented yet
    }

    private void HandleBackClicked()
    {
        battleUIManager.ShowActionButtons(true);
        battleUIManager.ShowCapaciteButtons(false, playerPokeIUT);
        // Add Here other UI elements to hide (PokeIUT, Bag, etc...)
    }

    private void HandlePokeiutClicked()
    {
        battleUIManager.ShowDescription("{0} choisit un pokeIUT", playerPokeIUT.pokeiutName);
        playerHasActed = true; // Not implemented yet
    }

    private void HandleRunClicked()
    {
        battleUIManager.ShowDescription("{0} fuit le combat !", playerPokeIUT.pokeiutName);
        combatSystem.EndBattle();
    }

    private void HandleTurnEnd()
    {
        battleUIManager.ShowActionButtons(false);
        battleUIManager.RefreshUI(combatSystem.Player, combatSystem.Enemy);
        playerHasActed = true;
    }

    void HandleBattleEnd()
    {
        battleOver = true;
        battleUIManager.ShowActionButtons(false);
        battleUIManager.ShowCapaciteButtons(false, playerPokeIUT);
        ResetPokeIUT();
    }

    //NOTE Temporary function to reset the pokeIUTs (Remove when PokeIUT center is implemented)
    void ResetPokeIUT()
    {
        playerPokeIUT.health = playerPokeIUT.maxHealth;
        enemyPokeIUT.health = enemyPokeIUT.maxHealth;
        playerPokeIUT.speed = playerPokeIUT.maxSpeed;
        enemyPokeIUT.speed = enemyPokeIUT.maxSpeed;
        foreach (var capacite in playerPokeIUT.capacites)
        {
            capacite.damage = capacite.maxDamage;
            capacite.powerPoints = capacite.maxPowerPoints;
        }
        foreach (var capacite in enemyPokeIUT.capacites)
        {
            capacite.damage = capacite.maxDamage;
            capacite.powerPoints = capacite.maxPowerPoints;
        }
    }
}