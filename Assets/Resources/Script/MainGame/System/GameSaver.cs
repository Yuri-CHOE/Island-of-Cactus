using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class GameSaver
{
    // ���̺� ���� ������
    static string saveFloder = "Save";

    // ���̺� ���� ���ϸ�
    static string fileName { get { return GameData.gameMode.ToString(); } }

    // ���̺� ���� ���� ����
    static char codeEnder = '#';
    static char codeChapter = '$';
    static char codeLine = '|';
    static char codeData = ',';

    // é�ͺ� ���̺� ���� �ڵ�
    static string[] scInfo = null;
    static List<string[]> scPlayers = new List<string[]>();
    static string[] scItem = null;
    static string[] scEvent = null;
    static string[] scTurn = null;




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
        CSVReader.SaveNew(saveFloder, fileName, true, true, code.ToString());
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
            .Append(GameData.cycle.now)
            .Append(codeData)
            .Append(GameData.cycle.goal)
            .Append(codeData)
            .Append(GameData.player.allPlayer.Count)

            .Append(codeChapter)
            ;

        // �÷��̾� ����
        for (int i = 0; i < GameData.player.allPlayer.Count; i++)
        {
            Player temp = GameData.player.allPlayer[i];

            sb
                .Append(i)
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

            if (GameData.player.allPlayer.Count > 1 && i != GameData.player.allPlayer.Count - 1)
                sb.Append(codeLine);
        }
        sb.Append(codeChapter);


        // ������ ������Ʈ ��ġ
        for (int i = 0; i < ItemManager.itemObjectList.Count; i++)
        {
            sb
                .Append(ItemManager.itemObjectList[i].location)
                .Append(codeData)
                .Append(ItemManager.itemObjectList[i].item.index)
                .Append(codeData)
                .Append(ItemManager.itemObjectList[i].count)
                ;

            if (i > 0 && i != ItemManager.itemObjectList.Count - 1)
                sb.Append(codeLine);
        }
        sb.Append(codeChapter);


        // �̺�Ʈ ������Ʈ ��ġ
        for (int i = 0; i < EventManager.eventObjectList.Count; i++)
        {
            sb
                .Append(EventManager.eventObjectList[i].location)
                .Append(codeData)
                .Append(EventManager.eventObjectList[i].iocEvent.index)
                .Append(codeData)
                .Append(EventManager.eventObjectList[i].count)
                ;

            if (i > 0 && i != EventManager.eventObjectList.Count - 1)
                sb.Append(codeLine);
        }
        sb.Append(codeChapter);


        // ��Ȳ ����
        sb
            .Append(GameData.turn.NowIndex())
            .Append(codeData)
            .Append(GameData.turn.now.dice.count)
            .Append(codeData)
            .Append(GameData.turn.now.dice.valueTotal)
            .Append(codeData)
            .Append(GameData.turn.now.dice.valueRecord)

            //.Append(codeChapter)
            ;


        // ���� ����
        sb.Append('#');

        return sb;
    }



    public static void CodeLoad()
    {
        // ���ϸ�
        string fName = GameData.gameMode.ToString();

        // ���� �б�
        CSVReader loader = new CSVReader(saveFloder, fName, true, false, codeChapter, codeEnder);

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
    public static void SetGameInfo()
    {
        // ���� ����
        GameRule.area = int.Parse(scInfo[0]);

        // ���� ����
        GameRule.section = int.Parse(scInfo[1]);

        // ����Ŭ ���� - ����
        GameData.cycle.now = int.Parse(scInfo[2]);

        // ����Ŭ ���� - ��ǥ
        GameData.cycle.goal = int.Parse(scInfo[3]);

        // �÷��� �ο���
        GameRule.playerCount = int.Parse(scInfo[4]);

    }

    /// <summary>
    /// �÷��̾� ����
    /// </summary>
    public static void SetPlayer(List<Player> playerList)
    {


    }

    public static void GameLoad()
    {
        CodeLoad();

        // ���� ����
        SetGameInfo();



        /*
        // �÷��̾� ����
        for (int i = 0; i < GameData.player.allPlayer.Count; i++)
        {
            Player temp = GameData.player.allPlayer[i];

            sb
                .Append(i)
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
                ;

            for (int j = 0; j < Player.inventoryMax; j++)
            {
                if (temp.inventory[j].count > 0)
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

            if (GameData.player.allPlayer.Count > 1 && i != GameData.player.allPlayer.Count - 1)
                sb.Append(codeLine);
        }
        sb.Append(codeChapter);


        // ������ ������Ʈ ��ġ
        for (int i = 0; i < ItemManager.itemObjectList.Count; i++)
        {
            sb
                .Append(ItemManager.itemObjectList[i].location)
                .Append(codeData)
                .Append(ItemManager.itemObjectList[i].item.index)
                .Append(codeData)
                .Append(ItemManager.itemObjectList[i].count)
                ;

            if (i > 0 && i != ItemManager.itemObjectList.Count - 1)
                sb.Append(codeLine);
        }
        sb.Append(codeChapter);


        // �̺�Ʈ ������Ʈ ��ġ
        for (int i = 0; i < EventManager.eventObjectList.Count; i++)
        {
            sb
                .Append(EventManager.eventObjectList[i].location)
                .Append(codeData)
                .Append(EventManager.eventObjectList[i].iocEvent.index)
                .Append(codeData)
                .Append(EventManager.eventObjectList[i].count)
                ;

            if (i > 0 && i != EventManager.eventObjectList.Count - 1)
                sb.Append(codeLine);
        }
        sb.Append(codeChapter);


        // ��Ȳ ����
        sb
            .Append(GameData.turn.NowIndex())
            .Append(codeData)
            .Append(GameData.turn.now.dice.count)
            .Append(codeData)
            .Append(GameData.turn.now.dice.valueTotal)
            .Append(codeData)
            .Append(GameData.turn.now.dice.valueRecord)

            //.Append(codeChapter)
            ;




        /*
         
        // Ȯ���� ���� ó��
        string fName;
        if (filename.Contains(extension))
            fName = filename;
        else
            fName = filename + extension;

        // ����� ���
        GameData.SetWorldFileName(fName);
        Debug.LogWarning("�� ���� :: " + fName);

        // ���� �б�
        CSVReader worldFileReader = new CSVReader(subRoot, fName, false, true, tableSplit, endSplit);

        worldFile = worldFileReader.table[0];

         */
    }
}
