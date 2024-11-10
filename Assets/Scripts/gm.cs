using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class gm : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public GameObject Oasis;

    public CharacterStateManager characterStateManager = null;

    public float speed = 0.0f;

    public int MIN_DISTANCE = 5;

    public float FOUND_DISTANCE = 15.0f;

    float foundTime = 0.0f;
    float crashTime = 0.0f;
    float targetOasisX = 0.0f;

    void Update()
    {
        UpdateCharacterState();
        if(Oasis.transform.position.x<characterStateManager.transform.position.x-10) MoveOasis();

        

		//마우스 클릭시
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(characterStateManager.getState()!=CharacterStateManager.States.Jumping){
                SceneManager.LoadScene(2);
            }else{
            Debug.Log("asdsadas");
        	MoveOasis();}

        }
    }

    public void UpdateCharacterState()
    {
        float closestOasisX = Oasis.transform.position.x;
        float characterX = characterStateManager.transform.position.x;

        switch (characterStateManager.getState())
        {
            case CharacterStateManager.States.Walking:
                if (closestOasisX - characterX < FOUND_DISTANCE && closestOasisX>characterX)
                {
                    characterStateManager.setState(CharacterStateManager.States.Found);
                    foundTime = Time.time;
                }
                break;
            case CharacterStateManager.States.Running:
                if (closestOasisX - characterX < characterStateManager.JUMP_LENGTH*2)
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
                    SceneManager.LoadScene(2);
                }
                break;
        }
    }

    public int score=0;
    public void MoveOasis()
    {
        float rd= GenerateRandomDistance(characterStateManager.WALKING_SPEED);
        Oasis.transform.position += Vector3.right*rd;
        score++;
        scoreText.text="Score: "+score.ToString();
    }

    public float GenerateRandomDistance(float _speed)
    {
        float distanceCoefficient=20;
        float randomDistance = Random.Range(0.7f, 1.0f) * _speed * distanceCoefficient;
        return randomDistance ;
    }


}
