using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    // ��
    static int _value = 0;
    public int value { get { return _value; } }

    // �ּҰ�
    public static int _min = 1;
    public int min { get { return _min + minAdded; } }

    // �ִ밪
    public static int _max = 6;
    public int max { get { return _max + maxAdded; } }

    // �ִ� �ּ� �ݿ���
    public static int minAdded = 0;
    public static int maxAdded = 0;



    // ������ ȣ��
    public int Rolling()
    {
        _value = Random.Range(min, max + 1);

        return value;
    }
}
