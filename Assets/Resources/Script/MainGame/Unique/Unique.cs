using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unique
{
    // ����ũ ���̺�
    static List<Unique> _table = new List<Unique>();
    public static List<Unique> table { get { return _table; } }

    // ����ũ ���̺� Ȯ�ο�
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }



    // ��ȣ
    int _index = -1;
    public int index { get { return _index; } }

    // �̸�
    string _name = null;
    public string name { get { return _name; } }

    // ��
    int _value = -1;
    public int value { get { return _value; } }

    // ����
    string _info = null;
    public string info { get { return _info; } }



    /// <summary>
    /// ��� ����
    /// </summary>
    Unique()
    {
        // ��� ����
    }
    /// <summary>
    /// ���̺� ������ �Է¹޾� ����
    /// </summary>
    /// <param name="strList">���̺� ����Ʈ�� �б�</param>
    public Unique(List<string> strList, List<string> loaclList)
    {
        // out of range ����
        if (strList.Count != 4)
            return;
        if (loaclList.Count != 3)
            return;

        // ���̺� �о����
        Set(
            int.Parse(strList[0]),
            loaclList[1],
            int.Parse(strList[2]),
            loaclList[2]
        );

    }


    /// <summary>
    /// ���̺� ����
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("���̺� ���� : ����ũ");

        // �ߺ� ���� ����
        if (_isReady)
            return;

        // ���̺� �о����
        CSVReader uniqueReader = new CSVReader(null, "Unique.csv");
        CSVReader local = new CSVReader(null, "Unique_local.csv",true, false);

        // ���� ����
        table.Add(new Unique());

        // ���̺�� ����Ʈ ����
        for (int i = 1; i < uniqueReader.table.Count; i++)
        {
            table.Add(new Unique(uniqueReader.table[i], local.table[i]));
        }

        // �غ�Ϸ�
        _isReady = true;
    }
    public static void SetUp(TextAsset dataAsset, TextAsset localAsset)
    {
        Debug.Log("���̺� ���� : " + dataAsset.name);

        // �ߺ� ���� ����
        if (_isReady)
            return;

        // ���̺� �о����
        CSVReader reader = new CSVReader(dataAsset);
        //CSVReader local = new CSVReader(localAsset, true, false);
        CSVReader local = new CSVReader(localAsset);
        Debug.Log(reader.table.Count);

        // ���� ����
        table.Add(new Unique());

        // ���̺�� ����Ʈ ����
        for (int i = 1; i < reader.table.Count; i++)
        {
            table.Add(new Unique(reader.table[i], local.table[i]));
        }

        // �غ�Ϸ�
        _isReady = true;

        Debug.Log("���̺� ���� : " + dataAsset.name + " -> �Ϸ� , table.count=" + table.Count);
    }


    void Set(int __index, string __name, int __value, string __info)
    {
        _index = __index;
        _name = __name;
        _value = __value;
        _info = __info;
    }
}
