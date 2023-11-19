using System.Collections.Generic;
using System;

public delegate void EventHandle(EventParam eventParam);

public sealed class EventManager : Singleton<EventManager>
{
    private event EventHandle EventHandle;
    public void Add(EventHandle eventHandle) => EventHandle += eventHandle;
    public void Del(EventHandle eventHandle) => EventHandle -= eventHandle;
    public void Broadcast(EventParam eventParam) => EventHandle?.Invoke(eventParam);
}

public class EventParam
{
    public EventType eventName;
    private Queue<object> @params = new Queue<object>();

    public int Count => @params.Count;

    public EventParam Push<T>(T param)
    {
        @params.Enqueue(param);
        return this;
    }

    public T Get<T>()
    {
        return (T)@params.Dequeue();
    }
}