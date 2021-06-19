using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cycle
{
    // 현재 사이클
    public int now = 0;

    // 목표 사이클
    public int goal = 999999;

    /// <summary>
    /// 자연수 보정
    /// </summary>
    public void CheckNegativet()
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
    public bool isEnd()
    {
        CheckNegativet();

        if (now < goal)
            return false;
        else
            return true;
    }

    public void NextCycle()
    {
        now++;

    }
}
