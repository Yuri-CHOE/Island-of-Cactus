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
        Debug.LogWarning("������ ���� :: " + blockIndex + " ���� ������");

        // ������ ������Ʈ ���� �� ��ũ��Ʈ Ȯ��
        DynamicEvent dEvent = Create(blockIndex);

        // ������ ����
        dEvent.SetUp(eventIndex, _count, creator);


        // ��Ͽ� �߰�
        eventObjectList.Add(dEvent);

        // ��ֹ� ���
        dEvent.CreateBarricade();

        return dEvent;
    }


    public static void ReCreateAll()
    {
        // ���
        List<DynamicEvent> temp = eventObjectList;

        // �ʱ�ȭ
        eventObjectList = new List<DynamicEvent>();

        // �ݺ� �����
        for (int i = 0; i < temp.Count; i++)
        {
            DynamicEvent dTemp = temp[i];

            // ����Ʈ �� ��ֹ� ����
            dTemp.Remove();

            // ����
            GameMaster.script.eventManager.CreateEventObject(
                dTemp.location,
                dTemp.iocEvent.index,
                dTemp.count,
                dTemp.creator
                );

            // ����
            Destroy(dTemp.transform);            
        }
    }









    public void Tester(int eventID)
    {
        CreateEventObject(
            GameData.player.me.movement.location, 
            eventID, 
            1,
            GameData.player.me
            );
    }
}
