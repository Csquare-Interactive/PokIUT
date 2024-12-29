using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemsData", menuName = "Player/ShopItemsData")]
public class ShopItemsDatabase : ScriptableObject
{
    public static ShopItemsDatabase Instance;

    public ItemData[] items;

   
}