using System;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Image = UnityEngine.UI.Image;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    public BattleUIManager battleUIManager;
    private CombatSystem combatSystem;
    private PlayerData playerData;
    private ItemInstance currentItem;
    [HideInInspector] public EnemyData enemyData;
    private bool isUsingItem = false;
    private bool battleOver = false;
    private bool isPlayerTurn = false;
    private bool playerHasActed = false;
    public event Action OnBattleScene;
    public event Action OnExploreScene;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple BattleManager instances found. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void Start()
    {
        if (BattleDataManager.Instance == null || BattleDataManager.Instance.GetEnemyData() == null)
        {
            Debug.LogError("No EnemyData available to initialize the battle!");
            return;
        }

        enemyData = BattleDataManager.Instance.GetEnemyData();

        OnBattleScene?.Invoke();
        Initialize(enemyData);
    }

    public void Initialize(EnemyData enemyData)
    {
        if (GameManager.Instance == null || GameManager.Instance.playerData == null) Debug.LogError("GameManager or playerData is not assigned");
        
        this.enemyData = enemyData;

        playerData = GameManager.Instance.playerData;

        playerData.InitializePokeIUTTeam(); // Initialize all the pokeIUTs in the team (To prevent having same PokeIUT references in the team and with the enemy)
        playerData.InitializeItems(); // Initialize all the items in the inventory
        enemyData.InitializePokeIUTTeam();
        enemyData.InitializeItems();

        if (playerData.currentPokeIUT == null)
            playerData.currentPokeIUT = playerData.pokeIUTTeam[0];
        if (enemyData.currentPokeIUT == null)
            enemyData.currentPokeIUT = enemyData.pokeIUTTeam[0];

        foreach(PokeIUTInstance pokeIUT in playerData.pokeIUTTeam)
        {
            if (pokeIUT.state == null)
                pokeIUT.state = new NormalState(pokeIUT);
        }
        foreach(PokeIUTInstance pokeIUT in enemyData.pokeIUTTeam)
        {
            if (pokeIUT.state == null)
                pokeIUT.state = new NormalState(pokeIUT);
        }

        combatSystem = new CombatSystem(playerData, enemyData);
        combatSystem.OnBattleStart += HandleBattleStart;
        combatSystem.OnTurnEnd += HandleTurnEnd;
        combatSystem.OnBattleEnd += HandleBattleEnd;
        combatSystem.OnPokeIUTDead += HandlePokeiutClicked;

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
            combatSystem.PlayerPokeIUT.UpdateStateDescription();
            combatSystem.EnemyPokeIUT.UpdateStateDescription();
            if (isPlayerTurn)
            {
                battleUIManager.ShowDescription("C'est au tour de {0} !", playerData.name);
                battleUIManager.ShowActionButtons(true);
                battleUIManager.RefreshUI(combatSystem.PlayerPokeIUT, combatSystem.EnemyPokeIUT);
                battleUIManager.ShowPokeIUTTeamIcons(playerData, enemyData, true);
                battleUIManager.UpdatePokeIUTTeamIcons(playerData, enemyData);
                combatSystem.PlayerPokeIUT.OnStartTurn(); // Get PlayerPokeIUT state effect at the start of the turn

                yield return new WaitUntil(() => playerHasActed); // Wait untill player hasn't acted

                playerHasActed = false; // Reset the flag

            }
            else
            {
                battleUIManager.ShowDescription("C'est au tour de {0} !", enemyData.name);
                combatSystem.EnemyPokeIUT.OnStartTurn(); // Get EnemyPokeIUT state effect at the start of the turn
                combatSystem.EnemyTurn();
                battleUIManager.ShowPokeIUTTeamIcons(playerData, enemyData, true);
                battleUIManager.RefreshUI(combatSystem.PlayerPokeIUT, combatSystem.EnemyPokeIUT);
                battleUIManager.UpdatePokeIUTTeamIcons(playerData, enemyData);
                combatSystem.EnemyPokeIUT.OnEndTurn(); // Get EnemyPokeIUT state effect at the end of the turn
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
            battleUIManager.ShowDescription("{0} a utilisé {1}", playerData.name, combatSystem.PlayerPokeIUT.capacites[index].baseData.name);
        else // Illegal Action (No PP)
        {
            battleUIManager.ShowDescription("{0} a utilisé {1} mais il n'y a plus de PP", playerData.name, combatSystem.PlayerPokeIUT.capacites[index].baseData.name);
            battleUIManager.ShowActionButtons(true);
        }
        battleUIManager.ShowCapaciteButtons(false, combatSystem.PlayerPokeIUT);
    }

    private void HandleBagClicked()
    {
        battleUIManager.ShowActionButtons(false);
        battleUIManager.ShowCurrentPokeIUTStats(false);
        battleUIManager.ShowPokeIUTTeamIcons(playerData, enemyData, false);
        battleUIManager.ShowBagUI(true);
        battleUIManager.UpdateBagInfos(playerData);
    }

    private void HandleItemClicked(int index)
    {
        var item = playerData.items[index];
        if (item.quantity <= 0) return;

        currentItem = item;
        if (currentItem.baseData.itemName == "Pokiutball")
        {
            // Use Pokiutball directly
            int result = combatSystem.PlayerUseItem(currentItem);
            if (result == 0) // Worked
            {
                battleUIManager.ShowDescription("{0} a utilisé {1}", playerData.name, currentItem.baseData.itemName);
                currentItem.quantity--;
                battleUIManager.UpdateBagInfos(playerData);
            }
            else // Illegal Action
            {
                battleUIManager.ShowDescription("Action Impossible");
            }
        }
        else
        {
            isUsingItem = true;
            battleUIManager.ShowBagUI(false);
            battleUIManager.ShowPokeIUTTeamUI(true, playerData);
            battleUIManager.UpdatePokeIUTTeamInfos(playerData);
        }
    }

    private void HandleBackClicked()
    {
        battleUIManager.ShowActionButtons(true);
        battleUIManager.ShowCurrentPokeIUTStats(true);
        battleUIManager.ShowPokeIUTTeamIcons(playerData, enemyData, true);
        battleUIManager.ShowBagUI(false);
        battleUIManager.ShowCapaciteButtons(false, combatSystem.PlayerPokeIUT);
        battleUIManager.ShowPokeIUTTeamUI(false, playerData);
        // Add Here other UI elements to hide (PokeIUT, Bag, etc...)
    }

    private void HandlePokeiutClicked()
    {
        battleUIManager.ShowActionButtons(false);
        battleUIManager.ShowCurrentPokeIUTStats(false);
        battleUIManager.ShowPokeIUTTeamIcons(playerData, enemyData, false);
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
            battleUIManager.ShowPokeIUTTeamUI(false, playerData);
            battleUIManager.ShowActionButtons(true);
            battleUIManager.ShowCurrentPokeIUTStats(true);
            battleUIManager.ShowPokeIUTTeamIcons(playerData, enemyData, true);
            int result = combatSystem.PlayerSwitchPokeIUT(index);
            if (result == 1) // Illegal Action
                battleUIManager.ShowDescription("Action Impossible");
            else
                battleUIManager.ShowDescription("{0} a envoyé {1}", playerData.name, combatSystem.PlayerPokeIUT.baseData.pokeiutName);
        }
    }

    private void HandlePokeIUTSelected(int pokeIUTIndex)
    {
        Debug.Log("Using Item on PokeIUT: " + pokeIUTIndex);
        if (isUsingItem)
        {
            PokeIUTInstance pokeIUT = playerData.pokeIUTTeam[pokeIUTIndex];
            battleUIManager.RefreshUI(combatSystem.PlayerPokeIUT, combatSystem.EnemyPokeIUT);
            int result = combatSystem.PlayerUseItem(currentItem, pokeIUT);
            if (result == 0) // Worked
            {
                battleUIManager.ShowDescription("{0} a utilisé {1} sur {2}", playerData.name, currentItem.baseData.itemName, pokeIUT.baseData.pokeiutName);
                currentItem.quantity--;
            }
            else // Illegal Action
                battleUIManager.ShowDescription("Action Impossible");
            isUsingItem = false;
        }

        battleUIManager.ShowPokeIUTTeamUI(false, playerData);
        battleUIManager.ShowActionButtons(true);
        battleUIManager.ShowCurrentPokeIUTStats(true);
        battleUIManager.ShowPokeIUTTeamIcons(playerData, enemyData, true);
    }

    private void HandleRunClicked()
    {
        battleUIManager.ShowDescription("{0} fuit le combat !", playerData.name);
        combatSystem.EndBattle();
    }

    private void HandleTurnEnd()
    {
        combatSystem.PlayerPokeIUT.OnEndTurn(); // Get PokeIUT state effect at the end of the turn
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
        enemyData = null; // Reset the enemyData for the next battle

        StartCoroutine(ReturnToExploration());
    }

    IEnumerator ReturnToExploration()
    {
        // Wait (To make some animations before)
        yield return new WaitForSeconds(2f);

        // Notice the ExploreManager to show the UI
        OnExploreScene?.Invoke();
        // Unload Battle Scene
        SceneManager.UnloadSceneAsync("BattleScene");
    }
}