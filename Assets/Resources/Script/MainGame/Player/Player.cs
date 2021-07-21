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

    public class PlayerSystem
    {
        public Player Starter = new Player(Player.Type.System, 1, true, "Starter");
        public Player Minigame = new Player(Player.Type.System, 1, true, "Minigame");
        public Player MinigameEnder = new Player(Player.Type.System, 1, true, "MinigameEnder");
        public Player Ender = new Player(Player.Type.System, 1, true, "Ender");
    }

    // �ý��� �÷��̾�
    public static PlayerSystem system = new PlayerSystem();

    // �ý��� ���� ��� �÷��̾�
    public static List<Player> allPlayer = new List<Player>();

    // �÷��̾� �ڽ�
    public static Player me = null;

    // �� ������ �÷��̾�
    public static Player isTurn { get { return Turn.now; } }

    // p1~4 �÷��̾�
    public static Player player_1 = null;
    public static Player player_2 = null;
    public static Player player_3 = null;
    public static Player player_4 = null;

    // �ʱ�ȭ
    public static void Clear()
    {
        //system = new PlayerSystem()
        allPlayer.Clear();
        me = null;
        player_1 = null;
        player_2 = null;
        player_3 = null;
        player_4 = null;
    }

    /// <summary>
    /// Ư�� ��ҿ� ��ġ�� �÷��̾� ����
    /// </summary>
    /// <param name="blockIndex"></param>
    /// <returns></returns>
    public List<Player> LocatedPlayer(int blockIndex)
    {
        List<Player> result = new List<Player>();

        for (int i = 0; i > allPlayer.Count; i++)
            if (allPlayer[i].movement.location == blockIndex)
                result.Add(allPlayer[i]);

        return result;
    }

    /// <summary>
    /// Ư�� �÷��̾��� �÷��̾� �ε��� ��ȯ
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    public static int Index(Player current)
    {
        for (int i = 0; i < allPlayer.Count; i++)
            if (allPlayer[i] == current)
                return i;

        return -1;
    }



    // �ٸ� �÷��̾�
    public List<Player> otherPlayers = new List<Player>();



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
    int _location = -1;
    public int location { get { return _location; } set { int i = BlockManager.script.indexLoop(value,0); dice.valueRecord += i - location; _location = i; } }
    //public CharacterMover movementMirror = new CharacterMover();
    //public CharacterMover movement { get { if (avatar == null) return null; else return avatar.GetComponent<CharacterMover>(); } }
    public CharacterMover movement = null;

    // AI ���� ��ũ��Ʈ
    public AIWorker ai { get { if (avatar == null) return null; else return avatar.GetComponent<AIWorker>(); } }




    // �ֻ���
    public Dice dice = new Dice();

    // �÷��̾� �ڿ�
    //public GameResource life = new GameResource(5, 10, -1);
    //public GameResource coin = new GameResource(0, 999, 0);
    public GameResource life = new GameResource(5, 10, -1);
    public GameResource coin = new GameResource(110, 999, 0);

    // ������ ����
    public List<ItemSlot> inventory = new List<ItemSlot>();
    public int inventoryCount { get { int c = 0; for (int i = 0; i < inventory.Count; i++) { if (!inventory[i].isEmpty) c++; } return c; } }
    public static int inventoryMax = 3;

    // �ൿ �Ұ��� ����
    public bool isDead = false;
    public bool isStun { get { return (stunCount > 0); } }
    public int stunCount = 0;

    // ���� �Ӽ�
    public Battle battle = new Battle(1f, 0f);




       

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
        
        Debug.Log("�÷��̾� ������ :: ĳ���� ��ȣ = " + characterIndex);
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

        // �̵����� �¾�
        movement = avatar.GetComponent<CharacterMover>();
        movement.owner = this;
        //if (movementMirror.location != -1)
        //    movement.CopyByMirror(movementMirror);
        //movementMirror = movement;
        if (location != -1)
            movement.location = location;

        // AI ������ ����
        ai.SetUp(this);

        Debug.Log("ĳ���� ���� :: " + avatar.name);
    }


    public void LoadFace()
    {
        //// ������ �ε�
        //Debug.Log(@"Data/Character/Face/Face" + character.index.ToString("D4"));
        //Sprite temp = Resources.Load<Sprite>(@"Data/Character/Face/Face" + character.index.ToString("D4"));

        //// �̹��� ��ȿ �˻�
        //if (temp == null)
        //{
        //    // �⺻ ������ ��ü ó��
        //    Debug.Log(@"UI/playerInfo/player");
        //    temp = Resources.Load<Sprite>(@"UI/playerInfo/player");
        //}

        //// ���� ���� ó��
        //if (temp == null)
        //    Debug.Log("�ε� ���� :: UI/playerInfo/player");
        //// ������ ����
        //else
        //    face = temp;

        face = character.GetIcon();
    }


    public void AddItem(ItemSlot itemSlot, int count)
    {
        AddItem(itemSlot.item, count);
    }
    public void AddItem(Item _item, int count)
    {
        // ���� �������� ���
        if(_item.index == 1)
        {
            coin.Add(count);
            return;
        }

        // �ܿ� ���� ���� �� ���� === �ð� ���� �ȴٸ� �Ұ��� ������ �����Ͽ� ������ �ٲܰ�
        if (inventoryCount >= Player.inventoryMax)
            return;

        for (int i = 0; i < inventory.Count; i++)
        {
            // ��ĭ�ϰ�� �ְ� ����
            if (inventory[i].isEmpty)
            {
                inventory[i].item = _item;
                inventory[i].count = count;
                break;
            }
        }
    }

    public void RemoveItem(ItemSlot currentSlot)
    {
        // �κ��丮 ��ȸ üũ
        for (int i = 0; i < inventory.Count; i++)
        {
            // ��ġ�ϴ� ���� �˻�
            if (inventory[i] == currentSlot)
                inventory[i].Clear();
        }

        // �κ��丮 ������
        SortInventory();
    }

    public void SortInventory()
    {
        // �������� ������ ��� ���� ��ȸ
        for (int i = 0; i < inventory.Count - 1; i++)
        {
            // �� ���� �˻�
            if (inventory[i].isEmpty)
            {
                // ��ܿ��� ����
                for (int j = i + 1; j < inventory.Count; j++)
                {
                    // ��ܿ� ���Կ� ������ ���� ���
                    if (!inventory[j].isEmpty)
                    {
                        // ����
                        inventory[i].CopyByMirror(inventory[j]);

                        // ��ܿ� ���� ����
                        inventory[j].Clear();
                    }
                }
            }
        }
    }


    public void MirrorLoaction()
    {
         _location = movement.location;
    }


    /// <summary>
    /// ����
    /// </summary>
    /// <param name="target">���� ���</param>
    public void Attack(Player target)
    {
        // ���� �ִϸ��̼�
        // �̱���=================

        // ������ ��û
        target.Hit(this, battle.atk.value);
    }

    /// <summary>
    /// �ǰ�
    /// </summary>
    /// <param name="Attacker">������</param>
    /// <param name="rawDamage">�̰��� ������</param>
    public void Hit(Player Attacker, float rawDamage)
    {
        // �ǰ� �ִϸ��̼�
        // �̱���=================

        // ������ ���
        float finalDamage = battle.Damage(rawDamage);

        // ü�� �ݿ�
        life.subtract((int)finalDamage);
    }

    /// <summary>
    /// ��Ȱ
    /// </summary>
    public void Resurrect()
    {
        // �ʱ�ȭ
        stunCount = 0;
        isDead = false;

        // ������ ����
        life.Add(5);

        // UI ����
        infoUI.dead.gameObject.SetActive(false);
    }

}

