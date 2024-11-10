using UnityEngine;

public class OasisManager : MonoBehaviour
{
    public GameObject character; // 캐릭터 오브젝트 참조
    private CharacterStateManager characterStateManager; // 캐릭터 상태 관리자
    private GameObject oasis1, oasis2, oasis3;

    public float FoundDistance = 30f; // 감지 거리
    public float minDistance = 100f; // 오아시스 간 최소 거리

    void Start()
    {
        // 태그로 오아시스 오브젝트 찾기
        oasis1 = GameObject.FindWithTag("Oasis1");
        oasis2 = GameObject.FindWithTag("Oasis2");
        oasis3 = GameObject.FindWithTag("Oasis3");

        // 캐릭터 상태 관리자 참조
        characterStateManager = character.GetComponent<CharacterStateManager>();

        // 오아시스 초기 위치 설정
        transform.position = new Vector3(Random.Range(100, 200), -1, -5);
    }

    void Update()
    {
        // 각 오아시스와 캐릭터 간 거리 체크
        CheckOasisDistance(oasis1);
        CheckOasisDistance(oasis2);
        CheckOasisDistance(oasis3);

        // 오아시스 간 최소 거리 유지
        MaintainMinimumDistance();
    }

    // 오아시스와 캐릭터 간의 거리를 확인하고, 조건을 만족하면 이동
    void CheckOasisDistance(GameObject oasis)
    {
        if (oasis == null || character == null)
            return;

        // 2D 거리 계산: y 값 제외하고 x, z 축으로만 거리 계산
        float distance = Vector2.Distance(
            new Vector2(oasis.transform.position.x, oasis.transform.position.z), 
            new Vector2(character.transform.position.x, character.transform.position.z)
        );

        // 거리가 1 이하이고 캐릭터보다 뒤에 있을 때 오아시스 이동 및 Walking 상태 전환
        if (distance <= characterStateManager.JUMP_LENGTH && oasis.transform.position.x > character.transform.position.x)
        {
            characterStateManager.setState(CharacterStateManager.States.Jumping);
        }
        //if (distance == 0 && oasis.)
        else if (distance < FoundDistance && characterStateManager.getState() != CharacterStateManager.States.Running)
        {
            // 감지 거리 내에 있을 때 Running 상태로 전환
            characterStateManager.setState(CharacterStateManager.States.Found);
        }

        
    }

    // 오아시스를 캐릭터 뒤쪽으로 이동시키기
    void RandomizeOasis(GameObject oasis)
    {
        if (oasis != null)
        {
            float randomX = Random.Range(100, 300); // 100~300 사이의 랜덤 X 좌표

            // 캐릭터 위치를 기준으로 뒤쪽에 배치하고 z를 -5로 고정
            oasis.transform.position = new Vector3(character.transform.position.x + randomX, -1, -5);
        }
    }

    // 오아시스 간 최소 거리 조건 유지
    void MaintainMinimumDistance()
    {
        if (oasis1 == null || oasis2 == null || oasis3 == null)
            return;

        float distance12 = Vector3.Distance(oasis1.transform.position, oasis2.transform.position);
        float distance13 = Vector3.Distance(oasis1.transform.position, oasis3.transform.position);
        float distance23 = Vector3.Distance(oasis2.transform.position, oasis3.transform.position);

        // 오아시스 간 거리가 최소 거리보다 작으면 위치를 조정하고 y값과 z값을 고정
        if (distance12 < minDistance)
        {
            AdjustOasisPosition(oasis1, oasis2);
        }

        if (distance13 < minDistance)
        {
            AdjustOasisPosition(oasis1, oasis3);
        }

        if (distance23 < minDistance)
        {
            AdjustOasisPosition(oasis2, oasis3);
        }
    }

    // 두 오아시스 위치가 너무 가까우면 조정
    void AdjustOasisPosition(GameObject oasisA, GameObject oasisB)
    {
        if (oasisA == null || oasisB == null)
            return;

        Vector3 direction = oasisB.transform.position - oasisA.transform.position;
        float distance = direction.magnitude;

        if (distance < minDistance)
        {
            float randomOffsetX = Random.Range(minDistance, minDistance + 50);

            // 방향을 더하여 오아시스 위치를 이동시키고 y값 -1, z값 -5로 고정
            oasisB.transform.position = oasisA.transform.position + direction.normalized * randomOffsetX;
            oasisB.transform.position = new Vector3(oasisB.transform.position.x, -1, -5);
        }
    }
}
