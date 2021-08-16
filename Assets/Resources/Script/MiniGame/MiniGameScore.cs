using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameScore
{
    //미니게임 참가 여부
    public bool join =  true;

    //현재 자신의 차례인지 확인
    public bool myTurn = false;

    //미니게임 라이프 확인
    public bool isDead = false;

    //미니게임 점수
    public int score = 0;

    //미니게임 등수
    public int rank = 0;
}
