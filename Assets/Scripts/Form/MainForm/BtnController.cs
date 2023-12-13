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

    private void Awake()
    {
        _btn = GetComponent<Button>();
        _normalColor = _btn.GetComponent<Image>().color;
        _changeColor = new Color(0.42f, 0.79f, 0.63f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _btn.GetComponent<Image>().color = _changeColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _btn.GetComponent<Image>().color = _normalColor;
    }
}
