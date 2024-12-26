using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Image = UnityEngine.UI.Image;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ExploreManager : MonoBehaviour
{
    public ExploreUIManager exploreUIManager;
    public bool IsInBattle { get; private set; }
    private PlayerData playerData;

    public void Start()
    {
        Debug.Log("ExploreManager Start called");


        if (GameManager.Instance == null || GameManager.Instance.playerData == null)
        {
            Debug.LogError("GameManager or playerData is not assigned");
            return;
        }

        playerData = GameManager.Instance.playerData;

        exploreUIManager.SetupExploreUI();
        exploreUIManager.OnInventoryClicked += HandleInventoryClicked;
        exploreUIManager.OnPokeiutClicked += HandlePokeiutClicked;
        exploreUIManager.OnBackClicked += HandleBackClicked;

        if (BattleManager.Instance != null)
        {
            SubscribeToBattleManager(BattleManager.Instance);
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // Listen to scene loaded event
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene" && BattleManager.Instance != null)
            SubscribeToBattleManager(BattleManager.Instance);
    }

    private void HandleBackClicked()
    {
        exploreUIManager.ShowPokeIUTTeamUI(false, playerData);
        exploreUIManager.ShowInventoryUI(false);
    }

    private void HandleInventoryClicked()
    {
        if (IsInBattle) return;
        Debug.Log("HandleInventoryClicked called");
        exploreUIManager.ShowInventoryUI(true);
        exploreUIManager.UpdateInventoryInfos(playerData);
    }

    private void HandlePokeiutClicked()
    {
        if (IsInBattle) return;
        Debug.Log("HandlePokeiutClicked called");
        exploreUIManager.ShowPokeIUTTeamUI(true, playerData);
        exploreUIManager.UpdatePokeIUTTeamInfos(playerData);
    }

    private void SubscribeToBattleManager(BattleManager battleManager)
    {
        Debug.Log("Subscribing to BattleManager events");
        battleManager.OnBattleScene += HandleEnterBattleScene;
        battleManager.OnExploreScene += HandleExitBattleScene;
    }

    private void HandleEnterBattleScene()
    {
        Debug.Log("HandleEnterBattleScene called");
        exploreUIManager.ShowExplorationUI(false);
        IsInBattle = true;
    }

    private void HandleExitBattleScene()
    {
        exploreUIManager.ShowExplorationUI(true);
        IsInBattle = false;
    }
}