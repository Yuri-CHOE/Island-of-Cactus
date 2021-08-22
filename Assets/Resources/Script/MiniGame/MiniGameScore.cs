using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniScore
{
    // 총 상금
    public static int reward = 0;

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
    }
}
