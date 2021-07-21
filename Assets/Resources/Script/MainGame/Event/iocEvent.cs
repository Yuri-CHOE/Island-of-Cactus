using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IocEvent
{
    public enum ActiveType
    {
        none,
        arrive,     // ���� (�̵� ����)
        contact,    // ����
    }

    public class iocEventEffect
    {

        // ȿ�� ī��Ʈ(����)
        int _count = -1;
        public int count { get { return _count; } }

        // ȿ�� Ÿ�� �÷��̾�
        IocEffect.Target _target = IocEffect.Target.Self;
        public IocEffect.Target target { get { return _target; } }

        // ȿ�� ������
        int _where = -1;
        public int where { get { return _where; } }

        // ȿ�� ���
        IocEffect.What _what = IocEffect.What.None;
        public IocEffect.What what { get { return _what; } }

        // ȿ�� ��
        int _value = -1;
        public int value { get { return _value; } }



        /// <summary>
        /// �缳�� �Լ�, ���� ������� ����
        /// </summary>
        /// <param name="__count"></param>
        /// <param name="__target"></param>
        /// <param name="__where"></param>
        /// <param name="__what"></param>
        /// <param name="__value"></param>
        public void Set(int __count, IocEffect.Target __target, int __where, IocEffect.What __what, int __value)
        {
            SetCount(__count);
            SetTarget(__target);
            SetWhere(__where);
            SetWhat(__what);
            SetValue(__value);
        }

        void SetCount(int __count)
        {
            if (__count < 0)
                return;

            _count = __count;
        }

        void SetTarget(IocEffect.Target __target)
        {
            _target = __target;
        }

        void SetWhere(int __where)
        {
            _where = __where;
        }

        void SetWhat(IocEffect.What __what)
        {
            _what = __what;
        }

        void SetValue(int __value)
        {
            _value = __value;
        }
    }

    // �̺�Ʈ ���̺�
    static List<IocEvent> _table = new List<IocEvent>();
    public static List<IocEvent> table { get { return _table; } }       // �ʱ�ȭ �ȉ����� �ʱ�ȭ �� ��ȯ

    // �̺�Ʈ Ȯ�ο�
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }



    // �̺�Ʈ ��ȣ
    int _index = -1;
    public int index { get { return _index; } }

    // �̺�Ʈ �۵� ���
    ActiveType _activeType = ActiveType.none;
    public ActiveType activeType { get { return _activeType; } }

    // �̺�Ʈ �̸�
    string _name = null;
    public string name { get { return _name; } }

    // �̺�Ʈ �� �ε���
    int _modelIndex = -1;
    public int modelIndex { get { return _modelIndex; } }

    // �̺�Ʈ ����
    string _info = null;
    public string info { get { return _info; } }

    // �̺�Ʈ ����
    IocEffect _effect = new IocEffect();
    public IocEffect effect { get { return _effect; } }



    // ������
    protected IocEvent()
    {
        // ��� ����
    }

    protected IocEvent(List<string> strList, List<string> loaclList)
    {
        // out of range ����
        if (strList.Count != 11)
            return;
        if (loaclList.Count != 3)
            return;

        // ���̺� �о����
        Set(
            int.Parse(strList[0]),
            (ActiveType)int.Parse(strList[1]),
            loaclList[1],
            int.Parse(strList[3]),

            (IocEffect.Expiration)(int.Parse(strList[4])),
            int.Parse(strList[5]),
            (IocEffect.Target)(int.Parse(strList[6])),
            int.Parse(strList[7]),
            (IocEffect.What)(int.Parse(strList[8])),
            int.Parse(strList[9]),

            loaclList[2].Replace("\\n", "\n").Replace("value", strList[9])
            );
    }


    /// <summary>
    /// ���̺� ����
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("���̺� ���� : �̺�Ʈ");

        // �ߺ� ���� ����
        if (_isReady)
            return;

        // ���̺� �о����
        CSVReader reader = new CSVReader(null, "Event.csv");
        CSVReader local = new CSVReader(null, "Event_local.csv", true, false);
        Debug.Log(reader.table.Count);

        // ���� ����
        table.Add(new IocEvent());

        // ���̺�� ����Ʈ ����
        for (int i = 1; i < reader.table.Count; i++)
        {
            table.Add(new IocEvent(reader.table[i], local.table[i]));
        }

        // �غ�Ϸ�
        _isReady = true;
    }
    

    void Set(int __index, ActiveType __activeType, string __name, int __modelIndex, IocEffect.Expiration __expiration, int __count, IocEffect.Target __target, int __where, IocEffect.What __what, int __value, string __info)
    {
        SetIndex(__index);
        SetActiveType(__activeType);
        SetName(__name);
        effect.Set(__expiration, __count, __target, __where, __what, __value);
        SetInfo(__info);
    }

    void SetIndex(int __index)
    {
        _index = __index;
    }

    void SetActiveType(ActiveType __activeType)
    {
        _activeType = __activeType;
    }

    void SetName(string __name)
    {
        _name = __name;
    }

    void SetInfo(string __info)
    {
        _info = __info;
    }


    /// <summary>
    /// �̺�Ʈ �۵� ����
    /// </summary>
    /// <param name="__iocEvent">�۵��� �̺�Ʈ</param>
    /// <param name="current">�۵���Ų �÷��̾�</param>
    /// <param name="creator">�̺�Ʈ�� ������ �÷��̾�</param>
    public static bool Condition(IocEvent __iocEvent, Player current, Player creator)
    {
        bool result = false ;

        // �ߵ��� �ڰ� üũ - ���� DB�� �ߵ��� �ڰ� �ʵ� ����
        //if(__iocEvent.effect.target == iocEventEffect.Target.Self)
        //    result = true;

        result = true;

        return result;
    }


    /// <summary>
    /// �̺�Ʈ ȿ��
    /// </summary>
    /// <param name="targetPlayer_Or_null">�۵���Ų �÷��̾�</param>
    //public static IEnumerator Effect(IocEvent __iocEvent, Player targetPlayer_Or_null, int __blockIndex)
    public IEnumerator Effect(Player targetPlayer_Or_null)
    {
        // Ÿ�� ����Ʈ
        List<Player> pl = IocEffect.TargetFiltering(effect.target, targetPlayer_Or_null);

        // ���� ȿ��
        yield return effect.GeneralEffect(targetPlayer_Or_null, pl);

        // ���� Ư�� ȿ��
        yield return EachEffect(this);
    }

    public static IEnumerator EachEffect(IocEvent __iocEvent)
    {
        // ���� Ư�� ȿ��
        switch (__iocEvent.index)
        {
            case 0:
                // 0���� ����
                Debug.LogError("error :: �������� �ʴ� �̺�Ʈ(0)�� ȿ�� ȣ���");
                Debug.Break();
                break;

            case 1:
                // ȿ��
                break;

            case 2:
                // ȿ��
                break;

            case 3:
                // ȿ��
                break;

            case 4:
                // ȿ��
                break;

            case 5:
                // ȿ��
                break;

        }

        yield return null;
    }

}
