using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas), typeof(RectTransform))]
public abstract class Form : MonoBehaviour
{
    [SerializeField] private FormLayer formLayer = FormLayer.Nothing;

    public bool IsOpen => gameObject.activeSelf;

    private readonly EventRegistry eventRegistry = new EventRegistry();

    private void Awake() 
    {
        SetSortingLayer();
        InitComponents(); 
        RegisterEvents(); 
    }
    private void Start() { }
    private void OnEnable() { eventRegistry.AddEvents(); }
    private void OnDisable() { eventRegistry.DelEvents(); }
    private void OnDestroy() { eventRegistry.ClearEvents(); }
    private void SetSortingLayer() => GetComponent<Canvas>().sortingOrder = (int)formLayer;

    protected virtual void InitComponents() { }
    protected virtual void RegisterEvents() { }
    protected virtual void OnOpen() { Refresh(); }
    protected virtual void OnRefresh() { }
    protected virtual void OnClose() { }
    protected void AddEvent(EventHandle eventHandle) => eventRegistry.AddEvent(eventHandle);
    protected void DelEvent(EventHandle eventHandle) => eventRegistry.DelEvent(eventHandle);

    public void Enable() { gameObject.SetActivate(true); OnOpen(); }
    public void Refresh() { OnRefresh(); }
    public void Disable() { gameObject.SetActivate(false); OnClose(); }
}