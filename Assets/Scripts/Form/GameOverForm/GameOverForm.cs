using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverForm : Form
{
    [SerializeField] private Button _btnTry, _btnQuit;

    private void Start()
    {
        _btnTry = GetComponent<Button>();
        _btnQuit = GetComponent<Button>();

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
        this.gameObject.SetActive(false);
    }

    private void OnBtnTryClicked()//”–¥˝…Ã»∂
    {

    }
}
