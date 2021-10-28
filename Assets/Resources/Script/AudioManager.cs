using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager script = null;

    [Header("audioPlayer")]
    public AudioSource bgmPlayer = null;
    public AudioSource sfxPlayer = null;

    // BGM
    //[System.Serializable]
    public enum BGMtype
    {
        None,
        First,
        Title,
        Maingame,
        Mini,
        MiniReport,
    }
    [Header("BGM File")]
    public AudioClip bgmFirst = null;
    public AudioClip bgmTitle = null;
    public AudioClip bgmMaingame = null;
    public AudioClip bgmMini = null;
    public AudioClip bgmMiniReport = null;

    // SFX
    //[System.Serializable]
    public enum SFXtype
    {
        None,
        Error,
        Button,
        Select,
        Creat,
        Apply,
        Coin,
        //Hit,
    }
    [Header("SFX File")]
    public AudioClip sfxError = null;
    public AudioClip sfxButton = null;
    public AudioClip sfxSelect = null;
    public AudioClip sfxCreat = null;
    public AudioClip sfxApply = null;
    public AudioClip sfxCoin = null;
    //public AudioClip sfxHit = null;

    // SFX
    //[System.Serializable]
    public enum OSFXtype
    {
        None,
        Dice,
        Footprint,
        Luckybox,
        Monster,
    }
    [Header("ObjectSFX File")]
    public AudioClip osfxHit = null;
    public AudioClip osfxDice = null;
    public AudioClip osfxFootprint = null;
    public AudioClip osfxLuckybox = null;
    public AudioClip osfxMonster = null;



    void Awake()
    {
        // 퀵등록
        script = this;
    }

    static AudioClip GetClip(BGMtype bgmType)
    {
        switch (bgmType)
        {
            case BGMtype.First:
                return script.bgmFirst;

            case BGMtype.Title:
                return script.bgmTitle;

            case BGMtype.Maingame:
                return script.bgmMaingame;

            case BGMtype.Mini:
                return script.bgmMini;

            case BGMtype.MiniReport:
                return script.bgmMiniReport;
        }
        return null;
    }
    static AudioClip GetClip(SFXtype sfxType)
    {
        switch (sfxType)
        {
            case SFXtype.Error:
                return script.sfxError;

            case SFXtype.Button:
                return script.sfxButton;

            case SFXtype.Select:
                return script.sfxSelect;

            case SFXtype.Creat:
                return script.sfxCreat;

            case SFXtype.Apply:
                return script.sfxApply;

            case SFXtype.Coin:
                return script.sfxCoin;

            //case SFXtype.Hit:
            //    return script.sfxHit;
        }
        return null;
    }
    
    public void Play(BGMtype bgmType)
    {
        try
        {
            // 재생중이면 중단
            if (bgmPlayer.isPlaying)
            {
                bgmPlayer.Stop();
                Debug.Log("사운드 :: BGM 중단 요청됨 -> " + bgmPlayer.clip.name);
            }

            // 재생 사운드 변경
            bgmPlayer.clip = GetClip(bgmType);
            Debug.Log("사운드 :: BGM 지정됨 -> " + bgmPlayer.clip.name);

            // 재생
            bgmPlayer.Play();
            Debug.Log("사운드 :: BGM 재생 성공");
        }
        catch
        {
            Debug.LogWarning("사운드 :: BGM 재생 실패");
        }
    }

    public void PlayMultiple(SFXtype sfxType)
    {
        try
        {
            // 추가 재생
            sfxPlayer.PlayOneShot(GetClip(sfxType));
            Debug.Log("사운드 :: SFX 지정됨 -> " + GetClip(sfxType).name);
        }
        catch
        {
            Debug.LogWarning("사운드 :: SFX 재생 실패");
        }
    }
}
