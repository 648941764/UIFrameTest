using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainForm : Form
{
    [SerializeField] Button btnStart, btnStory, btnQuit;
    [SerializeField] private List<RectTransform> _btnList = new List<RectTransform>();
    private Color _normalColor;
    private Color _changeColor;
    private Vector3 _mousePos;
    

    private void Start()
    {
        _normalColor = btnStart.GetComponent<Image>().color;
        _changeColor = new Color(0.42f, 0.79f, 0.63f, 1f);

        btnStart.onClick.AddListener(OnBtnStartClick);
        btnStory.onClick.AddListener(OnBtnStoryClick);
        btnQuit.onClick.AddListener(OnBtnQuitClick);
        GameManager.Instance.UpdateHandle += MainFormUpdate;
    }

    private void MainFormUpdate()
    {
        _mousePos = Input.mousePosition;
        int index = FindNearestBtnIndex(_mousePos);

        foreach (var btn in _btnList)
        {
            btn.GetComponent<Image>().color = _normalColor;
        }

        if (index >= 0)
        {
            RectTransform rect = _btnList[index];
            rect.GetComponent<Image>().color = _changeColor;
        }
    }

    protected override void OnClose()
    {
        base.OnClose();
        GameManager.Instance.UpdateHandle -= MainFormUpdate;
    }

    private void OnBtnStartClick()
    {
        GameManager.Instance.StartGame();
    }

    private void OnBtnStoryClick()
    {

    }

    private void OnBtnQuitClick()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private int FindNearestBtnIndex(Vector2 mousePos)
    {
        for (int i = 0; i < _btnList.Count; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_btnList[i], mousePos))
            {
                return i;
            }
        }
        return -1;
    }



}
