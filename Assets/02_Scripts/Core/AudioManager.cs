using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 싱글턴(Singleton) 디자인 패턴
    public static AudioManager Instance { get; private set; }

    // 오디오 데이터 SO
    public AudioDataSO AudioDataSO;
    
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

    private void Start()
    {
        // 컴포넌트 생성
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _sfxPlayerSource = gameObject.AddComponent<AudioSource>();
        _sfxEnemySource = gameObject.AddComponent<AudioSource>();
        
        _bgmSource.loop = true;
        _sfxPlayerSource.loop = false;
        _sfxEnemySource.loop = false;
        
        // 게임 시작시 BGM 재생
        PlayBGM(AudioDataSO.battleBGM);
    }
    #endregion

    #region 공통 메서드
    public void PlayBGM(AudioClip clip)
    {
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public void PlayerSFX(AudioClip clip)
    {
        _sfxPlayerSource.PlayOneShot(clip);
    }

    public void EnemySFX(AudioClip clip)
    {
        _sfxEnemySource.PlayOneShot(clip);
    }
    #endregion
}
