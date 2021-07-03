using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserData 
{
    // 유저 데이터
    public static CSVReader file = null;

    // 셋업 확인
    static bool checkSetup = false;

    // 유저 이름
    public static string userName = null;

    // 전체 플레이 시간 (초)
    public static int playTime = 0;

    // 전체 플레이 횟수
    public static int playCount = 0;

    // 등수 달성 횟수
    public static int count_1st = 0;
    public static int count_2nd = 0;
    public static int count_3th = 0;
    public static int count_4th = 0;

    /// <summary>
    /// 유저 데이터 파일 데이터화
    /// </summary>
    public static void SetUp()
    {
        if (file == null)
        {
            Debug.LogError("Error :: 유저 데이터 파일 지정 안됨");
            Debug.Break();
            return;
        }

        for (int i = 0; i < file.table.Count; i++)
        {
            // userName
            if (file.table[i][0] == "userName")
            {
                userName = file.table[i][1];
                continue;
            }

            // playTime
            if (file.table[i][0] == "playTime")
            {
                playTime = int.Parse(file.table[i][1]);
                continue;
            }

            // playCount
            if (file.table[i][0] == "playCount")
            {
                playCount = int.Parse(file.table[i][1]);
                continue;
            }

            // count_1st
            if (file.table[i][0] == "count_1st")
            {
                count_1st = int.Parse(file.table[i][1]);
                continue;
            }

            // count_2nd
            if (file.table[i][0] == "count_2nd")
            {
                count_2nd = int.Parse(file.table[i][1]);
                continue;
            }

            // count_3th
            if (file.table[i][0] == "count_3th")
            {
                count_3th = int.Parse(file.table[i][1]);
                continue;
            }

            // count_4th
            if (file.table[i][0] == "count_4th")
            {
                count_4th = int.Parse(file.table[i][1]);
                continue;
            }
        }

        checkSetup = true;
    }

    /// <summary>
    /// 유저 데이터 저장
    /// </summary>
    public static void SaveData()
    {
        if (file == null)
        {
            Debug.LogError("Error :: 유저 데이터 파일 지정 안됨");
            Debug.Break();
            return;
        }
        if (!checkSetup)
        {
            Debug.LogError("Error :: 유저 데이터 데이터( SetUp() )화 안됨");
            Debug.Break();
            return;
        }

        // 입력
        for (int i = 0; i < file.table.Count; i++)
        {
            // userName
            if (file.table[i][0] == "userName")
            {
                file.table[i][1] = userName;
                continue;
            }

            // playTime
            if (file.table[i][0] == "playTime")
            {
                file.table[i][1] = playTime.ToString();
                continue;
            }

            // playCount
            if (file.table[i][0] == "playCount")
            {
                file.table[i][1] = playCount.ToString();
                continue;
            }

            // count_1st
            if (file.table[i][0] == "count_1st")
            {
                file.table[i][1] = count_1st.ToString();
                continue;
            }

            // count_2nd
            if (file.table[i][0] == "count_2nd")
            {
                file.table[i][1] = count_2nd.ToString();
                continue;
            }

            // count_3th
            if (file.table[i][0] == "count_3th")
            {
                file.table[i][1] = count_3th.ToString();
                continue;
            }

            // count_4th
            if (file.table[i][0] == "count_4th")
            {
                file.table[i][1] = count_4th.ToString();
                continue;
            }
        }

        // 저장
        file.Save('=');
    }
}
