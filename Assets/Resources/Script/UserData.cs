using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserData 
{
    public enum FieldType
    {
        name,
        playTime,
        playCount,
        rank1,
        rank2,
        rank3,
        rank4,
    }

    // 셋업 확인
    public static bool checkSetup = false;

    // 유저 이름
    static string _userName = null;
    public static string defaultName = "DefaultUser";
    public static string userName { get { return _userName; } set { _userName = value; Save(FieldType.name); } }
    public const string userNameKey = "userName";

    // 전체 플레이 시간 (초)
    public static int _playTime = 0;
    public static int playTime { get { return _playTime; } set { _playTime = value; Save(FieldType.playTime); } }
    const string playTimeKey = "playTime";

    // 전체 플레이 횟수
    static int _playCount = 0;
    public static int playCount { get { return _playCount; } set { _playCount = value; Save(FieldType.playCount); } }
    const string playCountKey = "playCount";

    // 등수 달성 횟수
    static int _rank1 = 0;
    public static int rank1 { get { return _rank1; } set { _rank1 = value; Save(FieldType.rank1); } }
    const string rank1Key = "rank1";

    static int _rank2 = 0;
    public static int rank2 { get { return _rank2; } set { _rank2 = value; Save(FieldType.rank2); } }
    const string rank2Key = "rank2";

    static int _rank3 = 0;
    public static int rank3 { get { return _rank3; } set { _rank3 = value; Save(FieldType.rank3); } }
    const string rank3Key = "rank3";

    static int _rank4 = 0;
    public static int rank4 { get { return _rank4; } set { _rank4 = value; Save(FieldType.rank4); } }
    const string rank4Key = "rank4";

    /// <summary>
    /// 유저 데이터 파일 데이터화
    /// </summary>
    public static void SetUp()
    {
        // userName
        Load(FieldType.name);

        // playTime
        Load(FieldType.playTime);

        // playCount
        Load(FieldType.playCount);

        // rank1
        Load(FieldType.rank1);

        // rank2
        Load(FieldType.rank2);

        // rank3
        Load(FieldType.rank3);

        // rank4
        Load(FieldType.rank4);

        checkSetup = true;
    }

    /// <summary>
    /// 유저 데이터 저장
    /// </summary>
    public static void SaveData()
    {
        // userName
        Save(FieldType.name);

        // playTime
        Save(FieldType.playTime);

        // playCount
        Save(FieldType.playCount);

        // rank1
        Save(FieldType.rank1);

        // rank2
        Save(FieldType.rank2);

        // rank3
        Save(FieldType.rank3);

        // rank4
        Save(FieldType.rank4);
    }

    public static void Save(FieldType fieldType)
    {
        switch (fieldType)
        {
            case FieldType.name:
                PlayerPrefs.SetString(userNameKey, userName);
                return;

            case FieldType.playTime:
                PlayerPrefs.SetInt(playTimeKey , playTime);
                return;

            case FieldType.playCount:
                PlayerPrefs.SetInt(playCountKey, playCount);
                return;

            case FieldType.rank1:
                PlayerPrefs.SetInt(rank1Key, rank1);
                return;
            case FieldType.rank2:
                PlayerPrefs.SetInt(rank2Key, rank2);
                return;
            case FieldType.rank3:
                PlayerPrefs.SetInt(rank3Key, rank3);
                return;
            case FieldType.rank4:
                PlayerPrefs.SetInt(rank4Key, rank4);
                return;
        }
    }

    public static void Load(FieldType fieldType)
    {
        try
        {
            switch (fieldType)
            {
                case FieldType.name:
                    userName = PlayerPrefs.GetString(userNameKey, defaultName);
                    return;

                case FieldType.playTime:
                    playTime = PlayerPrefs.GetInt(playTimeKey, 0);
                    return;

                case FieldType.playCount:
                    playCount = PlayerPrefs.GetInt(playCountKey, 0);
                    return;

                case FieldType.rank1:
                    rank1 = PlayerPrefs.GetInt(rank1Key, 0);
                    return;
                case FieldType.rank2:
                    rank2 = PlayerPrefs.GetInt(rank2Key, 0);
                    return;
                case FieldType.rank3:
                    rank3 = PlayerPrefs.GetInt(rank3Key, 0);
                    return;
                case FieldType.rank4:
                    rank4 = PlayerPrefs.GetInt(rank4Key, 0);
                    return;
            }
        }
        catch { Debug.LogError("유저 데이터 :: 로드 실패 -> " + fieldType.ToString()); }

    }
}
