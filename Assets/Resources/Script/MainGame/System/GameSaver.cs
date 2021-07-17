using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class GameSaver
{
    // 세이브 파일 폴더명
    static string saveFloder = "Save";
    static string extension = ".iocs";

    // 세이브 파일 파일명
    static string fileName { get { return GameData.gameMode.ToString(); } }

    // 세이브 파일 구분 문자
    static char codeEnder = '#';
    static char codeChapter = '$';
    static char codeLine = '|';
    static char codeData = ',';

    // 챕터별 세이브 파일 코드
    public static string[] scInfo = null;
    public static List<string[]> scPlayers = new List<string[]>();
    public static string[] scItem = null;
    public static string[] scEvent = null;
    public static string[] scTurn = null;


    // 로딩 호출 여부
    public static bool useLoad = false;



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
        CSVReader.SaveNew(saveFloder, fileName + extension, true, true, code.ToString());
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
            .Append(Cycle.now)
            .Append(codeData)
            .Append(Cycle.goal)
            .Append(codeData)
            .Append(Player.allPlayer.Count)

            .Append(codeChapter)
            ;

        // 플레이어 정보
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

            if (Player.allPlayer.Count > 1 && i != Player.allPlayer.Count - 1)
                sb.Append(codeLine);
        }
        sb.Append(codeChapter);


        // 아이템 오브젝트 배치
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


        // 이벤트 오브젝트 배치
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


        // 상황 설정
        sb
            .Append(Player.Index(Turn.now))
            .Append(codeData)
            .Append((int)GameData.gameFlow)

            //.Append(codeChapter)
            ;


        // 종료 문자
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
        // 파일명
        string fName = GameData.gameMode.ToString() + extension;

        //// 파일 체크
        //if(!CSVReader.CheckFile(CSVReader.copyPath + '/' + saveFloder, fName))
        //{
        //    Debug.LogWarning("error :: 세이브 파일 없음");
        //    return;
        //}

        // 파일 읽기
        CSVReader loader = new CSVReader(saveFloder, fName, true, false, codeChapter, codeEnder);

        // 누락 체크
        if (loader.table.Count == 0)
        {
            Debug.LogWarning("miss :: 세이브 파일 없음");
            return;
        }

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
    public static void LoadGameInfo()
    {
        if (scInfo == null)
        {
            Debug.LogError("데이터 로드 :: 실패 -> scInfo is null");
            Debug.Break();
            return;
        }

        // 지역 설정
        GameRule.area = int.Parse(scInfo[0]);

        // 구역 설정
        GameRule.section = int.Parse(scInfo[1]);

        // 사이클 설정 - 현재
        Cycle.now = int.Parse(scInfo[2]);

        // 사이클 설정 - 목표
        GameRule.cycleMax = int.Parse(scInfo[3]);
        Cycle.goal = GameRule.cycleMax;

        // 플레이 인원수
        GameRule.playerCount = int.Parse(scInfo[4]);

    }

    /// <summary>
    /// 플레이어 셋팅
    /// </summary>
    public static void LoadPlayer()
    {
        if (scPlayers.Count == 0)
        {
            Debug.LogError("데이터 로드 :: 실패 -> scPlayers is empty");
            Debug.Break();
            return;
        }
        if (Player.allPlayer.Count == 0)
        {
            Debug.LogError("fatal error :: Player.allPlayer가 구성되기 전 GameSaver.LoadPlayer() 수행");
            Debug.Break();
            return;
        }

        for (int i = 0; i < scPlayers.Count; i++)
        {
            Player current = Player.allPlayer[i];
            string[] temp = scPlayers[i];

            // 턴 인덱스
            current.dice.SetValue(100 - int.Parse(temp[0]));
            current.dice.isRolled = true;

            // 플레이어 타입, 캐릭터 인덱스, 오토플레이, 플레이어 이름
            current.SetPlayer(
                (Player.Type)int.Parse(temp[1]),
                int.Parse(temp[2]),
                bool.Parse(temp[3]),
                temp[4]
                );

            // 라이프
            current.life.Set(int.Parse(temp[5]));

            // 코인
            current.coin.Set(int.Parse(temp[6]));

            // 주사위 개수
            current.dice.count = int.Parse(temp[7]);

            // 주사위 합산값 (잔여 이동력)
            current.dice.SetValueTotal(int.Parse(temp[8]));

            // 주사위 기록값
            current.dice.valueRecord = int.Parse(temp[9]);

            //current.inventory = new List<ItemSlot>();
            //for (int j = 0; j < Player.inventoryMax; j++)
            //{
            //    int itemIndex = int.Parse(temp[10 + j * 2]);
            //    if (itemIndex > 0)
            //    {
            //        // 임시 생성
            //        //ItemSlot slot = new ItemSlot();

            //        Item tempItem = null;
            //        int tempCount = 0;

            //        // 아이템 종류 파악
            //        if (itemIndex > 0)
            //        {
            //            // 아이템 인덱스
            //            tempItem = Item.table[itemIndex];

            //            // 아이템 수량
            //            tempCount = int.Parse(temp[11 + j * 2]);
            //        }
            //        else
            //            tempItem = Item.empty;

            //        // 임시 생성
            //        ItemSlot slot = GameMaster.script.itemManager.CreateItemSlot(tempItem, tempCount);

            //        // 등록
            //        current.inventory.Add(slot);

            //        // 임시 오브젝트 제거
            //        GameObject.Destroy(slot.gameObject);
            //    }

            //}

            // 적용중 효과
            // 미구현=========================================
        }
    }

    /// <summary>
    /// 플레이어 셋팅
    /// </summary>
    public static void LoadPlayerInventory()
    {
        if (scPlayers.Count == 0)
        {
            Debug.LogError("데이터 로드 :: 실패 -> scPlayers is empty");
            Debug.Break();
            return;
        }
        if (Player.allPlayer.Count == 0)
        {
            Debug.LogError("fatal error :: Player.allPlayer가 구성되기 전 GameSaver.LoadPlayer() 수행");
            Debug.Break();
            return;
        }

        for (int i = 0; i < scPlayers.Count; i++)
        {
            Player current = Player.allPlayer[i];
            string[] temp = scPlayers[i];

            if (current.infoUI == null)
            {
                Debug.LogError("fatal error :: Player.infoUI가 지정되기 전 GameSaver.LoadPlayerInventory() 수행");
                Debug.Break();
                return;
            }

            for (int j = 0; j < Player.inventoryMax; j++)
            {
                int tempIndex = int.Parse(temp[10 + j * 2]);

                // 없는 아이템 생략
                if (tempIndex < 1)
                    continue;

                // 아이템 인덱스
                Item tempItem = Item.table[tempIndex];

                // 아이템 수량
                int tempCount = int.Parse(temp[11 + j * 2]);

                current.AddItem(tempItem, tempCount);
            }

            // 적용중 효과
            // 미구현=========================================
        }
    }

    /// <summary>
    /// 아이템 오브젝트 셋팅
    /// </summary>
    public static void LoadItemObject()
    {
        if (scItem == null)
        {
            Debug.LogError("데이터 로드 :: 실패 -> scItem is null");
            Debug.Break();
            return;
        }

        for (int i = 0; i < scItem.Length; i++)
        {
            // Out of Range 차단
            if (2 + i * 3 >= scItem.Length)
                break;

            // 아이템 인덱스
            int _loc = int.Parse(scItem[0 + i * 3]);

            // 아이템 인덱스
            int _index = int.Parse(scItem[1 + i * 3]);

            // 아이템 수량
            int _count = int.Parse(scItem[2 + i * 3]);


            // 아이템 생성
            GameData.itemManager.CreateItemObject(_loc, _index, _count);
        }
    }

    /// <summary>
    /// 이벤트 오브젝트 셋팅
    /// </summary>
    public static void LoadEventObject()
    {
        if (scEvent == null)
        {
            Debug.LogError("데이터 로드 :: 실패 -> scEvent is null");
            Debug.Break();
            return;
        }
        if (Player.allPlayer.Count == 0)
        {
            Debug.LogError("fatal error :: Player.allPlayer가 구성되기 전 GameSaver.LoadEventObject() 수행");
            Debug.Break();
            return;
        }

        for (int i = 0; i < scEvent.Length; i++)
        {
            // Out of Range 차단
            if (2 + i * 4 >= scEvent.Length)
                break;

            // 이벤트 인덱스
            int _loc = int.Parse(scEvent[0 + i * 4]);

            // 이벤트 인덱스
            int _index = int.Parse(scEvent[1 + i * 4]);

            // 이벤트 수량
            int _count = int.Parse(scEvent[2 + i * 4]);

            // 이벤트 설치자
            int _turnIndex = int.Parse(scEvent[2 + i * 4]);


            // 이벤트 생성
            GameData.eventManager.CreateEventObject(_loc, _index, _count, Player.allPlayer[_turnIndex]);
        }
    }

    /// <summary>
    /// 상황 셋팅
    /// </summary>
    public static void LoadTurn()
    {
        if (scTurn == null)
        {
            Debug.LogError("데이터 로드 :: 실패 -> scTurn is null");
            Debug.Break();
            return;
        }

        // 현재 턴 셋팅
        Debug.LogError(Player.allPlayer[int.Parse(scTurn[0])].name);
        Turn.Skip(Player.allPlayer[int.Parse(scTurn[0])]);

        // 플로우 셋팅
        GameMaster.flowCopy = (GameMaster.Flow)int.Parse(scTurn[1]);
    }
}
