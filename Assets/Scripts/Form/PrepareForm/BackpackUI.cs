using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackpackUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _imgUISprite;
    [SerializeField] private Text _txtItemAmount;

    private int UIIndex;
    private Image _imgDrag;
    private Vector3 _mousePos;
    private bool _isDragging = false;
    private Color _normalColor = new Color(1f, 1f, 1f, 1f);

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_isDragging && eventData.button == PointerEventData.InputButton.Left)
        {
            _mousePos = eventData.position;
            _isDragging = true;
            _imgDrag = Instantiate(UIManager.Instance.GetForm<PrepareForm>().ImgDrag, UIManager.Instance.GetForm<PrepareForm>().BackpackParent);
            _imgDrag.transform.position = _mousePos;
            _imgDrag.transform.GetComponent<Image>().sprite = _imgUISprite.sprite;
            _imgDrag.transform.GetComponent<Image>().color = _normalColor;
            _imgDrag.SetActivate(true);

            _imgUISprite.color = new Color(1, 1, 1, 0);
            _txtItemAmount.text = " ";
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDragging && eventData.button == PointerEventData.InputButton.Left)
        {
            _mousePos = eventData.position;
            _imgDrag.gameObject.transform.position = _mousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        _mousePos = eventData.position;

        int index = UIManager.Instance.GetForm<PrepareForm>().FindNearestBackpackItems(_imgDrag);
        if (index >= 0) 
        {
            _imgUISprite.color = _normalColor;
            _txtItemAmount.text = default; 

            GameItem currentItem = Test.Instance.GameBackpack.Items[this.UIIndex];
            GameItem nearestItme = Test.Instance.GameBackpack.Items[index];


        }
    }

    public void RefreshBackpackUI(GameItem item)
    {
        ItemData data = DataManager.Instance.itemDatas[item.id];
        _imgUISprite.sprite = data.sprite;
        _txtItemAmount.text = item.amount.ToString();

    }

    public void SetIndex(int index)
    {
        UIIndex = index;
    }
}
