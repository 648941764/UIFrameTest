using System.Collections.Generic;

public sealed class EventRegistry
{
    private readonly List<EventHandle> eventHandles = new List<EventHandle>();

    public void AddEvent(EventHandle eventHandle)
    {
        eventHandles.Add(eventHandle);
    }

    public void DelEvent(EventHandle eventHandle)
    {
        eventHandles.Remove(eventHandle);
    }

    public void AddEvents()
    {
        int i = -1;
        while (++i < eventHandles.Count)
        {
            EventManager.Instance.Add(eventHandles[i]);
        }
    }

    public void DelEvents()
    {
        int i = -1;
        while (++i < eventHandles.Count)
        {
            EventManager.Instance.Del(eventHandles[i]);
        }
    }

    public void Broadcast(EventType eventType)
    {
        EventManager.Instance.Broadcast(eventType);
    }

    public void ClearEvents()
    {
        int i = -1;
        while (++i < eventHandles.Count)
        {
            EventManager.Instance.Del(eventHandles[i]);
        }
        eventHandles.Clear();
    }
}