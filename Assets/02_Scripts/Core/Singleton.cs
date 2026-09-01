using UnityEngine;
// Singleton<AudioManager>
// Singleton<CameraShake>
// 제너릭 싱글턴
// 이 클래스를 직접 사용하지 말고 반드시 상속해서 사용하세요.
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    
    // Getter : 외부 클래스에서 참조할 때 접근하는 코드
    // Warrior.cs
    // AudioManager.Instance.메서드();
    public static T Instance
    {
        get
        {
            // 이미 존재하는 경우에는 바로 리턴
            if (_instance != null) return _instance;
            
            // 씬에 존재하는지 여부를 검색
            _instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);

            // 씬에 존재하지 않으면 새로 생성
            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // 싱글턴 인스턴스가 이미 존재하는 경우
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        // 싱글턴 인스턴스에 현재 객체를 설정
        _instance = (T)this;
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        // 싱글턴 인스턴스가 파괴될 때 null 초기화
        if (_instance == this) _instance = null;
    }
    
}
