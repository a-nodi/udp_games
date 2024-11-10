using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterStateMnager : MonoBehaviour
{
    public enum States
    {
        Walking,
        Running,
        Found,
        Jumping,
        Crash
    }

    private Animator animator;
    public float CharacterSpeed = 2.0f;
    private float timer = 0f; // 경과 시간 추적용 타이머
    private float waitTimer = 0f; // Running 상태로 전환 후 대기 시간

    private States state = States.Walking;

    public const float WALKING_SPEED = 2.0f;
    public const float RUNNING_SPEED = 10.0f;
    public const float WAIT_TIME = 1.0f;

    public States getState()
    {
        return state;
    }

    public bool setState(States newState)
    {
        // 현재 상태와 다른 경우에만 변경
        if (state != newState)
        {
            switch (newState)
            {
                case States.Walking:
                    CharacterSpeed = WALKING_SPEED;
                    break;
                case States.Found:
                    CharacterSpeed = 0f; // 1초 동안 멈춤
                    waitTimer = 0f;  // 대기 타이머 초기화
                    break;
                case States.Running:
                    CharacterSpeed = RUNNING_SPEED; 
                    break;
                

                // 다른 상태들
            }
            setAnimationState(newState);
            state = newState;
            return true; // 상태가 성공적으로 변경됨
        }
        return false; // 상태가 변경되지 않음
    }

    void Start()
    {
        transform.localScale = new Vector3(1, 1, 1); // 캐릭터 크기 설정
        transform.localPosition = new Vector3(-6, -1, -10); // 초기 위치 설정
        setState(States.Walking); // 기본 상태는 Walking

        animator = transform.GetComponent<Animator>();
    }

    void Update()
    {
        if (state == States.Found)
        {
            waitTimer += Time.deltaTime; // 대기 시간 누적
            if (waitTimer>=WAIT_TIME){
                setState(States.Running);
            }
        }

        transform.position += Vector3.right * CharacterSpeed * Time.deltaTime;
    }

    private void setAnimationState(States newState){
        switch (newState)
        {
            case States.Walking:
                animator.SetBool("find",false);
                break;
            case States.Found:
                animator.SetBool("find",true);
                break;
            case States.Running:
                // do nothing
                break;
            case States.Jumping:
                animator.SetBool("find", true);
                animator.SetBool("near", true);
                break;
            case States.Crash:
                animator.SetBool("find", false);
                animator.SetBool("near", false);
                animator.SetBool("landGround", true);
                break;
        }
    }
}
