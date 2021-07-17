using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{

    // 최소값
    public static int _min = 1;
    public static int min { get { return _min + minAdded; } }

    // 최대값
    public static int _max = 6;
    public static int max { get { return _max + maxAdded; } }

    // 최대 최소 반영값
    public static int minAdded = 0;
    public static int maxAdded = 0;



    // 값
    int _value = 0;
    public int value { get { return _value; } }
    // 핪산 값
    int _valueTotal = 0;
    public int valueTotal { get { return _valueTotal; } } 

    // 누적 값 기록
    public int valueRecord = 0;

    // 굴리는중 여부
    public bool isRolling = false;
    public bool isRolled = false;

    // 주사위 개수
    public int count = 1;



    // 랜덤값 호출
    public int Rolling()
    {
        _value = Random.Range(min, max+1);
        _valueTotal += _value;

        // 주사위 개수 차감
        count--;
        Debug.Log("주사위 개수 :: -1 =>" + count);

        return value;
    }

    /// <summary>
    /// 주사위 값 및 누적값 제거
    /// </summary>
    public void Clear()
    {
        // 기록 처리
        valueRecord += valueTotal;

        // 초기화
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
