using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicItem : DynamicObject
{    
    // ������ ������Ʈ
    public Transform itemObject = null;

    // ������ ������Ʈ
    public SpriteRenderer iconObject = null;


    // ������ ��ġ
    //public int location = -2;         // ��ӹ��� Ŭ����(DynamicObject)�� ������


    // ������ ����  =====  (������ ����, ȹ�� ��� ���� �߰��ؾ��� ��� ���⿡ �߰�)
    public Item item = null;
    //public int count = 0;             // ��ӹ��� Ŭ����(DynamicObject)�� ������
    public Sprite icon = null;

    // ��� �غ�
    //public bool isReady = false;      // ��ӹ��� Ŭ����(DynamicObject)�� ������


    // ȸ�� ���� ����
    public bool doSpin = false;      
    // ȸ�� ����
    Coroutine coroutineRot = null;
    bool isSpin = false;

    DynamicItem()
    {
        type = Type.Item;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerStay(Collider target)
    {
        // ��� �浹�� ���
        if (target.transform.name == "road")
        {
            // �浹 �ź� ��� ��Ȱ��
            //transform.GetComponent<BoxCollider>().isTrigger = false;

            // ���� ����
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // ��ġ ����
            transform.position = GameData.blockManager.GetBlock(location).transform.position;

            // ���� ����
            BlockWork.isEnd = true;
            
            // ȸ�� ����
            Spin(true);
        }

    }

    //void OnTriggerExit(Collider target)
    ////
    //{
    //    // ĳ���Ϳ� �浹���� ��� ���
    //    if (target.transform.parent.name == "Character")
    //        transform.GetComponent<BoxCollider>().isTrigger = false;
    //}


    /// <summary>
    /// �̺�Ʈ �۵� ����
    /// </summary>
    /// <param name="current">�۵���Ų �÷��̾�</param>
    public override bool CheckCondition(Player current)
    {
        return current.inventoryCount < Player.inventoryMax;
    }

    public void GetItem(Player current)
    {
        // ȹ��
        current.AddItem(item, count);

        // ��� �� ��ֹ� ����
        Remove();

        // ����
        Destroy(gameObject);
    }

    /// <summary>
    /// ����Ʈ �� �ٸ�����Ʈ ����
    /// </summary>
    public void Remove()
    {
        // ��� ����
        ItemManager.itemObjectList.Remove(this);

        // ��ֹ� ����
        RemoveBarricade();
    }

    /// <summary>
    /// ������Ʈ ����
    /// </summary>
    /// <param name="itemIndex">������ �ε���</param>
    /// <param name="_count"> ����</param>
    /// <param name="_icon">������</param>
    //public void SetUp(int itemIndex, int _count, Sprite _icon)
    public void SetUp(int itemIndex, int _count)
    {
        // ������ ���� ����
        item = Item.table[itemIndex];

        // ������ ���� ����
        count = _count;

        // ������ ������ ����
        //icon = _icon;
        icon = item.GetIcon();
        iconObject.sprite = icon;


        // �غ� �Ϸ�
        isReady = true;
    }
    ///// <summary>
    ///// ������ �������� ������Ʈ ����
    ///// </summary>
    ///// <param name="itemSlot">������ ����</param>
    //public void SetUp(ItemSlot itemSlot)
    //{
    //    SetUp(
    //        itemSlot.item.index,
    //        itemSlot.count,
    //        itemSlot.icon.sprite
    //        );

    //    //// ������ ���� ����
    //    //item = itemSlot.item;

    //    //// ������ ���� ����
    //    //count = itemSlot.count;

    //    //// ������ ������ ����
    //    //icon = itemSlot.icon.sprite;
    //    //iconObject.sprite = icon;


    //    //// �غ� �Ϸ�
    //    //isReady = true;

    //    //// ��ֹ� ���
    //    //CreateBarricade();
    //}


    /// <summary>
    /// ȸ�� ��� ����
    /// </summary>
    /// <param name="_isOn">��� ����</param>
    public void Spin(bool _isOn)
    {
        // �ʱ�ȭ �ȉ��� ��� �ߴ�
        if (!isReady)
        {
            Debug.LogError("��� ���� :: ������ ������Ʈ �ʱ�ȭ �ȵ� ���·� ȸ�� ��û��");
            Debug.Break();
            return;
        }

        doSpin = _isOn;

        // ȸ�� ����
        if (doSpin)
        {
            // �̹� ȸ������ �ƴҶ�
            if (!isSpin)
            {
                // ȸ���� ó��
                isSpin = true;


                // ȸ�� �׼� �ʱ�ȭ
                if(coroutineRot != null)
                    StopCoroutine(coroutineRot);

                // ȸ�� �׼� ����
                coroutineRot = StartCoroutine(AcrTurn());
            }
        }
        // ȸ�� ����
        else
        {
            // �̹� ȸ���� �϶�
            if (isSpin)
            {
                // ȸ�� ���� ó��
                isSpin = false;

                // ȸ�� �׼� �ʱ�ȭ
                StopCoroutine(coroutineRot);
            }


        }
    }

    IEnumerator AcrTurn()
    {
        // ȸ�� �۵�
        while (doSpin)
        {
            Tool.SpinY(itemObject, 3f);

            yield return null;
        }
    }


}
