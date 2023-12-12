using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class MainForm : Form
{
    [SerializeField] Button btnStart, btnStory, btnQuit;

    private void Start()
    {
        btnStart.onClick.AddListener(OnBtnStartClick);
        btnStory.onClick.AddListener(OnBtnStoryClick);
        btnQuit.onClick.AddListener(OnBtnQuitClick);
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
}
