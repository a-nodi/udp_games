using UnityEngine;

public class CharacterStateMnager : MonoBehaviour
{
    public enum States
    {
        Walking,
        Running,
        Found,
        Jumping
    }

    public Animator animator;
    public float CharacterSpeed = 2.0f;
    private float timer = 0f; // 경과 시간 추적용 타이머
    private float waitTimer = 0f; // Running 상태로 전환 후 대기 시간
    private bool isWaiting = false; // 대기 중인지 여부
    private bool hasStartedRunning = false; // Running 시작 여부 추적

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
                    animator.SetBool("find", false);
                    CharacterSpeed = 2.0f;
                    break;
                case States.Running:
                    animator.SetBool("find", true);
                    isWaiting = true; // 대기 시작
                    waitTimer = 0f;  // 대기 타이머 초기화
                    hasStartedRunning = true; // Running 시작됨
                    break;
                // 다른 상태들
            }
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
    }

    void Update()
    {
        

        // Running 상태로 전환된 후 1초 동안 멈추기
        if (state == States.Running) // Running 상태일 때만
        {
            if (isWaiting)
            {
                waitTimer += Time.deltaTime; // 대기 시간 누적

                if (waitTimer >= 1f) // 1초가 경과하면
                {
                    CharacterSpeed = 10f; // 속도 10f로 변경
                    isWaiting = false; // 대기 종료
                    waitTimer = 0f; // 대기 타이머 초기화
                }
                else
                {
                    CharacterSpeed = 0f; // 1초 동안 멈춤
                }
            }
        }

        transform.position += Vector3.right * CharacterSpeed * Time.deltaTime;
    }
}
