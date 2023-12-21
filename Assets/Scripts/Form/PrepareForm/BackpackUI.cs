using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
            _imgDrag.transform.position = _mousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isDragging && eventData.button == PointerEventData.InputButton.Left)
        {
            _isDragging = false;
            _mousePos = eventData.position;

            int index = UIManager.Instance.GetForm<PrepareForm>().FindNearestBackpackItems(_imgDrag);
            if (index < 0)
            {
                _imgUISprite.color = _normalColor;
                _txtItemAmount.text = CharacterManager.Instance.GameBackpack.Items[UIIndex].amount.ToString();
                Destroy(_imgDrag.gameObject);
                return;
            }

            _txtItemAmount.text = default;
            GameItem currentItem = CharacterManager.Instance.GameBackpack.Items[UIIndex];
            GameItem nearestItem = CharacterManager.Instance.GameBackpack.Items[index];

            if (nearestItem != null && index != this.UIIndex)
            {
                if (currentItem.id == nearestItem.id)
                {
                    nearestItem.amount += currentItem.amount;
                    CharacterManager.Instance.GameBackpack.RemoveItme(currentItem);
                }
                else
                {
                    CharacterManager.Instance.GameBackpack.SwapItem(this.UIIndex, index);
                }
            }
            else
            {
                CharacterManager.Instance.GameBackpack.SwapItem(this.UIIndex, index);
            }

            _imgUISprite.color = _normalColor;
            Destroy(_imgDrag.gameObject);
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
