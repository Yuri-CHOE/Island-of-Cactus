using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniScore
{
    // �� ���
    public static int reward = 0;

    //�̴ϰ��� ���� ����
    public bool join = false;

    // �غ� ����
    public bool isReady = false;

    //���� �ڽ��� �������� Ȯ��
    //public bool myTurn = false;

    //�̴ϰ��� ������ Ȯ��
    public bool isDead = false;

    //�̴ϰ��� ����
    int _score = 0;
    public int score { get { return _score; } }


    //�̴ϰ��� ���
    public int rank = 0;



    /// <summary>
    /// ���� ���� UI �ݿ� ��ȸ��
    /// </summary>
    /// <param name="value"></param>
    public void ScorePlus(int value)
    {
        _score += value;
    }


    public void Reset()
    {
        join = false;

        isReady = false;

        //myTurn = false;

        isDead = false;

        _score = 0;

        rank = 0;
    }
}
