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
        // �ּ� ������ �ִ� ���� ����
        //return (-80.00f + 80.00f * value0to1);
        return (-80.00f + 80.00f * (value0to1 * (2 - value0to1)));
    }



    void Awake()
    {
        Debug.Log("ȯ�漳�� :: �����");

        // ȯ�漳�� �� �ݿ�
        bgm.value = Preferences.bgm.value;
        sfx.value = Preferences.sfx.value;

        // �ݱ�
        gameObject.SetActive(false);
    }





    public void Apply()
    {
        Preferences.bgm.value = bgm.value;
        Preferences.sfx.value = sfx.value;
        Preferences.Save();
        Debug.Log("ȯ�漳�� :: ���� �Ϸ�");
    }
    public void Cnacel()
    {
        bgm.value = Preferences.bgm.value;
        sfx.value = Preferences.sfx.value;

        Debug.Log("ȯ�漳�� :: ���� ���");
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

    public void SetAudioBgm()
    {
        audioMixer.SetFloat("BGM", ToMixerValue(bgm.value));
    }

    public void SetAudioSfx()
    {
        audioMixer.SetFloat("SFX", ToMixerValue(sfx.value));
    }

}
