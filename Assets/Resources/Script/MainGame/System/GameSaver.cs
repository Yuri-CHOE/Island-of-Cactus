using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class GameSaver
{
    // 세이브 파일 폴더명
    static string saveFloder = "Save";

    // 세이브 파일 파일명
    static string fileName { get { return GameData.gameMode.ToString(); } }

    // 세이브 파일 구분 문자
    static char codeEnder = '#';
    static char codeChapter = '$';
    static char codeLine = '|';
    static char codeData = ',';

    // 챕터별 세이브 파일 코드
    static string[] scInfo = null;
    static List<string[]> scPlayers = new List<string[]>();
    static string[] scItem = null;
    static string[] scEvent = null;
    static string[] scTurn = null;




    public static void GameSave()
    {
        // 게임 모드 누락시 저장 중단 - 필수 :: 게임모드로 파일명 설정
        if (GameData.gameMode == GameMode.Mode.None)
        {
            Debug.LogError("game save :: 저장 실패 " + "게임 모드 -> " + GameData.gameMode);
            return;
        }
        // 순서 주사위 미완료시 중단 - 필수 :: 최초 저장 시점
        if (GameData.gameFlow <= GameMaster.Flow.Ordering)
        {
            Debug.LogError("game save :: 저장 실패 " + "게임 플로우 -> " + GameData.gameMode);
            return;
        }

        // 세이브 파일 코드
        StringBuilder code = SaveCode();

        // 저장
        CSVReader.SaveNew(saveFloder, fileName, true, true, code.ToString());
    }


    /// <summary>
    /// 세이브 코드 반환
    /// </summary>
    static StringBuilder SaveCode()
    {
        StringBuilder sb = new StringBuilder();

        // 게임 정보
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

        // 플레이어 정보
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
                    // 유효한 아이템
                    sb
                        .Append(temp.inventory[j].item.index)
                        .Append(codeData)
                        .Append(temp.inventory[j].count)
                        ;
                }
                else
                {
                    // 아이템 없음
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


        // 아이템 오브젝트 배치
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


        // 이벤트 오브젝트 배치
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


        // 상황 설정
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


        // 종료 문자
        sb.Append('#');

        return sb;
    }



    public static void CodeLoad()
    {
        // 파일명
        string fName = GameData.gameMode.ToString();

        // 파일 읽기
        CSVReader loader = new CSVReader(saveFloder, fName, true, false, codeChapter, codeEnder);

        List<string> code = loader.table[0];

        // 챕터별 세이브 파일 코드
        scInfo = code[0].Split(codeData);

        string[] pCode = code[1].Split(codeLine);
        for (int i = 0; i < pCode.Length; i++)
            scPlayers.Add(pCode[i].Split(codeData));

        scItem = code[2].Split(codeData);

        scEvent = code[3].Split(codeData);

        scTurn = code[4].Split(codeData);
    }

    /// <summary>
    /// 게임 정보 셋팅
    /// </summary>
    public static void SetGameInfo()
    {
        // 지역 설정
        GameRule.area = int.Parse(scInfo[0]);

        // 구역 설정
        GameRule.section = int.Parse(scInfo[1]);

        // 사이클 설정 - 현재
        GameData.cycle.now = int.Parse(scInfo[2]);

        // 사이클 설정 - 목표
        GameData.cycle.goal = int.Parse(scInfo[3]);

        // 플레이 인원수
        GameRule.playerCount = int.Parse(scInfo[4]);

    }

    /// <summary>
    /// 플레이어 셋팅
    /// </summary>
    public static void SetPlayer(List<Player> playerList)
    {


    }

    public static void GameLoad()
    {
        CodeLoad();

        // 게임 정보
        SetGameInfo();



        /*
        // 플레이어 정보
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
                    // 유효한 아이템
                    sb
                        .Append(temp.inventory[j].item.index)
                        .Append(codeData)
                        .Append(temp.inventory[j].count)
                        ;
                }
                else
                {
                    // 아이템 없음
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


        // 아이템 오브젝트 배치
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


        // 이벤트 오브젝트 배치
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


        // 상황 설정
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
         
        // 확장자 생략 처리
        string fName;
        if (filename.Contains(extension))
            fName = filename;
        else
            fName = filename + extension;

        // 월드명 등록
        GameData.SetWorldFileName(fName);
        Debug.LogWarning("맵 파일 :: " + fName);

        // 파일 읽기
        CSVReader worldFileReader = new CSVReader(subRoot, fName, false, true, tableSplit, endSplit);

        worldFile = worldFileReader.table[0];

         */
    }
}
