using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Status
{
    // ����
    //public int level;

    // �⺻��
    public float basic;

    // ���尪
    //public float levelValue;

    // �߰���
    public List<Status> add;


    // �����
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
