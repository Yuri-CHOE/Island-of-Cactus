using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IocEffect
{
    // ��ȿ�Ⱓ
    public enum Expiration
    {
        Never,
        Forever,
        Cycle,
        Turn,
        Moment,
    }

    // Ÿ�� �÷��̾�
    public enum Target
    {
        Self,
        AllPlayer,
        OthersPlayer,
        SelectedPlayer,
        World,
    }

    // ��� �ʵ�
    public enum What
    {
        None,
        Character,
        Move,
        Block,
        Dice,
        Life,
        Coin,
        Item,
        Minigame,
    }




    // ȿ�� ��ȿ�Ⱓ
    Expiration _expiration = Expiration.Never;
    public Expiration expiration { get { return _expiration; } }

    // ȿ�� ī��Ʈ(����)
    int _count = -1;
    public int count { get { return _count; } }

    // ȿ�� Ÿ�� �÷��̾� (����ڿ��� ����)
    Target _target = Target.Self;
    public Target target { get { return _target; } }

    // ȿ�� ������
    int _where = -1;
    public int where { get { return _where; } }

    // ȿ�� ���
    What _what = What.None;
    public What what { get { return _what; } }

    // ȿ�� ��
    int _value = -1;
    public int value { get { return _value; } }




    /// <summary>
    /// �缳�� �Լ�, ���� ������� ����
    /// </summary>
    /// <param name="__expiration">���� �Ⱓ</param>
    /// <param name="__count">�ߵ� Ƚ��</param>
    /// <param name="__target">�ߵ��ڿ� ���� ������ ȿ�� ���� ���</param>
    /// <param name="__where">�ߵ� �������κ��� �Ÿ�</param>
    /// <param name="__what">ȿ�� ����</param>
    /// <param name="__value">ȿ�� ��</param>
    public void Set(Expiration __expiration, int __count, Target __target, int __where, What __what, int __value)
    {
        SetExpiration(__expiration);
        SetCount(__count);
        SetTarget(__target);
        SetWhere(__where);
        SetWhat(__what);
        SetValue(__value);
    }

    void SetExpiration(Expiration __expiration)
    {
        _expiration = __expiration;
    }

    void SetCount(int __count)
    {
        if (__count < 0)
            return;

        _count = __count;
    }

    void SetTarget(Target __target)
    {
        _target = __target;
    }

    void SetWhere(int __where)
    {
        _where = __where;
    }

    void SetWhat(What __what)
    {
        _what = __what;
    }

    void SetValue(int __value)
    {
        _value = __value;
    }
















    /// <summary>
    /// ȿ���� ������� �÷��̾� ����Ʈ
    /// SelectedPlayer �� �� ����Ʈ ��ȯ
    /// World �� null ��ȯ
    /// </summary>
    /// <param name="__iocEvent">�̺�Ʈ</param>
    /// <param name="targetPlayer_Or_null">�ߵ��� �Ǵ� null</param>
    /// <returns></returns>
    public static List<Player> TargetFiltering(Target target, Player targetPlayer_Or_null)
    {
        // �����
        List<Player> pl = new List<Player>();

        // �ڱ� �ڽ�
        if (target == Target.Self)
            pl.Add(targetPlayer_Or_null);

        // ��� �÷��̾�
        else if (target == Target.AllPlayer)
            pl.AddRange(Player.allPlayer);

        // �ٸ� �÷��̾�
        else if (target == Target.OthersPlayer)
            pl.AddRange(targetPlayer_Or_null.otherPlayers);

        // �ٸ� �÷��̾�
        else if (target == Target.SelectedPlayer)
            ;

        // �� ����
        else if (target == Target.World)
            return null;

        return pl;
    }


    /// <summary>
    /// ���� ȿ�� �ߵ�
    /// </summary>
    /// <param name="filteredTarget">ȿ���� ���� �÷��̾��</param>
    /// <param name="__blockIndex">��ġ</param>
    public static IEnumerator GeneralEffect(Expiration __expiration, List<Player> filteredTarget, int __where, What __what, int __value)
    {
        // ��� ����
        if (__what == What.None)
        {

        }

        // ĳ���� (�÷��̾� �ƹ�Ÿ)
        else if (__what == What.Character)
        {
            // �̱��� ===================================
        }

        // �̵�
        else if (__what == What.Move)
        {
            for (int i = 0; i < filteredTarget.Count; i++)
            {
                Player current = filteredTarget[i];

                /*
             ���� �̵� �ߴ�    
             ����ġ(current.movement.location)�� �ߴ���(current.location) ����
             �� �����Ʈ���� �޼ҵ�� ó��

            ���⼭ ȣ��
             */
            }
        }

        // ��� Ÿ�� ����
        else if (__what == What.Block)
        {

        }

        // �ֻ��� �� ����
        else if (__what == What.Dice)
        {

        }

        // ������ ȹ��
        else if (__what == What.Life)
        {

        }

        // ���� ȹ��
        else if (__what == What.Coin)
        {

        }

        // ������ ȹ��
        else if (__what == What.Item)
        {

        }

        // �̴ϰ��� ����
        else if (__what == What.Minigame)
        {

        }

        yield return null;
    }
}
