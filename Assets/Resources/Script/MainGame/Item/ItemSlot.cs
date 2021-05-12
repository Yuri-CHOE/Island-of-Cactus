using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour
{
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
            Refresh();
    }

    /// <summary>
    ///  �����ۿ� �°� ����
    /// </summary>
    void Refresh()
    {
        // ������ �ε�
        LoadIcon();

        // ��ũ
        itemMirror = item;
    }

    /// <summary>
    /// ������ �ε�
    /// </summary>
    void LoadIcon()
    {
        // ������ �ε�
        Debug.Log(@"Data/Item/icon/item" + item.index.ToString("D4"));
        Sprite temp = Resources.Load<Sprite>(@"Data/Item/icon/item" + item.index.ToString("D4"));

        // �̹��� ��ȿ �˻�
        if (temp == null)
        {
            // �⺻ ������ ��ü ó��
            Debug.Log(@"Data/Item/icon/item0000");
            temp = Resources.Load<Sprite>(@"Data/Item/icon/item0000");
        }

        // ���� ���� ó��
        if (temp == null)
            Debug.Log("�ε� ���� :: Data/Item/icon/item0000");
        // ������ ����
        else
            icon.sprite = temp;
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