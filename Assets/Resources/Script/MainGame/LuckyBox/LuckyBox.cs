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
        StealItem,
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
    
    // ��Ű�ڽ� ����
    string _info = null;
    public string info { get { return _info; } }

    // ȿ��
    public IocEffect effect = new IocEffect();

    // ��Ű�ڽ� ��� (�����)
    int _rare = -1;
    public int rare { get { return _rare; } }



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
        if (strList.Count != 11)
            return;
        if (loaclList.Count != 3)
            return;

        // ���̺� �о����

        // �ε���
        _index = int.Parse(strList[0]);

        // ī�װ�
        _type = (Type)int.Parse(strList[1]);

        // �̸�
        _name = loaclList[1];

        // ȿ��
        effect.Set(
        (IocEffect.Expiration)(int.Parse(strList[3])),
            int.Parse(strList[4]),
            (IocEffect.Target)(int.Parse(strList[5])),
            int.Parse(strList[6]),
            (IocEffect.What)(int.Parse(strList[7])),
            int.Parse(strList[8])
            );
        
        // ����
        _info = loaclList[2].Replace("\\n", "\n").Replace("value", strList[8]);

        // ���
        _rare = int.Parse(strList[10]);       
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
    /// <param name="targetPlayer_Or_null">�۵���</param>
    /// <returns></returns>
    public IEnumerator Effect(Player targetPlayer_Or_null)
    {
        // Ÿ�� ����Ʈ
        List<Player> pl = IocEffect.TargetFiltering(effect.target, targetPlayer_Or_null);

        // ���� ȿ��
        yield return effect.GeneralEffect(targetPlayer_Or_null, pl);

        // ���� Ư�� ȿ��
        yield return EachEffect(this, targetPlayer_Or_null, pl);
    }


    /// <summary>
    /// ��Ű�ڽ� ȿ��
    /// </summary>
    /// <param name="__luckyBox"></param>
    /// <param name="user">�۵���</param>
    /// <param name="filteredTarget">ȿ�� ���</param>
    /// <returns></returns>
    public static IEnumerator EachEffect(LuckyBox __luckyBox, Player user, List<Player> filteredTarget)
    {
        switch (__luckyBox.index)
        {
            // �ʿ�� �߰�

            case 0:
                // 0���� ����
                break;

            case 16:
                // ������ ������
                {
                    // �߸��� �ο��� ����
                    if (filteredTarget.Count != 0)
                        break;

                    // ����
                    // �̱���=============

                    ItemSlot slot = null;
                    for (int i = 0; i < filteredTarget.Count; i++)
                    {
                        // ������ ������ �ߴ�
                        if (filteredTarget[i].inventoryCount <= 0)
                            break;

                        // ������ ������ ����
                        slot = filteredTarget[i].inventory[0];

                        // ��Ż
                        filteredTarget[i].RemoveItem(slot);
                    }
                }

                break;

            case 19:
                // �߸� ���� ����
                {

                    // ���� ��ġ
                    int blockIndex = 0;

                    // �÷��̾� ������ ��ġ Ȯ��
                    if (user != null)
                        blockIndex = user.location;



                    // ������ �����
                    Player world = Player.system.World;

                    // ���
                    GameObject backAvatar = world.avatar;
                    Transform backAvatarBody = world.avatarBody;
                    //CharacterMover backMovement = avatar.movement;

                    // ������ ����
                    // �̱���==================== ������ �����Ұ�
                    GameObject obj = null;

                    // ���� �� ���
                    world.avatar = GameObject.Instantiate(
                        obj,
                        BlockManager.script.startBlock
                        ) as GameObject;
                    world.avatarBody = world.avatar.transform.Find("BodyObject");

                    // �̵����� �¾�
                    CharacterMover movement = world.avatar.GetComponent<CharacterMover>();
                    movement.owner = world;

                    // �����
                    Transform monster = world.avatar.transform;

                    // �ʱ���ġ ����
                    if (user.location != -1)
                        movement.location = user.location;

                    // ��ġ ����
                    monster.position = user.avatar.transform.position;



                    // ī�޶� ����
                    GameData.worldManager.cameraManager.CamMoveTo(monster, CameraManager.CamAngle.Top);

                    // ���� ��ȹ
                    movement.PlanMoveBy(__luckyBox.effect.where);


                    // �ܿ� �׼� ���� or �׼� ������ ���
                    while (movement.actionsQueue.Count > 0    ||    movement.actNow.type == Action.ActionType.None)
                    {
                        movement.ActionCall();
                    }




                    // ����
                    world.avatar = backAvatar;
                    world.avatarBody = backAvatarBody;

                    // �Ҵ� ����
                    movement = null;

                    // ���� ����
                    Transform.Destroy(monster);

                    // ī�޶� Ż��
                    GameData.worldManager.cameraManager.CamMoveTo(Turn.now.avatar.transform, CameraManager.CamAngle.Top);
                    GameData.worldManager.cameraManager.CamFree();

                }
                break;

            case 20:
                // ��Ű ������
                {
                    // ��Ű�ڽ� ������̺�
                    DropTable dropTable = new DropTable();

                    // ������̺� ����
                    dropTable.rare = new List<int>();
                    for (int i = 1; i < LuckyBox.table.Count; i++)
                    {
                        dropTable.rare.Add(LuckyBox.table[i].rare);
                        Debug.Log("��� ���̺� :: �߰��� -> " + LuckyBox.table[i].rare);
                    }
                    Debug.LogWarning("��� ���̺� :: ��� �ѷ� ->" + dropTable.rare.Count);

                    // ��� ���̺� �۵� �� ������ �ε��� Ȯ��
                    int select = 1 + dropTable.Drop();
                    Debug.LogWarning("��Ű ������ :: ���õ� -> " + Item.table[select].name);

                    // ����
                    user.AddItem(Item.table[select], 1);
                }
                break;

            case 21:
                // ������ ��
                {
                    // ���� �ִ� �÷��̾�
                    List<Player> best = new List<Player>();
                    Player current = null;

                    for (int i = 0; i < user.otherPlayers.Count; i++)
                    {
                        current = user.otherPlayers[i];

                        // �񱳴�� ���� ��� ��� ����
                        if (best == null)
                            best.Add(current);

                        // ���� �� ���� ��� ����
                        else if (current.coin.Value > best[0].coin.Value)
                        {
                            best.Clear();
                            best.Add(current);
                        }

                        // ���� ���
                        else if (current.coin.Value == best[0].coin.Value)
                        {
                            best.Add(current);
                        }
                    }

                    // ���� ��Ż
                    for (int i = 0; i < best.Count; i++)
                        best[i].coin.subtract(__luckyBox.effect.value / best.Count);

                    // ���� ����
                    user.coin.Add(__luckyBox.effect.value);
                }
                break;

            case 22:
                // ���� �ִ� �÷��̾�
                {
                    List<Player> least = new List<Player>();
                    Player current = null;

                    for (int i = 0; i < user.otherPlayers.Count; i++)
                    {
                        current = user.otherPlayers[i];

                        // �񱳴�� ���� ��� ��� ����
                        if (least == null)
                            least.Add(current);

                        // ���� �� ���� ��� ����
                        else if (current.coin.Value < least[0].coin.Value)
                        {
                            least.Clear();
                            least.Add(current);
                        }

                        // ���� ���
                        else if (current.coin.Value == least[0].coin.Value)
                        {
                            least.Add(current);
                        }
                    }

                    // ���� ��Ż
                    user.coin.subtract(__luckyBox.effect.value);

                    // ���� ����
                    for (int i = 0; i < least.Count; i++)
                        least[i].coin.subtract(__luckyBox.effect.value / least.Count);
                }
                break;


        }

        yield return null;
    }
}
