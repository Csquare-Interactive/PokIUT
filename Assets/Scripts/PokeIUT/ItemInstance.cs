using System.Collections.Generic;

[System.Serializable]
public class ItemInstance
{
    public ItemData baseData;
    public int quantity;

    public ItemInstance(ItemData data)
    {
        baseData = data;
        quantity = 0;

    }

    public void Reset()
    {
        quantity = baseData.maxQuantity;
    }

    public void Add(int amount)
    {
        quantity += amount;
    }
}