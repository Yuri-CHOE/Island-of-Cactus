using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Preferences
{
    public class PreferencesSound
    {
        // 음소거 기능
        public bool isMute = false;
        public int isMuteInt{
            get { if (isMute) return 1; else return 0; }
            set { isMute = (value == 1); }
        }

        // 설정된 볼륨
        public float value = 0.50f;

        // 최종 볼륨
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
                //Debug.Log("환경설정 :: ("+ keyMute + ") 값 발견 -> " + isMuteInt);
                Debug.Log("환경설정 :: ("+ keyMute + ") 값 발견 -> " + isMuteInt);
            }
            else
                Debug.LogWarning("환경설정 :: (" + keyMute + ") 값 없음 -> 기본값=" + isMuteInt);

            if (PlayerPrefs.HasKey(keyValue))
            {
                value = PlayerPrefs.GetFloat(keyValue, bgm.value);
                Debug.Log("환경설정 :: (" + keyValue + ") 값 발견 -> " + value);
            }
            else
                Debug.LogWarning("환경설정 :: (" + keyValue + ") 값 없음 -> 기본값=" + value);
        }
    }

    public enum Language
    {
        English,
        Korean,
    }

    // 배경 음악
    public static PreferencesSound bgm = new PreferencesSound();
    public static string keyBgmMute = "bgmM";
    public static string keyBgmValue = "bgmV";

    // 효과음
    public static PreferencesSound sfx = new PreferencesSound();
    public static string keySfxMute = "sfxM";
    public static string keySfxValue = "sfxV";

    // 언어
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
