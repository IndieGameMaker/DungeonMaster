using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 싱글턴(Singleton) 디자인 패턴
    public static AudioManager Instance { get; private set; }

    // 오디오 데이터 SO
    [SerializeField] private AudioDataSO _audioDataSO;
    
    // 오디오 소스 컴포넌트 변수
    private AudioSource _bgmSource;
    private AudioSource _sfxPlayerSource;
    private AudioSource _sfxEnemySource;
    
    #region 유니티 생명주기

    private void Awake()
    {
        if (Instance == null)
        {
            // 처음 생성된 경우
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 중복해서 생성된 인스턴스 삭제
            Destroy(gameObject);
        }
    }

    #endregion
}
