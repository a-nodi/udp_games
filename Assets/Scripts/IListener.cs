using UnityEngine;

public interface IListener
{
    void OnEvent(EVENT_TYPE eventType, Component sender, object param = null);
}
