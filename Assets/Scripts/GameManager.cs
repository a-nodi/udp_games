using UnityEngine;

public class GameManager : MonoBehaviour, IListener
{
    private Dictionary<string, gameObject> dictOfOasis = new Dictionary<string, gameObject>();

    private string closestOasisId = null;

    // TODO: Def priority queue (min heap) for closestOasisId
    public MinHeap<idDistancePair> priorityQueue = new MinHeap<idDistancePair>();
    private int distanceCoefficient = 10; // TODO: Hyperparameter

    public float speed = 0.0f;

    public int MIN_DISTANCE = 5;

    public GameObject OasisPrefab;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }; private set;
    }
    private static GameManager _instance = null;

    private void Awake()
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

    public void AddOasis(Vector2 position)
    {
        GameObject newOasis = Instantiate(OasisPrefab, position, Quaternion.identity);
        dictOfOasis.Add(newOasis.GetComponent<Oasis>().id, newOasis);

        priorityQueue.Add(new idDistancePair(newOasis.GetComponent<Oasis>().id, newOasis.GetComponent<Oasis>().transform.position.x));
        closestOasisId = priorityQueue.Peek().Id;
    }

    public void RemoveOasis(string id)
    {
        if (dictOfOasis.ContainsKey(id))
        {
            Destroy(dictOfOasis[id]);
            dictOfOasis.Remove(id);
        }
    }

    public void OnEvent(EVENT_TYPE eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case EVENT_TYPE.CLICK_OASIS:
                if (sender is Oasis)
                {
                    EventManager.instance.PostNotification(
                        eventType.MOVE_OASIS,
                        dictOfOasis[((Oasis)sender).id].GetComponent<Oasis>(),
                        GenerateRandomDistance());
                }
                break;
        }
    }
    public void MoveOasis(string id, float distance)
    {
        idDistancePair pair = priorityQueue.Peek();

        if (dictOfOasis.ContainsKey(id))
        {
            if (pair.Id != id)
            {
                Debug.LogError("Oasis with id " + id + " is not the closest oasis");
                return;
            }

            EventManager.instance.PostNotification(EVENT_TYPE.MOVE_OASIS, dictOfOasis[id].GetComponent<Oasis>(), distance);
        }
        else
        {
            Debug.LogError("Oasis with id " + id + " not found");
        }

        priorityQueue.Pop();
        priorityQueue.Add(new idDistancePair(id, dictOfOasis[id].GetComponent<Oasis>().transform.position.x + distance));

    }

    public float GenerateRandomDistance(float _speed)
    {
        int randomDistance = Random.Range(0.0f, 1.0f) * _speed * distanceCoefficient;
        return randomDistance < MIN_DISTANCE ? MIN_DISTANCE : randomDistance;
    }
}
