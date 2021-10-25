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
        Debug.Log("ȯ�漳�� :: �����");

        bgm.value = Preferences.bgm.value;
        sfx.value = Preferences.sfx.value;
    }

    public void Apply()
    {
        Preferences.bgm.value = bgm.value;
        Preferences.sfx.value = sfx.value;
        Preferences.Save();
        Debug.Log("ȯ�漳�� :: ���� �Ϸ�");
    }

    public void MuteBgm(Toggle obj) { MuteBgm(obj.isOn); }
    public void MuteBgm(bool isMute)
    {
        Preferences.bgm.isMute = isMute;
        Preferences.Save();
        Debug.Log("ȯ�漳�� :: BGM ��� ���� -> " + !isMute);
    }

    public void MuteSfx(Toggle obj) { MuteSfx(obj.isOn); }
    public void MuteSfx(bool isMute)
    {
        Preferences.sfx.isMute = isMute;
        Preferences.Save();
        Debug.Log("ȯ�漳�� :: SFX ��� ���� -> " + !isMute);
    }
}
