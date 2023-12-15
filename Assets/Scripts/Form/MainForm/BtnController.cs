using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _btn;
    private Color _normalColor;
    private Color _changeColor;

    private Image _btnImg;

    private void Awake()
    {
        _btn = GetComponent<Button>();
        _btnImg = _btn.image;
        _normalColor = _btnImg.color;
        _changeColor = new Color(0.42f, 0.79f, 0.63f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DOTween.To(
            () => _btnImg.color,
            _ => _btnImg.color = _,
            _changeColor,
            0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DOTween.To(
            () => _btnImg.color,
            _ => _btnImg.color = _,
            _normalColor,
            0.5f);
    }
}
