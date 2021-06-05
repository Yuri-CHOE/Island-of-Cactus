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



    // �÷��̾� ���� UI
    public PlayerInfoUI infoUI = null;

    // �÷��̾� ������
    public Sprite face = null;

    // ĳ���� ������Ʈ
    public GameObject avatar = null;
    public Transform avatarBody = null;

    //// ��ġ �ε���
    //int _locate = -1;
    //public int locate { get { return _locate; } set { _locate = GameData.blockManager.indexLoop(_locate, value); } }

    //// ��ġ ������Ʈ
    //public GameObject locateBlock { get { if (GameData.isMainGameScene) { if (locate >= 0) return GameData.blockManager.gol[locate]; else return GameData.blockManager.startBlock.gameObject; } else return null; } }

    // �̵����� ��ũ��Ʈ
    public CharacterMover movement { get { if (avatar == null) return null; else return avatar.GetComponent<CharacterMover>(); } }




    // �ֻ���
    public Dice dice = new Dice();

    // �÷��̾� �ڿ�
    public GameResource life = new GameResource(10, 10, -1);
    public GameResource coin = new GameResource(110, 999, 0);

    // ������ ����
    public List<ItemSlot> inventory = new List<ItemSlot>();
    public int inventoryCount { get { int c = 0; for (int i = 0; i < inventory.Count; i++) { if (!inventory[i].isEmpty) c++; } return c; } }
    public static int inventoryMax = 3;

    // �ൿ �Ұ��� ����
    public bool isStun { get { return false; /* �̱��� =======================*/ } }

       

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

        Debug.Log("�÷��̾� ������ :: ĳ���� ��ȣ = " + characterIndex);
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
        // ���̺� üũ
        if (!Character.isReady)
            Character.SetUp();

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



    public void CreateAvatar(Transform parentObject)
    {
        // �θ� �������� �ߴ�
        if (parentObject == null)
            return;

        // ���� �ƹ�Ÿ ���� ��� ������Ʈ ����
        if (avatar != null)
            Transform.Destroy(avatar);

        // ������Ʈ ��ȿ �˻�
        Debug.Log(@"Data/Character/Character" + character.index.ToString("D4"));
        GameObject obj = Resources.Load<GameObject>(@"Data/Character/Character" + character.index.ToString("D4"));
        if (obj == null)
        {
            obj = Resources.Load<GameObject>(@"Data/Character/Character0000");
            Debug.Log(@"Data/Character/Character0000");
        }
        if (obj == null)
            Debug.Log("�ε� ���� :: Data/Character/Character0000");

        // ���� �� ���
        avatar = GameObject.Instantiate(
            obj,
            parentObject
            ) as GameObject;
        avatarBody = avatar.transform.Find("BodyObject");

        Debug.Log("ĳ���� ���� :: " + avatar.name);
    }


    public void LoadFace()
    {
        // ������ �ε�
        Debug.Log(@"Data/Character/Face/Face" + character.index.ToString("D4"));
        Sprite temp = Resources.Load<Sprite>(@"Data/Character/Face/Face" + character.index.ToString("D4"));

        // �̹��� ��ȿ �˻�
        if (temp == null)
        {
            // �⺻ ������ ��ü ó��
            Debug.Log(@"UI/playerInfo/player");
            temp = Resources.Load<Sprite>(@"UI/playerInfo/player");
        }

        // ���� ���� ó��
        if (temp == null)
            Debug.Log("�ε� ���� :: UI/playerInfo/player");
        // ������ ����
        else
            face = temp;
    }

    public void AddItem(ItemSlot itemSlot, int count)
    {
        // �ܿ� ���� ���� �� ���� === �ð� ���� �ȴٸ� �Ұ��� ������ �����Ͽ� ������ �ٲܰ�
        if (inventoryCount >= Player.inventoryMax)
            return;

        for (int i = 0; i < inventory.Count; i++)
        {
            // ��ĭ�ϰ�� �ְ� ����
            if (inventory[i].isEmpty)
            {
                inventory[i].item = itemSlot.item;
                break;
            }

        }
    }
}

