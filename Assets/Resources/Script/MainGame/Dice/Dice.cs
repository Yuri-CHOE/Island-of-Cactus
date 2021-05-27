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
    // 누적 값
    int _valueTotal = 0;
    public int valueTotal { get { return _valueTotal + value; } }       // 주사위를 굴릴때 이전값을 누적처리 하므로 주사위 1개만 굴리면 누적값=0, 값=1~ 상태 => _valueTotal + value 해야 진짜 누적값 나옴

    // 굴리는중 여부
    public bool isRolling = false;
    public bool isRolled = false;

    // 주사위 개수
    public int count = 1;



    // 랜덤값 호출
    public int Rolling()
    {
        _valueTotal += _value;
        _value = Random.Range(min, max+1);

        return value;
    }

    /// <summary>
    /// 주사위 값 및 누적값 제거
    /// </summary>
    public void Clear()
    {
        _value = 0;
        _valueTotal = 0;
    }
}
