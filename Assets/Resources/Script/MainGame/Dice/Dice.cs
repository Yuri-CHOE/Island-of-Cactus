using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    // 값
    static int _value = 0;
    public int value { get { return _value; } }

    // 최소값
    public static int _min = 1;
    public int min { get { return _min + minAdded; } }

    // 최대값
    public static int _max = 6;
    public int max { get { return _max + maxAdded; } }

    // 최대 최소 반영값
    public static int minAdded = 0;
    public static int maxAdded = 0;



    // 랜덤값 호출
    public int Rolling()
    {
        _value = Random.Range(min, max + 1);

        return value;
    }
}
