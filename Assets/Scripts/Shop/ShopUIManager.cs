using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;


public class ShopUIManager : MonoBehaviour
{
    public static ShopUIManager Instance;
    public ExploreUIManager exploreUIManager;


    public Text itemNameText;
    public Text itemPriceText;
    public Text itemQuantityText;
    public Text itemDescriptionText;
    public Text playerMoneyText;
    public Button buyButton;
    public Button backButton;

    private ItemData currentItem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

       private void Start()
    {
        // Initialiser les textes avec les valeurs de la potion par défaut
        ItemData defaultItem = GetDefaultItem();
        if (defaultItem != null)
        {
            ShowItemDetails(defaultItem);
        }
        backButton.onClick.AddListener(CloseShop);
    }

    private ItemData GetDefaultItem()
    {
        // Rechercher l'item "Potion" dans la base de données des items du shop
        foreach (var item in GameManager.Instance.shop.shopItemsDatabase.items)
        {
            if (item.itemName == "Potion")
            {
                return item;
            }
        }
        return null;
    }

    public void ShowItemDetails(ItemData item)
    {
        currentItem = item;
        itemNameText.text = item.itemName;
        itemPriceText.text = "Prix : " + item.price.ToString();
        itemDescriptionText.text = item.description;

        UpdateItemQuantity(item);

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => BuyItem(item));
    }

    private void BuyItem(ItemData item)
    {
        GameManager.Instance.shop.BuyItem(item);
        UpdateItemQuantity(item);
        UpdatePlayerMoney();
    }

    private void UpdateItemQuantity(ItemData item)
    {
        int quantity = GameManager.Instance.playerData.GetItemQuantity(item);
        itemQuantityText.text = "Quantité actuelle : " + quantity.ToString();
    }

    public void UpdatePlayerMoney()
    {
        int money = GameManager.Instance.playerData.money;
        playerMoneyText.text = "Argent : " + money.ToString();
        exploreUIManager.UpdateMoney(); 
        
        }

    private void CloseShop()
    {
        GameManager.Instance.shop.shopUI.SetActive(false);
    }
}