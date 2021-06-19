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
        Turn,
        Attack,
        Stop,
        Barricade,
    }


    // �׼� ����
    public ActionType type;

    // ���� ����
    public ActionProgress progress;

    // �׼� Ƚ��
    public int count;

    // �׼� �ӵ�
    public float speed;

    // ���� �ð�
    public float elapsedTime;

    // �����
    public bool isFinish;
    //public bool isFinish { get { if (progress == ActionProgress.Finish) { return true; } else { return false; } } }


    // ������
    public Action(ActionType _type, float _speed)
    {
        type = _type;
        progress = ActionProgress.Ready;
        count = 0;
        speed = _speed;
        elapsedTime = 0.00f;
        isFinish = false;
    }

    // ������
    public Action(ActionType _type, int _count, float _speed)
    {
        type = _type;
        progress = ActionProgress.Ready;
        count = _count;
        speed = _speed;
        elapsedTime = 0.00f;
        isFinish = false;
    }
}
