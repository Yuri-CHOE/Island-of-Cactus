using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResource
{
    // �ִ�ġ
    public int max = 0;

    // �ּ�ġ
    public int min = 0;

    // �ڿ�
    int _Value = 0;
    public int Value
    {
        get { return _Value; }
        set { Set(value); }
    }

    void Set(int newValue)
    {
        _Value = newValue;

        if (checkMax())
            _Value = max;

        if (checkMin())
            _Value = min;
    }

    /// <summary>
    /// �ִ밪 �ʰ��� true
    /// </summary>
    public bool checkMax()
    {
        return checkMax(max);
    }
    /// <summary>
    /// ���ڰ� �ʰ��� true
    /// </summary>
    /// <param name="maxValue">�� ����</param>
    public bool checkMax(int maxValue)
    {
        if (_Value > maxValue)
            return true;
        else
            return false;
    }

    /// <summary>
    /// �ּҰ� �̴޽� true
    /// </summary>
    public bool checkMin()
    {
        return checkMin(min);
    }
    /// <summary>
    /// ���ڰ� �̴޽� true
    /// </summary>
    /// <param name="minValue">�� ����</param>
    public bool checkMin(int minValue)
    {
        if (_Value < minValue)
            return true;
        else
            return false;
    }

    //������
    /// <summary>
    /// ��� ����
    /// </summary>
    protected GameResource()
    {
        // ��� ����
    }
    public GameResource(int __value, int _max, int _min)
    {
        _Value = __value;
        max = _max;
        min = _min;
    }
}
