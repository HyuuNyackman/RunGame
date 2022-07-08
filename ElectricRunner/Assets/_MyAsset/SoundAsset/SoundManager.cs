using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�z�z���T�C�g
//https://studioshimazu.com/post-1037/
//����т���
//SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title);

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource seAudioSource;

    [SerializeField] List<BGMSoundData> bgmSoundDatas;
    [SerializeField] List<SESoundData> seSoundDatas;

    public float MasterVolume = 1;
    public float BgmMasterVolume = 1;
    public float SeMasterVolume = 1;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(BGMSoundData.BGM bgm)
    {
        BGMSoundData data = bgmSoundDatas.Find(data => data.Bgm == bgm);
        bgmAudioSource.clip = data.AudioClip;
        bgmAudioSource.volume = data.Volume * BgmMasterVolume * MasterVolume;
        bgmAudioSource.Play();
    }


    public void PlaySE(SESoundData.SE se)
    {
        SESoundData data = seSoundDatas.Find(data => data.Se == se);
        seAudioSource.volume = data.Volume * SeMasterVolume * MasterVolume;
        seAudioSource.PlayOneShot(data.AudioClip);
    }

}

[System.Serializable]
public class BGMSoundData
{
    public enum BGM
    {
        Title,
        Dungeon,
        Hoge, // ���ꂪ���x���ɂȂ�
    }

    public BGM Bgm;
    public AudioClip AudioClip;
    [Range(0, 1)]
    public float Volume = 1;
}

[System.Serializable]
public class SESoundData
{
    public enum SE
    {
        None,
        sliding,
        KnifeHit,
        Jump,
        throwKnife// ���ꂪ���x���ɂȂ�
    }

    public SE Se;
    public AudioClip AudioClip;
    [Range(0, 1)]
    public float Volume = 1;
}
