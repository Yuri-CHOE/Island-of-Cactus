using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicEvent : DynamicObject
{
    // �̺�Ʈ ������Ʈ
    public Transform eventObject = null;


    // �̺�Ʈ ��ġ
    //public int location = -2;             // ��ӹ��� Ŭ����(DynamicObject)�� ������


    // ������ �÷��̾�
    public Player creator = null;

    // �̺�Ʈ ����  =====  (�̺�Ʈ ����, ȹ�� ��� ���� �߰��ؾ��� ��� ���⿡ �߰�)
    public IocEvent iocEvent = null;
    //public int count = 0;                 // ��ӹ��� Ŭ����(DynamicObject)�� ������


    // ��� �غ�
    //public bool isReady = false;          // ��ӹ��� Ŭ����(DynamicObject)�� ������


    // �ִϸ��̼� ���� ����
    public bool doAnimate = false;
    //// �ִϸ��̼� ����
    //Coroutine coroutineAnimate = null;
    //bool isAnimate = false;

    DynamicEvent()
    {
        type = Type.Event;
    }

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




    /// <summary>
    /// �̺�Ʈ �۵� ����
    /// </summary>
    /// <param name="current">�۵���Ų �÷��̾�</param>
    public override bool CheckCondition(Player current)
    {
        return IocEvent.Condition(iocEvent, current, creator);
    }

    public void GetEvent(Player user)
    {
        // �۵�
        IocEvent.Effect(iocEvent, user);

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
        EventManager.eventObjectList.Remove(this);

        // ��ֹ� ����
        RemoveBarricade();
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
