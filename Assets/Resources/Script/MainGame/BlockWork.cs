using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockWork
{
    // ����� ����
    public static bool isWork = false; 

    // ���� ����
    public static bool isEnd = false; 

    // ������ ����
    static float liftY = 3f;

    // �븻 ��� ����ġ
    static public int plusBlockPlus = 0;
    static public int plusBlockMultiple = 1;
    static public int minusBlockPlus = 0;
    static public int minusBlockMultiple = 1;

    /// <summary>
    /// �÷��̾��� ���� ��ġ�� ������� ��� �۾� ����
    /// </summary>
    /// <param name="currentPlayer">�÷��̾�</param>
    public static void Work(Player currentPlayer)
    {
        //Debug.Break();
        // ���� ����
        if (currentPlayer == null)
            return;
        if (currentPlayer.avatar == null)
            return;
        if (currentPlayer.movement == null)
            return;
        if (currentPlayer.movement.location == -1)
            return;


        // �ʱ�ȭ
        Clear();

        // �޽��� �ڽ� ���� ����
        GameMaster.script.messageBox.Close();

        // ����� ���� ó��
        isWork = true;


        // ��ġ ����
        int location = currentPlayer.movement.location;

        // ��� ���� �ľ�
        BlockType.TypeDetail blockType = GetBlockType(location);

        Debug.Log("��� ��� :: " + currentPlayer.name  +" �� ���� �۵��� => "+ blockType );
        //Debug.Break();

        // ��� �� ���
        if (blockType == BlockType.TypeDetail.none)
            return;

        else if (blockType == BlockType.TypeDetail.plus)
            BlockPlus(currentPlayer);
        else if (blockType == BlockType.TypeDetail.minus)
            BlockMinus(currentPlayer);

        else if (blockType == BlockType.TypeDetail.boss)
            BlockBoss(currentPlayer);
        else if (blockType == BlockType.TypeDetail.trap)
            BlockTrap(currentPlayer);
        else if (blockType == BlockType.TypeDetail.lucky)
            BlockLuckyBox(currentPlayer);

        else if (blockType == BlockType.TypeDetail.shop)
            BlockShop(currentPlayer); 
        else if (blockType == BlockType.TypeDetail.unique)
            BlockUnique(currentPlayer); 
        else if (blockType == BlockType.TypeDetail.shortcutIn)
            BlockshortcutIn(currentPlayer);
        else if (blockType == BlockType.TypeDetail.shortcutOut)
            BlockPlus(currentPlayer);


        /*
        
        => �ʵ� ���� �Ϸ� : --
        => �׽�Ʈ �Ϸ� : ++
        


        ++none,

        ++plus,
        ++minus,

        boss,               ============== �̱���
        ++trap,
        ++lucky,

        ++shop,
        unique,
        ++shortcutIn,
        ++shortcutOut, 
         */
    }

    public static void Clear()
    {
        // ���� ���� �ʱ�ȭ
        isWork = false;

        // ���� ���� �ʱ�ȭ
        isEnd = false;
    }


    /// <summary>
    /// ��� Ÿ�� ����
    /// </summary>
    /// <param name="blockIndex"></param>
    /// <returns></returns>
    public static BlockType.TypeDetail GetBlockType(int blockIndex)
    {
        return  GameData.blockManager.GetBlock(blockIndex).GetComponent<DynamicBlock>().blockTypeDetail;
    }


    static void BlockPlus(Player currentPlayer)
    {
        int coinValue = 1 * plusBlockMultiple + plusBlockPlus;

        // ���� �߰�
        currentPlayer.coin.Add(coinValue);

        // ���� ����
        isEnd = true;
    }


    static void BlockMinus(Player currentPlayer)
    {
        int coinValue = 2 * minusBlockMultiple + minusBlockPlus;

        // ���� ����
        currentPlayer.coin.subtract(coinValue);

        // ���� ����
        isEnd = true;
    }


    static void BlockBoss(Player currentPlayer)
    {
        // ���� ����
        int reward = 100;

        // �̴ϰ���
        // �̱���==================
        Minigame mini = Minigame.RandomGame(2);

        


        // ������
        List<Player> entry = new List<Player>();
        entry.Add(Player.system.Monster);
        entry.Add(currentPlayer);

        // ȣ��
        GameMaster.script.loadingManager.LoadAsyncMiniGame(mini, reward, entry);
    }


    static void BlockTrap(Player currentPlayer)
    {
        // ���� ���� Ȯ��
        int coinValue = currentPlayer.coin.Value;

        // ���� ������ �ߴ�
        if (coinValue <= 0)
        {
            // ���� ����
            isEnd = true;

            return;
        }

        // ���� ����
        currentPlayer.coin.subtract(coinValue);


        // ���� ������ �ε���
        int index = 1;


        // ������Ʈȭ
        //DynamicItem obj = GameData.itemManager.CreateItemObject(currentPlayer.movement.location, index, coinValue, ItemSlot.LoadIcon(Item.table[index]));
        DynamicItem obj = GameData.itemManager.CreateItemObject(currentPlayer.movement.location, index, coinValue);

        // ��ġ�� ��ġ
        int loc = GameData.blockManager.indexLoop(currentPlayer.movement.location, -1);
        Debug.Log("Ʈ�� ��� : ���� ��ġ => " + loc);
        Vector3 pos = GameData.blockManager.GetBlock(loc).transform.position;


        // ������ ������
        Tool.ThrowParabola(obj.transform, pos, liftY, 1f);

        // ������Ʈ ���ġ
        obj.RemoveBarricade();
        obj.location = loc;
        obj.CreateBarricade();

        // ��ֹ� ���
        //DynamicObject.objectList[loc]++;          // ������ �ڵ� ���

        // ���� ����
        obj.DelayedBlockWorkEnd();
    }


    static void BlockLuckyBox(Player currentPlayer)
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
        Debug.Log("��� ���̺� :: ��� �ѷ� ->" + dropTable.rare.Count);

        // ��� ���̺� �۵� �� ������ �ε��� Ȯ��
        int select = 1 + dropTable.Drop();
        //select = 19;
        Debug.Log("��Ű�ڽ� :: ���õ� -> "+ select);

        // ��Ű�ڽ� ���� ����
        LuckyBoxManager lbm = LuckyBoxManager.script;

        // ���� �ʱ�ȭ
        lbm.ClearForced();

        // ��ȯ
        lbm.GetLuckyBox(currentPlayer);

        // ����
        lbm.Open();

        // �ڷ�ƾ �ʱ�ȭ
        if(lbm.coroutineOpen != null)
            lbm.StopCoroutine(lbm.coroutineOpen);
        // ���, ��� ���, ȿ�� ����, ���� ����
        lbm.coroutineOpen = lbm.StartCoroutine(lbm.WaitAndResult(LuckyBox.table[select], currentPlayer));

        // ��Ʈ�� �Է�
        lbm.SetTextByIndex(select);
    }


    static void BlockShop(Player currentPlayer)
    {
        // �����۹��� ������̺�
        DropTable dropTable = new DropTable();

        // ù��° ������ �δ콺 (���̺�� ���� ���� => 2)
        int first = 2;

        // ������̺� ����
        dropTable.rare = new List<int>();
        Debug.Log("��� ���̺� :: ��� �ѷ� ->" + Item.table.Count);
        for (int i = first; i < Item.table.Count; i++)
        {
            dropTable.rare.Add(Item.table[i].rare);
            Debug.Log("��� ���̺� :: �߰��� -> " + Item.table[i].rare);
        }


        // ��� ���̺� �۵�
        List<int> select = dropTable.Drop(ItemShop.script.bundle.Count);

        // ������ ���� ����
        int trueIndex;
        for (int i = 0; i < ItemShop.script.bundle.Count; i++)
        {
            trueIndex = select[i] + first;

            // ��� ���̺� �۵�
            Debug.Log("������ ���� :: �߰��� -> [" + trueIndex + "] " + Item.table[trueIndex].name);
             
            // ������ ���� ���
            ItemShop.script.SetItemBundle(i, Item.table[trueIndex]);
        }

        // UI ���
        //GameMaster.script.messageBox.PopUp(true, true, true, MessageBox.Type.Itemshop);
        //GameMaster.script.messageBox.PopUp(MessageBox.Type.Itemshop);
        ItemShop.script.OpenShop();


        // ���������� UI���� ��ư Ŭ������ ó����

    }


    static void BlockUnique(Player currentPlayer)
    {
        // �۵�
        UniqueManager.script.Active(currentPlayer);

        // ���� ����
        //isEnd = true;
    }


    static void BlockshortcutIn(Player currentPlayer)
    {
        /*
         
        ��� ���� UI ���
        ��� �źν� �ߴ� �� ��������

        ���� ��� ����
        �̵�
        ���� �� ���� ����
         
         */

        // ��� ���� UI ȣ��
        ShortcutManager.script.CallShortcutUI(currentPlayer);

    }

}
