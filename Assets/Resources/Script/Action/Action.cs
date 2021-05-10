using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionProgress
{
    Ready,
    Start,
    Working,
    Finish,
}

public struct Action
{
    public enum ActionType
    {
        None,
        Idle,
        Move,
        Attack,
        Hit,
        Stop,
    }


    // 액션 종류
    public ActionType type;

    // 진행 상태
    public ActionProgress progress;

    // 액션 횟수
    public int count;

    // 액션 속도
    public float speed;

    // 진행 시간
    public float elapsedTime;
    
    // 생성자
    public Action(ActionType _type, float _speed)
    {
        type = _type;
        progress = ActionProgress.Ready;
        count = 0;
        speed = _speed;
        elapsedTime = 0.00f;
    }

    // 생성자
    public Action(ActionType _type, int _count, float _speed)
    {
        type = _type;
        progress = ActionProgress.Ready;
        count = _count;
        speed = _speed;
        elapsedTime = 0.00f;
    }
}
