using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle
{
    // ���ݷ�
    public Status atk = new Status();

    // ����
    public Status def = new Status();

    /// <summary>
    /// ��� ����
    /// </summary>
    protected Battle()
    {
        // Battle(float atkBasic, float defBasic) ��� ����
    }
    public Battle(float atkBasic, float defBasic)
    {
        atk.basic = atkBasic;
        atk.add = new List<Status>();

        def.basic = defBasic;
        def.add = new List<Status>();
    }



    // �ܼ� ������ ���
    public float Damage(float rawDamage)
    {
        float result = rawDamage - def.value;

        if (result < 0f)
            result = 0f;

        return result;
    }
}
