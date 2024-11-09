using UnityEngine;

public class background1 : MonoBehaviour
{
    public float speed = 2.0f; // 이동 속도
    public float screenRightEdge = 17.6f; // 화면 오른쪽 경계
    public float screenLeftEdge = -17.6f; // 화면 왼쪽 경계

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 시작 위치와 크기 설정
        transform.position = new Vector3(17.6f, 0, transform.position.z);
        transform.localScale = new Vector3(0.75f, 0.6f, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        // 왼쪽으로 이동
        transform.position += Vector3.left * speed * Time.deltaTime;

        // 화면 왼쪽 경계를 벗어났을 때 오른쪽으로 이동
        if (transform.position.x < screenLeftEdge)
        {
            transform.position = new Vector3(screenRightEdge, transform.position.y, transform.position.z);
        }
    }
}
