using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum Type
    {
        None,
        Target,
        Installation,
        Consumable,
        WideArea,
    }


    // ������ ���̺�
    static List<Item> _table = new List<Item>();
    public static List<Item> table { get { return _table; } }

    // ������ ���̺�
    static List<Item> _tableLuckyDrop = new List<Item>();
    public static List<Item> tableLuckyDrop { get { return _tableLuckyDrop; } }

    // ������ ���̺� Ȯ�ο�
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }


    // ���� ������
    static string[] emptyItem = { "0", "0", "", "0", "0", "0", "0", "0", "0", "0", "", "0", "0", "0" };
    static string[] emptyItem_local = { "0", "empty", "is empty" };
    public static Item empty = new Item(new List<string>(emptyItem), new List<string>(emptyItem_local));


    // ������ ��ȣ
    int _index = -1;
    public int index { get { return _index; } }

    // ������ ī�װ�
    Type _type = Type.None;
    public Type type { get { return _type; } }

    // ������ �̸�
    string _name = null;
    public string name { get { return _name; } }

    // ������ ��ȣ
    int _icon = -1;
    public int icon { get { return _icon; } }

    // ������ ����
    string _info = null;
    public string info { get { return _info; } }

    // ȿ��
    public IocEffect effect = new IocEffect();

    // ������ ��� (�����)
    int _rare = -1;
    public int rare { get { return _rare; } }

    // ������ ����
    int _cost = 0;
    public int cost { get { return _cost; } }

    // ��Ű�ڽ� ȹ�濩��
    bool _isLuckyBoxGet = false;
    public bool isLuckyBoxGet { get { return _isLuckyBoxGet; } }



    // ������

    /// <summary>
    /// ��� ����
    /// </summary>
    protected Item()
    {
        // ��� ����
    }
    /// <summary>
    /// ���̺� ������ �Է¹޾� ����
    /// </summary>
    /// <param name="strList">���̺� ����Ʈ�� �б�</param>
    protected Item(List<string> strList, List<string> loaclList)
    {
        // out of range ����
        if (strList.Count != 14)
            return;
        if (loaclList.Count != 3)
            return;


        // ���̺� �о����
        Set(
            int.Parse(strList[0]),
            (Type)int.Parse(strList[1]),
            //strList[2],
            loaclList[1],
            int.Parse(strList[3]),

            (IocEffect.Expiration)(int.Parse(strList[4])),
            int.Parse(strList[5]),
            (IocEffect.Target)(int.Parse(strList[6])),
            int.Parse(strList[7]),
            (IocEffect.What)(int.Parse(strList[8])),
            int.Parse(strList[9]),
            //strList[12].Replace("value", strList[11])
            loaclList[2].Replace("\\n","\n").Replace("value", strList[9]).Replace("where", strList[7]),

            int.Parse(strList[11]),
            int.Parse(strList[12]),
            System.Convert.ToBoolean(int.Parse(strList[13]))
        );
    }

    /// <summary>
    /// ���̺� ����
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("���̺� ���� : ������");

        // �ߺ� ���� ����
        if (_isReady)
            return;

        // ���̺� �о����
        CSVReader itemReader = new CSVReader(null, "Item.csv");
        CSVReader local = new CSVReader(null, "Item_local.csv", true, false);

        // ���� ����
        table.Add(new Item());

        // ���̺�� ����Ʈ ����
        for (int i = 1; i < itemReader.table.Count; i++)
        {
            Item current = new Item(itemReader.table[i], local.table[i]);
            table.Add(current);

            // ��Ű�ڽ� ���� ������ ���̺�
            if (current.isLuckyBoxGet)
                tableLuckyDrop.Add(current);
        }

        // �غ�Ϸ�
        _isReady = true;
    }


    /// <summary>
    /// �缳�� �Լ�, ���̺� �о ����Ұ�
    /// </summary>
    /// <param name="__index"></param>
    /// <param name="__type"></param>
    /// <param name="__name"></param>
    /// <param name="__icon"></param>
    /// <param name="__rare"></param>
    /// <param name="__cost"></param>
    /// <param name="__isLuckyBoxGet"></param>
    /// <param name="__expiration"></param>
    /// <param name="__count"></param>
    /// <param name="__target"></param>
    /// <param name="__what"></param>
    /// <param name="__value"></param>
    /// <param name="__info"></param>
    void Set(int __index, Type __type, string __name, int __icon, IocEffect.Expiration __expiration, int __count, IocEffect.Target __target, int __where, IocEffect.What __what, int __value, string __info, int __rare, int __cost, bool __isLuckyBoxGet)
    {
        SetIndex(__index);
        SetType(__type);
        SetName(__name);
        SetIcon(__icon);
        effect.Set(__expiration, __count, __target, __where, __what, __value);
        SetInfo(__info);
        SetRare(__rare);
        SetCost(__cost);
        SetLuckyBoxGet(__isLuckyBoxGet);
    }

    void SetIndex(int __index)
    {
        _index = __index;
    }

    void SetType(Type __type)
    {
        _type = __type;
    }

    void SetName(string __name)
    {
        _name = __name;
    }

    void SetIcon(int __icon)
    {
        _icon = __icon;
    }

    void SetRare(int __rare)
    {
        _rare = __rare;
    }

    void SetCost(int __cost)
    {
        _cost = __cost;
    }

    void SetLuckyBoxGet(bool __isLuckyBoxGet)
    {
        _isLuckyBoxGet = __isLuckyBoxGet;
    }

    void SetInfo(string __info)
    {
        _info = __info;
    }




    /// <summary>
    /// ������ �ε�
    /// </summary>
    public Sprite GetIcon()
    {
        // ������ �ε�
        Debug.Log(@"Data/Item/icon/item" + index.ToString("D4"));
        Sprite temp = Resources.Load<Sprite>(@"Data/Item/icon/item" + index.ToString("D4"));

        // �̹��� ��ȿ �˻�
        if (temp == null)
        {
            // �⺻ ������ ��ü ó��
            Debug.Log(@"Data/Item/icon/item0000");
            temp = Resources.Load<Sprite>(@"Data/Item/icon/item0000");
        }

        //// ���� ���� ó��
        //if (temp == null)
        //    Debug.Log("�ε� ���� :: Data/Item/icon/item0000");
        //// ������ ����
        //else
        //    _icon.sprite = temp;


        // ������ ����
        if (temp != null)
            return temp;

        // ���� ���� ó��
        Debug.Log("�ε� ���� :: Data/Item/icon/item0000");
        return null;
    }



    /// <summary>
    /// �̺�Ʈ ȿ��
    /// </summary>
    /// <param name="targetPlayer_Or_null">�۵���Ų �÷��̾�</param>
    //public static void Effect(Item __item, Player targetPlayer_Or_null)
    public IEnumerator Effect(Player targetPlayer_Or_null)
    {
        // Ÿ�� ����Ʈ
        List<Player> pl = IocEffect.TargetFiltering(effect.target, targetPlayer_Or_null);

        // ���� ȿ��
        yield return effect.GeneralEffect(targetPlayer_Or_null, pl);

        // ���� Ư�� ȿ��
        yield return EachEffect(this, targetPlayer_Or_null, pl);

        // ���ν�Ʈ�� ��� ����
        GameMaster.useItemOrder = false;
    }

    public static IEnumerator EachEffect(Item __item, Player user, List<Player> filteredTarget)
    {
        switch (__item.index)
        {
            // �ʿ�� �߰�

            case 0:
                // 0���� ����
                break;

            case 17:
                // ���� ����

                for(int i = 0; i < filteredTarget.Count; i++)
                {
                    // �̵� ���� ����
                    filteredTarget[i].dice.multypleValue = __item.effect.where;

                    // ü�� �г�Ƽ ����
                    filteredTarget[i].life.Set(__item.effect.value);
                }

                break;

            case 18:
                // �帣�� ��

                for (int i = 0; i < filteredTarget.Count; i++)
                {
                    // �ֻ��� �м�
                    GameMaster.script.diceController.ResetDice();

                    // �̵�
                    yield return filteredTarget[i].movement.Tleport(BlockManager.script.shopBlockIndex, 1f);

                    // ������
                    //WaitForSeconds waiter = new WaitForSeconds(1f);
                    //yield return waiter;
                    
                    // �׼� ��ŵ
                    Turn.turnAction = Turn.TurnAction.Block;
                    Turn.actionProgress = ActionProgress.Ready;
                }

                break;

            case 19:
                // ���� ȿ���� IocEffect.GeneralEffect() ���� ȣ���
                Debug.Log("ȿ�� :: �ǵ� �۵���");

                // ����Ʈ�� ������ �� ====================== �̱���
                break;

            case 20:
                // ȿ��
                for (int i = 0; i < filteredTarget.Count; i++)
                {
                    // ����
                    // �̱���

                    // ����
                    filteredTarget[i].RemoveItem(filteredTarget[i].inventory[0]);
                }
                break;

            case 21:
                // ȿ��

                // �߸��� �ο��� ����
                if (filteredTarget == null)
                    break;

                // ������ ������ �ߴ�
                if (filteredTarget[0].inventoryCount <= 0)
                    break;


                // ������ ������ ����
                ItemSlot slot = filteredTarget[0].inventory[0];

                // ȹ��
                // ���� - �̱���=============
                user.AddItem(slot, slot.count);

                // ��Ż
                // ���� - �̱���=============
                filteredTarget[0].RemoveItem(slot);

                break;
        }

        yield return null;
    }

    public static float Efficiency(Item __item, Player user)
    {
        // ȿ�� ��� �簡��
        switch (__item.index)
        {
            case 0:     // ���� - ���� ������
            case 19:    // �ǵ� - ��� �Ұ���
                {
                    return 0;
                }


            case 3:     // �̱� ť��
            case 4:     // ��� ť��
            case 5:     // �����¡ ť��
            case 6:     // ��� ť��
                {
                    // Ư�� �ֻ��� ����� ȿ�� 0
                    if (user.dice.type != Dice.SpecialDice.Normal)
                        return 0f;

                    break;
                }

            case 8:     // ������ ����
                {
                    return 0.2f;
                }

            case 12:     // ���ִ� ����
                {
                    // ���� ������ >= ������
                    if (user.life.Value <= -__item.effect.value)
                        return 0f;

                    // Ÿ�� ������ üũ
                    float deadCount = 1f;
                    float percent = 0.6f / user.otherPlayers.Count;
                    for (int i = 0; i < user.otherPlayers.Count; i++)
                        // ����� �ݿ� - ȿ�� ����
                        if(user.otherPlayers[i].life.Value <= 0)
                            deadCount -= percent;

                    if (deadCount <= 0.4f)
                        return 0f;
                    else
                        return deadCount;
                }

            case 17:    // ���� ����
                {
                    // Ư�� �ֻ��� ������ ȿ�� 1
                    for (int i = 0; i < user.inventoryCount; i++)
                    {
                        if (user.inventory[i].item.index == 3 ||
                            user.inventory[i].item.index == 4 ||
                            user.inventory[i].item.index == 5 ||
                            user.inventory[i].item.index == 6)
                            return 1f;
                    }

                    // �׿� 0.5
                    return 0.5f;
                }

            case 18:     // �帣�� ��
                {
                    // ���� ������
                    if (user.coin.Value < 20)
                        return 0f;
                    else
                        return user.coin.Value / 50;
                }

            case 20:     // ������ ���Ǹ�
                {
                    // Ÿ�� ����Ʈ
                    List<Player> pl = IocEffect.TargetFiltering(__item.effect.target, user);

                    // Ÿ�� ����
                    int targetCount = 0;

                    // Ÿ�� ������ ���� �ݿ�
                    for (int i = 0; i < pl.Count; i++)
                    {
                        if (pl[i].inventoryCount > 0)
                            targetCount++;
                    }

                    return targetCount / pl.Count;
                }

            case 21:     // �������
                {
                    // Ÿ�� ����Ʈ
                    List<Player> pl = IocEffect.TargetFiltering(__item.effect.target, user);

                    // Ÿ�� ������ ���� �ݿ�
                    for (int i = 0; i < pl.Count; i++)
                    {
                        if (pl[i].inventoryCount > 0)
                            return 1f;
                    }

                    return 0f;
                }
        }

        // ���
        return 1f;
    }

    public static Player AutoTargeting(Item __item, Player user)
    {
        // ȿ�� ��� �簡��
        switch (__item.index)
        {
            case 21:     // �������
                {
                    // Ÿ�� ����Ʈ
                    List<Player> pl = IocEffect.TargetFiltering(__item.effect.target, user);

                    // Ÿ�� �ε���
                    int indexer = 0;

                    // Ÿ�� ������ ���� �ݿ�
                    for (int i = 0; i < pl.Count; i++)
                    {
                        // 1���� - ������ ���� ��
                        if (pl[i].inventoryCount > pl[indexer].inventoryCount)
                            indexer = i;
                        else if (pl[i].inventoryCount == pl[indexer].inventoryCount)
                        {
                            // 2���� - ���� ��
                            if (pl[i].coin.Value > pl[indexer].coin.Value)
                                indexer = i;
                            else if (pl[i].coin.Value == pl[indexer].coin.Value)
                            {
                                // 3���� - ������ ��
                                if (pl[i].life.Value > pl[indexer].life.Value)
                                    indexer = i;
                            }

                        }
                    }

                    // ���
                    return pl[indexer];
                }
        }

        // ���
        return null;
    }
}
