using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferencesManager : MonoBehaviour
{
    public Slider bgm = null;
    public Slider sfx = null;

    void Awake()
    {
        Debug.Log("환경설정 :: 적용됨");

        bgm.value = Preferences.bgm.value;
        sfx.value = Preferences.sfx.value;
    }

    public void Apply()
    {
        Preferences.bgm.value = bgm.value;
        Preferences.sfx.value = sfx.value;
        Preferences.Save();
        Debug.Log("환경설정 :: 변경 완료");
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
}
