using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct IocEffect
{
    // Ȱ��ȭ ����Ʈ ���
    public static List<IocEffect> activeEffects = new List<IocEffect>();


    // ��ȿ�Ⱓ
    public enum Expiration
    {
        Never,
        Forever,
        Cycle,
        Turn,
        Moment,
        Invalid,
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
    Expiration _expiration ;
    public Expiration expiration { get { return _expiration; } }
    public bool isInvalid { get { return expiration == Expiration.Invalid; } }

    // ȿ�� ī��Ʈ(����)
    int _count;
    public int count { get { return _count; } }

    // ȿ�� Ÿ�� �÷��̾� (����ڿ��� ����)
    Target _target;
    public Target target { get { return _target; } }

    // ȿ�� ������
    int _where;
    public int where { get { return _where; } }

    // ȿ�� ���
    What _what;
    public What what { get { return _what; } }

    // ȿ�� ��
    int _value;
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
    public IEnumerator GeneralEffect(Player user, List<Player> filteredTarget)
    {
        // �Ҹ� ó��
        if (_count == 1)        _count = -1;
        else if (_count > 0)    _count--;


        // ���� �̺�Ʈ ȣ��
        if (filteredTarget == null)
        {
            // �̱��� ============
            // yeild return WorldEvent();
        }
        else
        {
            // ������ ���� =========== �̱���
            //if (filteredTarget.Count == 0)
            //yield return ;


            Player current = null;
            int blockIndex;
            bool isExecute;

            for (int i = 0; i < filteredTarget.Count; i++)
            {
                // �����
                current = filteredTarget[i];

                // �̵� ����Ʈ Ȯ��
                blockIndex = current.location;

                // ȿ�� ���� ����
                isExecute = true;



                // ����� ����ġ ���
                if (current != user)
                {
                    // �κ��丮 ��ĵ
                    for (int j = 0; j < current.inventory.Count; j++)
                    {
                        // �ǵ� üũ
                        if (current.inventory[j].item.index == 19)
                        {
                            // �ǵ� �ڵ� ���
                            GameMaster.script.itemManager.ItemUse(current.inventory[j]);

                            // ��ĵ �ߴ�
                            break;
                        }
                    }
                }



                // ȿ�� ����
                if (isExecute)
                    continue;

                // ��� ����
                if (what == What.None)
                {

                }

                // ĳ���� (�÷��̾� �ƹ�Ÿ)
                else if (what == What.Character)
                {
                    // �̱��� ===================================
                }

                // �̵�
                else if (what == What.Move)
                {
                    // �ߴ�
                    current.movement.MoveStop();

                    // �̵� ����Ʈ ����
                    if (value == -3)
                    {
                        //blockIndex *= where;
                        current.dice.SetValue(current.dice.value * where);

                        // �̵� ȣ��
                        current.movement.PlanMoveBy(current.dice.valueTotal);
                    }
                    else if (value == -2)
                    {
                        //blockIndex += where;
                        current.dice.SetValue(current.dice.value + where);

                        // �̵� ȣ��
                        current.movement.PlanMoveBy(current.dice.valueTotal);
                    }
                    else
                    {
                        //blockIndex = value;
                        current.dice.SetValue(where);

                        // �̵� ȣ��
                        current.movement.PlanMoveTo(blockIndex);
                    }

                }

                // ��� Ÿ�� ����
                else if (what == What.Block)
                {
                    // �����
                    DynamicBlock dBlock = BlockManager.script.GetBlock(where).GetComponent<DynamicBlock>();

                    // ����
                    dBlock.blockTypeDetail = (BlockType.TypeDetail)value;
                    dBlock.blockType = BlockType.GetTypeByDetail(dBlock.blockTypeDetail);
                    dBlock.Refresh();
                }

                // �ֻ��� ����
                else if (what == What.Dice)
                {
                    if (value != 0)
                        // �ֻ��� �߰�
                        current.dice.count += value;
                    else
                        // �ֻ��� ����
                        current.dice.type = (Dice.SpecialDice)where;
                }

                // ������ ȹ��
                else if (what == What.Life)
                {
                    current.life.Add(value);
                }

                // ���� ȹ��
                else if (what == What.Coin)
                {
                    current.coin.Add(value);
                }

                // ������ ȹ��
                else if (what == What.Item)
                {
                    if(value > 0)
                        if (value < Item.table.Count)
                            current.AddItem(Item.table[value], count);
                }

                // �̴ϰ��� ����
                else if (what == What.Minigame)
                {
                    // �̱��� ===================================
                }
            }

        }


        // ���� ȿ�� ���
        if (expiration == Expiration.Forever || expiration == Expiration.Cycle || expiration == Expiration.Turn)
        {
            // �Ҹ�ó��
            _count--;
            
            // ȿ�� ���
            if (!activeEffects.Contains(this))
                activeEffects.Add(this);
        }
        // ��� ����
        else
            _expiration = Expiration.Invalid;

        Debug.Log("ioc effect :: ȿ�� �۵���");

        yield return null;
    }
}
