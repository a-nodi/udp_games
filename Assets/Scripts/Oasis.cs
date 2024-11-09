using UnityEngine;

public class Oasis : MonoBehaviour, IListener
{

    public string id;
    private void Awake()
    {
        id = Guid.NewGuid().ToString();
    }

    void OnMouseDown()
    {
        // TODO: Should be processed in GameManager
        EventManager.instance.PostNotification(EVENT_TYPE.CLICK_OASIS, this, this.transform.position.x);
    }

    void OnEvent(EVENT_TYPE eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case EVENT_TYPE.MOVE_OASIS:
                if (sender == this)
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
