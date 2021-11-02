using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IocReference
{
    public enum Type
    {
        None,
        Skybox,
        DesertObject,
        Mummy,
        Character,
        Trophy,
        BGM,
        SFX,
        Icon,
    }

    // ���̺�
    static List<IocReference> _table = new List<IocReference>();
    public static List<IocReference> table { get { return _table; } }


    // ���̺� Ȯ�ο�
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }




    // ��ȣ
    int _index = -1;
    public int index { get { return _index; } }

    // ī�װ�
    Type _type = Type.None;
    public Type type { get { return _type; } }

    // �̸�
    string _name = null;
    public string name { get { return _name; } }

    // ��ũ
    string _url = null;
    public string url { get { return _url; } }



    // ������

    /// <summary>
    /// ��� ����
    /// </summary>
    protected IocReference()
    {
        // ��� ����
    }
    /// <summary>
    /// ���̺� ������ �Է¹޾� ����
    /// </summary>
    /// <param name="strList">���̺� ����Ʈ�� �б�</param>
    protected IocReference(List<string> strList)
    {
        // out of range ����
        if (strList.Count != 4)
            return;

        // ���̺� �о����

        // �ε���
        _index = int.Parse(strList[0]);

        // ī�װ�
        _type = (Type)int.Parse(strList[1]);

        // �̸�
        _name = strList[2];

        // ��ũ
        _url = strList[3]; ;
    }






    public static void SetUp(TextAsset dataAsset)
    {
        Debug.Log("���̺� ���� : " + dataAsset.name);

        // �ߺ� ���� ����
        if (_isReady)
            return;

        // ���̺� �о����
        CSVReader reader = new CSVReader(dataAsset);
        Debug.Log(reader.table.Count);

        // ���� ����
        table.Add(new IocReference());

        // ���̺�� ����Ʈ ����
        for (int i = 1; i < reader.table.Count; i++)
        {
            table.Add(new IocReference(reader.table[i]));
        }

        // �غ�Ϸ�
        _isReady = true;

        Debug.Log("���̺� ���� : " + dataAsset.name + " -> �Ϸ� , table.count=" + table.Count);
    }

}
