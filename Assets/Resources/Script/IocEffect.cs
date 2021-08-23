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
        Event,
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

    public void SetExpiration(Expiration __expiration)
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




    public static IocEffect New() { IocEffect ef = new IocEffect(); ef.SetExpiration(IocEffect.Expiration.Invalid); return ef; }











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
        // ��� �������� Ÿ�� �����ϱ� ������ ���� Ÿ���� ȣ�� �� ���� ����
        else if (target == Target.SelectedPlayer)
            pl.Add(targetPlayer_Or_null);

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
        // ���� �̺�Ʈ ȣ��
        if (filteredTarget == null)
        {
            // ȣ��            
            yield return WorldEffect(this);
        }
        else
        {
            // ������ ����
            if (filteredTarget.Count == 0)
            {
                //=========== �̱���
                //yield return ;
            }


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
                    for (int j = 0; j < current.inventoryCount; j++)
                    {
                        // �ǵ� üũ
                        if (current.inventory[j].item.index == 19)
                        {
                            // �ǵ� �ڵ� ���
                            //GameMaster.script.itemManager.ItemUse(current.inventory[j]);
                            current.inventory[j].count--;

                            // ���� ����
                            isExecute = false;

                            // ��ĵ �ߴ�
                            break;
                        }
                    }
                }



                // ȿ�� ����
                if (!isExecute)
                    continue;

                // ��� ����
                if (what == What.None)
                {
                    Debug.Log("ȿ�� :: ���� ȿ�� ����");
                    break;
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
                    if (where == -3)
                    {
                        //blockIndex *= where;
                        current.dice.SetValueTotal(current.dice.valueTotal * value);

                        // �̵� ȣ��
                        current.movement.PlanMoveBy(current.dice.valueTotal);
                    }
                    else if (where == -2)
                    {
                        //blockIndex += where;
                        current.dice.SetValueTotal(current.dice.valueTotal + value);

                        // �̵� ȣ��
                        current.movement.PlanMoveBy(current.dice.valueTotal);
                    }
                    else
                    {
                        blockIndex = where;
                        current.dice.SetValueTotal(0);
                        Debug.LogError(blockIndex);


                        // ���ƽý� ����
                        if (blockIndex == -1)
                            current.movement.GotoJail();
                        // �̵� ȣ��
                        else
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
                    {
                        // �ֻ��� �߰�
                        current.dice.count += value;

                        Debug.Log("�ֻ��� :: " + value + " �� �߰� -> �� " + current.dice.count);
                    }
                    else
                    {
                        // �ֻ��� ����
                        current.dice.type = (Dice.SpecialDice)where;

                        Debug.Log("�ֻ��� :: Ÿ�� ���� -> " + current.dice.type);
                    }
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
                    if(value > 1)
                        if (value < Item.table.Count)
                            current.AddItem(Item.table[value], count);
                }

                // �̴ϰ��� ����
                else if (what == What.Minigame)
                {
                    // �̴ϰ��� ȣ��
                    // �ӽñ���======================================= �̴ϰ��� ī��� �ַ��÷��� �Ұ����̹Ƿ� ���� �̱������� ��ȸ��
                    // �ּ�ó���� ��ũ��Ʈ�� ��ü�Ұ�
                    GameMaster.script.loadingManager.LoadAsyncMiniGame(Minigame.RandomGame(), value, filteredTarget);
                    //GameMaster.script.loadingManager.LoadAsyncMiniGame(
                    //    Minigame.RandomGame(filteredTarget.Count), 
                    //    value, filteredTarget
                    //    );

                    // �ݺ� ����
                    break;
                }

                // �̴ϰ��� ����
                else if (what == What.Event)
                {
                    // ���� ��ġ
                    blockIndex = current.location + where;

                    // �̺�Ʈ ����
                    // ĳ���� ������ġ���� where ���������� value��° �̺�Ʈ�� count ��ŭ current�� ��ġ�Ѵ�
                    GameMaster.script.eventManager.CreateEventObject(blockIndex, value, count, current);
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



    public static IEnumerator WorldEffect(IocEffect __iocEffect)
    {
        // ���� Ư�� ȿ��
        switch ((int)__iocEffect.what)
        {
            // �ʿ�� �߰�

            case 0:
                // 0���� ����
                Debug.LogError("error :: �������� �ʴ� ���� �̺�Ʈ(0)�� ȿ�� ȣ���");
                Debug.Break();
                break;

            case 1:
                // ���̳ʽ� ��� ��ȭ

                // ����
                // �̱���=========

                // ���̳ʽ� ��� ��ȭ
                BlockWork.minusBlockValue++;

                break;

            case 2:
                // �÷��� ��� ��ȭ

                // ����
                // �̱���=========

                // �÷��� ��� ��ȭ
                BlockWork.plusBlockValue++;

                break;

            case 3:
                // �븻 ��� �ʱ�ȭ

                // ����
                // �̱���=========

                // �븻 ��� ��ȭ �ʱ�ȭ
                BlockWork.plusBlockValue = 0;
                BlockWork.minusBlockValue = 0;

                break;
        }

        yield return null;
    }
}
