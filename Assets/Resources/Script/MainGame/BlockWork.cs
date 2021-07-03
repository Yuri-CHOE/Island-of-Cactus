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
    static public int nomalValue = 0;

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


        // �ʱ�ȭ
        Clear();

        // ����� ���� ó��
        isWork = true;


        // ��ġ ����
        int location = currentPlayer.movement.location;

        // ��� ���� �ľ�
        BlockType.TypeDetail blockType = GetBlockType(location);

        Debug.LogError(currentPlayer.name);
        Debug.LogError(blockType);
        //Debug.Break();

        // ��� �� ���
        if (blockType == BlockType.TypeDetail.none)
            return;

        else if (blockType == BlockType.TypeDetail.plus)
            BlockPlus(currentPlayer);
        else if (blockType == BlockType.TypeDetail.minus)
            BlockMinus(currentPlayer);

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

        boss,
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
        int coinValue = 1 + nomalValue;

        // ���� �߰�
        currentPlayer.coin.Add(coinValue);

        // ���� ����
        isEnd = true;
    }


    static void BlockMinus(Player currentPlayer)
    {
        int coinValue = 2 + nomalValue;

        // ���� ����
        currentPlayer.coin.subtract(coinValue);

        // ���� ����
        isEnd = true;
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
        DynamicItem obj = GameData.itemManager.CreateItemObject(currentPlayer.movement.location, index, coinValue, ItemSlot.LoadIcon(Item.table[index]));

        // ��ġ�� ��ġ
        int loc = GameData.blockManager.indexLoop(currentPlayer.movement.location, -1);
        Debug.LogError("���� ��ġ : " + loc);
        Vector3 pos = GameData.blockManager.GetBlock(loc).transform.position;


        // ������ ������
        Tool.ThrowParabola(obj.transform, pos, liftY, 1f);

        // ������Ʈ ��ġ
        obj.location = loc;

        // ��ֹ� ���
        CharacterMover.barricade[loc]++;

        // ���� ���� -> DynamicItem ���� ��� �浹�Ҷ� ó��
        //isEnd = true;
    }


    static void BlockLuckyBox(Player currentPlayer)
    {
        // ��Ű�ڽ� ������̺�
        DropTable dropTable = new DropTable();

        // ������̺� ����
        dropTable.rare = new List<int>();
        Debug.LogError(LuckyBox.table.Count);
        for (int i = 1; i < LuckyBox.table.Count; i++)
        {
            dropTable.rare.Add(LuckyBox.table[i].rare);
            Debug.Log("��� ���̺� :: �߰��� -> " + LuckyBox.table[i].rare);
        }

        // ��� ���̺� �۵� �� ������ �ε��� Ȯ��
        int select = 1 + dropTable.Drop();
        Debug.LogError("��Ű�ڽ� :: ���õ� -> "+ select);


        // ��Ű�ڽ� ���� ����
        LuckyBoxManager lbm = LuckyBoxManager.obj;

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
        lbm.coroutineOpen = lbm.StartCoroutine(lbm.WaitAndResult());

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
        Debug.LogError(Item.table.Count);
        for (int i = first; i < Item.table.Count; i++)
        {
            dropTable.rare.Add(Item.table[i].rare);
            Debug.Log("��� ���̺� :: �߰��� -> " + Item.table[i].rare);
        }


        // ��� ���̺� �۵�
        List<int> select = dropTable.Drop(ItemShop.script.bundle.Count);

        // ������ ���� ����
        for (int i = 0; i < ItemShop.script.bundle.Count; i++)
        {
            int trueIndex = select[i] + first;

            // ��� ���̺� �۵�
            Debug.LogWarning("������ ���� :: �߰��� -> " + trueIndex);
             
            // ������ ���� ���
            ItemShop.script.SetItemBundle(i, Item.table[trueIndex]);
        }

        // UI ���
        GameMaster.script.messageBox.PopUp(true, true, true, MessageBox.Type.Itemshop);


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

        // �� ���
        ShortcutManager sm = ShortcutManager.script;

        // ��� ���� UI ȣ��
        sm.CallShortcutUI(currentPlayer);

    }

}
