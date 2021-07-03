using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyBox
{
    public enum Type
    {
        None,
        Move,
        WorldEvent,
        MonsterWave,
        MiniGame,
        GetItem,
        StealItem
    }

    // ���̺�
    static List<LuckyBox> _table = new List<LuckyBox>();
    public static List<LuckyBox> table { get { return _table; } }


    // ���̺� Ȯ�ο�
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }




    // ��Ű�ڽ� ��ȣ
    int _index = -1;
    public int index { get { return _index; } }

    // ��Ű�ڽ� ī�װ�
    Type _type = Type.None;
    public Type type { get { return _type; } }

    // ��Ű�ڽ� �̸�
    string _name = null;
    public string name { get { return _name; } }

    // ��Ű�ڽ� ��� (�����)
    int _rare = -1;
    public int rare { get { return _rare; } }

    // ȿ�� Ÿ�� �÷��̾�
    Item.ItemEffect.Target _target = Item.ItemEffect.Target.Self;
    public Item.ItemEffect.Target target { get { return _target; } }

    // ȿ�� ���
    Item.ItemEffect.What _what = Item.ItemEffect.What.None;
    public Item.ItemEffect.What what { get { return _what; } }

    // ȿ�� ����
    int _where = -1;
    public int where { get { return _where; } }

    // ȿ�� ��
    int _value = -1;
    public int value { get { return _value; } }

    // ��Ű�ڽ� ����
    string _info = null;
    public string info { get { return _info; } }



    // ������

    /// <summary>
    /// ��� ����
    /// </summary>
    protected LuckyBox()
    {
        // ��� ����
    }
    /// <summary>
    /// ���̺� ������ �Է¹޾� ����
    /// </summary>
    /// <param name="strList">���̺� ����Ʈ�� �б�</param>
    protected LuckyBox(List<string> strList, List<string> loaclList)
    {
        // out of range ����
        if (strList.Count != 9)
            return;
        if (loaclList.Count != 3)
            return;

        // ���̺� �о����

        // �ε���
        _index = int.Parse(strList[0]);

        // ī�װ�
        _type = (Type)int.Parse(strList[1]);

        // �̸�
        //_name = strList[2];
        _name = loaclList[1];

        // ���
        _rare = int.Parse(strList[3]);

        // Ÿ��(�÷��̾�)
        _target = (Item.ItemEffect.Target)(int.Parse(strList[4]));

        // ���
        _what = (Item.ItemEffect.What)(int.Parse(strList[5]));

        // ��ġ
        _where = int.Parse(strList[6]);

        // ��
        _value = int.Parse(strList[7]);

        // ����
        //_info = strList[8];
        _info = loaclList[2].Replace("\\n", "\n").Replace("value", strList[7]);
    }



    /// <summary>
    /// ���̺� ����
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("���̺� ���� : ��Ű�ڽ�");

        // �ߺ� ���� ����
        if (_isReady)
            return;

        // ���̺� �о����
        CSVReader luckyReader = new CSVReader(null, "LuckyBox.csv");
        CSVReader local = new CSVReader(null, "LuckyBox_local.csv", true, false);

        // ���� ����
        table.Add(new LuckyBox());

        // ���̺�� ����Ʈ ����
        for (int i = 1; i < luckyReader.table.Count; i++)
        {
            table.Add(new LuckyBox(luckyReader.table[i], local.table[i]));
        }

        // �غ�Ϸ�
        _isReady = true;
    }













    /// <summary>
    /// ��Ű�ڽ� ȿ��
    /// </summary>
    /// <param name="___index">�۵��� ��Ű�ڽ� �ε���</param>
    public static void Effect(int __index)
    {
        switch (__index)
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
