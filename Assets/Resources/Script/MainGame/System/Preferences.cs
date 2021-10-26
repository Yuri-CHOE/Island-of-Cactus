using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Preferences
{
    public class PreferencesSound
    {
        // ���Ұ� ���
        public bool isMute = false;
        public int isMuteInt{
            get { if (isMute) return 1; else return 0; }
            set { isMute = (value == 1); }
        }

        // ������ ����
        public float value = 0.50f;

        // ���� ����
        public float volume { get { if (isMute) return 0.00f; else return value; } }

        public void Save(string keyMute, string keyValue)
        {
            PlayerPrefs.SetInt( keyMute, isMuteInt);
            PlayerPrefs.SetFloat( keyValue, value);
        }

        public void Load(string keyMute, string keyValue)
        {
            if (PlayerPrefs.HasKey(keyMute))
            {
                isMuteInt = PlayerPrefs.GetInt(keyMute, bgm.isMuteInt);
                //Debug.Log("ȯ�漳�� :: ("+ keyMute + ") �� �߰� -> " + isMuteInt);
                Debug.Log("ȯ�漳�� :: ("+ keyMute + ") �� �߰� -> " + isMuteInt);
            }
            else
                Debug.LogWarning("ȯ�漳�� :: (" + keyMute + ") �� ���� -> �⺻��=" + isMuteInt);

            if (PlayerPrefs.HasKey(keyValue))
            {
                value = PlayerPrefs.GetFloat(keyValue, bgm.value);
                Debug.Log("ȯ�漳�� :: (" + keyValue + ") �� �߰� -> " + value);
            }
            else
                Debug.LogWarning("ȯ�漳�� :: (" + keyValue + ") �� ���� -> �⺻��=" + value);
        }
    }

    public enum Language
    {
        English,
        Korean,
    }

    // ��� ����
    public static PreferencesSound bgm = new PreferencesSound();
    public static string keyBgmMute = "bgmM";
    public static string keyBgmValue = "bgmV";

    // ȿ����
    public static PreferencesSound sfx = new PreferencesSound();
    public static string keySfxMute = "sfxM";
    public static string keySfxValue = "sfxV";

    // ���
    static Language language = Language.Korean;
    public static string keyLanguage = "language";


    public static void Save()
    {
        bgm.Save(keyBgmMute, keyBgmValue);
        sfx.Save(keySfxMute, keySfxValue);

        PlayerPrefs.SetInt(     keyLanguage , (int)language );
    }

    public static void Load()
    {
        bgm.Load(keyBgmMute, keyBgmValue);
        sfx.Load(keySfxMute, keySfxValue);

        if (PlayerPrefs.HasKey(keyLanguage))
            language = (Language)PlayerPrefs.GetInt(keyLanguage, (int)language);
    }
}
