using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame
{
    public struct MinigameReward
    {
        // ��ü ����
        public int total;

        // ����� ���з�
        public int rank_1;
        public int rank_2;
        public int rank_3;
        public int rank_4;

        public void Set(int _total, int _rank_1, int _rank_2, int _rank_3, int _rank_4)
        {
            total = _total;
            rank_1 = _rank_1;
            rank_2 = _rank_2;
            rank_3 = _rank_3;
            rank_4 = _rank_4;
            Debug.LogError(_total);
        }

        //public int Share(int value)
        //{
        //    return total * value / (rank_1 + rank_2 + rank_3 + rank_4);
        //}

        public int GetRank(int rank)
        {
            switch (rank)
            {
                case 1:
                    Debug.LogWarning("�̴ϰ��� :: ���� ���� -> " + rank + "�� = ����Ʈ " + rank_1);
                    return rank_1;
                case 2:
                    Debug.LogWarning("�̴ϰ��� :: ���� ���� -> " + rank + "�� = ����Ʈ " + rank_2);
                    return rank_2;
                case 3:
                    Debug.LogWarning("�̴ϰ��� :: ���� ���� -> " + rank + "�� = ����Ʈ " + rank_3);
                    return rank_3;
                case 4:
                    Debug.LogWarning("�̴ϰ��� :: ���� ���� -> " + rank + "�� = ����Ʈ " + rank_4);
                    return rank_4;
                default:
                    return 0;
            }
        }        
    }

    // ���̺�
    static List<Minigame> _table = new List<Minigame>();
    public static List<Minigame> table { get { return _table; } }
    


    // ���̺� Ȯ�ο�
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }





    // �̴ϰ��� ��ȣ
    int _index = -1;
    public int index { get { return _index; } }

    // �̴ϰ��� �̸�
    string _name = null;
    public string name { get { return _name; } }

    // �ּ� ������ ��
    int _playerMin = -1;
    public int playerMin { get { return _playerMin; } }

    // �̴ϰ��� ��ȣ
    int _sceneNum = -1;
    public int sceneNum { get { return _sceneNum; } }    

    // �̴ϰ��� ����
    MinigameReward _reward = new MinigameReward();
    public MinigameReward reward { get { return _reward; } }

    // �̴ϰ��� ����
    string _info = null;
    public string info { get { return _info; } }



    // ���� ����
    protected Minigame()
    {

    }
    /// <summary>
    /// ���̺� ������ �Է¹޾� ����
    /// </summary>
    /// <param name="strList">���̺� ����Ʈ�� �б�</param>
    protected Minigame(List<string> strList, List<string> loaclList)
    {
        // out of range ����
        if (strList.Count != 10)
            return;
        if (loaclList.Count != 3)
            return;
        
        // ���̺� �о����

        // �ε���
        _index = int.Parse(strList[0]);

        // �̸�
        _name = loaclList[1];

        // �ּ� ������ ��
        _playerMin = int.Parse(strList[2]);

        // �� ��ȣ
        _sceneNum = int.Parse(strList[3]);

        // ���� �� ���з�
        _reward.Set(
            int.Parse(strList[4]),
            int.Parse(strList[5]),
            int.Parse(strList[6]),
            int.Parse(strList[7]),
            int.Parse(strList[8])
            );

        // ����
        _info = loaclList[2].Replace("\\n", "\n");
    }



    /// <summary>
    /// ���̺� ����
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("���̺� ���� : �̴ϰ���");

        // �ߺ� ���� ����
        if (_isReady)
            return;

        // ���̺� �о����
        CSVReader miniReader = new CSVReader(null, "Minigame.csv");
        CSVReader local = new CSVReader(null, "Minigame_local.csv", true, false);

        // ���� ����
        table.Add(new Minigame());

        // ���̺�� ����Ʈ ����
        Minigame current = null;
        for (int i = 1; i < miniReader.table.Count; i++)
        {
            current = new Minigame(miniReader.table[i], local.table[i]);
            table.Add(current);
        }

        // �غ�Ϸ�
        _isReady = true;
    }


    public static Minigame RandomGame()
    {
        return RandomGame(1);
    }

    public static Minigame RandomGame(int entryCount)
    {
        // ���̺� ���� ����
        if (!isReady)
            return null;


        // ���� ��
        int indexer = Random.Range(1, table.Count);
        while (table[indexer].playerMin < entryCount)
            indexer = Random.Range(1, table.Count);

        Debug.LogWarning("�̴ϰ��� :: ���� �ε��� -> " + indexer);

        return table[indexer];
    }
}
