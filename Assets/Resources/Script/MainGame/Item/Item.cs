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

    public class ItemEffect
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
            DesignatedPlayer,
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

        // ȿ�� Ÿ�� �÷��̾�
        Target _target = Target.Self;
        public Target target { get { return _target; } }

        // ȿ�� ���
        What _what = What.None;
        public What what { get { return _what; } }

        // ȿ�� ��
        int _value = -1;
        public int value { get { return _value; } }



        /// <summary>
        /// �缳�� �Լ�, ���� ������� ����
        /// </summary>
        /// <param name="__expiration"></param>
        /// <param name="__count"></param>
        /// <param name="__target"></param>
        /// <param name="__what"></param>
        /// <param name="__value"></param>
        public void Set(Expiration __expiration, int __count, Target __target, What __what, int __value)
        {
            SetExpiration(__expiration);
            SetCount(__count);
            SetTarget(__target);
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

        void SetWhat(What __what)
        {
            _what = __what;
        }

        void SetValue(int __value)
        {
            _value = __value;
        }
    }

    // ������ ���̺�
    static List<Item> _table = new List<Item>();
    public static List<Item> table { get { return _table; } }

    // ������ ���̺� Ȯ�ο�
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }


    // ���� ������
    static string[] emptyItem = { "0", "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "" };
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

    // ������ ��� (�����)
    int _rare = -1;
    public int rare { get { return _rare; } }

    // ������ ����
    int _cost = 0;
    public int cost { get { return _cost; } }

    // ��Ű�ڽ� ȹ�濩��
    bool _isLuckyBoxGet = false;
    public bool isLuckyBoxGet { get { return _isLuckyBoxGet; } }

    // ������ ����
    string _info = null;
    public string info { get { return _info; } }

    // ȿ��
    public ItemEffect effect = new ItemEffect();



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
        if (strList.Count != 13)
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
            int.Parse(strList[4]),
            int.Parse(strList[5]),
            System.Convert.ToBoolean(int.Parse(strList[6])),
            (ItemEffect.Expiration)int.Parse(strList[7]),
            int.Parse(strList[8]),
            (ItemEffect.Target)(int.Parse(strList[9])),
            (ItemEffect.What)(int.Parse(strList[10])),
            int.Parse(strList[11]),
            //strList[12].Replace("value", strList[11])
            loaclList[2].Replace("\\n","\n").Replace("value", strList[11])
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
            table.Add(new Item(itemReader.table[i], local.table[i]));
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
    void Set(int __index, Type __type, string __name, int __icon, int __rare, int __cost, bool __isLuckyBoxGet, ItemEffect.Expiration __expiration, int __count, ItemEffect.Target __target, ItemEffect.What __what, int __value, string __info)
    {
        SetIndex(__index);
        SetType(__type);
        SetName(__name);
        SetIcon(__icon);
        SetRare(__rare);
        SetCost(__cost);
        SetLuckyBoxGet(__isLuckyBoxGet);
        effect.Set(__expiration, __count, __target, __what, __value);
        SetInfo(__info);
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
    /// ������ ȿ��
    /// </summary>
    /// <param name="___index">�۵��� ������</param>
    /// <param name="targetPlayer_Or_null">�۵���Ų �÷��̾�</param>
    public static void Effect(Item __item, Player targetPlayer_Or_null)
    {
        switch (__item.index)
        {
            case 0:
                // 0���� ����
                break;

            case 1:
                // ȿ��
                break;

            case 2:
                // ȿ��
                break;

                // ���� �߰� �ʿ�========================

        }
    }
}
