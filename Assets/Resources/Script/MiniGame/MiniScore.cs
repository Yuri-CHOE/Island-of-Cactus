using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniScore
{
    // ������ �̴ϰ��� �ε���
    public static int index = 0;

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

    // ���� ����
    int _recordScore = 0;
    public int recordScore { get { return _recordScore; } }

    // �� ���
    public static int totalReward = 0;
    public int reward { get { return totalReward * rewardRatio / totalRewardRatio; } }

    // ���� ����
    public static int totalRewardRatio = 0;
    public int rewardRatio = 0;



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

        rewardRatio = 0;
    }

    public void Record()
    {
        if (rank > 0)
        {
            // ���
            _recordScore += Player.allPlayer.Count - rank;
        }
        else if (rank != 0)
        {
            Debug.LogError("error :: �߸��� ��ũ �� -> " + rank);
            Debug.Break();
        }
    }
}
