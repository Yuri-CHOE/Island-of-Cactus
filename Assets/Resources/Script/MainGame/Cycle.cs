using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cycle
{
    // ���� ����Ŭ
    public static int now = 0;

    // ��ǥ ����Ŭ
    public static int goal { get { return GameRule.cycleMax; } }




    public static void Clear()
    {
        now = 0;
    }


    /// <summary>
    /// �ڿ��� ����
    /// </summary>
    public static void CheckNegativet()
    {
        if (now < 0)
            now = 0;
    }

    /// <summary>
    /// ���� Ȯ��
    /// </summary>
    /// <returns></returns>
    public static bool isEnd()
    {
        CheckNegativet();

        if (now <= goal)
            return false;
        else
            return true;
    }

    public static void NextCycle()
    {
        now++;

    }

}
