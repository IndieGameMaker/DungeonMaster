using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 싱글턴(Singleton) 디자인 패턴
    public static AudioManager Instance { get; private set; }

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
