using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static List<DynamicItem> itemObjectList = new List<DynamicItem>();

    [Header("itemObject")]
    // ������ ������
    [SerializeField]
    GameObject itemPrefab = null;

    // ������ ��� UI
    [Header("itemUseMessegeBox")]
    public ItemSlot selected = null;
    public Player target = null;
    public Transform itemUseBox = null;
    public Text nameText = null;
    public Text infoText = null;
    public Button btnUse = null;

    [Header("itemObject")]
    // ���� ������
    public GameObject itemSlotPrefab = null;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CallItemUseBox()
    {
        MessageBox mb = GameData.gameMaster.messageBox;

        // �޽��� �ڽ� �ݱ�
        if (!mb.gameObject.activeSelf)
            mb.PopUp(0);

        // ȣ��
        itemUseBox.gameObject.SetActive(true);
    }

    public void CloseItemUseBox()
    {
        //MessageBox mb = GameData.gameMaster.messageBox;
        MessageBox mb = GameMaster.script.messageBox;

        // �޽��� �ڽ� �ӽø���� ��� �ݱ�
        if (mb.pageSwitch.objectList[0].activeSelf)
            mb.PopUp(MessageBox.Type.Close);

        // ��Ȱ��
        ResetItemUseBox();
    }

    public void ResetItemUseBox()
    {
        // ��Ȱ��
        itemUseBox.gameObject.SetActive(false);

        // Ÿ���� UI ��Ȱ��
        GameData.gameMaster.playerSelecter[0].gameObject.SetActive(false);

    }

    /// <summary>
    /// PlayerInfoUI�� �������� playerSelecter�� Ȱ��ȭ�ϸ� �ڽ��� �͸� ��Ȱ��
    /// </summary>
    public void CallPlayerSelecter()
    {
        List<PlayerInfoUI> piuil = GameData.gameMaster.playerInfoUI;
        for (int i = 0; i < piuil.Count; i++)
        {
            // ���� ��ư ��ü Ȱ��ȭ
            GameData.gameMaster.playerSelecter[i + 1].gameObject.SetActive(true);

            // �ڽ� ��Ȱ��
            if (piuil[i].owner == Player.me)
            {
                GameData.gameMaster.playerSelecter[i + 1].gameObject.SetActive(false);
                continue;
            }

            // �������� �ʴ� �÷��̾� ��Ȱ��
            if (piuil[i].owner == null)
            {
                GameData.gameMaster.playerSelecter[i + 1].gameObject.SetActive(false);
                continue;
            }

            // ����� ��Ȱ��
            if (piuil[i].owner.isDead)
            {
                GameData.gameMaster.playerSelecter[i + 1].gameObject.SetActive(false);
                continue;
            }
        }

        // UI Ȱ��
        GameData.gameMaster.playerSelecter[0].gameObject.SetActive(true);
    }

    public void BtnUse()
    {
        // ������ ��� ����
        GameMaster.useItemOrder = true;

        // UI ��ü ��Ȱ��
        GameData.gameMaster.playerSelecter[0].gameObject.SetActive(false);

        // �޽��� �ڽ� �ݱ�
        GameMaster.script.messageBox.PopUp(MessageBox.Type.Close);
    }

    /// <summary>
    /// UI�� ���� ���õ� �������� ���
    /// �ʿ�� Ÿ���� UI ȣ���
    /// </summary>
    public void ItemUseByUI() { ItemUseByUI(selected.owner); }
    /// <summary>
    /// UI�� ���� ���õ� �������� Ư�� ��󿡰� ���
    /// </summary>
    /// <param name="targetPlayer_Or_null"></param>
    public void ItemUseByUI(Player targetPlayer_Or_null)
    {
        // UI Ȱ��ȭ
        BtnUse();

        // ������ ���
        ItemUse(selected, targetPlayer_Or_null);

        // ���� �ʱ�ȭ
        selected = null;

        // ���ν�Ʈ�� Ż�� - ���
        GameMaster.useItemOrder = false;
    }


    /// <summary>
    /// Ư�� �������� ���
    /// �ʿ�� Ÿ���� UI ȣ���
    /// </summary>
    /// <param name="_slot"></param>
    public void ItemUse(ItemSlot _slot)
    {
        // Ÿ���� �� ������
        if (_slot.item.type == Item.Type.Target)
        {
            // Ÿ���� UI ȣ��
            CallPlayerSelecter();

            // ���ν�Ʈ�� Ż�� - ���
            GameMaster.useItemOrder = false;

            return;
        }

        // ������ ���
        ItemUse(_slot, _slot.owner);
    }
    /// <summary>
    /// Ư�� �������� Ư�� ��󿡰� ���
    /// </summary>
    /// <param name="_slot"></param>
    /// <param name="targetPlayer_Or_null"></param>
    public void ItemUse(ItemSlot _slot, Player targetPlayer_Or_null)
    {
        Debug.Log(string.Format("item use :: ������ = {0}   ��� = {1}", _slot.item.index, _slot.owner.name));

        // ���� ����
        _slot.count--;

        // UI ��Ȱ��
        //GameData.gameMaster.playerSelecter[0].gameObject.SetActive(false);

        // ������ ��� ��û
        //Item.Effect(_slot.item, targetPlayer_Or_null);
        // ItemUse�� ��ư���� ȣ��Ǳ� ������ ���⼭ �ߴ� �Ŵ� ��� ã�ƾ��ҵ�
        // �� ������ �Ŵ������� static ���� bool�� ���� ������ ����Ʈ���� �����ϰ� ���ν�Ʈ������ bool������ �ߴ�ó�� �ؾ��ҵ�
        StartCoroutine(_slot.item.Effect(targetPlayer_Or_null));

        // ������ ����
        Player.me.RemoveItem(_slot);
    }


    /// <summary>
    /// Ư�� ��Ͽ� ������ ����
    /// </summary>
    /// <param name="blockIndex">Ư�� ����� �ε��� ��</param>
    DynamicItem Create(int blockIndex)
    {
        // ������ ��ǥ
        Vector3 pos = GameData.blockManager.GetBlock(blockIndex).transform.position;

        // y�� ����
        pos.y = 2.5f;

        // ������ ������Ʈ ����
        Transform obj = Instantiate(itemPrefab, pos, Quaternion.identity ,transform).transform;

        // �����
        DynamicItem result = obj.GetComponent<DynamicItem>();

        // ��ġ ����
        result.location = blockIndex;


        return result;
    }

    /// <summary>
    /// Ư�� ��Ͽ� ������ ���� �� �ʱ�ȭ
    /// </summary>
    /// <param name="blockIndex">Ư�� ����� �ε�����</param>
    /// <param name="itemSlot">�ʱ�ȭ ��</param>
    public void CreateItemObject(int blockIndex, ItemSlot itemSlot)
    {
        CreateItemObject(
            blockIndex,
            itemSlot.item.index,
            itemSlot.count
            //itemSlot.icon.sprite
            );
    }
    /// <summary>
    /// Ư�� ��Ͽ� ������ ���� �� �ʱ�ȭ
    /// </summary>
    /// <param name="blockIndex">���� ��ġ ����� �ε�����</param>
    /// <param name="itemIndex">�ʱ�ȭ �� : ������ �ε���</param>
    /// <param name="_count">�ʱ�ȭ �� : ����</param>
    /// <param name="_icon">�ʱ�ȭ �� : ������ ���ҽ�</param>
    //public DynamicItem CreateItemObject(int blockIndex, int itemIndex, int _count, Sprite _icon)
    public DynamicItem CreateItemObject(int blockIndex, int itemIndex, int _count )
    {
        Debug.LogWarning("������ ���� :: " + blockIndex + " ���� ������");

        // ������ ������Ʈ ���� �� ��ũ��Ʈ Ȯ��
        DynamicItem dItem = Create(blockIndex);

        // ������ ����
        //dItem.SetUp(itemIndex, _count, _icon);
        dItem.SetUp(itemIndex, _count);


        // ��Ͽ� �߰�
        itemObjectList.Add(dItem);

        // ��ֹ� ���
        dItem.CreateBarricade();



        return dItem;
    }


    public static void ReCreateAll() { ReCreateAll(false); }
    public static void ReCreateAll(bool isDeleted)
    {
        Debug.LogError("������ ������Ʈ :: ����� ��û�� => �� "+itemObjectList.Count);

        // ���
        List<DynamicItem> temp = itemObjectList;

        // �ʱ�ȭ
        itemObjectList = new List<DynamicItem>();

        // �ݺ� �����
        for (int i = 0; i < temp.Count; i++)
        {
            DynamicItem dTemp = temp[i];

            // ����Ʈ �� ��ֹ� ����
            //dTemp.Remove();
            dTemp.RemoveBarricade();

            // ����
            DynamicItem dNew =
            GameMaster.script.itemManager.CreateItemObject(
                dTemp.location,
                dTemp.item.index,
                dTemp.count
                //dTemp.icon
                );

            Tool.ThrowParabola(dNew.transform, dNew.transform.position, 2f, 1f);

            // ����
            if (!isDeleted)
                Destroy(dTemp.transform);
        }
    }




    public void Tester(string blockIndex_itemIndex_value)
    {
        int blockIndex = 7;
        int itemIndex = 1;
        int value = 1;

        string[] str = blockIndex_itemIndex_value.Split('_');
        int.TryParse(str[0], out blockIndex);
        if(str.Length > 1)
        int.TryParse(str[1], out itemIndex);
        if (str.Length > 2)
            int.TryParse(str[2], out value);

        DynamicItem di =
        CreateItemObject(
            blockIndex,
            itemIndex,
            value
            //ItemSlot.LoadIcon(Item.table[itemIndex])
            );

        Tool.ThrowParabola(di.transform, di.transform.position, 2f, 1f);
    }


    /// <summary>
    /// ������ ���� ����
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <param name="_count"></param>
    /// <returns></returns>
    public ItemSlot CreateItemSlot(Item _item, int _count)
    {
        // ������ ��ǥ
        Vector3 pos = GameData.blockManager.startBlock.position;

        // y�� ����
        pos.y = -5f;

        // ������ ������Ʈ ����
        ItemSlot result = Instantiate(itemSlotPrefab, pos, Quaternion.identity, GameMaster.script.transform).GetComponent<ItemSlot>();

        result.item = _item;
        result.count = _count;

        return result;
    }
}
