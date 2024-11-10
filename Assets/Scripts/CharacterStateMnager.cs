using Unity.Collections;
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
    // 상수
    const float GROUND_POS_Y = -1;


    // 상수 아님! 점점 난이도 어려워지면서 빨라질 거임.
    public float WALKING_SPEED = 2.0f;
    public float RUNNING_SPEED = 10.0f;
    public float WAIT_TIME = 1.0f;
    public float JUMP_HEIGHT = 4; // relative height from the GROUND_POS_Y
    public float JUMP_LENGTH = 4; 

    private Animator animator;
    public float CharacterSpeed = 2.0f;
    private float waitTimer = 0f; // Running 상태로 전환 후 대기 시간

    private float jumpedAt;

    private States state = States.Walking;

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
                case States.Jumping:
                    jumpedAt = transform.position.x; 
                    break;
                case States.Crash:
                    CharacterSpeed = 0f; // 1초 동안 멈춤
                    waitTimer = 0f;  // 대기 타이머 초기화
                    break;
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
        transform.localPosition = new Vector3(-6, GROUND_POS_Y, -10); // 초기 위치 설정
        setState(States.Walking); // 기본 상태는 Walking

        animator = transform.GetComponent<Animator>();
    }
    // public bool find = false;//debug
    // public bool near = false;//debug
    void Update()
    {
        // debug animation
        // if(state==States.Walking && find) setState(States.Found);
        // if(state==States.Running&&!find) setState(States.Walking);
        // if(state==States.Running&&near) setState(States.Jumping);


        switch (state){
            case States.Found:
                waitTimer += Time.deltaTime; // 대기 시간 누적
                if (waitTimer>=WAIT_TIME){
                    setState(States.Running);
                }
                break;
            case States.Jumping:
                float newPosY = getJumpPosYFromPosX();
                if(newPosY<GROUND_POS_Y){
                    newPosY = GROUND_POS_Y;
                    setState(States.Crash);
                }
                transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);
                break;
            case States.Crash:
                waitTimer += Time.deltaTime; // 대기 시간 누적
                if (waitTimer>=WAIT_TIME){
                    setState(States.Walking);
                }
                break;
        }

        transform.position += Vector3.right * CharacterSpeed * Time.deltaTime;
    }

    private float getJumpPosYFromPosX(){
        float deltaPosX = transform.position.x - jumpedAt;
        float jumpPeakPosY = GROUND_POS_Y + JUMP_HEIGHT;
        return -(deltaPosX-JUMP_LENGTH)*(deltaPosX-JUMP_LENGTH)*(jumpPeakPosY/(JUMP_LENGTH*JUMP_LENGTH)) + jumpPeakPosY;
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
                animator.SetBool("landGround", false);
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
