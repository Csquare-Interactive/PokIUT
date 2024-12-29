using UnityEngine;


public class Shop : MonoBehaviour
{
    public GameObject shopUI;
    public Transform itemsContainer;
    public GameObject itemButtonPrefab;
    public PlayerData playerData;
    public ShopItemsDatabase shopItemsDatabase; 

    private void Awake()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
    }

     public void ActivateShop()
    {
        shopUI.SetActive(true);
        PopulateShop();
        ShopUIManager.Instance.UpdatePlayerMoney();
    }

    private void PopulateShop()
    {
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in shopItemsDatabase.items)
        {
            var button = Instantiate(itemButtonPrefab, itemsContainer).GetComponent<ItemButton>();
            button.Setup(item, OnItemClicked);
        }
    }

    private void OnItemClicked(ItemData item)
    {
        // Afficher les détails de l'item et le bouton acheter
        ShopUIManager.Instance.ShowItemDetails(item);
    }

    public void BuyItem(ItemData item)
    {
        if (playerData.money >= item.price)
        {
            playerData.money -= item.price;
            playerData.AddItem(item);
            Debug.Log("Item acheté : " + item.itemName);
        }
        else
        {
            Debug.Log("Pas assez d'argent pour acheter cet item.");
        }
    }
}