using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

public static class GameSaveStream
{
    [System.Serializable]
    public struct SaveForm
    {
        [System.Serializable]
        public struct SaveFormObjectStack
        {
            public int loaction;
            public int index;
            public int count;
            public int ownerIndex;

            public SaveFormObjectStack(int _loaction, int _index, int _count, int _ownerIndex)
            {
                loaction = _loaction;
                index = _index;
                count = _count;
                ownerIndex = _ownerIndex;
            }
            public SaveFormObjectStack(ItemUnit itemUnit)
            {
                loaction = -1;

                if (itemUnit.item != null) index = itemUnit.item.index;
                else index = 0;

                count = itemUnit.count;

                ownerIndex = -1;
            }
            public SaveFormObjectStack(DynamicItem dynamicItem)
            {
                loaction = dynamicItem.location;
                index = dynamicItem.item.index;
                count = dynamicItem.count;
                ownerIndex = -1;
            }
            public SaveFormObjectStack(DynamicEvent dynamicEvent)
            {
                loaction = dynamicEvent.location;
                index = dynamicEvent.iocEvent.index;
                count = dynamicEvent.count;
                ownerIndex = Player.Index(dynamicEvent.creator);
            }
        }
        [System.Serializable]
        public struct SaveFormPlayer
        {
            public int turnIndex;               // Turn.Index(player)
            public Player.Type type;            // player.type
            public int characterIndex;          // player.character.index
            public bool isAutoPlay;             // player.isAutoPlay
            public string name;                 // player.name
            public int location;                // player.location
            public int stunCount;               // player.stunCount

            public GameResource life;           // player.life
            public GameResource coin;           // player.coin

            public Dice dice;                   // player.dice
            public MiniScore miniScore;         // player.miniInfo

            public SaveFormObjectStack[] inven;

            public SaveFormPlayer(Player player)
            {
                inven = new SaveFormObjectStack[Player.inventoryMax];

                turnIndex = Turn.Index(player);
                type = player.type;
                characterIndex = player.character.index;
                isAutoPlay = player.isAutoPlay;
                name = player.name;
                location = player.location;
                stunCount = player.stunCount;

                life = player.life;
                coin = player.coin;

                dice = player.dice;
                miniScore = player.miniInfo;

                for (int i = 0; i < Player.inventoryMax; i++)
                    inven[i] = new SaveFormObjectStack(player.inventory[i]);
            }
        }

        // 게임 정보
        public int area;                // GameRule.area
        public int section;             // GameRule.section
        public int cycleNow;            // Cycle.now
        public int cycleGoal;           // Cycle.goal
        public int playerCount;         // Player.allPlayer.Count
        public int plusBlockValue;      // BlockWork.plusBlockValue
        public int minusBlockValue;     // BlockWork.minusBlockValue

        // 플레이어 정보
        public SaveFormPlayer[] player;

        // 아이템 오브젝트 배치
        public SaveFormObjectStack[] itemObjects;

        // 이벤트 오브젝트 배치
        public SaveFormObjectStack[] eventObjects;

        // 상황 설정
        public int turnNow;                     // Player.Index(Turn.now))
        public GameMaster.Flow gameFlow;        // GameData.gameFlow
        public Turn.TurnAction turnAction;      // Turn.turnAction
               

        /// <summary>
        /// 초기화를 사용하는 생성자
        /// </summary>
        /// <param name="dumy"></param>
        public SaveForm(int dumy)
        {
            area                = GameRule.area                 ;
            section             = GameRule.section              ;
            cycleNow            = Cycle.now                     ;
            cycleGoal           = Cycle.goal                    ;
            playerCount         = Player.allPlayer.Count        ;
            plusBlockValue      = BlockWork.plusBlockValue      ;
            minusBlockValue     = BlockWork.minusBlockValue     ;


            player              = new SaveFormPlayer[Player.allPlayer.Count];
            for (int i = 0; i < player.Length; i++) { player[i] = new SaveFormPlayer(Player.allPlayer[i]); }

            itemObjects         = new SaveFormObjectStack[ItemManager.itemObjectList.Count];
            for (int i = 0; i < itemObjects.Length; i++) { itemObjects[i] = new SaveFormObjectStack(ItemManager.itemObjectList[i]); }

            eventObjects        = new SaveFormObjectStack[EventManager.eventObjectList.Count];
            for (int i = 0; i < eventObjects.Length; i++) { eventObjects[i] = new SaveFormObjectStack(EventManager.eventObjectList[i]); }


            turnNow             = Player.Index(Turn.now)    ;
            gameFlow            = GameData.gameFlow         ;
            turnAction          = Turn.turnAction           ;
        }


        /// <summary>
        /// 게임 정보 셋팅
        /// </summary>
        public void LoadGameInfo()
        {
            // 지역 설정
            GameRule.area = area;
            Debug.Log("로드 :: 게임 정보 -> GameRule.area = " + GameRule.area);

            // 구역 설정
            GameRule.section = section;
            Debug.Log("로드 :: 게임 정보 -> GameRule.section = " + GameRule.section);

            // 사이클 설정 - 현재
            Cycle.now = cycleNow;
            Debug.Log("로드 :: 게임 정보 -> Cycle.now = " + Cycle.now);

            // 사이클 설정 - 목표
            GameRule.cycleMax = cycleGoal;
            Debug.Log("로드 :: 게임 정보 -> GameRule.cycleMax = " + GameRule.cycleMax);

            // 플레이 인원수
            GameRule.playerCount = playerCount;
            Debug.Log("로드 :: 게임 정보 -> GameRule.playerCount = " + GameRule.playerCount);

            // 노말 블럭 강화 단계
            BlockWork.plusBlockValue =  plusBlockValue;
            Debug.Log("로드 :: 게임 정보 -> BlockWork.plusBlockValue = " + BlockWork.plusBlockValue);
            BlockWork.minusBlockValue = minusBlockValue;
            Debug.Log("로드 :: 게임 정보 -> BlockWork.minusBlockValue = " + BlockWork.minusBlockValue);

            Debug.LogWarning("로드 :: 게임 정보 -> 완료됨");
        }

        /// <summary>
        /// 플레이어 셋팅
        /// </summary>
        public void LoadPlayer()
        {
            if(Player.allPlayer.Count == 0)
            {
                Debug.LogError("error :: Player.allPlayer 정의되기 전 LoadPlayer() 호출됨");
                Debug.Break();
                return;
            }

            Player current = null;
            for (int i = 0; i < Player.allPlayer.Count; i++)
            {
                current = Player.allPlayer[i];
                Debug.Log("로드 :: 플레이어 " + current.name);

                //// 턴 인덱스
                //current.dice.SetValue(100 - player[i].turnIndex);
                //current.dice.isRolled = true;
                //Debug.Log("로드 :: 플레이어 -> 턴 주사위" + current.name);

                // 플레이어 타입, 캐릭터 인덱스, 오토플레이, 플레이어 이름
                current.SetPlayer(
                    player[i].type,
                    player[i].characterIndex,
                    player[i].isAutoPlay,
                    player[i].name
                    );
                Debug.Log(string.Format("로드 :: 플레이어 정보 변경 -> {0}타입의 {1}의 {2}은 자동플레이={3} 상태", current.type, current.name, current.character.index, current.isAutoPlay));

                // 위치 인덱스
                int loc = player[i].location;
                if (loc != -1)
                    current.movement.location = loc;
                Vector3 rePos = current.movement.locateBlock.position;
                rePos.y = current.movement.transform.position.y;
                current.movement.transform.position = rePos;

                Debug.Log("로드 :: 플레이어 " + current.name+"의 위치 -> " + current.location);


                // 이동 불가 턴수
                current.stunCount = player[i].stunCount;
                Debug.Log("로드 :: 플레이어 " + current.name + "의 스턴 -> " + current.stunCount);

                // 라이프
                current.life = player[i].life;
                Debug.Log("로드 :: 플레이어 " + current.name + "의 라이프 -> " + current.life.Value);

                // 코인
                current.coin = player[i].coin;
                Debug.Log("로드 :: 플레이어 " + current.name + "의 코인 -> " + current.coin.Value);

                // 주사위 개수
                current.dice = player[i].dice;
                Debug.Log("로드 :: 플레이어 " + current.name + "의 주사위 -> " + (current.dice.value + current.dice.valueTotal));

                // 미니게임 점수
                current.miniInfo = player[i].miniScore;
                Debug.Log("로드 :: 플레이어 " + current.name + "의 미니게임 -> " + current.miniInfo.score + " 로 등수 -> " + current.miniInfo.rank);

                // 인벤토리
                for (int j = 0; j < current.inventory.Count; j++)
                {
                    Debug.LogError(string.Format("로드 :: 플레이어 {0} 의 인벤은 {1} 칸이고 {2}번째 칸에는 {3}\n",
                        current.name,
                        current.inventory.Count,
                        j,
                        current.inventory[j].item
                        ) + string.Format("로드 :: 플레이어 {0} 의 인벤은 {1} 칸이고 {2}번째 칸에는 {3}\n",
                        player[i].name,
                        player[i].inven.Length,
                        j,
                        player[i].inven[j].index
                        ) + string.Format("로드 :: 플레이어 {0} 는 {1}중 {2}번쨰 이다 ",
                        player[i].name,
                        player.Length,
                        i
                        ));

                    if (player[i].inven[j].index > 0)
                    {
                    current.inventory[j].item = Item.table[player[i].inven[j].index];
                    current.inventory[j].count = player[i].inven[j].count;
                    }
                }
            }
        }

        /// <summary>
        /// 아이템 오브젝트 셋팅
        /// </summary>
        public void LoadItemObject()
        {
            if (GameData.itemManager == null)
            {
                Debug.LogError("error :: itemManager가 구성되기 전 LoadItemObject() 수행");
                Debug.Break();
                return;
            }

            for (int i = 0; i < itemObjects.Length; i++)
            {
                // 아이템 위치
                int _loc = itemObjects[i].loaction;

                // 아이템 인덱스
                int _index = itemObjects[i].index;

                // 아이템 수량
                int _count = itemObjects[i].count;


                // 아이템 생성
                GameData.itemManager.CreateItemObject(_loc, _index, _count);
            }
        }

        /// <summary>
        /// 이벤트 오브젝트 셋팅
        /// </summary>
        public void LoadEventObject()
        {
            if (GameData.eventManager == null)
            {
                Debug.LogError("error :: eventManager가 구성되기 전 LoadEventObject() 수행");
                Debug.Break();
                return;
            }

            for (int i = 0; i < eventObjects.Length; i++)
            {
                // 이벤트 위치
                int _loc = eventObjects[i].loaction;

                // 이벤트 인덱스
                int _index = eventObjects[i].index;

                // 이벤트 수량
                int _count = eventObjects[i].count;

                // 이벤트 설치자
                int _turnIndex = eventObjects[i].ownerIndex;


                // 이벤트 생성
                GameData.eventManager.CreateEventObject(_loc, _index, _count, Player.allPlayer[_turnIndex]);
            }
        }

        /// <summary>
        /// 상황 셋팅
        /// </summary>
        public void LoadTurn()
        {
            if (Turn.queue == null || Turn.queue.Count == 0)
            {
                Debug.LogError("세이브 :: Turn 스크립트 정의 안됨");
                Debug.Break();
                return;
            }

            // 현재 턴 셋팅
            Turn.Skip(Player.allPlayer[turnNow]);
            Debug.Log("세이브 :: 현재 턴 설정 -> " + Turn.now);

            // 게임 플로우 셋팅
            GameMaster.flowCopy = gameFlow;

            // 턴 플로우 셋팅
            Turn.turnAction = turnAction;
        }
    }


    // 세이브 파일 폴더명
    static string path = string.Format("{0}/{1}", CSVReader.copyPath, saveFloder);
    static string saveFloder = "Save";
    static string fullPath { get { return string.Format("{0}/{1}/{2}{3}", CSVReader.copyPath, saveFloder, fileName, extension); } }

    // 세이브 파일 파일명
    static string fileName { get { return GameData.gameMode.ToString(); } }
    static string extension = ".iocs";

    // 세이브 내용
    public static SaveForm saveForm;

    // 세이브 파일 구분 문자
    static char codeEnder = '#';
    static char codeChapter = '$';
    static char codeLine = '|';
    static char codeData = ',';

    // 챕터별 세이브 파일 코드
    public static string[] scInfo = null;
    public static List<string[]> scPlayers = new List<string[]>();
    public static List<string[]> scItem = new List<string[]>();
    public static List<string[]> scEvent = new List<string[]>();
    public static string[] scTurn = null;


    // 로딩 호출 여부
    public static bool useLoad = false;

    // 암호화 사용 여부
    //static bool useEncrypt = true;
    static bool useEncrypt = false;
    static string password = "This_is_Password";
    static string vec = "GrowupGrowupGrowupGrowup";

    // 세이브 파일 관리
    public static bool isFileOpen { get { return saveFileInfo != null; } }
    static FileInfo saveFileInfo = null;
    static StringBuilder saveCodeBuilder = new StringBuilder();

    public enum LockType
    {
        None,
        Lock,
        Unlock,
    }

    public static void Clear()
    {
        saveFileInfo = null;
        saveCodeBuilder.Clear();

        scInfo = null;
        scPlayers.Clear();
        scItem.Clear();
        scEvent.Clear();
        scTurn = null;

        useLoad = false;
    }


    public static void SaveRemove()
    {
        // 게임 모드 누락시 저장 중단 - 필수 :: 게임모드로 파일명 설정
        if (GameData.gameMode == GameMode.Mode.None)
        {
            Debug.LogError("game save :: 세이브파일 삭제 실패 " + "게임 모드 -> " + GameData.gameMode);
            return;
        }
        // 게임 모드 누락시 저장 중단 - 필수 :: 게임모드로 파일명 설정
        if (saveFileInfo == null || !saveFileInfo.Exists)
        {
            Debug.LogError("game save :: 세이브파일 삭제 실패 " + "파일 확인 불가");
            return;
        }

        // 제거
        Debug.LogError("세이브 파일 :: 제거 요청됨 -> " + saveFileInfo.FullName);
        saveFileInfo.Delete();
        Debug.LogError("세이브 파일 :: 제거 성공여부 -> " + saveFileInfo.Exists);
    }

    /// <summary>
    /// 게임 저장
    /// </summary>
    /// <returns>성공 여부</returns>
    public static bool GameSave1()
    //public static IEnumerator GameSave()
    {
        try
        {
            // 데이터 -> 세이브 코드 변환
            SaveCode();
            string codeStr = saveCodeBuilder.ToString();

            // 성공여부
            bool result = false;

            // 암호화 선택형 저장
            if (useEncrypt)
                result = Save(LockType.Lock, codeStr);
            else
                result = Save(LockType.None, codeStr);
            Debug.LogError("파일 생성됨 :: " + saveFileInfo.FullName);

            return result;
        }
        catch
        {
            // 게임 모드 누락시 저장 중단 - 필수 :: 게임모드로 파일명 설정
            if (GameData.gameMode == GameMode.Mode.None)
            {
                Debug.LogError("game save :: 저장 실패 " + "게임 모드 -> " + GameData.gameMode);
                return false;
            }
            // 저장 시점 아니면 중단
            if (GameData.gameFlow <= GameMaster.Flow.Ordering || GameData.gameFlow >= GameMaster.Flow.End)
            {
                Debug.LogError("game save :: 저장 실패 " + "게임 플로우 -> " + GameData.gameMode);
                return false;
            }
        }
        return false;
    }


    /// <summary>
    /// 데이터 -> 세이브 코드 변환
    /// </summary>
    //static StringBuilder SaveCode()
    static void SaveCode()
    {
        saveCodeBuilder.Clear();

        // 게임 정보
        saveCodeBuilder
            .Append(GameRule.area)
            .Append(codeData)
            .Append(GameRule.section)
            .Append(codeData)
            .Append(Cycle.now)
            .Append(codeData)
            .Append(Cycle.goal)
            .Append(codeData)
            .Append(Player.allPlayer.Count)
            .Append(codeData)
            .Append(BlockWork.plusBlockValue)
            .Append(codeData)
            .Append(BlockWork.minusBlockValue)

            .Append(codeChapter)
            ;

        // 플레이어 정보
        for (int i = 0; i < Player.allPlayer.Count; i++)
        {
            Player temp = Player.allPlayer[i];

            saveCodeBuilder
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
                .Append(temp.movement.location)
                .Append(codeData)
                .Append(temp.stunCount)
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
                if (temp.inventory[j].count > 0)
                {
                    // 유효한 아이템
                    saveCodeBuilder
                        .Append(temp.inventory[j].item.index)
                        .Append(codeData)
                        .Append(temp.inventory[j].count)
                        ;
                }
                else
                {
                    // 아이템 없음
                    saveCodeBuilder
                        .Append(-1)
                        .Append(codeData)
                        .Append(0)
                        ;
                }


                if (j < Player.inventoryMax - 1)
                    saveCodeBuilder.Append(codeData);
            }

            if (Player.allPlayer.Count > 1 && i != Player.allPlayer.Count - 1)
                saveCodeBuilder.Append(codeLine);
        }
        saveCodeBuilder.Append(codeChapter);


        // 아이템 오브젝트 배치
        for (int i = 0; i < ItemManager.itemObjectList.Count; i++)
        {
            DynamicItem obj = ItemManager.itemObjectList[i];

            saveCodeBuilder
                .Append(obj.location)
                .Append(codeData)
                .Append(obj.item.index)
                .Append(codeData)
                .Append(obj.count)
                ;

            if (i >= 0 && i != ItemManager.itemObjectList.Count - 1)
                saveCodeBuilder.Append(codeLine);
        }
        saveCodeBuilder.Append(codeChapter);


        // 이벤트 오브젝트 배치
        for (int i = 0; i < EventManager.eventObjectList.Count; i++)
        {
            DynamicEvent obj = EventManager.eventObjectList[i];

            saveCodeBuilder
                .Append(obj.location)
                .Append(codeData)
                .Append(obj.iocEvent.index)
                .Append(codeData)
                .Append(obj.count)
                .Append(codeData)
                .Append(Player.Index(obj.creator))
                ;

            if (i >= 0 && i != EventManager.eventObjectList.Count - 1)
                saveCodeBuilder.Append(codeLine);
        }
        saveCodeBuilder.Append(codeChapter);


        // 상황 설정
        saveCodeBuilder
            .Append(Player.Index(Turn.now))
            .Append(codeData)
            .Append((int)GameData.gameFlow)
            .Append(codeData)
            .Append((int)Turn.turnAction)

            //.Append(codeChapter)
            ;


        // 종료 문자
        saveCodeBuilder.Append('#');

        //return saveCodeBuilder;
    }

    //public static void CodeLoad()
    //{
    //    // 파일명
    //    string fName = GameData.gameMode.ToString() + extension;

    //    // 파일 읽기
    //    List<List<string>> loader = new List<List<string>>();
    //    string temp = Read(LockType.Unlock);
    //    loader.Add(new List<string>(
    //                                temp.Split(codeEnder)[0]
    //                                    .Split(codeChapter)
    //                                ));
    //    Debug.LogError(temp);


    //    // 누락 체크
    //    //if (loader.table.Count == 0)
    //    if (loader.Count == 0)
    //    {
    //        Debug.LogWarning("miss :: 세이브 파일 없음");
    //        return;
    //    }

    //    // 초기화
    //    scInfo = null;
    //    scPlayers.Clear();
    //    scItem.Clear();
    //    scEvent.Clear();
    //    scTurn = null;

    //    //List<string> code = loader.table[0];
    //    List<string> code = loader[0];

    //    // 챕터별 세이브 파일 코드
    //    scInfo = code[0].Split(codeData);

    //    string[] pCode = code[1].Split(codeLine);
    //    for (int i = 0; i < pCode.Length; i++)
    //        scPlayers.Add(pCode[i].Split(codeData));

    //    //scItem = code[2].Split(codeData);

    //    //scEvent = code[3].Split(codeData);

    //    pCode = code[2].Split(codeLine);
    //    for (int i = 0; i < pCode.Length; i++)
    //        scItem.Add(pCode[i].Split(codeData));

    //    pCode = code[3].Split(codeLine);
    //    for (int i = 0; i < pCode.Length; i++)
    //        scEvent.Add(pCode[i].Split(codeData));

    //    scTurn = code[4].Split(codeData);
    //}





    static Rfc2898DeriveBytes CreateKey(string _password)
    {
        //키값 생성
        byte[] keyBytes = Encoding.Default.GetBytes(_password);

        //솔트값(원본값을 알기 어렵게 하는 값)
        byte[] saltBytes = SHA512.Create().ComputeHash(keyBytes);

        //password를 암호화한 키 생성, 100000은 해시 생성의 반복 횟수
        Rfc2898DeriveBytes result = new Rfc2898DeriveBytes(keyBytes, saltBytes, 100000);

        return result;  //키값 반환

        // 출처 : https://fred16157.github.io/.net/csharp-encryption/
    }


    public static byte[] UnLock(byte[] origin)
    {
        return Ccryptor(origin, LockType.Unlock);
    }
    public static byte[] Lock(byte[] origin)
    {
        return Ccryptor(origin, LockType.Lock);
    }
    public static byte[] Ccryptor(byte[] origin, LockType useEncryptor)
    {
        // 바이트로 변환
        //byte[] origin = Encoding.Default.GetBytes(code);
        //Debug.LogError(origin.Length);

        //AES 알고리즘
        RijndaelManaged aes = new RijndaelManaged();

        //키값 생성
        Rfc2898DeriveBytes key = CreateKey(password);

        //벡터 생성 
        //Rfc2898DeriveBytes vector = CreateKey("GrowupGrowupGrowupGrowup");
        Rfc2898DeriveBytes vector = CreateKey(vec);

        aes.BlockSize = 128;            //AES의 블록 크기는 128 고정
        aes.KeySize = 256;              //AES의 키 크기는 128, 192, 256을 지원한다.
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key.GetBytes(32);     //AES-256을 사용하므로 키값의 길이는 32여야 한다.
        aes.IV = vector.GetBytes(16);   //초기화 벡터는 언제나 길이가 16이어야 한다.

        //Debug.LogError("key :: " + Encoding.Default.GetString(aes.Key));
        //Debug.LogError("vector :: " + Encoding.Default.GetString(aes.IV));

        //키값과 초기화 벡터를 기반으로 암호화 또는 복호화 작업을 하는 클래스 변수를 생성
        ICryptoTransform cryptor;
        if (useEncryptor == LockType.Lock)
            cryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        else if (useEncryptor == LockType.Unlock)
            cryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        else
            return origin;

        //using블록으로 변수를 사용하면 블록에서 나올때 자동으로 변수가 가비지컬렉팅 된다. 
        using (MemoryStream ms = new MemoryStream()) //결과를 담을 스트림 
        {
            //cryptor 변수에서 암호화 또는 복호화된 데이터를 결과에 쓰는 스트림
            using (CryptoStream cs = new CryptoStream(ms, cryptor, CryptoStreamMode.Write))
            {
                cs.Write(origin, 0, origin.Length);
            }
            //return Encoding.UTF8.GetString(ms.ToArray());    //암호화된 스트링 반환
            return ms.ToArray();
        }

        // 출처 : https://fred16157.github.io/.net/csharp-encryption/
    }


    /// <summary>
    /// 세이브파일 생성
    /// </summary>
    /// <param name="fileName">경로가 제외된 파일명과 확장자</param>
    /// <param name="useEncryptor">일반읽기, 암호화, 복호화</param>
    /// <param name="strData">파일 내용</param>
    /// <returns></returns>
    public static bool Save(LockType useEncryptor, string strData)
    {
        // 암호화 사용
        if (useEncryptor == LockType.Lock)
            return SaveFileCreate(
                    Lock(Encoding.Default.GetBytes(strData))
                    );
        // 암호화 사용 안함
        else
            return SaveFileCreate(
                    Encoding.Default.GetBytes(strData)
                    );
    }
    static bool SaveFileCreate(byte[] byteData)
    {
        // 게임 모드 누락시 저장 중단 - 필수 :: 게임모드로 파일명 설정
        if (GameData.gameMode == GameMode.Mode.None)
        {
            Debug.LogError("game save :: 세이브파일 생성 실패 " + "게임 모드 -> " + GameData.gameMode);
            return false;
        }

        // 파일 생성
        using (FileStream fs = new FileStream(@fullPath, FileMode.Create))
        {
            // 작성
            fs.Write(byteData, 0, byteData.Length);
        }


        saveFileInfo = new FileInfo(@fullPath);

        return saveFileInfo.Exists;
    }

    //public static string Read(LockType useDecryptor)
    //{
    //    byte[] result = SaveFileRead();

    //    // 복호화 사용
    //    if (useDecryptor == LockType.Unlock)
    //        result = UnLock(result);

    //    return Encoding.Default.GetString(result);
    //}
    //static byte[] SaveFileRead()
    //{
    //    // 잘못된 접근 제어
    //    if (saveFileStream == null)
    //    {
    //        Debug.LogWarning("세이브 :: 파일 닫혀있음 -> 파일 열기 시도");
    //        SaveFileOpen();
    //    }
    //    if (saveFileStream == null)
    //    {
    //        Debug.LogError("세이브 :: 파일 읽기 실패");
    //        Debug.Break();
    //        //return ;
    //        return null;
    //    }

    //    // 결과물
    //    List<byte> result = new List<byte>();

    //    // 파일 첫 위치로
    //    saveFileStream.Seek(0, SeekOrigin.Begin);

    //    // 파일 읽기
    //    int data;
    //    while ((data = saveFileStream.ReadByte()) != -1)
    //        result.Add((byte)data);

    //    // 결과 반환
    //    return result.ToArray();
    //}

    //static void SaveFileOpen()
    //{
    //    // 파일 불량 중단
    //    if (saveFileInfo == null || !saveFileInfo.Exists)
    //    {
    //        Debug.LogError("세이브 :: 파일 열수 없음 -> 파일 확인 불가");
    //        Debug.Break();
    //        return;
    //    }

    //    Debug.Log("세이브 :: 파일 열기 -> " + saveFileInfo.FullName);
    //    if(saveFileStream == null)
    //        saveFileStream = new FileStream(saveFileInfo.FullName, FileMode.Open, FileAccess.ReadWrite);
    //    else
    //    {
    //        saveFileStream.Close();
    //        saveFileStream = new FileStream(saveFileInfo.FullName, FileMode.Open, FileAccess.ReadWrite);
    //    }
    //}

    //public static void SaveFileClose()
    //{
    //    saveFileStream.Close();
    //}





    public static void GameSave()
    { 
        if(useEncrypt)
            GameSave(LockType.Lock);
        else
            GameSave(LockType.None);
    }
    public static void GameSave(LockType useEncryptor)
    {
        // 파일 오픈
        //using (FileStream fs = new FileStream(saveFileInfo.FullName, FileMode.Open, FileAccess.Write))
        using (FileStream fs = new FileStream(@fullPath, FileMode.Create, FileAccess.Write))
        {
            Debug.LogError("세이브 :: 파일 작성 요청됨 -> " + @fullPath);

            BinaryFormatter bf = new BinaryFormatter();

            if (useEncryptor == LockType.None)
            {
                bf.Serialize(fs, new SaveForm(0));
                Debug.LogError("세이브 :: 암호화 사용 안함 " + fs.CanWrite);
            }
            else
            {
                //cryptor 변수에서 암호화 또는 복호화된 데이터를 결과에 쓰는 스트림
                using (CryptoStream cs = new CryptoStream(fs, GetCcryptor(useEncryptor), CryptoStreamMode.Write))
                {
                    bf.Serialize(cs, new SaveForm(0));
                }
                Debug.LogError("세이브 :: 암호화 사용 " + fs.CanWrite);
            }
        }

        // 파일 지정
        saveFileInfo = new FileInfo(@fullPath);
        Debug.LogError("세이브 :: 파일 작성 결과 -> " + saveFileInfo.Exists);

    }
    public static bool GameRead()
    {
        if (useEncrypt)
            return GameRead(LockType.Unlock);
        else
            return GameRead(LockType.None);
    }
    public static bool GameRead(LockType useDecryptor)
    {
        // 파일 체크
        if (saveFileInfo == null)
        {
            Debug.LogWarning("error :: 세이브 파일 지정 안됨");

            saveFileInfo = new FileInfo(@fullPath);

            // 파일 체크
            if (!saveFileInfo.Exists)
            {
                Debug.LogError("error :: 세이브 파일 없음");
                Debug.Break();
                return false;
            }
            else
                Debug.Log("세이브 :: 세이브 파일 지정 성공");
        }

        // 파일 오픈
        using (MemoryStream ms = new MemoryStream())
        {
            // 파일 바이트화
            byte[] origin = File.ReadAllBytes(@saveFileInfo.FullName);
            Debug.Log("세이브 :: 파일 열기 성공");

            // 복호화 생략
            if (useDecryptor == LockType.None)
            {
                Debug.Log("세이브 :: 복호화 필요 없음");
                //ms.Read(origin, 0, origin.Length);
                ms.Write(origin, 0, origin.Length);
            }
            // 복호화
            else if (useDecryptor == LockType.Unlock)
            {
                Debug.Log("세이브 :: 복호화 필요함");
                using (CryptoStream cs = new CryptoStream(ms, GetCcryptor(useDecryptor), CryptoStreamMode.Write))
                {
                    Debug.LogError(origin.Length);
                    Debug.LogError(ms.Length);
                    Debug.LogError(cs.Length);
                    cs.Write(origin, 0, origin.Length);
                    Debug.LogError(cs.Length);
                }
            }
            Debug.Log("세이브 :: 파일 데이터화 시작");

            // 읽기
            BinaryFormatter bf = new BinaryFormatter();
            ms.Position = 0;
            saveForm = (SaveForm)bf.Deserialize(ms);
            Debug.Log("세이브 :: 파일 데이터화 성공");
        }
        try
        {

            Debug.Log("세이브 :: 파일 읽기 성공");
            return true;
        }
        catch
        {
            Debug.LogError("세이브 :: 파일 읽기 실패");
            return false;
        }
    }

    public static ICryptoTransform GetCcryptor(LockType useEncryptor)
    {

        //AES 알고리즘
        RijndaelManaged aes = new RijndaelManaged();

        //키값 생성
        Rfc2898DeriveBytes key = CreateKey(password);

        //벡터 생성 
        Rfc2898DeriveBytes vector = CreateKey(vec);

        aes.BlockSize = 128;            //AES의 블록 크기는 128 고정
        aes.KeySize = 256;              //AES의 키 크기는 128, 192, 256을 지원한다.
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key.GetBytes(32);     //AES-256을 사용하므로 키값의 길이는 32여야 한다.
        aes.IV = vector.GetBytes(16);   //초기화 벡터는 언제나 길이가 16이어야 한다.

        //Debug.LogError("key :: " + Encoding.Default.GetString(aes.Key));
        //Debug.LogError("vector :: " + Encoding.Default.GetString(aes.IV));

        bool isSkip = false;

        //키값과 초기화 벡터를 기반으로 암호화 또는 복호화 작업을 하는 클래스 변수를 생성
        ICryptoTransform cryptor;
        if (useEncryptor == LockType.Lock)
            cryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        else if (useEncryptor == LockType.Unlock)
            cryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        else return null;

        return cryptor;

        // 원본 출처 : https://fred16157.github.io/.net/csharp-encryption/
    }
}
