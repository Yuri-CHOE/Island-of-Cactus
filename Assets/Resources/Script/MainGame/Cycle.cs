using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cycle
{
    // ���� ����Ŭ
    public int now = 0;

    // ��ǥ ����Ŭ
    public int goal = 999999;

    /// <summary>
    /// �ڿ��� ����
    /// </summary>
    public void CheckNegativet()
    {
        if (now < 0)
            now = 0;

        if (goal < 0)
            goal = 999999;
    }

    /// <summary>
    /// ���� Ȯ��
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
