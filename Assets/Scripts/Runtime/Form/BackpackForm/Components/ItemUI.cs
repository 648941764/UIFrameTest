using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image itemSpriteImg;
    [SerializeField] private Text  itemAmountText;

    private bool isDragging;
    private Vector2 mousePos, lastPos;
    private RectTransform dragItem;

    private void Start()
    {


    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDragging && eventData.button == PointerEventData.InputButton.Left) 
        {
            isDragging = true;
            this.gameObject.SetActive(false);
            //����dragItem
            ItemUI DragUI = Instantiate<ItemUI>(this);

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
        //�ж�������������ĸ�������һ����������������ݣ���ô��ֱ�ӽ������ݣ����Ϊ�վ�ֱ�Ӵ���
    }

    public void RefreshItemUI(Item item)
    {
        ItemData itemData = DataManager.Instance.ItemDatas[item.id];
        itemSpriteImg.sprite = itemData.sprite;
        itemAmountText.text = item.amount.ToString();
    }
}
