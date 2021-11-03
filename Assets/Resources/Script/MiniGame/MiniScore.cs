using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MiniScore
{
    // 진행중 미니게임 인덱스
    public static int index = 0;

    //미니게임 참가 여부
    public bool join;

    // 준비 상태
    public bool isReady;

    //현재 자신의 차례인지 확인
    //public bool myTurn = false;

    //미니게임 라이프 확인
    public bool isDead;

    //미니게임 점수
    int _score;
    public int score { get { return _score; } }


    //미니게임 등수
    public int rank;

    // 누적 점수
    int _recordScore;
    public int recordScore { get { return _recordScore; } }

    // 총 상금
    public static int totalReward = 0;
    public int reward { get { return totalReward * rewardRatio / totalRewardRatio; } }

    // 보상 지분
    public static int totalRewardRatio = 0;
    public int rewardRatio;



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

    public static void GiveRewardAll()
    {
        // 모든 플레이어 대상
        for (int i = 0; i < Player.allPlayer.Count; i++)
        {
            // 미니게임 정산

            // 불참자 제외
            if (!Player.allPlayer[i].miniInfo.join)
                continue;

            // 코인 지급
            Player.allPlayer[i].coin.Add(Player.allPlayer[i].miniInfo.reward);

            // 기록
            Player.allPlayer[i].miniInfo.Record();

            //// 초기화
            //Player.allPlayer[i].miniInfo.Reset();
        }

        //// 미니게임 보상 및 지분 초기화
        //MiniScore.totalReward = 0;
        //MiniScore.totalRewardRatio = 0;
        //MiniGameManager.minigameNow = null;
        //MiniGameManager.progress = ActionProgress.Ready;
    }

    public static void RewardReset()
    {
        // 모든 플레이어 대상
        for (int i = 0; i < Player.allPlayer.Count; i++)
        {
            //// 미니게임 정산

            //// 불참자 제외
            //if (!Player.allPlayer[i].miniInfo.join)
            //    continue;

            //// 코인 지급
            //Player.allPlayer[i].coin.Add(Player.allPlayer[i].miniInfo.reward);

            //// 기록
            //Player.allPlayer[i].miniInfo.Record();

            // 초기화
            Player.allPlayer[i].miniInfo.Reset();
        }

        // 미니게임 보상 및 지분 초기화
        MiniScore.totalReward = 0;
        MiniScore.totalRewardRatio = 0;
        MiniGameManager.minigameNow = null;
        MiniGameManager.progress = ActionProgress.Ready;

        Debug.Log("미니게임 :: 이전 게임 초기화 수행됨");
    }
}
