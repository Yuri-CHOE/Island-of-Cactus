using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PreferencesManager : MonoBehaviour
{
    public Slider bgm = null;
    public Slider sfx = null;

    public AudioMixer audioMixer = null;


    public static float ToMixerValue(float value0to1)
    {
        // 최소 음량과 최대 음량 보간
        //return (-80.00f + 80.00f * value0to1);
        return (-80.00f + 80.00f * (value0to1 * (2 - value0to1)));
    }



    void Awake()
    {
        Debug.Log("환경설정 :: 적용됨");

        // 환경설정 값 반영
        bgm.value = Preferences.bgm.value;
        sfx.value = Preferences.sfx.value;

        // 닫기
        gameObject.SetActive(false);
    }





    public void Apply()
    {
        Preferences.bgm.value = bgm.value;
        Preferences.sfx.value = sfx.value;
        Preferences.Save();
        Debug.Log("환경설정 :: 변경 완료");
    }
    public void Cnacel()
    {
        bgm.value = Preferences.bgm.value;
        sfx.value = Preferences.sfx.value;

        Debug.Log("환경설정 :: 변경 취소");
    }


    public void MuteBgm(Toggle obj) { MuteBgm(obj.isOn); }
    public void MuteBgm(bool isMute)
    {
        Preferences.bgm.isMute = isMute;
        Preferences.Save();
        Debug.Log("환경설정 :: BGM 출력 여부 -> " + !isMute);
    }

    public void MuteSfx(Toggle obj) { MuteSfx(obj.isOn); }
    public void MuteSfx(bool isMute)
    {
        Preferences.sfx.isMute = isMute;
        Preferences.Save();
        Debug.Log("환경설정 :: SFX 출력 여부 -> " + !isMute);
    }

    public void SetAudioBgm()
    {
        audioMixer.SetFloat("BGM", ToMixerValue(bgm.value));
    }

    public void SetAudioSfx()
    {
        audioMixer.SetFloat("SFX", ToMixerValue(sfx.value));
    }

}
