using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Dice
{
    public enum SpecialDice
    {
        Normal,
        Odd,
        Even,
        Amazing,
        Gold,
    }

    // �ּҰ�
    public static int _min = 1;
    public static int min { get { return _min + minAdded; } }

    // �ִ밪
    public static int _max = 6;
    public static int max { get { return _max + maxAdded; } }

    // �ִ� �ּ� �ݿ���
    public static int minAdded = 0;
    public static int maxAdded = 0;


    // Ư�� �ֻ���
    public SpecialDice type;


    // ��
    int _value;
    public int value { get { return _value; } }
    // �D�� ��
    int _valueTotal;
    public int valueTotal { get { return _valueTotal; } } 

    // ���� �� ���
    public int valueRecord;

    // �������� ����
    public bool isRolling;
    public bool isRolled;

    // �ֻ��� ����
    public int count;

    // �ֻ��� ����
    public int multypleValue;



    // ������ ȣ��
    public int Rolling()
    {
        _value = Random.Range(min, max+1);
        Debug.Log("�ֻ��� :: �� ȣ�� ->" + _value);

        // Ư�� �ֻ��� �� ���� - Ȧ��
        if (type == SpecialDice.Odd)
            _value += -1 + _value % 2;

        // Ư�� �ֻ��� �� ���� - ¦��
        else if (type == SpecialDice.Even)
            _value -= _value % 2;
        
        // Ư�� �ֻ��� �� ���� - ����
        else if (type == SpecialDice.Amazing)
            _value = 1 + 5 * (_value % 2);

        // Ư�� �ֻ��� �� ���� - �߰�
        else if (type == SpecialDice.Gold)
            _value++;
        
        Debug.Log("�ֻ��� :: " + type + " ���� �� ->" + _value);

        _value *= multypleValue;
        Debug.Log("�ֻ��� :: " + multypleValue + "���� ���� �� ->" + _value);


        _valueTotal += _value;

        // �ֻ��� ���� ����
        count--;
        Debug.Log("�ֻ��� :: 1�� �Ҹ�� : �ܿ� ->" + count);

        return value;
    }

    /// <summary>
    /// �ֻ��� �� �� ������, Ư���ֻ��� ����
    /// </summary>
    public void Clear()
    {
        // ��� ó�� = �� �ֻ�����
        // �ǽð� ��ġ ������ �ڵ� �������� ��ü = �� ������
        //valueRecord += valueTotal;

        // �ʱ�ȭ
        type = SpecialDice.Normal;
        multypleValue = 1;
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
