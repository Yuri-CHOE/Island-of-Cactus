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


    //public void CallMessageBox()
    //{
    //    MessageBox mb = GameData.gameMaster.messageBox;

    //    // �޽��� �ڽ� �ӽø��
    //    if (!mb.gameObject.activeSelf)
    //        mb.PopUp(0);

    //    // ȣ��
    //    eventBox.gameObject.SetActive(true);
    //}

    //public void CloseMessageBox()
    //{
    //    MessageBox mb = GameData.gameMaster.messageBox;

    //    // �޽��� �ڽ� �ӽø���� ��� �ݱ�
    //    if (mb.pageSwitch.objectList[0].activeSelf)
    //        mb.PopUp(MessageBox.Type.Close);

    //    // ��Ȱ��
    //    eventBox.gameObject.SetActive(false);
    //}



    /// <summary>
    /// Ư�� ��Ͽ� ������ ����
    /// </summary>
    /// <param name="blockIndex">Ư�� ����� �ε��� ��</param>
    DynamicEvent Create(int blockIndex)
    {
        // ������ ��ǥ
        Vector3 pos = GameData.blockManager.GetBlock(blockIndex).transform.position;

        // y�� ����
        pos.y = 2.5f;

        // �̺�Ʈ ������Ʈ ����
        Transform obj = Instantiate(eventPrefab, pos, Quaternion.identity, transform).transform;

        // �����
        DynamicEvent result = obj.GetComponent<DynamicEvent>();

        // ��ġ ����
        result.location = blockIndex;


        return result;
    }

    /// <summary>
    /// Ư�� ��Ͽ� �̺�Ʈ ���� �� �ʱ�ȭ
    /// </summary>
    /// <param name="blockIndex">���� ��ġ ����� �ε�����</param>
    /// <param name="eventIndex">�ʱ�ȭ �� : �̺�Ʈ �ε���</param>
    /// <param name="_count">�ʱ�ȭ �� : ����</param>
    /// <param name="creator">�ʱ�ȭ �� : �̺�Ʈ ������</param>
    public DynamicEvent CreateEventObject(int blockIndex, int eventIndex, int _count, Player creator)
    {
        // �̺�Ʈ ������Ʈ ���� �� ��ũ��Ʈ Ȯ��
        DynamicEvent dEvent = Create(blockIndex);

        // �̺�Ʈ ����
        dEvent.SetUp(eventIndex, _count, creator);

        Debug.LogWarning("�̺�Ʈ ���� :: [" + dEvent.iocEvent.name + "] �̺�Ʈ�� " + blockIndex + " ���� ������");


        // ��Ͽ� �߰�
        eventObjectList.Add(dEvent);

        // ��ֹ� ���
        dEvent.CreateBarricade();

        return dEvent;
    }


    public static void ReCreateAll() { ReCreateAll(false); }
    public static void ReCreateAll(bool isDeleted)
    {
        Debug.LogError("�̺�Ʈ ������Ʈ :: ����� ��û�� => �� " + eventObjectList.Count);

        // ���
        List<DynamicEvent> temp = eventObjectList;

        // �ʱ�ȭ
        eventObjectList = new List<DynamicEvent>();

        // �ݺ� �����
        for (int i = 0; i < temp.Count; i++)
        {
            DynamicEvent dTemp = temp[i];

            // ����Ʈ �� ��ֹ� ����
            //dTemp.Remove();
            dTemp.RemoveBarricade();

            // ����
            GameMaster.script.eventManager.CreateEventObject(
                dTemp.location,
                dTemp.iocEvent.index,
                dTemp.count,
                dTemp.creator
                );

            // ����
            if (!isDeleted)
                Destroy(dTemp.transform);            
        }
    }









    public void Tester(string blockIndex_eventID)
    {
        int blockIndex = 7;
        int eventID = 1;

        string[] str = blockIndex_eventID.Split('_');
        int.TryParse(str[0], out blockIndex);
        if (str.Length > 1)
            int.TryParse(str[1], out eventID);

        DynamicEvent de =
        CreateEventObject(
            blockIndex,
            eventID, 
            1,
            Player.me
            );
    }
}
