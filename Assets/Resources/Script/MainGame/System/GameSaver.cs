using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class GameSaver
{
    // ���̺� ���� ������
    static string saveFloder = "Save";
    static string extension = ".iocs";

    // ���̺� ���� ���ϸ�
    static string fileName { get { return GameData.gameMode.ToString(); } }

    // ���̺� ���� ���� ����
    static char codeEnder = '#';
    static char codeChapter = '$';
    static char codeLine = '|';
    static char codeData = ',';

    // é�ͺ� ���̺� ���� �ڵ�
    public static string[] scInfo = null;
    public static List<string[]> scPlayers = new List<string[]>();
    public static string[] scItem = null;
    public static string[] scEvent = null;
    public static string[] scTurn = null;


    // �ε� ȣ�� ����
    public static bool useLoad = false;



    public static void GameSave()
    {
        // ���� ��� ������ ���� �ߴ� - �ʼ� :: ���Ӹ��� ���ϸ� ����
        if (GameData.gameMode == GameMode.Mode.None)
        {
            Debug.LogError("game save :: ���� ���� " + "���� ��� -> " + GameData.gameMode);
            return;
        }
        // ���� �ֻ��� �̿Ϸ�� �ߴ� - �ʼ� :: ���� ���� ����
        if (GameData.gameFlow <= GameMaster.Flow.Ordering)
        {
            Debug.LogError("game save :: ���� ���� " + "���� �÷ο� -> " + GameData.gameMode);
            return;
        }

        // ���̺� ���� �ڵ�
        StringBuilder code = SaveCode();

        // ����
        CSVReader.SaveNew(saveFloder, fileName + extension, true, true, code.ToString());
    }


    /// <summary>
    /// ���̺� �ڵ� ��ȯ
    /// </summary>
    static StringBuilder SaveCode()
    {
        StringBuilder sb = new StringBuilder();

        // ���� ����
        sb
            .Append(GameRule.area)
            .Append(codeData)
            .Append(GameRule.section)
            .Append(codeData)
            .Append(Cycle.now)
            .Append(codeData)
            .Append(Cycle.goal)
            .Append(codeData)
            .Append(Player.allPlayer.Count)

            .Append(codeChapter)
            ;

        // �÷��̾� ����
        for (int i = 0; i < Player.allPlayer.Count; i++)
        {
            Player temp = Player.allPlayer[i];

            sb
                .Append(Turn.Index(temp))
                .Append(codeData)
                .Append((int)(temp.type))
                .Append(codeData)
                .Append(temp.character.index)
                .Append(codeData)
                .Append(temp.isAutoPlay)
                .Append(codeData)
                .Append(temp.name)
                .Append(codeData)

                .Append(temp.life.Value)
                .Append(codeData)
                .Append(temp.coin.Value)
                .Append(codeData)

                .Append(temp.dice.count)
                .Append(codeData)
                .Append(temp.dice.valueTotal)
                .Append(codeData)
                .Append(temp.dice.valueRecord)
                .Append(codeData)
                ;

            for (int j = 0; j < Player.inventoryMax; j++)
            {
                if(temp.inventory[j].count > 0)
                {
                    // ��ȿ�� ������
                    sb
                        .Append(temp.inventory[j].item.index)
                        .Append(codeData)
                        .Append(temp.inventory[j].count)
                        ;
                }
                else
                {
                    // ������ ����
                    sb
                        .Append(-1)
                        .Append(codeData)
                        .Append(0)
                        ;
                }


                if (j < Player.inventoryMax - 1)
                    sb.Append(codeData);
            }

            if (Player.allPlayer.Count > 1 && i != Player.allPlayer.Count - 1)
                sb.Append(codeLine);
        }
        sb.Append(codeChapter);


        // ������ ������Ʈ ��ġ
        for (int i = 0; i < ItemManager.itemObjectList.Count; i++)
        {
            DynamicItem obj = ItemManager.itemObjectList[i];

            sb
                .Append(obj.location)
                .Append(codeData)
                .Append(obj.item.index)
                .Append(codeData)
                .Append(obj.count)
                ;

            if (i > 0 && i != ItemManager.itemObjectList.Count - 1)
                sb.Append(codeLine);
        }
        sb.Append(codeChapter);


        // �̺�Ʈ ������Ʈ ��ġ
        for (int i = 0; i < EventManager.eventObjectList.Count; i++)
        {
            DynamicEvent obj = EventManager.eventObjectList[i];

            sb
                .Append(obj.location)
                .Append(codeData)
                .Append(obj.iocEvent.index)
                .Append(codeData)
                .Append(obj.count)
                .Append(codeData)
                .Append(Player.Index(obj.creator))
                ;

            if (i > 0 && i != EventManager.eventObjectList.Count - 1)
                sb.Append(codeLine);
        }
        sb.Append(codeChapter);


        // ��Ȳ ����
        sb
            .Append(Player.Index(Turn.now))
            .Append(codeData)
            .Append((int)GameData.gameFlow)

            //.Append(codeChapter)
            ;


        // ���� ����
        sb.Append('#');

        return sb;
    }



    public static void Clear()
    {
        scInfo = null;
        scPlayers.Clear();
        scItem = null;
        scEvent = null;
        scTurn = null;

        useLoad = false;
    }



    public static void CodeLoad()
    {
        // ���ϸ�
        string fName = GameData.gameMode.ToString() + extension;

        //// ���� üũ
        //if(!CSVReader.CheckFile(CSVReader.copyPath + '/' + saveFloder, fName))
        //{
        //    Debug.LogWarning("error :: ���̺� ���� ����");
        //    return;
        //}

        // ���� �б�
        CSVReader loader = new CSVReader(saveFloder, fName, true, false, codeChapter, codeEnder);

        // ���� üũ
        if (loader.table.Count == 0)
        {
            Debug.LogWarning("miss :: ���̺� ���� ����");
            return;
        }

        List<string> code = loader.table[0];

        // é�ͺ� ���̺� ���� �ڵ�
        scInfo = code[0].Split(codeData);

        string[] pCode = code[1].Split(codeLine);
        for (int i = 0; i < pCode.Length; i++)
            scPlayers.Add(pCode[i].Split(codeData));

        scItem = code[2].Split(codeData);

        scEvent = code[3].Split(codeData);

        scTurn = code[4].Split(codeData);
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    public static void LoadGameInfo()
    {
        if (scInfo == null)
        {
            Debug.LogError("������ �ε� :: ���� -> scInfo is null");
            Debug.Break();
            return;
        }

        // ���� ����
        GameRule.area = int.Parse(scInfo[0]);

        // ���� ����
        GameRule.section = int.Parse(scInfo[1]);

        // ����Ŭ ���� - ����
        Cycle.now = int.Parse(scInfo[2]);

        // ����Ŭ ���� - ��ǥ
        GameRule.cycleMax = int.Parse(scInfo[3]);
        Cycle.goal = GameRule.cycleMax;

        // �÷��� �ο���
        GameRule.playerCount = int.Parse(scInfo[4]);

    }

    /// <summary>
    /// �÷��̾� ����
    /// </summary>
    public static void LoadPlayer()
    {
        if (scPlayers.Count == 0)
        {
            Debug.LogError("������ �ε� :: ���� -> scPlayers is empty");
            Debug.Break();
            return;
        }
        if (Player.allPlayer.Count == 0)
        {
            Debug.LogError("fatal error :: Player.allPlayer�� �����Ǳ� �� GameSaver.LoadPlayer() ����");
            Debug.Break();
            return;
        }

        for (int i = 0; i < scPlayers.Count; i++)
        {
            Player current = Player.allPlayer[i];
            string[] temp = scPlayers[i];

            // �� �ε���
            current.dice.SetValue(100 - int.Parse(temp[0]));
            current.dice.isRolled = true;

            // �÷��̾� Ÿ��, ĳ���� �ε���, �����÷���, �÷��̾� �̸�
            current.SetPlayer(
                (Player.Type)int.Parse(temp[1]),
                int.Parse(temp[2]),
                bool.Parse(temp[3]),
                temp[4]
                );

            // ������
            current.life.Set(int.Parse(temp[5]));

            // ����
            current.coin.Set(int.Parse(temp[6]));

            // �ֻ��� ����
            current.dice.count = int.Parse(temp[7]);

            // �ֻ��� �ջ갪 (�ܿ� �̵���)
            current.dice.SetValueTotal(int.Parse(temp[8]));

            // �ֻ��� ��ϰ�
            current.dice.valueRecord = int.Parse(temp[9]);

            //current.inventory = new List<ItemSlot>();
            //for (int j = 0; j < Player.inventoryMax; j++)
            //{
            //    int itemIndex = int.Parse(temp[10 + j * 2]);
            //    if (itemIndex > 0)
            //    {
            //        // �ӽ� ����
            //        //ItemSlot slot = new ItemSlot();

            //        Item tempItem = null;
            //        int tempCount = 0;

            //        // ������ ���� �ľ�
            //        if (itemIndex > 0)
            //        {
            //            // ������ �ε���
            //            tempItem = Item.table[itemIndex];

            //            // ������ ����
            //            tempCount = int.Parse(temp[11 + j * 2]);
            //        }
            //        else
            //            tempItem = Item.empty;

            //        // �ӽ� ����
            //        ItemSlot slot = GameMaster.script.itemManager.CreateItemSlot(tempItem, tempCount);

            //        // ���
            //        current.inventory.Add(slot);

            //        // �ӽ� ������Ʈ ����
            //        GameObject.Destroy(slot.gameObject);
            //    }

            //}

            // ������ ȿ��
            // �̱���=========================================
        }
    }

    /// <summary>
    /// �÷��̾� ����
    /// </summary>
    public static void LoadPlayerInventory()
    {
        if (scPlayers.Count == 0)
        {
            Debug.LogError("������ �ε� :: ���� -> scPlayers is empty");
            Debug.Break();
            return;
        }
        if (Player.allPlayer.Count == 0)
        {
            Debug.LogError("fatal error :: Player.allPlayer�� �����Ǳ� �� GameSaver.LoadPlayer() ����");
            Debug.Break();
            return;
        }

        for (int i = 0; i < scPlayers.Count; i++)
        {
            Player current = Player.allPlayer[i];
            string[] temp = scPlayers[i];

            if (current.infoUI == null)
            {
                Debug.LogError("fatal error :: Player.infoUI�� �����Ǳ� �� GameSaver.LoadPlayerInventory() ����");
                Debug.Break();
                return;
            }

            for (int j = 0; j < Player.inventoryMax; j++)
            {
                int tempIndex = int.Parse(temp[10 + j * 2]);

                // ���� ������ ����
                if (tempIndex < 1)
                    continue;

                // ������ �ε���
                Item tempItem = Item.table[tempIndex];

                // ������ ����
                int tempCount = int.Parse(temp[11 + j * 2]);

                current.AddItem(tempItem, tempCount);
            }

            // ������ ȿ��
            // �̱���=========================================
        }
    }

    /// <summary>
    /// ������ ������Ʈ ����
    /// </summary>
    public static void LoadItemObject()
    {
        if (scItem == null)
        {
            Debug.LogError("������ �ε� :: ���� -> scItem is null");
            Debug.Break();
            return;
        }

        for (int i = 0; i < scItem.Length; i++)
        {
            // Out of Range ����
            if (2 + i * 3 >= scItem.Length)
                break;

            // ������ �ε���
            int _loc = int.Parse(scItem[0 + i * 3]);

            // ������ �ε���
            int _index = int.Parse(scItem[1 + i * 3]);

            // ������ ����
            int _count = int.Parse(scItem[2 + i * 3]);


            // ������ ����
            GameData.itemManager.CreateItemObject(_loc, _index, _count);
        }
    }

    /// <summary>
    /// �̺�Ʈ ������Ʈ ����
    /// </summary>
    public static void LoadEventObject()
    {
        if (scEvent == null)
        {
            Debug.LogError("������ �ε� :: ���� -> scEvent is null");
            Debug.Break();
            return;
        }
        if (Player.allPlayer.Count == 0)
        {
            Debug.LogError("fatal error :: Player.allPlayer�� �����Ǳ� �� GameSaver.LoadEventObject() ����");
            Debug.Break();
            return;
        }

        for (int i = 0; i < scEvent.Length; i++)
        {
            // Out of Range ����
            if (2 + i * 4 >= scEvent.Length)
                break;

            // �̺�Ʈ �ε���
            int _loc = int.Parse(scEvent[0 + i * 4]);

            // �̺�Ʈ �ε���
            int _index = int.Parse(scEvent[1 + i * 4]);

            // �̺�Ʈ ����
            int _count = int.Parse(scEvent[2 + i * 4]);

            // �̺�Ʈ ��ġ��
            int _turnIndex = int.Parse(scEvent[2 + i * 4]);


            // �̺�Ʈ ����
            GameData.eventManager.CreateEventObject(_loc, _index, _count, Player.allPlayer[_turnIndex]);
        }
    }

    /// <summary>
    /// ��Ȳ ����
    /// </summary>
    public static void LoadTurn()
    {
        if (scTurn == null)
        {
            Debug.LogError("������ �ε� :: ���� -> scTurn is null");
            Debug.Break();
            return;
        }

        // ���� �� ����
        Debug.LogError(Player.allPlayer[int.Parse(scTurn[0])].name);
        Turn.Skip(Player.allPlayer[int.Parse(scTurn[0])]);

        // �÷ο� ����
        GameMaster.flowCopy = (GameMaster.Flow)int.Parse(scTurn[1]);
    }
}
