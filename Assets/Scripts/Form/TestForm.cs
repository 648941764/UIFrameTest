using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class TestForm : Form
{
    protected override void OnRefresh()
    {
        base.OnRefresh();
        Debug.Log("TestForm Refresh.");
    }

    protected override void RegisterEvents()
    {
        base.RegisterEvents();
        AddEvent(TestFormEvent1);
        AddEvent(TestFormEvent2);
    }

    private void TestFormEvent1(EventParam eventParam)
    {
        if (eventParam.eventName == EventType.Example)
        {
            Debug.Log("Test Form, TestFormEvent1, EventType");
        }
    }

    private void TestFormEvent2(EventParam eventParam)
    {
        if (eventParam.eventName == EventType.Example2)
        {
            Debug.Log("Test Form, TestFormEvent2, Example2");
        }
    }
}
