using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public CharacterStateManager characterStateMnager; // CharacterStateMnager 참조
    public float cameraSpeed = 2.0f; // 카메라의 이동 속도

    void Start()
    {
        // 배경의 초기 위치 저장
        transform.position = new Vector3(0, 0, -1000); // 초기 카메라 위치
        characterStateMnager = FindObjectOfType<CharacterStateManager>();

    }

    void Update()
    {
            cameraSpeed = characterStateMnager.CharacterSpeed; // 캐릭터의 속도에 맞춰 카메라 속도 설정
            transform.position += Vector3.right * cameraSpeed * Time.deltaTime; // 카메라 이동
    }
}
