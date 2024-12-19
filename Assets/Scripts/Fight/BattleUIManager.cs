using System;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using Image = UnityEngine.UI.Image;
using System.Collections.Generic;

public class BattleUIManager : MonoBehaviour
{
    [Header("PlayerInfos")]
    public Text playerNameText;
    public Text playerLevelText;
    public Text playerHealthText;
    public Image playerIcon;

    [Header("EnemyInfos")]
    public Text enemyNameText;
    public Text enemyLevelText;
    public Text enemyHealthText;
    public Image enemyIcon;

    [Header("PlayerActions")]
    public Button fightButton;
    public Button bagButton;
    public Button pokeiutButton;
    public Button runButton;
    public Button backButton;

    [Header("PlayerCapacites")]
    public Button[] capaciteButtons;

    [Header("Description")]
    public Text descriptionText;

    [Header("PokeIUTTeam")]
    public Canvas pokeIUTTeamUI;
    public Button backButtonPokeIUTTeam;
    private List<Button> pokeIUTTeamButtons;

    [Header("Bag")]
    public GameObject bagUI;
    public Button backButtonBag;
    public GameObject itemButtonPrefab;
    public Transform itemButtonContainer;
    private List<Button> itemButtons = new List<Button>();




    public event Action OnFightClicked;
    public event Action<int> OnCapaciteClicked;
    public event Action OnBagClicked;
    public event Action OnPokeiutClicked;
    public event Action OnRunClicked;
    public event Action OnBackClicked;
    public event Action<int> OnItemClicked;
    public event Action<int> OnPokeIUTTeamClicked;

    public void SetupBattleUI()
    {
        if (playerNameText == null) Debug.LogError("playerNameText is not assigned");
        if (playerLevelText == null) Debug.LogError("playerLevelText is not assigned");
        if (playerHealthText == null) Debug.LogError("playerHealthText is not assigned");
        if (playerIcon == null) Debug.LogError("playerIcon is not assigned");
        if (enemyNameText == null) Debug.LogError("enemyNameText is not assigned");
        if (enemyLevelText == null) Debug.LogError("enemyLevelText is not assigned");
        if (enemyHealthText == null) Debug.LogError("enemyHealthText is not assigned");
        if (enemyIcon == null) Debug.LogError("enemyIcon is not assigned");
        if (fightButton == null) Debug.LogError("fightButton is not assigned");
        if (bagButton == null) Debug.LogError("bagButton is not assigned");
        if (pokeiutButton == null) Debug.LogError("pokeiutButton is not assigned");
        if (runButton == null) Debug.LogError("runButton is not assigned");
        if (backButton == null) Debug.LogError("backButton is not assigned");
        if (capaciteButtons == null) Debug.LogError("capaciteButtons is not assigned");
        if (capaciteButtons.Length < 4 || capaciteButtons.Length > 4) Debug.LogError("capaciteButtons must have 4 buttons");
        if (descriptionText == null) Debug.LogError("descriptionText is not assigned");
        if (pokeIUTTeamUI == null) Debug.LogError("pokeIUTTeamUI is not assigned");
        if (backButtonPokeIUTTeam == null) Debug.LogError("backButtonPokeIUTTeam is not assigned");
        if (bagUI == null) Debug.LogError("bagUI is not assigned");
        if (backButtonBag == null) Debug.LogError("backButtonBag is not assigned");

        // Get All PokeIUT buttons from the PokeIUTTeamUI
        pokeIUTTeamButtons = new List<Button>();
        foreach (Transform child in pokeIUTTeamUI.transform)
        {
            if (child.name.StartsWith("PokeIUT"))
            {
                Button button = child.GetComponent<Button>();
                if (button != null)
                    pokeIUTTeamButtons.Add(button);
            }
        }

        // Events listeners when buttons are clicked (To help BattleManager to know what to do)
        fightButton.onClick.AddListener(() => OnFightClicked?.Invoke());
        bagButton.onClick.AddListener(() => OnBagClicked?.Invoke());
        pokeiutButton.onClick.AddListener(() => OnPokeiutClicked?.Invoke());
        runButton.onClick.AddListener(() => OnRunClicked?.Invoke());
        backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
        backButtonPokeIUTTeam.onClick.AddListener(() => OnBackClicked?.Invoke());
        foreach (var button in capaciteButtons)
        {
            int index = Array.IndexOf(capaciteButtons, button);
            button.onClick.AddListener(() => OnCapaciteClicked?.Invoke(index));
        }
        foreach (var button in pokeIUTTeamButtons)
        {
            int index = pokeIUTTeamButtons.IndexOf(button);
            button.onClick.AddListener(() => OnPokeIUTTeamClicked?.Invoke(index));
        }

        fightButton.gameObject.SetActive(false);
        bagButton.gameObject.SetActive(false);
        pokeiutButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        pokeIUTTeamUI.gameObject.SetActive(false);
        bagUI.gameObject.SetActive(false);

        foreach (var button in capaciteButtons)
        {
            button.gameObject.SetActive(false);
        }

        itemButtons = new List<Button>();
        foreach (Transform child in bagUI.transform)
        {
            if (child.name.StartsWith("Item"))
            {
                Button button = child.GetComponent<Button>();
                if (button != null)
                    itemButtons.Add(button);
            }
        }

        backButtonBag.onClick.AddListener(() => OnBackClicked?.Invoke());
        foreach (var button in itemButtons)
        {
            int index = itemButtons.IndexOf(button);
            button.onClick.AddListener(() => OnItemClicked?.Invoke(index));
        }
    }

    public void UpdateUI(PokeIUTInstance player, PokeIUTInstance enemy)
    {
        playerNameText.text = player.baseData.pokeiutName;
        playerLevelText.text = $"Lvl {player.level}";
        playerHealthText.text = $"HP {player.health}";
        playerIcon.sprite = player.baseData.icon;

        enemyNameText.text = enemy.baseData.pokeiutName;
        enemyLevelText.text = $"Lvl {enemy.level}";
        enemyHealthText.text = $"HP {enemy.health}";
        enemyIcon.sprite = enemy.baseData.icon;
    }

    public void RefreshUI(PokeIUTInstance player, PokeIUTInstance enemy)
    {
        playerHealthText.text = $"HP {player.health}";
        playerLevelText.text = $"Lvl {player.level}";
        playerNameText.text = player.baseData.pokeiutName;
        playerIcon.sprite = player.baseData.icon;
        enemyHealthText.text = $"HP {enemy.health}";
        enemyLevelText.text = $"Lvl {enemy.level}";
        enemyNameText.text = enemy.baseData.pokeiutName;
        enemyIcon.sprite = enemy.baseData.icon;
    }

    public void ShowActionButtons(bool show)
    {
        fightButton.gameObject.SetActive(show);
        bagButton.gameObject.SetActive(show);
        pokeiutButton.gameObject.SetActive(show);
        runButton.gameObject.SetActive(show);
    }

    public void ShowCurrentPokeIUTStats(bool show)
    {
        playerNameText.gameObject.SetActive(show);
        playerLevelText.gameObject.SetActive(show);
        playerHealthText.gameObject.SetActive(show);
    }

    public void ShowCapaciteButtons(bool show, PokeIUTInstance player)
    {
        for (int i = 0; i < player.capacites.Count; i++)
        {
            capaciteButtons[i].gameObject.SetActive(show);
            capaciteButtons[i].GetComponentInChildren<Text>().text = player.capacites[i].baseData.name + $" ({player.capacites[i].powerPoints})";
        }

        backButton.gameObject.SetActive(show);
    }

    public void ShowDescription(string format, params object[] args)
    {
        descriptionText.text = string.Format(format, args);
    }

    public void ShowPokeIUTTeamUI(bool show, PlayerData playerData)
    {
        pokeIUTTeamUI.gameObject.SetActive(show);
        foreach (var button in pokeIUTTeamButtons)
        {
            button.gameObject.SetActive(false);
        }
        for (int i = 0; i < playerData.pokIUTTeam.Length; i++)
        {
            pokeIUTTeamButtons[i].gameObject.SetActive(show);
        }
    }

    public void ShowBagUI(bool show)
    {
        bagUI.gameObject.SetActive(show);
    }

    public void UpdatePokeIUTTeamInfos(PlayerData playerData)
    {
        // Create new buttons
        for (int index = 0; index < playerData.pokIUTTeam.Length; index++)
        {
            var pokeIUT = playerData.pokIUTTeam[index];
            var button = pokeIUTTeamButtons[index];
            button.transform.Find("PokeIUT_Name").GetComponent<Text>().text = pokeIUT.baseData.pokeiutName;
            button.transform.Find("PokeIUT_Health").GetComponent<Text>().text = $"HP {pokeIUT.health}";
            button.transform.Find("PokeIUT_Icon").GetComponent<Image>().sprite = pokeIUT.baseData.icon;
        }
    }

    public void UpdateBagInfos(PlayerData playerData)
    {
        // Clear existing buttons
        foreach (Transform child in itemButtonContainer)
        {
            Destroy(child.gameObject);
        }
        itemButtons.Clear();

        // Create new buttons
        for (int i = 0; i < playerData.items.Length; i++)
        {
            var item = playerData.items[i];
            var button = Instantiate(itemButtonPrefab, itemButtonContainer).GetComponent<Button>();
            button.transform.Find("Item_Name").GetComponent<Text>().text = item.baseData.itemName;
            button.transform.Find("Item_Quantity").GetComponent<Text>().text = $"Qty {item.quantity}";
            int index = i;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnItemClicked?.Invoke(index));
            itemButtons.Add(button);
        }
    }


}