using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 객체가 로딩(생성) 될 때 한번 호출
    // 스크립트가 비활성화 되어도 실행된다.
    void Awake()
    {
        // 전역 게임 데이터 초기화
        Debug.Log("Awake 호출");
    }

    void OnEnable()
    {
        // 스크립트가 활성화 될때마다 매번 호출
        Debug.Log("OnEnable 호출");
    }
    
    // 스크립트가 실행할 때 한번 호출
    void Start()
    {
        // 자신의 클래스 내의 변수 초기화
        Debug.Log("Start 호출");
    }

    // 매 프레임 마다 호출되는 콜백(CallBack Function/Method, Event Function)
    void Update()
    {
        // 화면을 랜더링하는 주기 (60 fps => 1/60 간격으로 호출? X)
        Debug.Log("Update 호출");
    }

    // 0.02f 호출, 정확한 간격으로 호출
    // 호출 주기는 물리엔진의 계산 주기
    void FixedUpdate()
    {
        Debug.Log($"호출 간격 : {Time.fixedDeltaTime}");
    }

    void LateUpdate()
    {
        // Update에서 선행 작업 결과 데이터를 바탕으로 후속작업을 할 때 사용
        // 3
    }
}
