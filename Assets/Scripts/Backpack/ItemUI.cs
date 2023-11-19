using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Image itemSpriteImg;
    [SerializeField] private Text  itemAmountText;

    public void RefreshItemUI(Item item)
    {
        ItemData itemData = DataManager.Instance.ItemDatas[item.id];
        itemSpriteImg.sprite = itemData.sprite;
        itemAmountText.text = item.amount.ToString();
    }
}
