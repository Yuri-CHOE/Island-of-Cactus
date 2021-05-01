using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public enum Type {
        System,
        User,
        AI,
    }

    // ī����
    static int count = 0;               // �ý��� ���� Player ī����



    // �÷��̾� Ÿ��
    Type _type = Type.System;
    public Type type { get { return _type; } }

    // ĳ���� Ŭ���� ����
    Character _character = null;
    public Character character { get { return _character; } }

    // ���� �÷��� ����
    bool _isAutoPlay = false;
    public bool isAutoPlay { get { return _isAutoPlay; } }

    // �÷��̾� �̸�
    string _name = null;
    public string name { get { return _name; } }



    // �÷��̾� �ڿ�
    public GameResource life = new GameResource(10, 10, -1);
    public GameResource coin = new GameResource(0, 999, 0);

    // ������ ����
    public List<Item> inventory = new List<Item>();



    // ������
    /// <summary>
    /// ��� ����
    /// </summary>
    protected Player()
    {
        // ��� ����
    }
    public Player(Type __type, int characterIndex, bool __isAutoPlay, string playerName)
    {
        SetPlayer(__type, characterIndex, __isAutoPlay, playerName);

        // ī���� �ݿ�
        if (_type != Type.System)
            count++;
    }

    // �Ҹ���
    ~Player()
    {
        // ī���� �ݿ�
        if (_type != Type.System)
            count--;
    }



    /// <summary>
    /// �÷��̾� �ο��� ��ȯ
    /// </summary>
    public static int Count()
    {
        return count;
    }



    public void SetPlayer(Type __type, int characterIndex, bool __isAutoPlay, string playerName)
    {
        // ĳ���� Ÿ��
        _type = __type;

        // ĳ���� ����
        Debug.Log(characterIndex + " => " + Character.table.Count);
        _character = Character.table[characterIndex];

        // ���� ����
        _isAutoPlay = __isAutoPlay;

        // �÷��̾� �̸� ����
        _name = playerName;
    }

    /// <summary>
    /// �ڵ� �÷��� ����
    /// </summary>
    /// <param name="__isAutoPlay"></param>
    public void SetAutoPlay(bool __isAutoPlay)
    {
        _isAutoPlay = __isAutoPlay;
    }

}

