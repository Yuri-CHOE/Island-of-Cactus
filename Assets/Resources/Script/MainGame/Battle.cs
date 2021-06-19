using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle
{
    // 공격력
    public Status atk = new Status();

    // 방어력
    public Status def = new Status();

    /// <summary>
    /// 사용 금지
    /// </summary>
    protected Battle()
    {
        // Battle(float atkBasic, float defBasic) 사용 권장
    }
    public Battle(float atkBasic, float defBasic)
    {
        atk.basic = atkBasic;
        atk.add = new List<Status>();

        def.basic = defBasic;
        def.add = new List<Status>();
    }



    // 단순 데미지 계산
    public float Damage(float rawDamage)
    {
        float result = rawDamage - def.value;

        if (result < 0f)
            result = 0f;

        return result;
    }
}
