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
    public int valueTotal = 0;

    // 굴리는중 여부
    public bool isRolling = false;
    public bool isRolled = false;

    // 주사위 개수
    public int count = 1;



    // 랜덤값 호출
    public int Rolling()
    {
        valueTotal += _value;
        _value = Random.Range(min, max);

        return value;
    }
}
