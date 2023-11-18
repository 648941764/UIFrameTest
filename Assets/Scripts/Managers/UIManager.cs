using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : SingletonMono<UIManager>
{
    public const string FORM_PATH = "Forms/{0}";

    public static readonly Vector3 FORM_POS = new Vector3(0f, -10000f, 0f);

    private readonly Dictionary<Type, Form> forms = new Dictionary<Type, Form>();

    private Transform formParents;

    protected override void OnAwake()
    {
        base.OnAwake();
        formParents = new GameObject("Forms").transform;
        DontDestroyOnLoad(formParents);
    }

    private T CreateForm<T>() where T : Form
    {
        T form = Instantiate(Resources.Load<T>(GetFormPath(typeof(T).Name)), FORM_POS, Quaternion.identity);
        form.name = typeof(T).Name;
        form.transform.SetParent(formParents);
        return form;
    }

    private string GetFormPath(string formName) => string.Format(FORM_PATH, formName);

    private bool IsCreated<T>() where T : Form
    {
        return forms.ContainsKey(typeof(T));
    }

    public void Open<T>() where T : Form
    {
        bool isCreate = !IsCreated<T>();
        T form = isCreate ? CreateForm<T>() : (T)forms[typeof(T)];
        if (isCreate)
        {
            forms.Add(typeof(T), form);
        }
        form.Enable();
    }

    public void Close<T>() where T : Form
    {
        if (!IsOpen<T>())
        {
            return;
        }
        forms[typeof(T)].Disable();
    }

    public void DestoryForm<T>()
    {

    }

    public bool IsOpen<T>() where T : Form
    {
        if (!IsCreated<T>())
        {
            return false;
        }
        
        return forms[typeof(T)].IsOpen;
    }
}
