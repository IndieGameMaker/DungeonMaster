using UnityEngine;

[CreateAssetMenu(fileName = "AudioDataSO", menuName = "DungeonMaster/AudioDataSO")]
public class AudioDataSO : ScriptableObject
{
    [Header("BGM Clips")] 
    public AudioClip mainBGM;
    public AudioClip battleBGM;
}
