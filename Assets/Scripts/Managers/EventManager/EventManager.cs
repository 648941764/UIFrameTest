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
    private readonly List<object> _params = new List<object>();

    public static readonly Stack<EventParam> pool = new Stack<EventParam>();

    private EventParam() { }

    public int Count => _params.Count;

    public EventParam Push(object param)
    {
        _params.Add(param);//添加元素
        return this;
    }

    public T Get<T>(int index)
    {
        if ((uint)index < _params.Count)
        {
            return (T)_params[index]; //取出元素
        }
        return default;
    }

    public static EventParam Get(EventType eventType, params object[] eventParams)
    {
        EventParam eventParam = pool.Count > 0 ? pool.Pop() : new EventParam();
        eventParam.eventName = eventType;
        for (int i = -1; ++i < eventParams.Length;)
        {
            eventParam.Push(eventParams[i]);
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