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
    public Transform itemUseBox = null;
    public Text nameText = null;
    public Text infoText = null;
    public Button btnUse = null;


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
        MessageBox mb = GameData.gameMaster.messageBox;

        // �޽��� �ڽ� �ӽø���� ��� �ݱ�
        if (mb.pageSwitch.objectList[0].activeSelf)
            mb.PopUp(MessageBox.Type.Close);

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
            if (piuil[i].owner == GameData.player.me)
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

    public void ItemUse()
    {
        // ���� ����
        selected.count--;

        // Ÿ���� �� ������
        if (selected.item.type == Item.Type.Target)
        {
            CallPlayerSelecter();
            return;
        }
        else
            Debug.LogError("Ÿ���� �ƴ�");

        // ������ ���
        ItemUse(null);
    }

    public void ItemUse(Player targetPlayer_Or_null)
    {
        // UI ��Ȱ��
        GameData.gameMaster.playerSelecter[0].gameObject.SetActive(false);

        // ������ ����
        GameData.player.me.RemoveItem(selected);

        // ������ ��� ��û
        Item.Effect(selected.item, targetPlayer_Or_null);
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


        return obj.GetComponent<DynamicItem>();
    }

    /// <summary>
    /// Ư�� ��Ͽ� ������ ���� �� �ʱ�ȭ
    /// </summary>
    /// <param name="blockIndex">Ư�� ����� �ε�����</param>
    /// <param name="itemSlot">�ʱ�ȭ ��</param>
    public void CreateItemObject(int blockIndex, ItemSlot itemSlot)
    {
        // ������ ������Ʈ ���� �� ��ũ��Ʈ Ȯ��
        DynamicItem dItem = Create(blockIndex);

        // ������ ����
        dItem.SetUp(itemSlot);


        // ��Ͽ� �߰�
        itemObjectList.Add(dItem);
    }
    /// <summary>
    /// Ư�� ��Ͽ� ������ ���� �� �ʱ�ȭ
    /// </summary>
    /// <param name="blockIndex">���� ��ġ ����� �ε�����</param>
    /// <param name="itemIndex">�ʱ�ȭ �� : ������ �ε���</param>
    /// <param name="_count">�ʱ�ȭ �� : ����</param>
    /// <param name="_icon">�ʱ�ȭ �� : ������ ���ҽ�</param>
    public DynamicItem CreateItemObject(int blockIndex, int itemIndex, int _count, Sprite _icon)
    {
        Debug.LogError("������ ���� :: " + blockIndex + " ���� ������");

        // ������ ������Ʈ ���� �� ��ũ��Ʈ Ȯ��
        DynamicItem dItem = Create(blockIndex);

        // ������ ����
        dItem.SetUp(itemIndex, _count, _icon);


        // ��Ͽ� �߰�
        itemObjectList.Add(dItem);

        return dItem;
    }
}
