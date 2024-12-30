using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItem", menuName = "Player/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int maxQuantity;
    public ItemType itemType;
    public int price;
    public string description;
}

[Serializable]
public enum ItemType
{
    General,
    Healing,
    Attack,
    Defense,
    Pokiutball,
}