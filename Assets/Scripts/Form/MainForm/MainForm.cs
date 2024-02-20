using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class MainForm : Form
{
    [SerializeField] Button btnStart, btnStory, btnQuit;
    private List<RectTransform> _btnList = new List<RectTransform>();
    private List<Image> _imaList = new List<Image>();
    private Color _normalColor;
    private Color _changeColor;
    private Vector3 _mousePos;
    private bool _needChange;



    private void Start()
    {
        _btnList.Add(btnStart.transform as RectTransform);
        _btnList.Add(btnStory.transform as RectTransform);
        _btnList.Add(btnQuit.transform as RectTransform);

        _imaList.Add(btnStart.image);
        _imaList.Add(btnStory.image);
        _imaList.Add(btnQuit.image);

        _normalColor = btnStart.GetComponent<Image>().color;
        _changeColor = new Color(0.42f, 0.79f, 0.63f, 1f);

        btnStart.onClick.AddListener(OnBtnStartClick);
        btnStory.onClick.AddListener(OnBtnStoryClick);
        btnQuit.onClick.AddListener(OnBtnQuitClick);
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        GameManager.Instance.UpdateHandle += MainFormUpdate;
    }

    private void MainFormUpdate()
    {
        _mousePos = Input.mousePosition;
        int index = FindNearestBtnIndex(_mousePos);

        if (_needChange) 
        {
            foreach (var image in _imaList)
            {
                DOTween.To(
                    () => image.color,
                    _ => image.color = _,
                    _normalColor, 0.5f);
            }
            _needChange = false;
            
        }

        if (index >= 0)
        {
            _imaList[index].color = _changeColor;

            DOTween.To(
                () => _imaList[index].color,
                _ => _imaList[index].color = _,
                _changeColor, 0.5f);

            _needChange = true;
        }
    }

    protected override void OnClose()
    {
        base.OnClose();
        GameManager.Instance.UpdateHandle -= MainFormUpdate;
        foreach (var image in _imaList)
        {
            image.color = _normalColor;
        }
    }

    private void OnBtnStartClick()
    {
        UIManager.Instance.Open<PrepareForm>();
        UIManager.Instance.Close<MainForm>();
        UIManager.Instance.Open<DialogueForm>();
        UIManager.Instance.GetForm<DialogueForm>().DialogueBox.SetActivate(false);
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
