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


        /*
        
        => �ʵ� ���� �Ϸ� : --
        => �׽�Ʈ �Ϸ� : ++
        


        --none,

        --plus,
        --minus,

        boss,
        --trap,
        lucky,

        shop,
        unique,
        shortcutIn,
        shortcutOut, 
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
        int coinValue = 1;

        // ���� �߰�
        currentPlayer.coin.Add(coinValue);

        // ���� ����
        isEnd = true;
    }


    static void BlockMinus(Player currentPlayer)
    {
        int coinValue = 2;

        // ���� ����
        currentPlayer.coin.subtract(coinValue);

        // ���� ����
        isEnd = true;
    }


    static void BlockTrap(Player currentPlayer)
    {
        // ���� ���� Ȯ��
        int coinValue = currentPlayer.coin.Value;

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

        // ���� ����
        isEnd = true;
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
            Debug.LogError("��� ���̺� :: �߰��� -> " + LuckyBox.table[i].rare);
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

}
