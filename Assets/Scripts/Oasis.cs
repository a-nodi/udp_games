using UnityEngine;
using System;

public class Oasis : MonoBehaviour, IListener
{

    public string id;
    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }

    void OnMouseDown()
    {
        //EventManager.instance.PostNotification(EVENT_TYPE.CLICK_OASIS, this, transform.position.x);
        Debug.Log("aaa");
    }

    public void OnEvent(EVENT_TYPE eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case EVENT_TYPE.MOVE_OASIS:
                if (sender != this)
                {
                    // param is the new distance
                    Vector3 pos = transform.position;
                    pos.x = pos.x + (float)param;
                    transform.position = pos;
                }
                break;
        }
    }
}
