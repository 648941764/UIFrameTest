using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonForm : Form
{
    [SerializeField] private Button button1, button2, button3;

    protected override void OnRefresh()
    {
        base.OnRefresh();
    }
    protected override void RegisterEvents()
    {
        base.RegisterEvents();
        button1.onClick.AddListener(OnButton1Clicked);
        button2.onClick.AddListener(OnButton2Clicked);
        button3.onClick.AddListener(OnButton3Clicked);
    }

    public void OnButton1Clicked()
    {
        Debug.Log("1");
    }
    public void OnButton2Clicked()
    {
        Debug.Log("2");
    }
    public void OnButton3Clicked() 
    {
        Debug.Log("3");
    }
}
