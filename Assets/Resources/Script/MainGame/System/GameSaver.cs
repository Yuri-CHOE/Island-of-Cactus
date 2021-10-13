using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using System.IO;
using System.Security.Cryptography;

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
    public static List<string[]> scItem = new List<string[]>();
    public static List<string[]> scEvent = new List<string[]>();
    public static string[] scTurn = null;


    // 로딩 호출 여부
    public static bool useLoad = false;

    // 암호화 사용 여부
    //static bool useEncrypt = false;
    static string password = "This_is_Password";
    static string vec = "GrowupGrowupGrowupGrowup";

    public enum LockType
    {
        None,
        Lock,
        Unlock,
    }



    public static void SaveRemove()
    {
        // 게임 모드 누락시 저장 중단 - 필수 :: 게임모드로 파일명 설정
        if (GameData.gameMode == GameMode.Mode.None)
        {
            Debug.LogError("game save :: 저장 실패 " + "게임 모드 -> " + GameData.gameMode);
            return;
        }

        // 경로 지정
        string path = string.Format("{0}/{1}", CSVReader.copyPath, saveFloder);
        string fullPath = string.Format("{0}/{1}{2}", @path, @fileName, extension);

        // 파일 확인
        FileInfo fi = new FileInfo(@fullPath);
        if (fi.Exists)
        {
            fi.Delete();

            Debug.LogError("세이브 파일 :: 제거됨 -> " + fileName);
        }
        else
            Debug.LogError("세이브 파일 :: 제거 불가 -> 파일 없음");
    }

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
        string codeStr = code.ToString();

        // 저장
        FileInfo saveF0 = Save(fileName + extension, LockType.None, codeStr);
        Debug.LogError("파일 생성됨 :: " + saveF0);
        //CSVReader.SaveNew(saveFloder, fileName + extension, true, true, codeStr);

        // 암호화 저장
        FileInfo saveF1 = Save(fileName + "_" + extension, LockType.Lock, codeStr);
        Debug.LogError("파일 생성됨 :: " + saveF1);

        // 복호화 저장  
        FileInfo saveF2 =
            Save(
                fileName + "__" + extension,
                LockType.None,
                Read(saveF1.Name, LockType.Unlock)
                );
        Debug.LogError("파일 생성됨 :: " + saveF2);
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

            if (i >= 0 && i != ItemManager.itemObjectList.Count - 1)
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

            if (i >= 0 && i != EventManager.eventObjectList.Count - 1)
                sb.Append(codeLine);
        }
        sb.Append(codeChapter);


        // 상황 설정
        sb
            .Append(Player.Index(Turn.now))
            .Append(codeData)
            .Append((int)GameData.gameFlow)
            .Append(codeData)
            .Append((int)Turn.turnAction)

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
        scItem.Clear();
        scEvent.Clear();
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

        // 초기화
        scInfo = null;
        scPlayers.Clear();
        scItem.Clear();
        scEvent.Clear();
        scTurn = null;

        List<string> code = loader.table[0];

        // 챕터별 세이브 파일 코드
        scInfo = code[0].Split(codeData);

        string[] pCode = code[1].Split(codeLine);
        for (int i = 0; i < pCode.Length; i++)
            scPlayers.Add(pCode[i].Split(codeData));

        //scItem = code[2].Split(codeData);

        //scEvent = code[3].Split(codeData);

        pCode = code[2].Split(codeLine);
        for (int i = 0; i < pCode.Length; i++)
            scItem.Add(pCode[i].Split(codeData));

        pCode = code[3].Split(codeLine);
        for (int i = 0; i < pCode.Length; i++)
            scEvent.Add(pCode[i].Split(codeData));

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

        // 노말 블럭 강화 단계
        BlockWork.plusBlockValue = int.Parse(scInfo[5]);
        BlockWork.minusBlockValue = int.Parse(scInfo[6]);

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

            // 위치 인덱스
            int loc = int.Parse(temp[5]);
            if (loc != -1)
            current.movement.location = loc;
            Vector3 rePos = current.movement.locateBlock.position;
            rePos.y = current.movement.transform.position.y;
            current.movement.transform.position = rePos;



            // 이동 불가 턴수
            current.stunCount = int.Parse(temp[6]);

            // 라이프
            current.life.Set(int.Parse(temp[7]));

            // 코인
            current.coin.Set(int.Parse(temp[8]));

            // 주사위 개수
            current.dice.count = int.Parse(temp[9]);

            // 주사위 합산값 (잔여 이동력)
            current.dice.SetValueTotal(int.Parse(temp[10]));

            // 주사위 기록값
            current.dice.valueRecord = int.Parse(temp[11]);
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
                int tempIndex = int.Parse(temp[12 + j * 2]);

                // 없는 아이템 생략
                if (tempIndex < 1)
                    continue;

                // 아이템 인덱스
                Item tempItem = Item.table[tempIndex];

                // 아이템 수량
                int tempCount = int.Parse(temp[13 + j * 2]);

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

        for (int i = 0; i < scItem.Count; i++)
        {
            // Out of Range 차단
            //if (2 + i * 3 >= scItem.Length)
            if (2 > scItem[i].Length)
                break;

            // 아이템 위치
            //int _loc = int.Parse(scItem[0 + i * 3]);
            int _loc = int.Parse(scItem[i][0]);

            // 아이템 인덱스
            //int _index = int.Parse(scItem[1 + i * 3]);
            int _index = int.Parse(scItem[i][1]);

            // 아이템 수량
            //int _count = int.Parse(scItem[2 + i * 3]);
            int _count = int.Parse(scItem[i][2]);


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

        for (int i = 0; i < scEvent.Count; i++)
        {
            // Out of Range 차단
            //if (3 + i * 4 >= scEvent.Length)
            if (3 > scEvent[i].Length)
            {
                Debug.LogError("데이터 로드 :: 실패 -> scEvent - Out of Range " + scEvent[i].Length);
                break;
            }

            // 이벤트 위치
            //int _loc = int.Parse(scEvent[0 + i * 4]);
            int _loc = int.Parse(scEvent[i][0]);

            // 이벤트 인덱스
            //int _index = int.Parse(scEvent[1 + i * 4]);
            int _index = int.Parse(scEvent[i][1]);

            // 이벤트 수량
            //int _count = int.Parse(scEvent[2 + i * 4]);
            int _count = int.Parse(scEvent[i][2]);

            // 이벤트 설치자
            //int _turnIndex = int.Parse(scEvent[3 + i * 4]);
            int _turnIndex = int.Parse(scEvent[i][3]);


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

        // 게임 플로우 셋팅
        GameMaster.flowCopy = (GameMaster.Flow)int.Parse(scTurn[1]);

        // 턴 플로우 셋팅
        Turn.turnAction = (Turn.TurnAction)int.Parse(scTurn[2]);
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

    public static FileInfo Save(string fileName, LockType useEncryptor, string strData)
    {
        string path = string.Format("{0}/{1}", CSVReader.copyPath, saveFloder);

        FileInfo fi = null;

        // 암호화 사용
        if (useEncryptor == LockType.Lock)
            fi = SaveFileCreate(
                    @path,
                    fileName,
                    Lock(Encoding.Default.GetBytes(strData))
                    );
        // 암호화 사용 안함
        else
            fi = SaveFileCreate(
                    @path,
                    fileName,
                    Encoding.Default.GetBytes(strData)
                    );

        return fi;
    }
    static FileInfo SaveFileCreate(string path, string fileName, byte[] byteData)
    {
        // 폴더 체크
        CSVReader.CheckPath(@path);

        // 파일 경로
        string fullPath = string.Format("{0}/{1}", @path, @fileName);

        // 파일 생성
        System.IO.FileStream fs = new System.IO.FileStream(@fullPath, System.IO.FileMode.Create);

        // 작성
        fs.Write(byteData, 0, byteData.Length);

        // 파일 닫기
        fs.Close();

        // 세이브 파일 반환
        return new FileInfo(fullPath);
    }

    public static string Read(string fileName, LockType useDecryptor)
    {
        string path = string.Format("{0}/{1}", CSVReader.copyPath, saveFloder);

        byte[] result = SaveFileRead(path, fileName);

        // 복호화 사용
        if (useDecryptor == LockType.Unlock)
            result = UnLock(result);

        return Encoding.Default.GetString(result);
    }
    static byte[] SaveFileRead(string path, string fileName)
    {
        // 폴더 및 파일 체크
        CSVReader.CheckFile(@path, @fileName);

        // 파일 경로
        string fullPath = string.Format("{0}/{1}", @path , @fileName);

        // 파일 열기
        System.IO.FileStream fs = new System.IO.FileStream(@fullPath, System.IO.FileMode.Open);

        // 결과물
        List<byte> result = new List<byte>();

        // 파일 읽기
        int data;
        while ((data = fs.ReadByte()) != -1)
            result.Add((byte)data);

        // 파일 닫기
        fs.Close();

        // 결과 반환
        return result.ToArray();
    }
}
