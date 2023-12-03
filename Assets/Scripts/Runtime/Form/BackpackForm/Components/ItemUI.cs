using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image itemSpriteImg;
    [SerializeField] private Text  itemAmountText;
    private int itemUIIndex;


    private bool isDragging;
    private Vector2 mousePos;
    private int cilckIndex;
    private Image UI;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDragging && eventData.button == PointerEventData.InputButton.Left) 
        {
            mousePos = eventData.position;
            isDragging = true;
            //生成dragItem
            UI = Instantiate(UIManager.Instance.GetForm<BackpackForm>().image, UIManager.Instance.GetForm<BackpackForm>().DragItem);
            UI.gameObject.GetComponent<Image>().sprite = this.itemSpriteImg.sprite;
            UI.gameObject.SetActive(true);
            UI.transform.position = this.transform.position;
            //把所点击的ItemUI给关掉
            this.transform.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            this.transform.GetChild(0).GetComponent<Text>().text = "";


        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && eventData.button == PointerEventData.InputButton.Left)
        {
            mousePos = eventData.position;
            UI.gameObject.transform.position = mousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)//这里需要重新判断
    {
        if (isDragging && eventData.button == PointerEventData.InputButton.Left)
        {
            mousePos = eventData.position;
            isDragging = false;
            //找到最近的一个格子，然后交换数据，并且把this的父节点重新设置到最近的格子那里
            int nearest = UIManager.Instance.GetForm<BackpackForm>().FindNearestItemSlot(UI);
            if (nearest >= 0)
            {
                this.transform.GetComponent<Image>().color = new Color(255, 255, 255, 255);
                this.transform.GetChild(0).GetComponent<Text>().text = default;
                Item currentItem = Test.Instance.Backpack.Items[this.itemUIIndex];
                Item nearestItem = Test.Instance.Backpack.Items[nearest];
                if(nearestItem != null && nearest != itemUIIndex)
                {
                    if (currentItem.id == nearestItem.id)
                    {
                        nearestItem.amount += currentItem.amount;
                        Test.Instance.Backpack.RemoveItem(currentItem);
                        UIManager.Instance.GetForm<BackpackForm>().RefreshSlotImage();
                    }
                    else
                    {
                        Test.Instance.Backpack.SwapItem(itemUIIndex, nearest);
                    }
                }
                else
                {
                    Test.Instance.Backpack.SwapItem(itemUIIndex, nearest);
                }
            }
            else
            {
                this.transform.GetComponent<Image>().color = new Color(255, 255, 255, 255);
                this.transform.GetChild(0).GetComponent<Text>().text = Test.Instance.Backpack.Items[this.itemUIIndex].amount.ToString();
            }
            Destroy(UI.gameObject);
        }
    }


    public void RefreshItemUI(Item item)
    {
        ItemData itemData = DataManager.Instance.itemDatas[item.id];
        itemSpriteImg.sprite = itemData.sprite;
        itemAmountText.text = item.amount.ToString();
    }

    public void Setindex(int index)
    {
        itemUIIndex = index;
    }

   
}
