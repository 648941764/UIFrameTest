using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverForm : Form
{
    [SerializeField] private Button _btnTry, _btnQuit;

    private void Start()
    {
        _btnTry.onClick.AddListener(OnBtnTryClicked);
        _btnQuit.onClick.AddListener(OnBtnQuitCliked);
    }

    protected override void OnOpen()
    {
        base.OnOpen();
    }

    protected override void OnClose()
    {
        base.OnClose();
    }

    private void OnBtnQuitCliked()
    {
        UIManager.Instance.Open<MainForm>();
        GameManager.Instance.SwithScene(GameScene.Nothing);
        UIManager.Instance.Close<GameOverForm>();
    }

    private void OnBtnTryClicked()//”–¥˝…Ã»∂
    {
        Debug.Log("1");
        GameManager.Instance.SwithScene(GameScene.Level1);
        UIManager.Instance.Close<GameOverForm>();
    }
}
