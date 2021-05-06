using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    public enum DiceAction
    {
        Wait,
        Hovering,
        Rising,
        Spinning,
        Landing,
        Finish,
    }

    // �ֻ��� ����
    public Dice dice { get { return GameData.turn.now.dice; } }

    // �ֻ��� ����
    public Player owner { get { return GameData.turn.now; } }

    // �ּ� ����
    [SerializeField]
    float minHeight = 1.9f;

    // �ִ� ����
    [SerializeField]
    float maxHeight = 50.0f;

    // �ִ� ����
    [SerializeField]
    Vector3 diceDistance = new Vector3(0, 1, -2);

    // ȸ�� �ӵ�
    [Header("spin")]
    float _rotSpeed = 1.00f;
    float rotSpeed { get { return _rotSpeed + rotAccel; } }
    [SerializeField]
    float rotAccel = 0.00f;

    // ��� �ϰ� �ӵ�
    [Header("rise")]
    float _posSpeed = 1.00f;
    float posSpeed { get { return _posSpeed + posAccel; } }
    float posAccel = 0.00f;


    [Header("Action")]
    // �׼� ����
    public DiceAction action = DiceAction.Wait;
    // �׼� ���൵
    [SerializeField]
    ActionProgress actionProgress = ActionProgress.Ready;
    bool isTimmerWork = false;
    float elapsedTime = 0.00f;


    [Header("TestTool")]
    [SerializeField]
    bool testRun = false;
    [SerializeField]
    Transform testObject;



    // Start is called before the first frame update
    void Start()
    {
        ResetDice();
    }

    // Update is called once per frame
    void Update()
    {
        test();
        ActUpdate();
        
    }

    void test()
    {
        if (!testRun)
            return;

        Debug.Log("�׽�Ʈ ��û��");
        testRun = false;

        CallDice(testObject);

    }


    void ActUpdate()
    {
        if(action == DiceAction.Wait)
        {
            if (actionProgress == ActionProgress.Ready)
            {

            }
            else if (actionProgress == ActionProgress.Start)
            {

            }
            else if (actionProgress == ActionProgress.Working)
            {

            }
            else if (actionProgress == ActionProgress.Finish)
            {

            }
            return;
        }
        else if (action == DiceAction.Hovering)
        {
            if (actionProgress == ActionProgress.Ready)
            {
                // �ֻ��� ���̵� �� ����??
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // ��ŵ
                actionProgress = ActionProgress.Working;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                // ����
                Tool.Spin(transform, rotSpeed);

                // �ּ�, �ִ� ���� ����
                HeightFix();

                // �� ��������
                if (Input.GetMouseButton(0))
                {
                    // ���ӵ�
                    if (rotAccel < 100f)
                        rotAccel += Time.deltaTime * 5.0f + rotAccel * Time.deltaTime * 0.5f;
                    else if (rotAccel > 100f)
                        rotAccel = 100f;
                }

                // Ŭ�� ����� ��
                if (Input.GetMouseButtonUp(0))
                    actionProgress = ActionProgress.Finish;
            }
            else if (actionProgress == ActionProgress.Finish)
            {
                // ��ŵ
                actionProgress = ActionProgress.Ready;
                action = DiceAction.Rising;
            }
            return;
        }
        else if (action == DiceAction.Rising)
        {
            if (actionProgress == ActionProgress.Ready)
            {
                // ��ŵ
                actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // ��ŵ
                actionProgress = ActionProgress.Working;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                // ��� ó��
            }
            else if (actionProgress == ActionProgress.Finish)
            {

            }
            return;
        }
        else if (action == DiceAction.Spinning)
        {
            if (actionProgress == ActionProgress.Ready)
            {

            }
            else if (actionProgress == ActionProgress.Start)
            {

            }
            else if (actionProgress == ActionProgress.Working)
            {

            }
            else if (actionProgress == ActionProgress.Finish)
            {

            }
            return;
        }
        else if (action == DiceAction.Landing)
        {
            if (actionProgress == ActionProgress.Ready)
            {

            }
            else if (actionProgress == ActionProgress.Start)
            {

            }
            else if (actionProgress == ActionProgress.Working)
            {

            }
            else if (actionProgress == ActionProgress.Finish)
            {

            }
            return;
        }
        else if (action == DiceAction.Finish)
        {
            if (actionProgress == ActionProgress.Ready)
            {

            }
            else if (actionProgress == ActionProgress.Start)
            {

            }
            else if (actionProgress == ActionProgress.Working)
            {

            }
            else if (actionProgress == ActionProgress.Finish)
            {

            }
            return;
        }
    }





    
    /// <summary>
    /// ��� �׼� ���� ����
    /// </summary>
    public void ResetDice()
    {
        action = DiceAction.Wait;
        actionProgress = ActionProgress.Ready;

        // Ÿ�̸� ����
        isTimmerWork = false;

        // �ֻ��� ���� ����
        transform.parent = transform.root;

        // �Ⱥ��̵��� ���ӿ� ����
        transform.position = new Vector3(0, -10, 0) ;

        // �׼ǻ��� �ʱ�ȭ
        action = DiceAction.Wait;

        // ������Ʈ ��Ȱ��
        transform.GetComponent<MeshRenderer>().enabled = false;
        Debug.Log(transform.GetComponent<MeshRenderer>().enabled);
        //gameObject.SetActive(false);


    }


    public void CallDice(Transform obj) { CallDice(obj, diceDistance); }
    public void CallDice(Transform obj, Vector3 distance)
    {
        // �ֻ��� ������̸� �ߴ�
        if (action != DiceAction.Wait)
            return;

        action = DiceAction.Hovering;
        actionProgress = ActionProgress.Start;

        // ������Ʈ �̵�
        Vector3 pos = new Vector3(obj.position.x, obj.position.y, obj.position.z);
        pos += distance;
        transform.position = pos;
        Debug.Log(pos.ToString());
        Debug.Log(transform.position.ToString());


        // �ֻ��� ����
        //transform.parent = obj;

        // ������Ʈ Ȱ��
        transform.GetComponent<MeshRenderer>().enabled = true;
        Debug.Log(transform.GetComponent<MeshRenderer>().enabled);
        //gameObject.SetActive(true);

    }






    /// <summary>
    /// �ֻ��� �� ����
    /// </summary>
    void HeightFix()
    {
        // �ּҰ� ����
        if (transform.position.y < minHeight)
        {
            transform.position = new Vector3(
                transform.position.x,
                minHeight,
                transform.position.z
                );
            Debug.Log("�ּҰ� ����");
            Debug.Log(transform.position.ToString());
        }

        // �ִ밪 ����
        if (transform.position.y > maxHeight)
        {
            transform.position = new Vector3(
                    transform.position.x,
                    maxHeight,
                    transform.position.z
                    );
            Debug.Log("�ִ밪 ����");
            Debug.Log(transform.position.ToString());
        }
    }
}
