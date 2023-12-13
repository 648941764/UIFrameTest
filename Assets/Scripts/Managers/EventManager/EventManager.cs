using System.Collections.Generic;
using System;

public delegate void EventHandle(EventParam eventParam);

public sealed class EventManager : Singleton<EventManager>
{
    private event EventHandle EventHandle;

    public void Add(EventHandle eventHandle) => EventHandle += eventHandle;

    public void Del(EventHandle eventHandle) => EventHandle -= eventHandle;

    public void Broadcast(EventParam eventParam)
    {
        EventHandle?.Invoke(eventParam);
        EventParam.Release(eventParam);
    }

    //public void Broadcast(EventType eventType)
    //{
    //    EventParam eventParam = paramPool.Get();
    //    eventParam.Push(eventType);
    //    eventParam.eventName = eventType;
    //    EventHandle?.Invoke(eventParam);
    //    paramPool.Return(eventParam);
    //}
}

public class EventParam
{
    public EventType eventName;
    private Queue<object> _params = new Queue<object>();

    public static readonly Stack<EventParam> pool = new Stack<EventParam>();

    private EventParam() { }

    public int Count => _params.Count;

    public EventParam Push<T>(T param)
    {
        _params.Enqueue(param);//添加元素
        return this;
    }

    public T Get<T>()
    {
        return (T)_params.Dequeue();//取出元素
    }

    public static EventParam Get(EventType eventType, params object[] @params)
    {
        EventParam eventParam = pool.Count > 0 ? eventParam = pool.Pop() : new EventParam();
        eventParam.eventName = eventType;
        for (int i = -1; ++i < @params.Length;)
        {
            eventParam.Push(@params[i]);
        }
        return eventParam;
    }

    public static void Release(EventParam eventParam)
    {
        eventParam.eventName = EventType.None;
        eventParam._params.Clear();
        pool.Push(eventParam);
    }
}

public class EventParamPool<T> where T : new()
{
    private Queue<T> _pool = new Queue<T>();

    public T Get()
    {
        if (_pool.Count == 0)
        {
            return new T();
        }
        else
        {
            return _pool.Dequeue();
        }
    }

    public void Return(T param)
    {
        _pool.Enqueue(param);
    }
}