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
    public GameObject playerHealthBar;
    public GameObject playerPokeIUTCount;
    public Image playerIcon;

    [Header("EnemyInfos")]
    public Text enemyNameText;
    public Text enemyLevelText;
    public GameObject enemyHealthBar;
    public GameObject enemyPokeIUTCount;
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
    public Button leftArrowButton;
    public Button rightArrowButton;
    public Text itemTypeText;


    private ItemType currentItemType = ItemType.General;
    private ItemType[] itemTypes = (ItemType[])System.Enum.GetValues(typeof(ItemType));
    private int currentItemTypeIndex = 0;

    private Slider playerHealthSlider;
    private Slider enemyHealthSlider;
    private List<Image> playerPokeIUTTeamIcons = new List<Image>();
    private List<Image> enemyPokeIUTTeamIcons = new List<Image>();

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
        if (playerHealthBar == null) Debug.LogError("playerHealthBar is not assigned");
        if (playerPokeIUTCount == null) Debug.LogError("playerPokeIUTCount is not assigned");
        if (playerIcon == null) Debug.LogError("playerIcon is not assigned");
        if (enemyNameText == null) Debug.LogError("enemyNameText is not assigned");
        if (enemyLevelText == null) Debug.LogError("enemyLevelText is not assigned");
        if (enemyHealthBar == null) Debug.LogError("enemyHealthBar is not assigned");
        if (enemyPokeIUTCount == null) Debug.LogError("enemyPokeIUTCount is not assigned");
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

        playerHealthSlider = playerHealthBar.GetComponent<Slider>();
        enemyHealthSlider = enemyHealthBar.GetComponent<Slider>();

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

        // Get All PokeIUT icons from the PokeIUTCount (Main interface)
        foreach (Transform child in playerPokeIUTCount.transform)
        {
            if (child.name.StartsWith("PokeIUT"))
            {
                Image image = child.GetComponent<Image>();
                if (image != null)
                    playerPokeIUTTeamIcons.Add(image);
            }
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in enemyPokeIUTCount.transform)
        {
            if (child.name.StartsWith("PokeIUT"))
            {
                Image image = child.GetComponent<Image>();
                if (image != null)
                    enemyPokeIUTTeamIcons.Add(image);
            }
            child.gameObject.SetActive(false);
        }

        // Events listeners when buttons are clicked (To help BattleManager to know what to do)
        fightButton.onClick.AddListener(() => OnFightClicked?.Invoke());
        bagButton.onClick.AddListener(() => OnBagClicked?.Invoke());
        pokeiutButton.onClick.AddListener(() => OnPokeiutClicked?.Invoke());
        runButton.onClick.AddListener(() => OnRunClicked?.Invoke());
        backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
        backButtonPokeIUTTeam.onClick.AddListener(() => OnBackClicked?.Invoke());
        leftArrowButton.onClick.AddListener(ShowPreviousItemType);
        rightArrowButton.onClick.AddListener(ShowNextItemType);
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
        playerHealthSlider.maxValue = player.baseData.maxHealth;
        playerHealthSlider.currentValue = player.health;
        playerIcon.sprite = player.baseData.icon;

        enemyNameText.text = enemy.baseData.pokeiutName;
        enemyLevelText.text = $"Lvl {enemy.level}";
        enemyHealthSlider.maxValue = enemy.baseData.maxHealth;
        enemyHealthSlider.currentValue = enemy.health;
        enemyIcon.sprite = enemy.baseData.icon;
    }

    public void RefreshUI(PokeIUTInstance player, PokeIUTInstance enemy)
    {
        playerHealthSlider.maxValue = player.baseData.maxHealth;
        playerHealthSlider.currentValue = player.health;
        playerLevelText.text = $"Lvl {player.level}";
        playerNameText.text = player.baseData.pokeiutName;
        playerIcon.sprite = player.baseData.icon;
        enemyHealthSlider.maxValue = enemy.baseData.maxHealth;
        enemyHealthSlider.currentValue = enemy.health;
        enemyLevelText.text = $"Lvl {enemy.level}";
        enemyNameText.text = enemy.baseData.pokeiutName;
        enemyIcon.sprite = enemy.baseData.icon;
    }

    public void ShowPokeIUTTeamIcons(PlayerData playerData, EnemyData enemyData, bool show)
    {
        for (int i = 0; i < playerData.pokeIUTTeam.Length; i++)
        {
            playerPokeIUTTeamIcons[i].gameObject.SetActive(show);
        }
        for (int i = 0; i < enemyData.pokeIUTTeam.Length; i++)
        {
            enemyPokeIUTTeamIcons[i].gameObject.SetActive(show);
        }
    }

    public void UpdatePokeIUTTeamIcons(PlayerData playerData, EnemyData enemyData)
    {
        for (int i = 0; i < playerData.pokeIUTTeam.Length; i++)
        {
            if (playerData.pokeIUTTeam[i].health <= 0)
            {
                playerPokeIUTTeamIcons[i].color = Color.red;
            }
        }
        for (int i = 0; i < enemyData.pokeIUTTeam.Length; i++)
        {
            if (enemyData.pokeIUTTeam[i].health <= 0)
            {
                enemyPokeIUTTeamIcons[i].color = Color.red;
            }
        }
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
        playerHealthBar.SetActive(show);
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
        for (int i = 0; i < playerData.pokeIUTTeam.Length; i++)
        {
            pokeIUTTeamButtons[i].gameObject.SetActive(show);
        }
    }

    public void ShowBagUI(bool show)
    {
        bagUI.gameObject.SetActive(show);
        if (show)
        {
            UpdateBagInfos(GameManager.Instance.playerData);
        }
    }

    public void UpdatePokeIUTTeamInfos(PlayerData playerData)
    {
        // Create new buttons
        for (int index = 0; index < playerData.pokeIUTTeam.Length; index++)
        {
            var pokeIUT = playerData.pokeIUTTeam[index];
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

        int index = -1;
        foreach (var item in playerData.items)
        {
            index++;
            if (currentItemType == ItemType.General || item.baseData.itemType == currentItemType)
            {
                var button = Instantiate(itemButtonPrefab, itemButtonContainer).GetComponent<Button>();
                button.transform.Find("Item_Name").GetComponent<Text>().text = item.baseData.itemName;
                button.transform.Find("Item_Quantity").GetComponent<Text>().text = $"Qty {item.quantity}";
                int globalIndex = index;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnItemClicked?.Invoke(globalIndex));
                Debug.Log("Adding Listener to Item with index : " + index);
                itemButtons.Add(button);
            }
        }
        UpdateItemTypeText();
    }

    private void ShowPreviousItemType()
    {
        currentItemTypeIndex = (currentItemTypeIndex - 1 + itemTypes.Length) % itemTypes.Length;
        currentItemType = itemTypes[currentItemTypeIndex];
        UpdateBagInfos(GameManager.Instance.playerData);
    }

    private void ShowNextItemType()
    {
        currentItemTypeIndex = (currentItemTypeIndex + 1) % itemTypes.Length;
        currentItemType = itemTypes[currentItemTypeIndex];
        UpdateBagInfos(GameManager.Instance.playerData);
    }

    private void UpdateItemTypeText()
    {
        itemTypeText.text = $"{currentItemType}";
    }

}