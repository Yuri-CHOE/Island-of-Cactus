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

        // ���� ����
        public int area;                // GameRule.area
        public int section;             // GameRule.section
        public int cycleNow;            // Cycle.now
        public int cycleGoal;           // Cycle.goal
        public int playerCount;         // Player.allPlayer.Count
        public int plusBlockValue;      // BlockWork.plusBlockValue
        public int minusBlockValue;     // BlockWork.minusBlockValue

        // �÷��̾� ����
        public SaveFormPlayer[] player;

        // ������ ������Ʈ ��ġ
        public SaveFormObjectStack[] itemObjects;

        // �̺�Ʈ ������Ʈ ��ġ
        public SaveFormObjectStack[] eventObjects;

        // ��Ȳ ����
        public int turnNow;                     // Player.Index(Turn.now))
        public GameMaster.Flow gameFlow;        // GameData.gameFlow
        public Turn.TurnAction turnAction;      // Turn.turnAction
               

        /// <summary>
        /// �ʱ�ȭ�� ����ϴ� ������
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
        /// ���� ���� ����
        /// </summary>
        public void LoadGameInfo()
        {
            // ���� ����
            GameRule.area = area;
            Debug.Log("�ε� :: ���� ���� -> GameRule.area = " + GameRule.area);

            // ���� ����
            GameRule.section = section;
            Debug.Log("�ε� :: ���� ���� -> GameRule.section = " + GameRule.section);

            // ����Ŭ ���� - ����
            Cycle.now = cycleNow;
            Debug.Log("�ε� :: ���� ���� -> Cycle.now = " + Cycle.now);

            // ����Ŭ ���� - ��ǥ
            GameRule.cycleMax = cycleGoal;
            Debug.Log("�ε� :: ���� ���� -> GameRule.cycleMax = " + GameRule.cycleMax);

            // �÷��� �ο���
            GameRule.playerCount = playerCount;
            Debug.Log("�ε� :: ���� ���� -> GameRule.playerCount = " + GameRule.playerCount);

            // �븻 �� ��ȭ �ܰ�
            BlockWork.plusBlockValue =  plusBlockValue;
            Debug.Log("�ε� :: ���� ���� -> BlockWork.plusBlockValue = " + BlockWork.plusBlockValue);
            BlockWork.minusBlockValue = minusBlockValue;
            Debug.Log("�ε� :: ���� ���� -> BlockWork.minusBlockValue = " + BlockWork.minusBlockValue);

            Debug.LogWarning("�ε� :: ���� ���� -> �Ϸ��");
        }

        /// <summary>
        /// �÷��̾� ����
        /// </summary>
        public void LoadPlayer()
        {
            if(Player.allPlayer.Count == 0)
            {
                Debug.LogError("error :: Player.allPlayer ���ǵǱ� �� LoadPlayer() ȣ���");
                Debug.Break();
                return;
            }

            Player current = null;
            for (int i = 0; i < Player.allPlayer.Count; i++)
            {
                current = Player.allPlayer[i];
                Debug.Log("�ε� :: �÷��̾� " + current.name);

                //// �� �ε���
                //current.dice.SetValue(100 - player[i].turnIndex);
                //current.dice.isRolled = true;
                //Debug.Log("�ε� :: �÷��̾� -> �� �ֻ���" + current.name);

                // �÷��̾� Ÿ��, ĳ���� �ε���, �����÷���, �÷��̾� �̸�
                current.SetPlayer(
                    player[i].type,
                    player[i].characterIndex,
                    player[i].isAutoPlay,
                    player[i].name
                    );
                Debug.Log(string.Format("�ε� :: �÷��̾� ���� ���� -> {0}Ÿ���� {1}�� {2}�� �ڵ��÷���={3} ����", current.type, current.name, current.character.index, current.isAutoPlay));

                // ��ġ �ε���
                int loc = player[i].location;
                if (loc != -1)
                    current.movement.location = loc;
                Vector3 rePos = current.movement.locateBlock.position;
                rePos.y = current.movement.transform.position.y;
                current.movement.transform.position = rePos;

                Debug.Log("�ε� :: �÷��̾� " + current.name+"�� ��ġ -> " + current.location);


                // �̵� �Ұ� �ϼ�
                current.stunCount = player[i].stunCount;
                Debug.Log("�ε� :: �÷��̾� " + current.name + "�� ���� -> " + current.stunCount);

                // ������
                current.life = player[i].life;
                Debug.Log("�ε� :: �÷��̾� " + current.name + "�� ������ -> " + current.life.Value);

                // ����
                current.coin = player[i].coin;
                Debug.Log("�ε� :: �÷��̾� " + current.name + "�� ���� -> " + current.coin.Value);

                // �ֻ��� ����
                current.dice = player[i].dice;
                Debug.Log("�ε� :: �÷��̾� " + current.name + "�� �ֻ��� -> " + (current.dice.value + current.dice.valueTotal));

                // �̴ϰ��� ����
                current.miniInfo = player[i].miniScore;
                Debug.Log("�ε� :: �÷��̾� " + current.name + "�� �̴ϰ��� -> " + current.miniInfo.score + " �� ��� -> " + current.miniInfo.rank);

                // �κ��丮
                for (int j = 0; j < current.inventory.Count; j++)
                {
                    Debug.LogError(string.Format("�ε� :: �÷��̾� {0} �� �κ��� {1} ĭ�̰� {2}��° ĭ���� {3}\n",
                        current.name,
                        current.inventory.Count,
                        j,
                        current.inventory[j].item
                        ) + string.Format("�ε� :: �÷��̾� {0} �� �κ��� {1} ĭ�̰� {2}��° ĭ���� {3}\n",
                        player[i].name,
                        player[i].inven.Length,
                        j,
                        player[i].inven[j].index
                        ) + string.Format("�ε� :: �÷��̾� {0} �� {1}�� {2}���� �̴� ",
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
        /// ������ ������Ʈ ����
        /// </summary>
        public void LoadItemObject()
        {
            if (GameData.itemManager == null)
            {
                Debug.LogError("error :: itemManager�� �����Ǳ� �� LoadItemObject() ����");
                Debug.Break();
                return;
            }

            for (int i = 0; i < itemObjects.Length; i++)
            {
                // ������ ��ġ
                int _loc = itemObjects[i].loaction;

                // ������ �ε���
                int _index = itemObjects[i].index;

                // ������ ����
                int _count = itemObjects[i].count;


                // ������ ����
                GameData.itemManager.CreateItemObject(_loc, _index, _count);
            }
        }

        /// <summary>
        /// �̺�Ʈ ������Ʈ ����
        /// </summary>
        public void LoadEventObject()
        {
            if (GameData.eventManager == null)
            {
                Debug.LogError("error :: eventManager�� �����Ǳ� �� LoadEventObject() ����");
                Debug.Break();
                return;
            }

            for (int i = 0; i < eventObjects.Length; i++)
            {
                // �̺�Ʈ ��ġ
                int _loc = eventObjects[i].loaction;

                // �̺�Ʈ �ε���
                int _index = eventObjects[i].index;

                // �̺�Ʈ ����
                int _count = eventObjects[i].count;

                // �̺�Ʈ ��ġ��
                int _turnIndex = eventObjects[i].ownerIndex;


                // �̺�Ʈ ����
                GameData.eventManager.CreateEventObject(_loc, _index, _count, Player.allPlayer[_turnIndex]);
            }
        }

        /// <summary>
        /// ��Ȳ ����
        /// </summary>
        public void LoadTurn()
        {
            if (Turn.queue == null || Turn.queue.Count == 0)
            {
                Debug.LogError("���̺� :: Turn ��ũ��Ʈ ���� �ȵ�");
                Debug.Break();
                return;
            }

            // ���� �� ����
            Turn.Skip(Player.allPlayer[turnNow]);
            Debug.Log("���̺� :: ���� �� ���� -> " + Turn.now);

            // ���� �÷ο� ����
            GameMaster.flowCopy = gameFlow;

            // �� �÷ο� ����
            Turn.turnAction = turnAction;
        }
    }


    // ���̺� ���� ������
    static string path = string.Format("{0}/{1}", CSVReader.copyPath, saveFloder);
    static string saveFloder = "Save";
    static string fullPath { get { return string.Format("{0}/{1}/{2}{3}", CSVReader.copyPath, saveFloder, fileName, extension); } }

    // ���̺� ���� ���ϸ�
    static string fileName { get { return GameData.gameMode.ToString(); } }
    static string extension = ".iocs";

    // ���̺� ����
    public static SaveForm saveForm;

    // ���̺� ���� ���� ����
    static char codeEnder = '#';
    static char codeChapter = '$';
    static char codeLine = '|';
    static char codeData = ',';

    // é�ͺ� ���̺� ���� �ڵ�
    public static string[] scInfo = null;
    public static List<string[]> scPlayers = new List<string[]>();
    public static List<string[]> scItem = new List<string[]>();
    public static List<string[]> scEvent = new List<string[]>();
    public static string[] scTurn = null;


    // �ε� ȣ�� ����
    public static bool useLoad = false;

    // ��ȣȭ ��� ����
    //static bool useEncrypt = true;
    static bool useEncrypt = false;
    static string password = "This_is_Password";
    static string vec = "GrowupGrowupGrowupGrowup";

    // ���̺� ���� ����
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
        // ���� ��� ������ ���� �ߴ� - �ʼ� :: ���Ӹ��� ���ϸ� ����
        if (GameData.gameMode == GameMode.Mode.None)
        {
            Debug.LogError("game save :: ���̺����� ���� ���� " + "���� ��� -> " + GameData.gameMode);
            return;
        }
        // ���� ��� ������ ���� �ߴ� - �ʼ� :: ���Ӹ��� ���ϸ� ����
        if (saveFileInfo == null || !saveFileInfo.Exists)
        {
            Debug.LogError("game save :: ���̺����� ���� ���� " + "���� Ȯ�� �Ұ�");
            return;
        }

        // ����
        Debug.LogError("���̺� ���� :: ���� ��û�� -> " + saveFileInfo.FullName);
        saveFileInfo.Delete();
        Debug.LogError("���̺� ���� :: ���� �������� -> " + saveFileInfo.Exists);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <returns>���� ����</returns>
    public static bool GameSave1()
    //public static IEnumerator GameSave()
    {
        try
        {
            // ������ -> ���̺� �ڵ� ��ȯ
            SaveCode();
            string codeStr = saveCodeBuilder.ToString();

            // ��������
            bool result = false;

            // ��ȣȭ ������ ����
            if (useEncrypt)
                result = Save(LockType.Lock, codeStr);
            else
                result = Save(LockType.None, codeStr);
            Debug.LogError("���� ������ :: " + saveFileInfo.FullName);

            return result;
        }
        catch
        {
            // ���� ��� ������ ���� �ߴ� - �ʼ� :: ���Ӹ��� ���ϸ� ����
            if (GameData.gameMode == GameMode.Mode.None)
            {
                Debug.LogError("game save :: ���� ���� " + "���� ��� -> " + GameData.gameMode);
                return false;
            }
            // ���� ���� �ƴϸ� �ߴ�
            if (GameData.gameFlow <= GameMaster.Flow.Ordering || GameData.gameFlow >= GameMaster.Flow.End)
            {
                Debug.LogError("game save :: ���� ���� " + "���� �÷ο� -> " + GameData.gameMode);
                return false;
            }
        }
        return false;
    }


    /// <summary>
    /// ������ -> ���̺� �ڵ� ��ȯ
    /// </summary>
    //static StringBuilder SaveCode()
    static void SaveCode()
    {
        saveCodeBuilder.Clear();

        // ���� ����
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

        // �÷��̾� ����
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
                    // ��ȿ�� ������
                    saveCodeBuilder
                        .Append(temp.inventory[j].item.index)
                        .Append(codeData)
                        .Append(temp.inventory[j].count)
                        ;
                }
                else
                {
                    // ������ ����
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


        // ������ ������Ʈ ��ġ
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


        // �̺�Ʈ ������Ʈ ��ġ
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


        // ��Ȳ ����
        saveCodeBuilder
            .Append(Player.Index(Turn.now))
            .Append(codeData)
            .Append((int)GameData.gameFlow)
            .Append(codeData)
            .Append((int)Turn.turnAction)

            //.Append(codeChapter)
            ;


        // ���� ����
        saveCodeBuilder.Append('#');

        //return saveCodeBuilder;
    }

    //public static void CodeLoad()
    //{
    //    // ���ϸ�
    //    string fName = GameData.gameMode.ToString() + extension;

    //    // ���� �б�
    //    List<List<string>> loader = new List<List<string>>();
    //    string temp = Read(LockType.Unlock);
    //    loader.Add(new List<string>(
    //                                temp.Split(codeEnder)[0]
    //                                    .Split(codeChapter)
    //                                ));
    //    Debug.LogError(temp);


    //    // ���� üũ
    //    //if (loader.table.Count == 0)
    //    if (loader.Count == 0)
    //    {
    //        Debug.LogWarning("miss :: ���̺� ���� ����");
    //        return;
    //    }

    //    // �ʱ�ȭ
    //    scInfo = null;
    //    scPlayers.Clear();
    //    scItem.Clear();
    //    scEvent.Clear();
    //    scTurn = null;

    //    //List<string> code = loader.table[0];
    //    List<string> code = loader[0];

    //    // é�ͺ� ���̺� ���� �ڵ�
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
        //Ű�� ����
        byte[] keyBytes = Encoding.Default.GetBytes(_password);

        //��Ʈ��(�������� �˱� ��ư� �ϴ� ��)
        byte[] saltBytes = SHA512.Create().ComputeHash(keyBytes);

        //password�� ��ȣȭ�� Ű ����, 100000�� �ؽ� ������ �ݺ� Ƚ��
        Rfc2898DeriveBytes result = new Rfc2898DeriveBytes(keyBytes, saltBytes, 100000);

        return result;  //Ű�� ��ȯ

        // ��ó : https://fred16157.github.io/.net/csharp-encryption/
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
        // ����Ʈ�� ��ȯ
        //byte[] origin = Encoding.Default.GetBytes(code);
        //Debug.LogError(origin.Length);

        //AES �˰���
        RijndaelManaged aes = new RijndaelManaged();

        //Ű�� ����
        Rfc2898DeriveBytes key = CreateKey(password);

        //���� ���� 
        //Rfc2898DeriveBytes vector = CreateKey("GrowupGrowupGrowupGrowup");
        Rfc2898DeriveBytes vector = CreateKey(vec);

        aes.BlockSize = 128;            //AES�� ��� ũ��� 128 ����
        aes.KeySize = 256;              //AES�� Ű ũ��� 128, 192, 256�� �����Ѵ�.
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key.GetBytes(32);     //AES-256�� ����ϹǷ� Ű���� ���̴� 32���� �Ѵ�.
        aes.IV = vector.GetBytes(16);   //�ʱ�ȭ ���ʹ� ������ ���̰� 16�̾�� �Ѵ�.

        //Debug.LogError("key :: " + Encoding.Default.GetString(aes.Key));
        //Debug.LogError("vector :: " + Encoding.Default.GetString(aes.IV));

        //Ű���� �ʱ�ȭ ���͸� ������� ��ȣȭ �Ǵ� ��ȣȭ �۾��� �ϴ� Ŭ���� ������ ����
        ICryptoTransform cryptor;
        if (useEncryptor == LockType.Lock)
            cryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        else if (useEncryptor == LockType.Unlock)
            cryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        else
            return origin;

        //using������� ������ ����ϸ� ��Ͽ��� ���ö� �ڵ����� ������ �������÷��� �ȴ�. 
        using (MemoryStream ms = new MemoryStream()) //����� ���� ��Ʈ�� 
        {
            //cryptor �������� ��ȣȭ �Ǵ� ��ȣȭ�� �����͸� ����� ���� ��Ʈ��
            using (CryptoStream cs = new CryptoStream(ms, cryptor, CryptoStreamMode.Write))
            {
                cs.Write(origin, 0, origin.Length);
            }
            //return Encoding.UTF8.GetString(ms.ToArray());    //��ȣȭ�� ��Ʈ�� ��ȯ
            return ms.ToArray();
        }

        // ��ó : https://fred16157.github.io/.net/csharp-encryption/
    }


    /// <summary>
    /// ���̺����� ����
    /// </summary>
    /// <param name="fileName">��ΰ� ���ܵ� ���ϸ�� Ȯ����</param>
    /// <param name="useEncryptor">�Ϲ��б�, ��ȣȭ, ��ȣȭ</param>
    /// <param name="strData">���� ����</param>
    /// <returns></returns>
    public static bool Save(LockType useEncryptor, string strData)
    {
        // ��ȣȭ ���
        if (useEncryptor == LockType.Lock)
            return SaveFileCreate(
                    Lock(Encoding.Default.GetBytes(strData))
                    );
        // ��ȣȭ ��� ����
        else
            return SaveFileCreate(
                    Encoding.Default.GetBytes(strData)
                    );
    }
    static bool SaveFileCreate(byte[] byteData)
    {
        // ���� ��� ������ ���� �ߴ� - �ʼ� :: ���Ӹ��� ���ϸ� ����
        if (GameData.gameMode == GameMode.Mode.None)
        {
            Debug.LogError("game save :: ���̺����� ���� ���� " + "���� ��� -> " + GameData.gameMode);
            return false;
        }

        // ���� ����
        using (FileStream fs = new FileStream(@fullPath, FileMode.Create))
        {
            // �ۼ�
            fs.Write(byteData, 0, byteData.Length);
        }


        saveFileInfo = new FileInfo(@fullPath);

        return saveFileInfo.Exists;
    }

    //public static string Read(LockType useDecryptor)
    //{
    //    byte[] result = SaveFileRead();

    //    // ��ȣȭ ���
    //    if (useDecryptor == LockType.Unlock)
    //        result = UnLock(result);

    //    return Encoding.Default.GetString(result);
    //}
    //static byte[] SaveFileRead()
    //{
    //    // �߸��� ���� ����
    //    if (saveFileStream == null)
    //    {
    //        Debug.LogWarning("���̺� :: ���� �������� -> ���� ���� �õ�");
    //        SaveFileOpen();
    //    }
    //    if (saveFileStream == null)
    //    {
    //        Debug.LogError("���̺� :: ���� �б� ����");
    //        Debug.Break();
    //        //return ;
    //        return null;
    //    }

    //    // �����
    //    List<byte> result = new List<byte>();

    //    // ���� ù ��ġ��
    //    saveFileStream.Seek(0, SeekOrigin.Begin);

    //    // ���� �б�
    //    int data;
    //    while ((data = saveFileStream.ReadByte()) != -1)
    //        result.Add((byte)data);

    //    // ��� ��ȯ
    //    return result.ToArray();
    //}

    //static void SaveFileOpen()
    //{
    //    // ���� �ҷ� �ߴ�
    //    if (saveFileInfo == null || !saveFileInfo.Exists)
    //    {
    //        Debug.LogError("���̺� :: ���� ���� ���� -> ���� Ȯ�� �Ұ�");
    //        Debug.Break();
    //        return;
    //    }

    //    Debug.Log("���̺� :: ���� ���� -> " + saveFileInfo.FullName);
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
        // ���� ����
        //using (FileStream fs = new FileStream(saveFileInfo.FullName, FileMode.Open, FileAccess.Write))
        using (FileStream fs = new FileStream(@fullPath, FileMode.Create, FileAccess.Write))
        {
            Debug.LogError("���̺� :: ���� �ۼ� ��û�� -> " + @fullPath);

            BinaryFormatter bf = new BinaryFormatter();

            if (useEncryptor == LockType.None)
            {
                bf.Serialize(fs, new SaveForm(0));
                Debug.LogError("���̺� :: ��ȣȭ ��� ���� " + fs.CanWrite);
            }
            else
            {
                //cryptor �������� ��ȣȭ �Ǵ� ��ȣȭ�� �����͸� ����� ���� ��Ʈ��
                using (CryptoStream cs = new CryptoStream(fs, GetCcryptor(useEncryptor), CryptoStreamMode.Write))
                {
                    bf.Serialize(cs, new SaveForm(0));
                }
                Debug.LogError("���̺� :: ��ȣȭ ��� " + fs.CanWrite);
            }
        }

        // ���� ����
        saveFileInfo = new FileInfo(@fullPath);
        Debug.LogError("���̺� :: ���� �ۼ� ��� -> " + saveFileInfo.Exists);

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
        // ���� üũ
        if (saveFileInfo == null)
        {
            Debug.LogWarning("error :: ���̺� ���� ���� �ȵ�");

            saveFileInfo = new FileInfo(@fullPath);

            // ���� üũ
            if (!saveFileInfo.Exists)
            {
                Debug.LogError("error :: ���̺� ���� ����");
                Debug.Break();
                return false;
            }
            else
                Debug.Log("���̺� :: ���̺� ���� ���� ����");
        }

        // ���� ����
        using (MemoryStream ms = new MemoryStream())
        {
            // ���� ����Ʈȭ
            byte[] origin = File.ReadAllBytes(@saveFileInfo.FullName);
            Debug.Log("���̺� :: ���� ���� ����");

            // ��ȣȭ ����
            if (useDecryptor == LockType.None)
            {
                Debug.Log("���̺� :: ��ȣȭ �ʿ� ����");
                //ms.Read(origin, 0, origin.Length);
                ms.Write(origin, 0, origin.Length);
            }
            // ��ȣȭ
            else if (useDecryptor == LockType.Unlock)
            {
                Debug.Log("���̺� :: ��ȣȭ �ʿ���");
                using (CryptoStream cs = new CryptoStream(ms, GetCcryptor(useDecryptor), CryptoStreamMode.Write))
                {
                    Debug.LogError(origin.Length);
                    Debug.LogError(ms.Length);
                    Debug.LogError(cs.Length);
                    cs.Write(origin, 0, origin.Length);
                    Debug.LogError(cs.Length);
                }
            }
            Debug.Log("���̺� :: ���� ������ȭ ����");

            // �б�
            BinaryFormatter bf = new BinaryFormatter();
            ms.Position = 0;
            saveForm = (SaveForm)bf.Deserialize(ms);
            Debug.Log("���̺� :: ���� ������ȭ ����");
        }
        try
        {

            Debug.Log("���̺� :: ���� �б� ����");
            return true;
        }
        catch
        {
            Debug.LogError("���̺� :: ���� �б� ����");
            return false;
        }
    }

    public static ICryptoTransform GetCcryptor(LockType useEncryptor)
    {

        //AES �˰���
        RijndaelManaged aes = new RijndaelManaged();

        //Ű�� ����
        Rfc2898DeriveBytes key = CreateKey(password);

        //���� ���� 
        Rfc2898DeriveBytes vector = CreateKey(vec);

        aes.BlockSize = 128;            //AES�� ��� ũ��� 128 ����
        aes.KeySize = 256;              //AES�� Ű ũ��� 128, 192, 256�� �����Ѵ�.
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = key.GetBytes(32);     //AES-256�� ����ϹǷ� Ű���� ���̴� 32���� �Ѵ�.
        aes.IV = vector.GetBytes(16);   //�ʱ�ȭ ���ʹ� ������ ���̰� 16�̾�� �Ѵ�.

        //Debug.LogError("key :: " + Encoding.Default.GetString(aes.Key));
        //Debug.LogError("vector :: " + Encoding.Default.GetString(aes.IV));

        bool isSkip = false;

        //Ű���� �ʱ�ȭ ���͸� ������� ��ȣȭ �Ǵ� ��ȣȭ �۾��� �ϴ� Ŭ���� ������ ����
        ICryptoTransform cryptor;
        if (useEncryptor == LockType.Lock)
            cryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        else if (useEncryptor == LockType.Unlock)
            cryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        else return null;

        return cryptor;

        // ���� ��ó : https://fred16157.github.io/.net/csharp-encryption/
    }
}
