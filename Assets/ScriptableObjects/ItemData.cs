using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Player/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int quantity;
}