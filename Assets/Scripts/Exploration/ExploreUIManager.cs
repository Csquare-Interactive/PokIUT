using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Text = TMPro.TextMeshProUGUI;
using Image = UnityEngine.UI.Image;

public class ExploreUIManager : MonoBehaviour
{

    public Button inventoryButton;
    public Button pokeiutButton;
    public Text playerMoneyText;

    [Header("Inventory")]
    public GameObject inventoryUI;
    public Button backButtonBag;
    public Transform itemButtonContainer;
    public GameObject itemButtonPrefab;
    private List<Button> itemButtons = new List<Button>();
    public Button leftArrowButton;
    public Button rightArrowButton;
    public Text itemTypeText;

    [Header("PokeIUTTeam")]
    public Canvas pokeIUTTeamUI;
    public Button backButtonPokeIUTTeam;
    private List<Button> pokeIUTTeamButtons;

    public event Action OnInventoryClicked;
    public event Action OnPokeiutClicked;
    public event Action OnBackClicked;

    private ItemType currentItemType = ItemType.General;
    private ItemType[] itemTypes = (ItemType[])System.Enum.GetValues(typeof(ItemType));
    private int currentItemTypeIndex = 0;

    void Start()
    {
        Debug.Log("ExploreUIManager Start called");

        if (inventoryButton == null) Debug.LogError("inventoryButton is not assigned");
        if (pokeiutButton == null) Debug.LogError("pokeiutButton is not assigned");

        // Position the buttons at the bottom right
        RectTransform inventoryButtonRect = inventoryButton.GetComponent<RectTransform>();
        RectTransform pokeiutButtonRect = pokeiutButton.GetComponent<RectTransform>();

        inventoryButtonRect.anchorMin = new Vector2(1, 0);
        inventoryButtonRect.anchorMax = new Vector2(1, 0);
        inventoryButtonRect.pivot = new Vector2(1, 0);
        inventoryButtonRect.anchoredPosition = new Vector2(-10, 10); // Adjust the position as needed

        pokeiutButtonRect.anchorMin = new Vector2(1, 0);
        pokeiutButtonRect.anchorMax = new Vector2(1, 0);
        pokeiutButtonRect.pivot = new Vector2(1, 0);
        pokeiutButtonRect.anchoredPosition = new Vector2(-10, 60); // Adjust the position as needed


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

        // Add listeners for the buttons
        inventoryButton.onClick.AddListener(() => OnInventoryClicked?.Invoke()); 
        pokeiutButton.onClick.AddListener(() =>  OnPokeiutClicked?.Invoke()); 
        backButtonPokeIUTTeam.onClick.AddListener(() => OnBackClicked?.Invoke());
        backButtonBag.onClick.AddListener(() => OnBackClicked?.Invoke());
        leftArrowButton.onClick.AddListener(ShowPreviousItemType);
        rightArrowButton.onClick.AddListener(ShowNextItemType);


        UpdateMoney();

        SetupExploreUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            // Simulate pressing the PokeIUT button
            OnPokeiutClicked?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            // Simulate pressing the Inventory button
            OnInventoryClicked?.Invoke();
        }
    }

    public void SetupExploreUI()
    {
        Debug.Log("SetupExploreUI called");

        // Initialize UI elements
        inventoryUI.SetActive(false);
        pokeIUTTeamUI.gameObject.SetActive(false);
    }

    public void ShowInventoryUI(bool show)
    {
        Debug.Log("ShowInventoryUI called with show = " + show);
        inventoryUI.SetActive(show);
        if (show)
        {
            UpdateInventoryInfos(GameManager.Instance.playerData);
        }
    }

    public void ShowExplorationUI(bool show)
    {
        inventoryButton.gameObject.SetActive(show);
        pokeiutButton.gameObject.SetActive(show);
    }

    public void UpdateInventoryInfos(PlayerData playerData)
    {
        Debug.Log("UpdateInventoryInfos called");
        // Clear existing buttons
        foreach (Transform child in itemButtonContainer)
        {
            Destroy(child.gameObject);
        }
        itemButtons.Clear();

        // Create new buttons
        foreach (var item in playerData.items)
        {
            if (currentItemType == ItemType.General || item.baseData.itemType == currentItemType)
            {
                var button = Instantiate(itemButtonPrefab, itemButtonContainer).GetComponent<Button>();
                button.transform.Find("Item_Name").GetComponent<Text>().text = item.baseData.itemName;
                button.transform.Find("Item_Quantity").GetComponent<Text>().text = $"Qty {item.quantity}";
                itemButtons.Add(button);
            }
        }
        UpdateItemTypeText();
    }

    private void ShowPreviousItemType()
    {
        currentItemTypeIndex = (currentItemTypeIndex - 1 + itemTypes.Length) % itemTypes.Length;
        currentItemType = itemTypes[currentItemTypeIndex];
        UpdateInventoryInfos(GameManager.Instance.playerData);
    }

    private void ShowNextItemType()
    {
        currentItemTypeIndex = (currentItemTypeIndex + 1) % itemTypes.Length;
        currentItemType = itemTypes[currentItemTypeIndex];
        UpdateInventoryInfos(GameManager.Instance.playerData);
    }

    private void UpdateItemTypeText()
    {
        itemTypeText.text = $"{currentItemType}";
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

    public void UpdatePokeIUTTeamInfos(PlayerData playerData)
    {
    for (int index = 0; index < playerData.pokeIUTTeam.Length; index++)
        {
            var pokeIUT = playerData.pokeIUTTeam[index];
            var button = pokeIUTTeamButtons[index];
            button.transform.Find("PokeIUT_Name").GetComponent<Text>().text = pokeIUT.baseData.pokeiutName;
            button.transform.Find("PokeIUT_Health").GetComponent<Text>().text = $"HP {pokeIUT.health}";
            button.transform.Find("PokeIUT_Icon").GetComponent<Image>().sprite = pokeIUT.baseData.icon;
            button.transform.Find("PokeIUT_Level").GetComponent<Text>().text = $"LVL {pokeIUT.level}";

        }
    }

    public void UpdateMoney()
    {
        int money = GameManager.Instance.playerData.money;
        playerMoneyText.text = "Argent : " + money.ToString();
    }
}