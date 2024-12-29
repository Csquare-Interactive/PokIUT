using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;




public class ItemButton : MonoBehaviour
{
    public Text itemNameText;
    private ItemData itemData;
    private System.Action<ItemData> onClick;

    public void Setup(ItemData item, System.Action<ItemData> onClickAction)
    {
        itemData = item;
        itemNameText.text = item.itemName;
        onClick = onClickAction;
    }

    public void OnClick()
    {
        onClick?.Invoke(itemData);
    }
}