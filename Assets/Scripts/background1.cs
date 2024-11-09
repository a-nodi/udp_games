using UnityEngine;

public class background1 : MonoBehaviour
{
    public float backgroundWidth = 35.2f; // 배경의 가로 길이

    private Camera mainCamera; // 메인 카메라 참조

    void Start()
    {
        // 메인 카메라 참조
        mainCamera = Camera.main;

        // 배경 크기 설정 (기본 크기를 0.75, 0.6으로 설정)
        transform.localScale = new Vector3(0.75f, 0.6f, transform.localScale.z);

        // 초기 배경 위치 설정
        transform.position = new Vector3(backgroundWidth/2, 0, 0);
    }

    void Update()
    {
        // 배경이 화면 왼쪽 경계를 벗어났을 때 자기 자신을 삭제
        if (transform.position.x + backgroundWidth / 2 <= mainCamera.transform.position.x)
        {
            // 새로운 위치 계산 후 할당
            transform.position = new Vector3(transform.position.x + backgroundWidth, transform.position.y, transform.position.z);
        }
    }
}
