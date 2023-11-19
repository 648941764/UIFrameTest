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
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDragging && eventData.button == PointerEventData.InputButton.Left) 
        {
            mousePos = eventData.position;
            isDragging = true;
            //����dragItem
            UIManager.Instance.GetForm<BackpackForm>().SetItemUIToDragLayer(this);
            //this.gameObject.SetActive(false);

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && eventData.button == PointerEventData.InputButton.Left)
        {
            mousePos = eventData.position;
            this.gameObject.transform.position = mousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging && eventData.button == PointerEventData.InputButton.Left)
        {
            mousePos = eventData.position;
            isDragging = false;
            //�ҵ������һ�����ӣ�Ȼ�󽻻����ݣ����Ұ�this�ĸ��ڵ��������õ�����ĸ�������
            RectTransform rectTransform = transform as RectTransform;
            int nearest = UIManager.Instance.GetForm<BackpackForm>().FindNearestItemSlot(transform.position);
            if (nearest >= 0)
            {
                Test.Instance.Backpack.SwapItem(itemUIIndex, nearest);
            }
            else
            {
                this.transform.SetParent(UIManager.Instance.GetForm<BackpackForm>().ItemSlots[itemUIIndex]);
            }
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public void RefreshItemUI(Item item)
    {
        ItemData itemData = DataManager.Instance.ItemDatas[item.id];
        itemSpriteImg.sprite = itemData.sprite;
        itemAmountText.text = item.amount.ToString();
    }

    public void Setindex(int index)
    {
        itemUIIndex = index;
    }
}
