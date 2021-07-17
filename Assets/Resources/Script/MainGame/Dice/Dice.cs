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
    // �D�� ��
    int _valueTotal = 0;
    public int valueTotal { get { return _valueTotal; } } 

    // ���� �� ���
    public int valueRecord = 0;

    // �������� ����
    public bool isRolling = false;
    public bool isRolled = false;

    // �ֻ��� ����
    public int count = 1;



    // ������ ȣ��
    public int Rolling()
    {
        _value = Random.Range(min, max+1);
        _valueTotal += _value;

        // �ֻ��� ���� ����
        count--;
        Debug.Log("�ֻ��� ���� :: -1 =>" + count);

        return value;
    }

    /// <summary>
    /// �ֻ��� �� �� ������ ����
    /// </summary>
    public void Clear()
    {
        // ��� ó��
        valueRecord += valueTotal;

        // �ʱ�ȭ
        _value = 0;
        _valueTotal = 0;
        isRolling = false;
        isRolled = false;
    }

    public void SetValue(int __value)
    {
        _value = __value;
    }

    public void SetValueTotal(int __valueTotal)
    {
        _valueTotal = __valueTotal;
    }
}
