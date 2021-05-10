using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{

    // �ּҰ�
    public static int _min = 1;
    public static int min { get { return _min + minAdded; } }

    // �ִ밪
    public static int _max = 6;
    public static int max { get { return _max + maxAdded; } }

    // �ִ� �ּ� �ݿ���
    public static int minAdded = 0;
    public static int maxAdded = 0;



    // ��
    int _value = 0;
    public int value { get { return _value; } }
    public int valueTotal = 0;

    // �������� ����
    public bool isRolling = false;
    public bool isRolled = false;

    // �ֻ��� ����
    public int count = 1;



    // ������ ȣ��
    public int Rolling()
    {
        valueTotal += _value;
        _value = Random.Range(min, max);

        return value;
    }
}
