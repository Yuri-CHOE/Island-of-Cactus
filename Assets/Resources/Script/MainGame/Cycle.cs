using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cycle
{
    // 현재 사이클
    public static int now = 0;

    // 목표 사이클
    public static int goal = 999999;




    public static void Clear()
    {
        now = 0;
        goal = 999999;
    }


    /// <summary>
    /// 자연수 보정
    /// </summary>
    public static void CheckNegativet()
    {
        if (now < 0)
            now = 0;

        if (goal < 0)
            goal = 999999;
    }

    /// <summary>
    /// 종료 확인
    /// </summary>
    /// <returns></returns>
    public static bool isEnd()
    {
        CheckNegativet();

        if (now < goal)
            return false;
        else
            return true;
    }

    public static void NextCycle()
    {
        now++;

    }

}
