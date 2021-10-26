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

    [Header("soundFile")]
    public List<AudioClip> bgmClips = new List<AudioClip>();
    public List<AudioClip> sfxClips = new List<AudioClip>();

    // 빠른 접근 - BGM
    public AudioClip bgmTitle { get { return bgmClips[0]; } }
    public AudioClip bgmMaingame { get { return bgmClips[1]; } }
    public AudioClip bgmMini { get { return bgmClips[2]; } }
    public AudioClip bgmMiniReport { get { return bgmClips[3]; } }

    // 빠른 접근 - SFX
    public AudioClip sfxButton { get { return sfxClips[0]; } }
    public AudioClip sfxSelect { get { return sfxClips[1]; } }
    public AudioClip sfxCreat { get { return sfxClips[2]; } }
    public AudioClip sfxCoin { get { return sfxClips[3]; } }
    public AudioClip sfxLife { get { return sfxClips[4]; } }



    void Awake()
    {
        // 퀵등록
        script = this;
    }
}
