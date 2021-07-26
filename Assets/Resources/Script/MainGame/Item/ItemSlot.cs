using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour
{

    // �� ���� �ľ�
    public bool isEmpty {get { return item == null; } }

    // ������
    public Player owner = null;

    // ������
    //public Item item = null;
    Item _item = null;
    public Item item { get { return _item; } set { if (value == null) Clear(); else effect = value.effect; _item = value; } }
    Item itemMirror = null;


    // ������ ����
    public int count = 0;

    // ȿ��
    public IocEffect effect = IocEffect.New();

    // ������
    public Image icon = null;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ������ ����� �ڵ� ���ΰ�ħ
        if (item != itemMirror)
        {

            Refresh();
        }
    }

    public void Clear()
    {
        //item = Item.table[0];
        //item = null;
        _item = null;
        //count = 0;
        //effect = IocEffect.New();

        Refresh();
    }

    /// <summary>
    ///  �����ۿ� �°� ����
    /// </summary>
    public void Refresh()
    {
        // ������ �ε�
        if (item == null)
        {
            // ������ ����
            icon.sprite = LoadIcon(Item.empty);

            // ���� ����
            count = 0;

            // ȿ�� ����
            effect = IocEffect.New();
        }
        else
        {
            // ������ ����
            icon.sprite = LoadIcon(item);

            // ȿ�� ����
            effect = item.effect;
        }

        // ��ũ
        itemMirror = item;
    }

    public void CopyByMirror(ItemSlot mirror)
    {
        Debug.LogWarning("������ ��� :: " + mirror.item.name + " -> " + mirror.count);

        item = mirror.item;
        count = mirror.count;
        effect = mirror.effect;
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    public static Sprite LoadIcon(Item _item)
    {
        return _item.GetIcon();

        //// ������ �ε�
        //Debug.Log(@"Data/Item/icon/item" + _item.index.ToString("D4"));
        //Sprite temp = Resources.Load<Sprite>(@"Data/Item/icon/item" + _item.index.ToString("D4"));

        //// �̹��� ��ȿ �˻�
        //if (temp == null)
        //{
        //    // �⺻ ������ ��ü ó��
        //    Debug.Log(@"Data/Item/icon/item0000");
        //    temp = Resources.Load<Sprite>(@"Data/Item/icon/item0000");
        //}

        ////// ���� ���� ó��
        ////if (temp == null)
        ////    Debug.Log("�ε� ���� :: Data/Item/icon/item0000");
        ////// ������ ����
        ////else
        ////    _icon.sprite = temp;


        //// ������ ����
        //if (temp != null)
        //    return temp;

        //// ���� ���� ó��
        //Debug.Log("�ε� ���� :: Data/Item/icon/item0000");
        //return null;
    }

    /// <summary>
    /// ���� ������ ��� ó��
    /// </summary>
    public void Clicked()
    {
        // ���� ������ �ߴ�
        if (item == null)
            return;

        if (count <= 0)
            return;

        // �����
        ItemManager im = GameMaster.script.itemManager;

        // UI ����
        im.selected = this;
        Debug.LogWarning("������ :: �󼼺��� => " + GameData.gameMaster.itemManager.selected.item.index);
        im.nameText.text = item.name;
        im.infoText.text = item.info;

        // UI ȣ��
        im.CallItemUseBox();

        // ��� ��ư ��Ȱ��
        im.btnUse.interactable = false;

        // ������ ���� �� ��� ��ư ��Ȱ��
        if (transform.parent.name == "item") // �÷��̾� ���� UI ������Ʈ�� ���
        {
            PlayerInfoUI piui = transform.parent.parent.parent.parent.GetComponent<PlayerInfoUI>();

            if (piui.owner == Player.me) // ����ڰ� ������ ���������� ���
            {
                if (Turn.now == Player.me)    // ����ڰ� �� �������� ���
                {
                    // ��� ��ư Ȱ��
                    GameData.gameMaster.itemManager.btnUse.interactable = true;
                }

            }


        }
    }

}
