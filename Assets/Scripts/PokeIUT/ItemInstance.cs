using System.Collections.Generic;

[System.Serializable]
public class ItemInstance
{
    public ItemData baseData;
    public int quantity;

    public ItemInstance(ItemData data)
    {
        baseData = data;
    }

    public void Reset()
    {
        quantity = baseData.maxQuantity;
    }
}