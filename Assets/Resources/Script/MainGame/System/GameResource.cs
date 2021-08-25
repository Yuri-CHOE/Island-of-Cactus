using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResource
{
    // 최대치
    public int max = 0;

    // 최소치
    public int min = 0;

    // 자원
    int _Value = 0;
    public int Value
    {
        get { return _Value; }
        //set { Set(value); }
    }

    // 자원 변경 요청값
    int waitingValue = 0;

    //생성자
    /// <summary>
    /// 사용 금지
    /// </summary>
    protected GameResource()
    {
        // 사용 금지
    }
    public GameResource(int __value, int _max, int _min)
    {
        _Value = __value;
        max = _max;
        min = _min;
    }



    void ChangeValue(int newValue)
    {
        _Value = newValue;

        if (checkMax())
        {
            _Value = max;
            waitingValue = 0;
        }

        if (checkMin())
        {
            _Value = min;
            waitingValue = 0;
        }
    }

    /// <summary>
    /// 최대값 초과시 true
    /// </summary>
    public bool checkMax()
    {
        return checkMax(max);
    }
    /// <summary>
    /// 인자값 초과시 true
    /// </summary>
    /// <param name="maxValue">비교 기준</param>
    public bool checkMax(int maxValue)
    {
        if (_Value > maxValue)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 최소값 미달시 true
    /// </summary>
    public bool checkMin()
    {
        return checkMin(min);
    }
    /// <summary>
    /// 인자값 미달시 true
    /// </summary>
    /// <param name="minValue">비교 기준</param>
    public bool checkMin(int minValue)
    {
        if (_Value < minValue)
            return true;
        else
            return false;
    }

    public void Add(int __value)
    {
        waitingValue += __value;
    }
    public void subtract(int __value)
    {
        waitingValue -= __value;
    }
    public void Set(int __value)
    {
        ChangeValue(__value);
    }

    public void RefreshOne()
    {
        if (waitingValue != 0)
        {
            // 갱신 단위수량
            int i = (int)Mathf.Sign(waitingValue);

            // 단위수량 만큼 제거
            waitingValue -= i;

            // 값에 반영
            ChangeValue(_Value + i);
        }
    }
}
