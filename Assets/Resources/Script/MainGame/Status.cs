using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Status
{
    // 레벨
    //public int level;

    // 기본값
    public float basic;

    // 성장값
    //public float levelValue;

    // 추가값
    public List<Status> add;


    // 결과값
    public float value
    {
        get
        {
            float result = basic;
            for (int i = 0; i < add.Count; i++) result += add[i].value;
            return (basic);
        }
        //get
        //{
        //    float result = basic;
        //    result += level * levelValue;
        //    for (int i = 0; i < add.Count; i++) result += add[i].value;
        //    return (basic);
        //}
    }

}
