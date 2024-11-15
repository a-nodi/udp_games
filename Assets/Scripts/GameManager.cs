using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IListener
{
    private Dictionary<string, GameObject> dictOfOasis = new Dictionary<string, GameObject>();

    private string closestOasisId = null;

    public string ClosestOasisId
    {
        get
        {
            return closestOasisId;
        }
    }

    private float score = 0.0f;

    // TODO: Def priority queue (min heap) for closestOasisId
    public MinHeap<IdDistancePair> priorityQueue = new MinHeap<IdDistancePair>();
    private int distanceCoefficient = 10; // TODO: Hyperparameter

    public CharacterStateManager characterStateManager = null;

    public float speed = 0.0f;

    public int MIN_DISTANCE = 5;

    public float FOUND_DISTANCE = 6.0f;

    float foundTime = 0.0f;
    float crashTime = 0.0f;
    float targetOasisX = 0.0f;
    public GameObject OasisPrefab;

    public bool isAddedOasis = false;

    private int currentSceneIndex;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
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

    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneIndex = scene.buildIndex;
    }

    void Update()
    {
        switch (currentSceneIndex)
        {
            case 1:
                UpdateCharacterState();
                break;
            default:
                break;
        }
    }

    public void UpdateCharacterState()
    {
        float closestOasisX = dictOfOasis[closestOasisId].GetComponent<Oasis>().transform.position.x;
        float characterX = characterStateManager.transform.position.x;

        if (!isAddedOasis)
        {
            isAddedOasis = true;
            for (int i = 0; i < 5; i++)
            {
                AddOasis(new Vector2(i * 10, 0));
            }
        }

        switch (characterStateManager.getState())
        {
            case CharacterStateManager.States.Walking:
                if (closestOasisX - characterX < FOUND_DISTANCE)
                {
                    characterStateManager.setState(CharacterStateManager.States.Found);
                    foundTime = Time.time;
                }
                break;
            case CharacterStateManager.States.Found:
                if (closestOasisX - characterX > FOUND_DISTANCE)
                {
                    characterStateManager.setState(CharacterStateManager.States.Walking);
                }
                if (Time.time - foundTime > 1.0f)
                {
                    characterStateManager.setState(CharacterStateManager.States.Running);
                }
                break;
            case CharacterStateManager.States.Running:
                if (closestOasisX - characterX < characterStateManager.JUMP_LENGTH)
                {
                    characterStateManager.setState(CharacterStateManager.States.Jumping);
                    targetOasisX = closestOasisX;
                }
                if (closestOasisX - characterX > FOUND_DISTANCE)
                {
                    characterStateManager.setState(CharacterStateManager.States.Walking);
                }
                break;
            case CharacterStateManager.States.Jumping:
                if (closestOasisX - characterX < 0.01)
                {
                    EventManager.instance.PostNotification(EVENT_TYPE.GAME_OVER, this);
                }
                if (!(-0.01 < targetOasisX - closestOasisX && targetOasisX - closestOasisX < 0.01))
                {
                    characterStateManager.setState(CharacterStateManager.States.Crash);
                    crashTime = Time.time;
                }
                break;

            case CharacterStateManager.States.Crash:
                if (Time.time - crashTime > 1.0f)
                {
                    characterStateManager.setState(CharacterStateManager.States.Walking);
                }
                break;
        }
    }

    public void AddOasis(Vector2 position)
    {
        GameObject newOasis = Instantiate(OasisPrefab, position, Quaternion.identity);
        dictOfOasis.Add(newOasis.GetComponent<Oasis>().id, newOasis);

        priorityQueue.Add(new IdDistancePair(newOasis.GetComponent<Oasis>().id, newOasis.GetComponent<Oasis>().transform.position.x));
        closestOasisId = priorityQueue.Peek().Id;

        EventManager.instance.AddListener(EVENT_TYPE.CLICK_OASIS, newOasis.GetComponent<Oasis>());
    }

    public void RemoveOasis(string id)
    {
        if (dictOfOasis.ContainsKey(id))
        {
            Destroy(dictOfOasis[id]);
            dictOfOasis.Remove(id);
            //EventManager.instance.RemoveListener(EVENT_TYPE.CLICK_OASIS, dictOfOasis[id].GetComponent<Oasis>());
        }
    }

    public void OnEvent(EVENT_TYPE eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case EVENT_TYPE.CLICK_OASIS:
                if (sender is Oasis)
                {
                    AcumulateScore(((Oasis)sender).id);

                    EventManager.instance.PostNotification(
                        EVENT_TYPE.MOVE_OASIS,
                        dictOfOasis[((Oasis)sender).id].GetComponent<Oasis>(),
                        GenerateRandomDistance(speed)
                    );

                    EventManager.instance.PostNotification(
                        EVENT_TYPE.UPDATE_SCORE,
                        this,
                        score
                    );
                }
                break;
        }
    }
    public void MoveOasis(string id, float distance)
    {
        IdDistancePair pair = priorityQueue.Peek();

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

        priorityQueue.RemoveMin();
        priorityQueue.Add(new IdDistancePair(id, dictOfOasis[id].GetComponent<Oasis>().transform.position.x + distance));
    }

    public float GenerateRandomDistance(float _speed)
    {
        float randomDistance = Random.Range(0.0f, 1.0f) * _speed * distanceCoefficient;
        return randomDistance < MIN_DISTANCE ? MIN_DISTANCE : randomDistance;
    }

    public float scoreFuntion(float distance)
    {
        if (distance < 0)
        {
            return 0.0f;
        }

        else if (distance < 0.1)
        {
            return 100.0f;
        }

        else if (distance < 10)
        {
            return 10.0f / distance;
        }

        else
        {
            return 0.0f;
        }
    }

    void AcumulateScore(string id)
    {
        score += scoreFuntion(dictOfOasis[id].GetComponent<Oasis>().transform.position.x);
    }
}
