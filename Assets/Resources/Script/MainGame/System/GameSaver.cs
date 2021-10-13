using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using System.IO;
using System.Security.Cryptography;

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
    public static List<string[]> scItem = new List<string[]>();
    public static List<string[]> scEvent = new List<string[]>();
    public static string[] scTurn = null;


    // �ε� ȣ�� ����
    public static bool useLoad = false;

    // ��ȣȭ ��� ����
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
        // ���� ��� ������ ���� �ߴ� - �ʼ� :: ���Ӹ��� ���ϸ� ����
        if (GameData.gameMode == GameMode.Mode.None)
        {
            Debug.LogError("game save :: ���� ���� " + "���� ��� -> " + GameData.gameMode);
            return;
        }

        // ��� ����
        string path = string.Format("{0}/{1}", CSVReader.copyPath, saveFloder);
        string fullPath = string.Format("{0}/{1}{2}", @path, @fileName, extension);

        // ���� Ȯ��
        FileInfo fi = new FileInfo(@fullPath);
        if (fi.Exists)
        {
            fi.Delete();

            Debug.LogError("���̺� ���� :: ���ŵ� -> " + fileName);
        }
        else
            Debug.LogError("���̺� ���� :: ���� �Ұ� -> ���� ����");
    }

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
        string codeStr = code.ToString();

        // ����
        FileInfo saveF0 = Save(fileName + extension, LockType.None, codeStr);
        Debug.LogError("���� ������ :: " + saveF0);
        //CSVReader.SaveNew(saveFloder, fileName + extension, true, true, codeStr);

        // ��ȣȭ ����
        FileInfo saveF1 = Save(fileName + "_" + extension, LockType.Lock, codeStr);
        Debug.LogError("���� ������ :: " + saveF1);

        // ��ȣȭ ����  
        FileInfo saveF2 =
            Save(
                fileName + "__" + extension,
                LockType.None,
                Read(saveF1.Name, LockType.Unlock)
                );
        Debug.LogError("���� ������ :: " + saveF2);
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

            if (i >= 0 && i != ItemManager.itemObjectList.Count - 1)
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

            if (i >= 0 && i != EventManager.eventObjectList.Count - 1)
                sb.Append(codeLine);
        }
        sb.Append(codeChapter);


        // ��Ȳ ����
        sb
            .Append(Player.Index(Turn.now))
            .Append(codeData)
            .Append((int)GameData.gameFlow)
            .Append(codeData)
            .Append((int)Turn.turnAction)

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
        scItem.Clear();
        scEvent.Clear();
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

        // �ʱ�ȭ
        scInfo = null;
        scPlayers.Clear();
        scItem.Clear();
        scEvent.Clear();
        scTurn = null;

        List<string> code = loader.table[0];

        // é�ͺ� ���̺� ���� �ڵ�
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

        // �븻 �� ��ȭ �ܰ�
        BlockWork.plusBlockValue = int.Parse(scInfo[5]);
        BlockWork.minusBlockValue = int.Parse(scInfo[6]);

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

            // ��ġ �ε���
            int loc = int.Parse(temp[5]);
            if (loc != -1)
            current.movement.location = loc;
            Vector3 rePos = current.movement.locateBlock.position;
            rePos.y = current.movement.transform.position.y;
            current.movement.transform.position = rePos;



            // �̵� �Ұ� �ϼ�
            current.stunCount = int.Parse(temp[6]);

            // ������
            current.life.Set(int.Parse(temp[7]));

            // ����
            current.coin.Set(int.Parse(temp[8]));

            // �ֻ��� ����
            current.dice.count = int.Parse(temp[9]);

            // �ֻ��� �ջ갪 (�ܿ� �̵���)
            current.dice.SetValueTotal(int.Parse(temp[10]));

            // �ֻ��� ��ϰ�
            current.dice.valueRecord = int.Parse(temp[11]);
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
                int tempIndex = int.Parse(temp[12 + j * 2]);

                // ���� ������ ����
                if (tempIndex < 1)
                    continue;

                // ������ �ε���
                Item tempItem = Item.table[tempIndex];

                // ������ ����
                int tempCount = int.Parse(temp[13 + j * 2]);

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

        for (int i = 0; i < scItem.Count; i++)
        {
            // Out of Range ����
            //if (2 + i * 3 >= scItem.Length)
            if (2 > scItem[i].Length)
                break;

            // ������ ��ġ
            //int _loc = int.Parse(scItem[0 + i * 3]);
            int _loc = int.Parse(scItem[i][0]);

            // ������ �ε���
            //int _index = int.Parse(scItem[1 + i * 3]);
            int _index = int.Parse(scItem[i][1]);

            // ������ ����
            //int _count = int.Parse(scItem[2 + i * 3]);
            int _count = int.Parse(scItem[i][2]);


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

        for (int i = 0; i < scEvent.Count; i++)
        {
            // Out of Range ����
            //if (3 + i * 4 >= scEvent.Length)
            if (3 > scEvent[i].Length)
            {
                Debug.LogError("������ �ε� :: ���� -> scEvent - Out of Range " + scEvent[i].Length);
                break;
            }

            // �̺�Ʈ ��ġ
            //int _loc = int.Parse(scEvent[0 + i * 4]);
            int _loc = int.Parse(scEvent[i][0]);

            // �̺�Ʈ �ε���
            //int _index = int.Parse(scEvent[1 + i * 4]);
            int _index = int.Parse(scEvent[i][1]);

            // �̺�Ʈ ����
            //int _count = int.Parse(scEvent[2 + i * 4]);
            int _count = int.Parse(scEvent[i][2]);

            // �̺�Ʈ ��ġ��
            //int _turnIndex = int.Parse(scEvent[3 + i * 4]);
            int _turnIndex = int.Parse(scEvent[i][3]);


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

        // ���� �÷ο� ����
        GameMaster.flowCopy = (GameMaster.Flow)int.Parse(scTurn[1]);

        // �� �÷ο� ����
        Turn.turnAction = (Turn.TurnAction)int.Parse(scTurn[2]);
    }




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

    public static FileInfo Save(string fileName, LockType useEncryptor, string strData)
    {
        string path = string.Format("{0}/{1}", CSVReader.copyPath, saveFloder);

        FileInfo fi = null;

        // ��ȣȭ ���
        if (useEncryptor == LockType.Lock)
            fi = SaveFileCreate(
                    @path,
                    fileName,
                    Lock(Encoding.Default.GetBytes(strData))
                    );
        // ��ȣȭ ��� ����
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
        // ���� üũ
        CSVReader.CheckPath(@path);

        // ���� ���
        string fullPath = string.Format("{0}/{1}", @path, @fileName);

        // ���� ����
        System.IO.FileStream fs = new System.IO.FileStream(@fullPath, System.IO.FileMode.Create);

        // �ۼ�
        fs.Write(byteData, 0, byteData.Length);

        // ���� �ݱ�
        fs.Close();

        // ���̺� ���� ��ȯ
        return new FileInfo(fullPath);
    }

    public static string Read(string fileName, LockType useDecryptor)
    {
        string path = string.Format("{0}/{1}", CSVReader.copyPath, saveFloder);

        byte[] result = SaveFileRead(path, fileName);

        // ��ȣȭ ���
        if (useDecryptor == LockType.Unlock)
            result = UnLock(result);

        return Encoding.Default.GetString(result);
    }
    static byte[] SaveFileRead(string path, string fileName)
    {
        // ���� �� ���� üũ
        CSVReader.CheckFile(@path, @fileName);

        // ���� ���
        string fullPath = string.Format("{0}/{1}", @path , @fileName);

        // ���� ����
        System.IO.FileStream fs = new System.IO.FileStream(@fullPath, System.IO.FileMode.Open);

        // �����
        List<byte> result = new List<byte>();

        // ���� �б�
        int data;
        while ((data = fs.ReadByte()) != -1)
            result.Add((byte)data);

        // ���� �ݱ�
        fs.Close();

        // ��� ��ȯ
        return result.ToArray();
    }
}
