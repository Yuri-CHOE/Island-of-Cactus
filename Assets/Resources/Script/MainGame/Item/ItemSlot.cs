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
        /// �̱���===========
        /// ������ �޽��� �ڽ��� ������ ���� �� �ݱ��ư ��� + �ڽ��� ��, �ڽ��� �������� ��� ����ư ���
    }

}
