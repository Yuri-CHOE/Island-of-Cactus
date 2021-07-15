using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicEvent : MonoBehaviour
{
    // �̺�Ʈ ������Ʈ
    public Transform eventObject = null;


    // �̺�Ʈ ��ġ
    public int location = -2;


    // ������ �÷��̾�
    public Player creator = null;

    // �̺�Ʈ ����  =====  (�̺�Ʈ ����, ȹ�� ��� ���� �߰��ؾ��� ��� ���⿡ �߰�)
    public IocEvent iocEvent = null;
    public int count = 0;


    // ��� �غ�
    public bool isReady = false;


    // �ִϸ��̼� ���� ����
    public bool doAnimate = false;
    //// �ִϸ��̼� ����
    //Coroutine coroutineAnimate = null;
    //bool isAnimate = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    // �浹�� ���
    private void OnTriggerEnter(Collider target)
    {
        // ĳ���Ϳ� �浹�� ���
        if (target.transform.parent.name == "Character")
        {

        }
    }

    // �浹���� ���
    //private void OnTriggerStay(Collider target)
    //{
    //    // ĳ���Ϳ� �浹�� ���
    //    if (target.transform.parent.name == "Character")
    //    {

    //    }

    //}

    //    // �浹���� ��� ���
    //void OnTriggerExit(Collider target)
    ////
    //{
    //    // ĳ���Ϳ� �浹���� ��� ���
    //    if (target.transform.parent.name == "Character")
    //        transform.GetComponent<BoxCollider>().isTrigger = false;
    //}


    public void GetEvent(Player current)
    {
        // �۵�
        IocEvent.Effect(iocEvent, current);

        // ��� ����
        EventManager.eventObjectList.Remove(this);

        // ��ֹ� ����
        CharacterMover.barricade[location]--;

        // ����
        Destroy(gameObject);
    }

    /// <summary>
    /// ������Ʈ ����
    /// </summary>
    /// <param name="index">�ε���</param>
    /// <param name="_count"> ����</param>
    public void SetUp(int index, int _count, Player _creator)
    {
        // ������ ���� ����
        iocEvent = IocEvent.table[index];

        // ������ ���� ����
        count = _count;

        // ������ ����
        creator = _creator;

        // �� ������Ʈ üũ
        GameObject obj = Resources.Load<GameObject>(@"Data/Event/Event" + iocEvent.index.ToString("D4"));
        if (obj == null)
        {
            obj = Resources.Load<GameObject>(@"Data/Event/Event0000");
            Debug.Log(@"Data/Event/Event0000");
        }
        if (obj == null)
            Debug.LogError("�ε� ���� :: Data/Event/Event0000");

        // �� ������Ʈ ���� �� ����
        eventObject = Instantiate(
            obj,
            transform.position,
            Quaternion.identity,
            transform
            ).transform;


        // �غ� �Ϸ�
        isReady = true;
    }

}
