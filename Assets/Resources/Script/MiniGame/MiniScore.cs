using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MiniScore
{
    // ������ �̴ϰ��� �ε���
    public static int index = 0;

    //�̴ϰ��� ���� ����
    public bool join;

    // �غ� ����
    public bool isReady;

    //���� �ڽ��� �������� Ȯ��
    //public bool myTurn = false;

    //�̴ϰ��� ������ Ȯ��
    public bool isDead;

    //�̴ϰ��� ����
    int _score;
    public int score { get { return _score; } }


    //�̴ϰ��� ���
    public int rank;

    // ���� ����
    int _recordScore;
    public int recordScore { get { return _recordScore; } }

    // �� ���
    public static int totalReward = 0;
    public int reward { get { return totalReward * rewardRatio / totalRewardRatio; } }

    // ���� ����
    public static int totalRewardRatio = 0;
    public int rewardRatio;



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

    public static void GiveRewardAll()
    {
        // ��� �÷��̾� ���
        for (int i = 0; i < Player.allPlayer.Count; i++)
        {
            // �̴ϰ��� ����

            // ������ ����
            if (!Player.allPlayer[i].miniInfo.join)
                continue;

            // ���� ����
            Player.allPlayer[i].coin.Add(Player.allPlayer[i].miniInfo.reward);

            // ���
            Player.allPlayer[i].miniInfo.Record();

            //// �ʱ�ȭ
            //Player.allPlayer[i].miniInfo.Reset();
        }

        //// �̴ϰ��� ���� �� ���� �ʱ�ȭ
        //MiniScore.totalReward = 0;
        //MiniScore.totalRewardRatio = 0;
        //MiniGameManager.minigameNow = null;
        //MiniGameManager.progress = ActionProgress.Ready;
    }

    public static void RewardReset()
    {
        // ��� �÷��̾� ���
        for (int i = 0; i < Player.allPlayer.Count; i++)
        {
            //// �̴ϰ��� ����

            //// ������ ����
            //if (!Player.allPlayer[i].miniInfo.join)
            //    continue;

            //// ���� ����
            //Player.allPlayer[i].coin.Add(Player.allPlayer[i].miniInfo.reward);

            //// ���
            //Player.allPlayer[i].miniInfo.Record();

            // �ʱ�ȭ
            Player.allPlayer[i].miniInfo.Reset();
        }

        // �̴ϰ��� ���� �� ���� �ʱ�ȭ
        MiniScore.totalReward = 0;
        MiniScore.totalRewardRatio = 0;
        MiniGameManager.minigameNow = null;
        MiniGameManager.progress = ActionProgress.Ready;

        Debug.Log("�̴ϰ��� :: ���� ���� �ʱ�ȭ �����");
    }
}
