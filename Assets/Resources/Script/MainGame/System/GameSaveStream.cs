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


            turnNow             = Turn.Index(Turn.now)    ;
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
            //최적화 Debug.Log("로드 :: 게임 정보 -> GameRule.area = " + GameRule.area);

            // 구역 설정
            GameRule.section = section;
            //최적화 Debug.Log("로드 :: 게임 정보 -> GameRule.section = " + GameRule.section);

            // 사이클 설정 - 현재
            Cycle.now = cycleNow;
            //최적화 Debug.Log("로드 :: 게임 정보 -> Cycle.now = " + Cycle.now);

            // 사이클 설정 - 목표
            GameRule.cycleMax = cycleGoal;
            //최적화 Debug.Log("로드 :: 게임 정보 -> GameRule.cycleMax = " + GameRule.cycleMax);

            // 플레이 인원수
            GameRule.playerCount = playerCount;
            //최적화 Debug.Log("로드 :: 게임 정보 -> GameRule.playerCount = " + GameRule.playerCount);

            // 노말 블럭 강화 단계
            BlockWork.plusBlockValue =  plusBlockValue;
            //최적화 Debug.Log("로드 :: 게임 정보 -> BlockWork.plusBlockValue = " + BlockWork.plusBlockValue);
            BlockWork.minusBlockValue = minusBlockValue;
            //최적화 Debug.Log("로드 :: 게임 정보 -> BlockWork.minusBlockValue = " + BlockWork.minusBlockValue);

            //최적화 Debug.Log("로드 :: 게임 정보 -> 완료됨");
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
                //최적화 Debug.Log("로드 :: 플레이어 " + current.name);

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
                //최적화 Debug.Log(string.Format("로드 :: 플레이어 정보 변경 -> {0}타입의 {1}의 {2}은 자동플레이={3} 상태", current.type, current.name, current.character.index, current.isAutoPlay));

                // 위치 인덱스
                int loc = player[i].location;
                if (loc != -1)
                    current.movement.location = loc;
                Vector3 rePos = current.movement.locateBlock.position;
                rePos.y = current.movement.transform.position.y;
                current.movement.transform.position = rePos;

                //최적화 Debug.Log("로드 :: 플레이어 " + current.name+"의 위치 -> " + current.location);


                // 이동 불가 턴수
                current.stunCount = player[i].stunCount;
                //최적화 Debug.Log("로드 :: 플레이어 " + current.name + "의 스턴 -> " + current.stunCount);

                // 라이프
                current.life = player[i].life;
                //최적화 Debug.Log("로드 :: 플레이어 " + current.name + "의 라이프 -> " + current.life.Value);

                // 코인
                current.coin = player[i].coin;
                //최적화 Debug.Log("로드 :: 플레이어 " + current.name + "의 코인 -> " + current.coin.Value);

                // 주사위 개수
                current.dice = player[i].dice;
                //최적화 Debug.Log("로드 :: 플레이어 " + current.name + "의 주사위 -> " + (current.dice.value + current.dice.valueTotal));

                // 미니게임 점수
                current.miniInfo = player[i].miniScore;
                //최적화 Debug.Log("로드 :: 플레이어 " + current.name + "의 미니게임 -> " + current.miniInfo.score + " 로 등수 -> " + current.miniInfo.rank);

                // 인벤토리
                for (int j = 0; j < current.inventory.Count; j++)
                {
                    //Debug.LogError(string.Format("로드 :: 플레이어 {0} 의 인벤은 {1} 칸이고 {2}번째 칸에는 {3}\n",
                    //    current.name,
                    //    current.inventory.Count,
                    //    j,
                    //    current.inventory[j].item
                    //    ) + string.Format("로드 :: 플레이어 {0} 의 인벤은 {1} 칸이고 {2}번째 칸에는 {3}\n",
                    //    player[i].name,
                    //    player[i].inven.Length,
                    //    j,
                    //    player[i].inven[j].index
                    //    ) + string.Format("로드 :: 플레이어 {0} 는 {1}중 {2}번쨰 이다 ",
                    //    player[i].name,
                    //    player.Length,
                    //    i
                    //    ));

                    if (player[i].inven[j].index > 0)
                    {
                        current.inventory[j].item = Item.table[player[i].inven[j].index];
                        current.inventory[j].count = player[i].inven[j].count;
                        //최적화 Debug.Log(string.Format("로드 :: 플레이어 {0}의 인벤토리 {1}번 설정됨 -> 아이템({2}) = {3}개", current.name, j, current.inventory[j].item.name, current.inventory[j].count));
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
                Debug.LogError("error :: 세이브 로드 -> Turn 스크립트 정의 안됨");
                Debug.Break();
                return;
            }

            // 현재 턴 셋팅
            //if(turnNow > 0)
            //{
            //    // 유저 턴일 경우
            //    Turn.Skip(Player.allPlayer[turnNow]);
            //}
            //else
            //{
            //    // 시스템 턴일 경우
            //    Turn.Skip(Player.allPlayer[turnNow]);
            //}

            Turn.Skip(Turn.origin[turnNow]);
            //최적화 Debug.Log("로드 :: 현재 턴 설정 -> " + Turn.now.name);

            // 게임 플로우 셋팅
            GameMaster.flowCopy = gameFlow;
            //최적화 Debug.Log("로드 :: 현재 게임 플로우 -> " + GameMaster.flowCopy.ToString());

            // 턴 플로우 셋팅
            Turn.turnAction = turnAction;
            //최적화 Debug.Log("로드 :: 현재 턴 플로우 -> " + Turn.turnAction.ToString());
        }
    }

    // 세이브 파일 폴더명
    static string path = string.Format("{0}/{1}", CSVReader.copyPath, saveFloder);
    const string saveFloder = "Save";
    static string fullPath { get { return string.Format("{0}/{1}/{2}{3}", CSVReader.copyPath, saveFloder, fileName, extension); } }
    static string tempPath { get { return string.Format("{0}/{1}/_{2}{3}", CSVReader.copyPath, saveFloder, fileName, extension); } }

    // 세이브 파일 파일명
    static string fileName { get { return GameData.gameMode.ToString(); } }
    const string extension = ".iocs";

    // 세이브 내용
    public static SaveForm saveForm;


    // 로딩 호출 여부
    public static bool useLoad = false;

    // 암호화 사용 여부
    static bool useEncrypt = true;
    //static bool useEncrypt = false;
    const string password = "This_is_Password";
    const string vec = "GrowupGrowupGrowupGrowup";

    static ICryptoTransform encryptor = GetCcryptor(LockType.Lock);
    static ICryptoTransform decryptor = GetCcryptor(LockType.Unlock);

    // 세이브 파일 관리
    public static bool isFileOpen { get { return saveFileInfo != null; } }
    static FileInfo saveFileInfo = null;

    public static Coroutine saveControl = null;

    public enum LockType
    {
        None,
        Lock,
        Unlock,
    }

    public static void Clear()
    {
        saveFileInfo = null;
        useLoad = false;
    }


    public static void SaveRemove()
    {
        //최적화 Debug.Log("세이브 :: 파일 제거 요청됨");

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
        saveFileInfo.Delete();
        //최적화 Debug.Log("세이브 :: 파일 제거 성공여부 -> " + saveFileInfo.Exists);
    }

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
    //public static byte[] UnLock(byte[] origin)
    //{
    //    return Ccryptor(origin, LockType.Unlock);
    //}
    //public static byte[] Lock(byte[] origin)
    //{
    //    return Ccryptor(origin, LockType.Lock);
    //}
    //public static byte[] Ccryptor(byte[] origin, LockType useEncryptor)
    //{
    //    // 바이트로 변환
    //    //byte[] origin = Encoding.Default.GetBytes(code);
    //    //Debug.LogError(origin.Length);

    //    //AES 알고리즘
    //    RijndaelManaged aes = new RijndaelManaged();

    //    //키값 생성
    //    Rfc2898DeriveBytes key = CreateKey(password);

    //    //벡터 생성 
    //    //Rfc2898DeriveBytes vector = CreateKey("GrowupGrowupGrowupGrowup");
    //    Rfc2898DeriveBytes vector = CreateKey(vec);

    //    aes.BlockSize = 128;            //AES의 블록 크기는 128 고정
    //    aes.KeySize = 256;              //AES의 키 크기는 128, 192, 256을 지원한다.
    //    aes.Mode = CipherMode.CBC;
    //    aes.Padding = PaddingMode.PKCS7;

    //    aes.Key = key.GetBytes(32);     //AES-256을 사용하므로 키값의 길이는 32여야 한다.
    //    aes.IV = vector.GetBytes(16);   //초기화 벡터는 언제나 길이가 16이어야 한다.

    //    //Debug.LogError("key :: " + Encoding.Default.GetString(aes.Key));
    //    //Debug.LogError("vector :: " + Encoding.Default.GetString(aes.IV));

    //    //키값과 초기화 벡터를 기반으로 암호화 또는 복호화 작업을 하는 클래스 변수를 생성
    //    ICryptoTransform cryptor;
    //    if (useEncryptor == LockType.Lock)
    //        cryptor = aes.CreateEncryptor(aes.Key, aes.IV);
    //    else if (useEncryptor == LockType.Unlock)
    //        cryptor = aes.CreateDecryptor(aes.Key, aes.IV);
    //    else
    //        return origin;

    //    //using블록으로 변수를 사용하면 블록에서 나올때 자동으로 변수가 가비지컬렉팅 된다. 
    //    using (MemoryStream ms = new MemoryStream()) //결과를 담을 스트림 
    //    {
    //        //cryptor 변수에서 암호화 또는 복호화된 데이터를 결과에 쓰는 스트림
    //        using (CryptoStream cs = new CryptoStream(ms, cryptor, CryptoStreamMode.Write))
    //        {
    //            cs.Write(origin, 0, origin.Length);
    //        }
    //        //return Encoding.UTF8.GetString(ms.ToArray());    //암호화된 스트링 반환
    //        return ms.ToArray();
    //    }

    //    // 출처 : https://fred16157.github.io/.net/csharp-encryption/
    //}
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

    public static byte[] Ccryptor(byte[] origin, LockType useEncryptor)
    {
        //using블록으로 변수를 사용하면 블록에서 나올때 자동으로 변수가 가비지컬렉팅 된다. 
        using (MemoryStream ms = new MemoryStream()) //결과를 담을 스트림 
        {
            if (useEncryptor == LockType.Lock)
            {
                //cryptor 변수에서 암호화 또는 복호화된 데이터를 결과에 쓰는 스트림
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(origin, 0, origin.Length);
                }
            }
            else if (useEncryptor == LockType.Unlock)
            {
                //cryptor 변수에서 암호화 또는 복호화된 데이터를 결과에 쓰는 스트림
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(origin, 0, origin.Length);
                }
            }
            return ms.ToArray();
        }
    }


    public static IEnumerator GameSave()
    { 
        if(useEncrypt)
            yield return GameSave(LockType.Lock, new SaveForm(0));
        else
            yield return GameSave(LockType.None, new SaveForm(0));
    }
    public static IEnumerator GameSave(LockType useEncryptor, SaveForm saveData)
    {
        // 성공 여부 제어
        bool ctrl = true;
        try
        {
            // 경로 체크
            CSVReader.CheckPath(path);

            // 파일 오픈
            using (FileStream fs = new FileStream(@tempPath, FileMode.Create, FileAccess.Write))
            {
                //최적화 Debug.Log("세이브 :: 파일 작성 요청됨 -> " + @fullPath);

                BinaryFormatter bf = new BinaryFormatter();

                if (useEncryptor == LockType.None)
                {
                    bf.Serialize(fs, saveData);
                    //최적화 Debug.Log("세이브 :: 암호화 사용 안함 " + fs.CanWrite);
                }
                else
                {
                    //cryptor 변수에서 암호화 또는 복호화된 데이터를 결과에 쓰는 스트림
                    using (CryptoStream cs = new CryptoStream(fs, GetCcryptor(useEncryptor), CryptoStreamMode.Write))
                    {
                        bf.Serialize(cs, saveData);
                    }
                    //최적화 Debug.Log("세이브 :: 암호화 사용 " + fs.CanWrite);
                }
            }
        }
        catch
        {
            // 실패 처리
            ctrl = false;
            Debug.LogError("세이브 :: 파일 작성 실패");
        }

        if (ctrl)
        {
            // 파일 덮어쓰기
            yield return SaveOverwriteTask();

            // 파일 지정
            saveFileInfo = new FileInfo(@fullPath);
            //최적화 Debug.Log("세이브 :: 파일 작성 결과 -> " + saveFileInfo.Exists);
        }

        // 종료처리
        saveControl = null;

        yield return null;
    }

    public static void SaveOverwrite()
    {
        // 임시 파일 확인
        if (File.Exists(@tempPath))
        {
            // 정규 파일 확인 후 제거
            if (File.Exists(@fullPath))
                File.Delete(@fullPath);

            // 정규 파일로 변경
            File.Move(@tempPath, @fullPath);
        }
    }
    public static IEnumerator SaveOverwriteTask()
    {
        // 파일 덮어쓰기
        System.Threading.Tasks.Task overwriter = new System.Threading.Tasks.Task(() => SaveOverwrite());
        overwriter.Start();

        // 덮어쓰기 대기
        while (!overwriter.IsCompleted)
            yield return null;
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
        try
        {
            // 경로 체크
            CSVReader.CheckPath(path);

            // 파일 체크
            if (saveFileInfo == null)
            {
                Debug.LogWarning("error :: 세이브 파일 지정 안됨");

                saveFileInfo = new FileInfo(@fullPath);

            }

            // 임시 파일 체크
            if (File.Exists(@tempPath))
            {
                if (File.Exists(@fullPath))
                    File.Delete(@fullPath);
                File.Move(@tempPath, @fullPath);
            }

            // 파일 오픈
            using (MemoryStream ms = new MemoryStream())
            {
                // 파일 바이트화
                byte[] origin = File.ReadAllBytes(@saveFileInfo.FullName);
                //최적화 Debug.Log("세이브 :: 파일 열기 성공 -> " + origin.Length);

                // 복호화
                if (useDecryptor == LockType.Unlock)
                {
                    //최적화 Debug.Log("세이브 :: 복호화 필요함");
                    origin = Ccryptor(origin, useDecryptor);
                }

                //최적화 Debug.Log("세이브 :: 파일 데이터화 시작");
                ms.Write(origin, 0, origin.Length);

                // 읽기
                BinaryFormatter bf = new BinaryFormatter();
                ms.Position = 0;
                saveForm = (SaveForm)bf.Deserialize(ms);
                //최적화 Debug.Log("세이브 :: 파일 데이터화 성공");
            }

            //최적화 Debug.Log("세이브 :: 파일 읽기 성공");
            return true;
        }
        catch
        {
            // 파일 체크
            if (!saveFileInfo.Exists)
            {
                Debug.LogWarning("error :: 세이브 파일 없음");
                //Debug.Break();
                return false;
            }
            else
            {
                //최적화 Debug.Log("세이브 :: 세이브 파일 지정 성공");
            }

            Debug.LogError("세이브 :: 파일 읽기 실패");
            return false;
        }
    }

}
