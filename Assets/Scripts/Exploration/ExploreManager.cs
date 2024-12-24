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


    }

    private void HandleBackClicked()
    {
        exploreUIManager.ShowPokeIUTTeamUI(false, playerData);
        exploreUIManager.ShowInventoryUI(false);

    }

    

    private void HandleInventoryClicked()
    {
        Debug.Log("HandleInventoryClicked called");
        exploreUIManager.ShowInventoryUI(true);
        exploreUIManager.UpdateInventoryInfos(playerData);
    }

    private void HandlePokeiutClicked()
    {
        Debug.Log("HandlePokeiutClicked called");
        exploreUIManager.ShowPokeIUTTeamUI(true, playerData);
        exploreUIManager.UpdatePokeIUTTeamInfos(playerData);
    }
}