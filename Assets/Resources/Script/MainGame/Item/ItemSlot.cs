using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour
{

    // �� ���� �ľ�
    public bool isEmpty {get { return item == null; } }

    // ������
    public Item item = null;
    Item itemMirror = null;


    // ������ ����
    public int count = 0;

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
        item = Item.table[0];
        count = 0;

        Refresh();
    }

    /// <summary>
    ///  �����ۿ� �°� ����
    /// </summary>
    public void Refresh()
    {
        // ������ �ε�
        icon.sprite = LoadIcon(item);

        // ��ũ
        itemMirror = item;
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    public static Sprite LoadIcon(Item _item)
    {
        // ������ �ε�
        Debug.Log(@"Data/Item/icon/item" + _item.index.ToString("D4"));
        Sprite temp = Resources.Load<Sprite>(@"Data/Item/icon/item" + _item.index.ToString("D4"));

        // �̹��� ��ȿ �˻�
        if (temp == null)
        {
            // �⺻ ������ ��ü ó��
            Debug.Log(@"Data/Item/icon/item0000");
            temp = Resources.Load<Sprite>(@"Data/Item/icon/item0000");
        }

        //// ���� ���� ó��
        //if (temp == null)
        //    Debug.Log("�ε� ���� :: Data/Item/icon/item0000");
        //// ������ ����
        //else
        //    _icon.sprite = temp;


        // ������ ����
        if (temp != null)
            return temp;

        // ���� ���� ó��
        Debug.Log("�ε� ���� :: Data/Item/icon/item0000");
        return null;
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
        GameData.gameMaster.itemManager.selected = this;
        Debug.LogError(GameData.gameMaster.itemManager.selected.item.index);
        GameData.gameMaster.itemManager.nameText.text = item.name;
        GameData.gameMaster.itemManager.infoText.text = item.info;

        // UI ȣ��
        GameMaster.script.itemManager.CallItemUseBox();

        // ��� ��ư ��Ȱ��
        GameData.gameMaster.itemManager.btnUse.interactable = false;

        // ������ ���� �� ��� ��ư ��Ȱ��
        if (transform.parent.name == "item") // �÷��̾� ���� UI ������Ʈ�� ���
        {

            PlayerInfoUI piui = transform.parent.parent.parent.parent.GetComponent<PlayerInfoUI>();
            
            if (piui.owner == GameData.player.me) // ����ڰ� ������ ���������� ���
            {
                if (GameData.turn.now == GameData.player.me)    // ����ڰ� �� �������� ���
                {
                    // ��� ��ư Ȱ��
                    GameData.gameMaster.itemManager.btnUse.interactable = true;
                }

            }


        }
    }

}
