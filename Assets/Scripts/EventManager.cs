using UnityEngine;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public static EventManager instance { get; private set; }
    private static EventManager _instance = null;
    private Dictionary<EVENT_TYPE, List<IListener>> Listeners = new Dictionary<EVENT_TYPE, List<IListener>>();

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddListener(EVENT_TYPE eventType, IListener listener)
    {
        List<IListener> listenerList = null;
        if (Listeners.TryGetValue(eventType, out listenerList))
        {
            listenerList = new List<IListener>();
            Listeners.Add(eventType, listenerList);
        }

        listenerList.Add(listener);
    }

    public void PostNotification(EVENT_TYPE eventType, Component sender, object param = null)
    {
        List<IListener> listenerList = null;
        if (!Listeners.TryGetValue(eventType, out listenerList))
        {
            return;
        }

        for (int i = 0; i < listenerList.Count; i++)
        {
            if (!listenerList[i].Equals(null))
            {
                listenerList[i].OnEvent(eventType, sender, param);
            }
        }
    }

    public void RemoveEvent(EVENT_TYPE eventType)
    {
        Listeners.Remove(eventType);
    }

    void RemoveRedundancies()
    {
        Dictionary<EVENT_TYPE, List<IListener>> tempListeners = new Dictionary<EVENT_TYPE, List<IListener>>();

        foreach (KeyValuePair<EVENT_TYPE, List<IListener>> item in Listeners)
        {
            for (int i = item.Value.Count - 1; i >= 0; i--)
            {
                if (item.Value[i].Equals(null))
                {
                    item.Value.RemoveAt(i);
                }
            }

            if (item.Value.Count > 0)
            {
                tempListeners.Add(item.Key, item.Value);
            }
        }

        Listeners = tempListeners;
    }

    void OnDisable()
    {
        RemoveRedundancies();
    }

    void OnDestroy()
    {
        RemoveRedundancies();
    }

}
