using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy
{
    // 코인 보유량
    public int rich = 0;

    // 누적 이동거리
    public int runner = 0;

    // 미니게임 성적
    public int mini = 0;

    // 총점
    public int score { get { return /*5 * 3 -*/ (rich + runner + mini); } }

    // 최종 성적
    public int final = 0;
}
