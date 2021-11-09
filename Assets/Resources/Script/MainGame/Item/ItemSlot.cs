using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemUnit
{
    // ������
    Item _item = null;
    public Item item { get { return _item; } set { if (value == null) Clear(); else effect = value.effect; _item = value; } }

    // ȿ��
    public IocEffect effect = IocEffect.New();
    
    // ������ ����
    //public int count = 0;
    public int count { get { return effect.count; } set { effect.SetCount(value); } }

    public bool isEmpty { get { return item == null || item == Item.empty; } }


    void Change(Item newItemOrNull)
    {
        if (newItemOrNull == null || newItemOrNull.index == 0) Clear();
        else { _item = newItemOrNull; effect = newItemOrNull.effect; }
    }

    void Clear()
    {
        _item = null;
        effect = IocEffect.New();
    }
}

public class ItemSlot : MonoBehaviour
{
    //public static operator ==(ItemSlot s1, ItemSlot s2) => ();
    //public static operator !=(ItemSlot s1, ItemSlot s2) => ();

    // �� ���� �ľ�
    public bool isEmpty {get { return item == null; } }

    // ������
    public Player owner = null;

    // ������
    ItemUnit itemUnit = null;
    public Item item {
        get {
            if (itemUnit == null)
                return null;
            else
                return itemUnit.item;
        }
        set
        {
            // �ٸ� �������� ���
            //if (value == null || itemUnit.item != value)
            if (itemUnit.item != value)
            {
                // ������ ��ü
                itemUnit.item = value;

                // ������ ���ΰ�ħ
                if (itemUnit.item != null)
                    icon.sprite = item.GetIcon();
                else
                    icon.sprite = Item.empty.GetIcon();
            }
            else
                Debug.LogWarning("������ :: �κ��丮 ������ ���� �źε� -> ������ ������");
        }
    }

    // ������
    public Image icon = null;

    // ����
    public int count { get { return itemUnit.count; } set { itemUnit.count = value; } }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //// ������ ����� �ڵ� ���ΰ�ħ
        //if (item != itemMirror)
        //{

        //    Refresh();
        //}
    }

    //public void Clear()
    //{
    //    //item = Item.table[0];
    //    //item = null;
    //    _item = null;
    //    //count = 0;
    //    //effect = IocEffect.New();

    //    Refresh();
    //}

    ///// <summary>
    /////  �����ۿ� �°� ����
    ///// </summary>
    //public void Refresh()
    //{
    //    // ������ �ε�
    //    if (item == null)
    //    {
    //        // ������ ����
    //        icon.sprite = LoadIcon(Item.empty);

    //        // ���� ����
    //        count = 0;

    //        // ȿ�� ����
    //        effect = IocEffect.New();
    //    }
    //    else
    //    {
    //        // ������ ����
    //        icon.sprite = item.GetIcon();

    //        // ȿ�� ����
    //        effect = item.effect;
    //    }

    //    // ��ũ
    //    itemMirror = item;
    //}

    //public void CopyByMirror(ItemSlot mirror)
    //{
    //    Debug.LogWarning("������ ��� :: " + mirror.item.name + " -> " + mirror.count);

    //    item = mirror.item;
    //    count = mirror.count;
    //    effect = mirror.effect;
    //}

    public void SetUp(Player _owner, ItemUnit _itemUnit)
    {
        owner = _owner;
        itemUnit = _itemUnit;

        // ������ ���ΰ�ħ
        //icon.sprite = item.GetIcon();
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

        // UI ����
        GameMaster.script.itemManager.selected = this;
        Debug.Log("������ :: �󼼺��� => " + GameData.gameMaster.itemManager.selected.item.index);
        GameMaster.script.itemManager.nameText.text = item.name;
        GameMaster.script.itemManager.infoText.text = item.info;

        // UI ȣ��
        GameMaster.script.itemManager.CallItemUseBox();

        // ��� ��ư ��Ȱ��
        GameMaster.script.itemManager.btnUse.interactable = false;

        // ������ ���� �� ��� ��ư ��Ȱ��
        if (transform.parent.name == "item") // �÷��̾� ���� UI ������Ʈ�� ���
        {
            PlayerInfoUI piui = transform.parent.parent.parent.parent.GetComponent<PlayerInfoUI>();

            if (piui.owner == Player.me) // ����ڰ� ������ ���������� ���
            {
                if (Turn.now == Player.me)    // ����ڰ� �� �������� ���
                {
                    if (Player.me.dice.valueTotal == 0)    // �ֻ��� ���� �ȱ����� ���
                    {
                        // ��� ��ư Ȱ��
                        GameData.gameMaster.itemManager.btnUse.interactable = true;
                    }
                }

            }


        }
    }

    public bool CheckUnit(ItemUnit _itemUnit)
    {
        return (itemUnit == _itemUnit);
    }

    public void Take(ItemSlot target)
    {
        Debug.Log("������ ��� :: " + target.item.name + " -> " + target.count);

        item = target.item;
        target.item = null;
    }

    public ItemUnit GetUnit()
    {
        return itemUnit;
    }

}
