using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniScore
{
    // 진행중 미니게임 인덱스
    public static int index = 0;

    //미니게임 참가 여부
    public bool join = false;

    // 준비 상태
    public bool isReady = false;

    //현재 자신의 차례인지 확인
    //public bool myTurn = false;

    //미니게임 라이프 확인
    public bool isDead = false;

    //미니게임 점수
    int _score = 0;
    public int score { get { return _score; } }


    //미니게임 등수
    public int rank = 0;

    // 누적 점수
    int _recordScore = 0;
    public int recordScore { get { return _recordScore; } }

    // 총 상금
    public static int totalReward = 0;
    public int reward { get { return totalReward * rewardRatio / totalRewardRatio; } }

    // 보상 지분
    public static int totalRewardRatio = 0;
    public int rewardRatio = 0;



    /// <summary>
    /// 직접 사용시 UI 반영 우회됨
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
            // 기록
            _recordScore += Player.allPlayer.Count - rank;
        }
        else if (rank != 0)
        {
            Debug.LogError("error :: 잘못된 랭크 값 -> " + rank);
            Debug.Break();
        }
    }
}
