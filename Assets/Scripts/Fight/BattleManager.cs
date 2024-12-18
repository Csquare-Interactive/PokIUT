using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Image = UnityEngine.UI.Image;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public BattleUIManager battleUIManager;
    public PlayerData playerData;
    public PokeIUTData enemyPokeIUT;
    private CombatSystem combatSystem;
    private PokeIUTData PlayerPokeIUT { get; set; }
    private ItemData currentItem;
    private bool isUsingItem = false;
    private bool battleOver = false;
    private bool isPlayerTurn = false;
    private bool playerHasActed = false;

    void Start()
    {
        if (playerData == null) Debug.LogError("playerData is not assigned");
        if (enemyPokeIUT == null) Debug.LogError("enemyPokeIUT is not assigned");

        if (playerData.currentPokeIUT == null)
            playerData.currentPokeIUT = playerData.pokIUTTeam[0];

        PlayerPokeIUT = playerData.currentPokeIUT;

        isPlayerTurn = PlayerPokeIUT.speed >= enemyPokeIUT.speed;

        combatSystem = new CombatSystem(playerData, enemyPokeIUT);
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
        battleUIManager.OnPokeIUTTeamClicked += HandlePokeIUTTeamClicked;
        battleUIManager.OnItemClicked += HandleItemClicked;


        combatSystem.StartBattle();
    }

    public void HandleBattleStart()
    {
        battleUIManager.UpdateUI(combatSystem.PlayerPokeIUT, combatSystem.Enemy);
        StartCoroutine(HandleBattleLoop());
    }

    public IEnumerator HandleBattleLoop()
    {
        while(!battleOver)
        {
            if (isPlayerTurn)
            {
                battleUIManager.ShowDescription("C'est au tour de {0} !", playerData.playerName);
                battleUIManager.ShowActionButtons(true);
                battleUIManager.RefreshUI(combatSystem.PlayerPokeIUT, combatSystem.Enemy);

                yield return new WaitUntil(() => playerHasActed); // Wait untill player hasn't acted

                playerHasActed = false; // Reset the flag

            }
            else
            {
                battleUIManager.ShowDescription("C'est au tour de {0} !", enemyPokeIUT.pokeiutName);
                combatSystem.EnemyTurn();
                battleUIManager.RefreshUI(combatSystem.PlayerPokeIUT, combatSystem.Enemy);
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void HandleFightClicked()
    {
        battleUIManager.ShowActionButtons(false);
        battleUIManager.ShowCapaciteButtons(true, PlayerPokeIUT);
    }

    private void HandleCapaciteClicked(int index)
    {
        battleUIManager.ShowDescription("{0} a utilisé {1}", playerData.playerName, PlayerPokeIUT.capacites[index].name);
        combatSystem.PlayerUseCapacite(index);
        battleUIManager.ShowCapaciteButtons(false, PlayerPokeIUT);
    }

    private void HandleBagClicked()
    {
        battleUIManager.ShowActionButtons(false);
        battleUIManager.ShowCurrentPokeIUTStats(false);
        battleUIManager.ShowBagUI(true);
        battleUIManager.UpdateBagInfos(playerData);
    }

    private void HandleItemClicked(int index)
    {
        var item = playerData.items[index];
        if (item.quantity <= 0) return;

        currentItem = item;
        isUsingItem = true;
        battleUIManager.ShowBagUI(false);
        battleUIManager.ShowPokeIUTTeamUI(true);
        battleUIManager.UpdatePokeIUTTeamInfos(playerData);
    }

    private void HandlePokeIUTSelected(int pokeIUTIndex)
    {
        if (isUsingItem)
        {
            var pokeIUT = playerData.pokIUTTeam[pokeIUTIndex];
            UseItemOnPokeIUT(currentItem, pokeIUT);
            currentItem.quantity--;
            isUsingItem = false;
        }
        // Ajoutez ici d'autres actions possibles si nécessaire

        battleUIManager.ShowPokeIUTTeamUI(false);
        battleUIManager.ShowActionButtons(true);
        battleUIManager.ShowCurrentPokeIUTStats(true);
    }

    private void UseItemOnPokeIUT(ItemData item, PokeIUTData pokeIUT)
    {
        switch (item.itemName)
        {
            case "Potion":
                pokeIUT.health = Mathf.Min(pokeIUT.maxHealth, pokeIUT.health + 20);
                battleUIManager.ShowDescription("{0} a utilisé une {1} sur {2} et a récupéré 20 PV", playerData.playerName, item.itemName, pokeIUT.pokeiutName);
                break;
            case "Super Potion":
                pokeIUT.health = Mathf.Min(pokeIUT.maxHealth, pokeIUT.health + 50);
                battleUIManager.ShowDescription("{0} a utilisé une {1} sur {2} et a récupéré 50 PV", playerData.playerName, item.itemName, pokeIUT.pokeiutName);
                break;
            // Ajoutez d'autres cas pour différents items
            default:
                battleUIManager.ShowDescription("{0} a utilisé {1} sur {2}", playerData.playerName, item.itemName, pokeIUT.pokeiutName);
                break;
        }
        battleUIManager.RefreshUI(PlayerPokeIUT, enemyPokeIUT);
    }

    private void HandleBackClicked()
    {
        battleUIManager.ShowActionButtons(true);
        battleUIManager.ShowCurrentPokeIUTStats(true);
        battleUIManager.ShowCapaciteButtons(false, PlayerPokeIUT);
        battleUIManager.ShowBagUI(false);
        battleUIManager.ShowPokeIUTTeamUI(false);
        // Add Here other UI elements to hide (PokeIUT, Bag, etc...)
    }

    private void HandlePokeiutClicked()
    {
        battleUIManager.ShowActionButtons(false);
        battleUIManager.ShowCurrentPokeIUTStats(false);
        battleUIManager.ShowPokeIUTTeamUI(true);
        battleUIManager.UpdatePokeIUTTeamInfos(playerData);
    }

    private void HandlePokeIUTTeamClicked(int index)
    {
        if (isUsingItem)
        {
            HandlePokeIUTSelected(index);
        }
        else
        {
            battleUIManager.ShowDescription("{0} a envoyé {1}", playerData.playerName, playerData.pokIUTTeam[index].pokeiutName);
            battleUIManager.ShowPokeIUTTeamUI(false);
            battleUIManager.ShowActionButtons(true);
            battleUIManager.ShowCurrentPokeIUTStats(true);
            combatSystem.PlayerSwitchPokeIUT(index);
        }
    }

    private void HandleRunClicked()
    {
        battleUIManager.ShowDescription("{0} fuit le combat !", playerData.playerName);
        combatSystem.EndBattle();
    }

    private void HandleTurnEnd()
    {
        isPlayerTurn = !isPlayerTurn;
        battleUIManager.ShowActionButtons(isPlayerTurn);
        battleUIManager.RefreshUI(combatSystem.PlayerPokeIUT, combatSystem.Enemy);
        playerHasActed = true;
    }

    void HandleBattleEnd()
    {
        battleOver = true;
        battleUIManager.ShowActionButtons(false);
        battleUIManager.ShowCapaciteButtons(false, PlayerPokeIUT);
        ResetPokeIUT();
    }

    //NOTE Temporary function to reset the pokeIUTs (Remove when PokeIUT center is implemented)
    void ResetPokeIUT()
    {
        PlayerPokeIUT.health = PlayerPokeIUT.maxHealth;
        enemyPokeIUT.health = enemyPokeIUT.maxHealth;
        PlayerPokeIUT.speed = PlayerPokeIUT.maxSpeed;
        enemyPokeIUT.speed = enemyPokeIUT.maxSpeed;
        foreach (var capacite in PlayerPokeIUT.capacites)
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