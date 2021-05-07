using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    // ĳ���� ���̺�
    static List<Character> _table = new List<Character>();
    public static List<Character> table { get { return _table; } }       // �ʱ�ȭ �ȉ����� �ʱ�ȭ �� ��ȯ

    // ĳ���� ���̺� Ȯ�ο�
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }



    // ĳ���� ��ȣ
    int _index = -1;
    public int index { get { return _index; } }

    // ĳ���� �̸�
    string _name = null;
    public string name { get { return _name; } }

    // ĳ���� �̸�
    Job.JobType _job = Job.JobType.None;
    public Job.JobType job { get { return _job; } }

    // ĳ���� ����
    string _info = null;
    public string info { get { return _info; } }



    // ������
    protected Character()
    {
        // ��� ����
    }

    protected Character(List<string> strList)
    {
        // out of range ����
        if (strList.Count != 4)
            return;

        // ���̺� �о����
        SetCharacter(
            int.Parse(strList[0]),
            strList[1],
            (Job.JobType)int.Parse(strList[2]),
            strList[3]
            );
    }


    /// <summary>
    /// ���̺� ����
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("���̺� ���� : ĳ����");

        // �ߺ� ���� ����
        if (_isReady)
            return;

        // ���̺� �о����
        CSVReader characterReader = new CSVReader(null, "Character.csv");
        Debug.Log(characterReader.table.Count);

        // ���� ����
        table.Add(new Character());

        // ���̺�� ����Ʈ ����
        for (int i = 1; i < characterReader.table.Count; i++)
        {
            table.Add(new Character(characterReader.table[i]));
        }

        // �غ�Ϸ�
        _isReady = true;
    }

    /// <summary>
    /// ���̺� �ε��������� ĳ���� �����Է�
    /// </summary>
    /// <param name="characterIndex">���̺� �ε��� ��</param>
    public void SetCharacter(int characterIndex)
    {        
        if (characterIndex < 0)
            SetCharacter(-1, null, Job.JobType.None, null);     // �߸��� �� �ʱ�ȭ
        else
        {
            // ���̺� ����� ����� �����Ƿ� ����==========================================
            //SetCharacter(
            //    ���̺�[characterIndex][0], 
            //    ���̺�[characterIndex][1], 
            //    (Job.JobType)���̺�[characterIndex][2], 
            //    ���̺�[characterIndex][3]
            //    );     // ĳ���� ����
        }

    }
    void SetCharacter(int __index, string __name, Job.JobType __job, string __info)
    {
        // �߸��� �� �ʱ�ȭ
        if (__index < 0)
        {
            SetIndex(-1);
            SetName(null);
            SetJob(Job.JobType.None);
            SetInfo(null);
        }

        SetIndex(__index);
        SetName(__name);
        SetJob(__job);
        SetInfo(__info);
    }

    void SetIndex(int __index)
    {
        _index = __index;
    }

    void SetName(string __name)
    {
        _name = __name;
    }

    void SetJob(Job.JobType __job)
    {
        _job = __job;
    }

    void SetInfo(string __info)
    {
        _info = __info;
    }


}
