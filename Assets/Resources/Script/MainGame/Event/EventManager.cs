using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static List<DynamicEvent> eventObjectList = new List<DynamicEvent>();

    [Header("eventObject")]
    // ������ ������
    [SerializeField]
    GameObject eventPrefab = null;

    // �̺�Ʈ �۵� UI
    [Header("EventMessegeBox")]
    //public IocEvent selected = null;
    public Transform eventBox = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CallMessageBox()
    {
        MessageBox mb = GameData.gameMaster.messageBox;

        // �޽��� �ڽ� �ݱ�
        if (!mb.gameObject.activeSelf)
            mb.PopUp(0);

        // ȣ��
        eventBox.gameObject.SetActive(true);
    }

    public void CloseMessageBox()
    {
        MessageBox mb = GameData.gameMaster.messageBox;

        // �޽��� �ڽ� �ӽø���� ��� �ݱ�
        if (mb.pageSwitch.objectList[0].activeSelf)
            mb.PopUp(-1);

        // ��Ȱ��
        eventBox.gameObject.SetActive(false);
    }
    /*


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
        Transform obj = Instantiate(itemPrefab, pos, Quaternion.identity, transform).transform;


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
    */
}
