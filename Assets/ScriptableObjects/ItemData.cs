using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItem", menuName = "Player/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int maxQuantity;
    public ItemType itemType;
}

[Serializable]
public enum ItemType
{
    General,
    Healing,
    Attack,
    Defense,
}