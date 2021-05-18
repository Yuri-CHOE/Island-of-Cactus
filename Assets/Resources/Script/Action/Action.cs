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

    // 종료됨
    public bool isFinish;
    //public bool isFinish { get { if (progress == ActionProgress.Finish) { return true; } else { return false; } } }


    // 생성자
    public Action(ActionType _type, float _speed)
    {
        type = _type;
        progress = ActionProgress.Ready;
        count = 0;
        speed = _speed;
        elapsedTime = 0.00f;
        isFinish = false;
    }

    // 생성자
    public Action(ActionType _type, int _count, float _speed)
    {
        type = _type;
        progress = ActionProgress.Ready;
        count = _count;
        speed = _speed;
        elapsedTime = 0.00f;
        isFinish = false;
    }



    /// <summary>
    /// 업데이트 및 그 하위 함수를 통해 액션 수행
    /// </summary>
    public void ActMove()
    {
        if (progress == ActionProgress.Ready)
        {
            // 각종 초기화

            // 스킵
            progress = ActionProgress.Start;
        }
        else if (progress == ActionProgress.Start)
        {
            // 시작 연출

            // 스킵
            progress = ActionProgress.Working;
        }
        else if (progress == ActionProgress.Working)
        {
            // 이동

            // 스킵
            //progress = ActionProgress.Finish;
        }
        else if (progress == ActionProgress.Finish)
        {
            // 종료 처리
            isFinish = true;
        }
    }
}
