using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Image = UnityEngine.UI.Image;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public BattleUIManager battleUIManager;
    private CombatSystem combatSystem;
    public EnemyData enemyData;
    private PlayerData playerData;
    private ItemInstance currentItem;
    private bool isUsingItem = false;
    private bool battleOver = false;
    private bool isPlayerTurn = false;
    private bool playerHasActed = false;
    private string explorationSceneName = "IUT";

    void Start()
    {
        if (GameManager.Instance == null || GameManager.Instance.playerData == null) Debug.LogError("GameManager or playerData is not assigned");

        playerData = GameManager.Instance.playerData;

        playerData.InitializePokeIUTTeam(); // Initialize all the pokeIUTs in the team (To prevent having same PokeIUT references in the team and with the enemy)
        playerData.InitializeItems(); // Initialize all the items in the inventory
        enemyData.InitializePokeIUTTeam();
        enemyData.InitializeItems();

        if (playerData.currentPokeIUT == null)
            playerData.currentPokeIUT = playerData.pokIUTTeam[0];
        if (enemyData.currentPokeIUT == null)
            enemyData.currentPokeIUT = enemyData.pokIUTTeam[0];

        combatSystem = new CombatSystem(playerData, enemyData);
        combatSystem.OnBattleStart += HandleBattleStart;
        combatSystem.OnTurnEnd += HandleTurnEnd;
        combatSystem.OnBattleEnd += HandleBattleEnd;

        isPlayerTurn = combatSystem.IsPlayerFirst();

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
        battleUIManager.UpdateUI(combatSystem.PlayerPokeIUT, combatSystem.EnemyPokeIUT);
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
                battleUIManager.RefreshUI(combatSystem.PlayerPokeIUT, combatSystem.EnemyPokeIUT);

                yield return new WaitUntil(() => playerHasActed); // Wait untill player hasn't acted

                playerHasActed = false; // Reset the flag

            }
            else
            {
                battleUIManager.ShowDescription("C'est au tour de {0} !", enemyData.enemyName);
                combatSystem.EnemyTurn();
                battleUIManager.RefreshUI(combatSystem.PlayerPokeIUT, combatSystem.EnemyPokeIUT);
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void HandleFightClicked()
    {
        battleUIManager.ShowActionButtons(false);
        battleUIManager.ShowCapaciteButtons(true, combatSystem.PlayerPokeIUT);
    }

    private void HandleCapaciteClicked(int index)
    {
        int result = combatSystem.PlayerUseCapacite(index);
        if (result == 0) // Worked
            battleUIManager.ShowDescription("{0} a utilisé {1}", playerData.playerName, combatSystem.PlayerPokeIUT.capacites[index].baseData.name);
        else // Illegal Action (No PP)
        {
            battleUIManager.ShowDescription("{0} a utilisé {1} mais il n'y a plus de PP", playerData.playerName, combatSystem.PlayerPokeIUT.capacites[index].baseData.name);
            battleUIManager.ShowActionButtons(true);
        }
        battleUIManager.ShowCapaciteButtons(false, combatSystem.PlayerPokeIUT);
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
        battleUIManager.ShowPokeIUTTeamUI(true, playerData);
        battleUIManager.UpdatePokeIUTTeamInfos(playerData);
    }

    private void HandleBackClicked()
    {
        battleUIManager.ShowActionButtons(true);
        battleUIManager.ShowCurrentPokeIUTStats(true);
        battleUIManager.ShowBagUI(false);
        battleUIManager.ShowCapaciteButtons(false, combatSystem.PlayerPokeIUT);
        battleUIManager.ShowPokeIUTTeamUI(false, playerData);
        // Add Here other UI elements to hide (PokeIUT, Bag, etc...)
    }

    private void HandlePokeiutClicked()
    {
        battleUIManager.ShowActionButtons(false);
        battleUIManager.ShowCurrentPokeIUTStats(false);
        battleUIManager.ShowPokeIUTTeamUI(true, playerData);
        battleUIManager.UpdatePokeIUTTeamInfos(playerData);
    }

    private void HandlePokeIUTTeamClicked(int index)
    {
        // Handler when using an item or switching PokeIUT
        if (isUsingItem)
        {
            HandlePokeIUTSelected(index);
        }
        else
        {
            battleUIManager.ShowDescription("{0} a envoyé {1}", playerData.playerName, combatSystem.PlayerPokeIUT.baseData.pokeiutName);
            battleUIManager.ShowPokeIUTTeamUI(false, playerData);
            battleUIManager.ShowActionButtons(true);
            battleUIManager.ShowCurrentPokeIUTStats(true);
            combatSystem.PlayerSwitchPokeIUT(index);
        }
    }

    private void HandlePokeIUTSelected(int pokeIUTIndex)
    {
        if (isUsingItem)
        {
            PokeIUTInstance pokeIUT = playerData.pokIUTTeam[pokeIUTIndex];
            battleUIManager.RefreshUI(combatSystem.PlayerPokeIUT, combatSystem.EnemyPokeIUT);
            int result = combatSystem.PlayerUseItem(currentItem, pokeIUT);
            if (result == 0) // Worked
            {
                battleUIManager.ShowDescription("{0} a utilisé {1} sur {2}", playerData.playerName, currentItem.baseData.itemName, pokeIUT.baseData.pokeiutName);
                currentItem.quantity--;
            }
            else // Illegal Action
                battleUIManager.ShowDescription("Action Impossible");
            isUsingItem = false;
        }

        battleUIManager.ShowPokeIUTTeamUI(false, playerData);
        battleUIManager.ShowActionButtons(true);
        battleUIManager.ShowCurrentPokeIUTStats(true);
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
        battleUIManager.RefreshUI(combatSystem.PlayerPokeIUT, combatSystem.EnemyPokeIUT);
        playerHasActed = true;
    }

    void HandleBattleEnd()
    {
        battleOver = true;
        battleUIManager.ShowActionButtons(false);
        battleUIManager.ShowCapaciteButtons(false, combatSystem.PlayerPokeIUT);

        StartCoroutine(ReturnToExploration());
    }

    IEnumerator ReturnToExploration()
    {
        // Wait (To make some animations before)
        yield return new WaitForSeconds(2f);

        // Unload Battle Scene
        SceneManager.UnloadSceneAsync("BattleScene");
    }
}