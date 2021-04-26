using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    // ī����
    static int count = 0;               // ������ + �ý��� Player ī����
    static int sysCount = 0;            // �ý��� Player ī����

    // ĳ���� Ŭ���� ����
    public Character character = new Character(-1);

    // �÷��̾� ���� UI ��ġ
    public PlayerInfoUI InfoUI = null;


    // ���� �÷��� ����
    bool _isAutoPlay = false;
    public bool isAutoPlay { get { return _isAutoPlay; } }

    // �÷��̾� ��ȣ
    int _index = -1;
    public int index { get { return _index; } }

    // �÷��̾� �̸�
    string _name = null;
    public string name { get { return _name; } }

    // �÷��̾� ����
    int _order = -1;
    public int order { get { return _order; } }



    // �÷��̾� �ڿ�
    public GameResource life = new GameResource(10, 10, -1);
    public GameResource coin = new GameResource(0, 999, 0);

    // ������ ����
    public List<Item> inventory = new List<Item>();



    // ������
    /// <summary>
    /// ��� ���� => Player(int characterIndex) ���
    /// </summary>
    private Player()
    {
        // ��� ����
    }
    public Player(string playerName, int characterIndex, bool __isAutoPlay)
    {
        // ĳ���� ����
        character.SetCharacter(characterIndex);


        // ���� ����
        _isAutoPlay = __isAutoPlay;

        // �÷��̾� ��ȣ ����
        _index = count;

        // �÷��̾� �̸� ����
        _name = playerName;



        // ī���� �ݿ�
        if (character.job == Job.JobType.System)
        {
            sysCount++;
            count++;
        }
        else
        {
            count++;
        }
    }

    // �Ҹ���
    ~Player()
    {
        // ī���� �ݿ�
        if (character.job == Job.JobType.System)
        {
            sysCount--;
            count--;
        }
        else
        {
            count--;
        }
    }



    /// <summary>
    /// �÷��̾� �ο��� ��ȯ
    /// </summary>
    public static int Count()
    {
        return count - sysCount;
    }


    /// <summary>
    /// �ý��� �÷��̾� �ο��� ��ȯ
    /// </summary>
    public static int CountSystem()
    {
        return sysCount;
    }



    public void SetPlayer(string playerName, int characterIndex, bool __isAutoPlay)
    {
        // ĳ���� ����
        character.SetCharacter(characterIndex);


        // ���� ����
        _isAutoPlay = __isAutoPlay;

        // �÷��̾� ��ȣ ����
        _index = count;

        // �÷��̾� �̸� ����
        _name = playerName;
    }
}

