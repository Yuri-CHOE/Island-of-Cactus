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
    public Dice dice = null;
    //public Dice diceTurnPlayer { get { return GameData.turn.now.dice; } }

    // �ֻ��� ����
    public Player owner = null;
    //public Player owner { get { return GameData.turn.now; } }

    // ������ Ȯ��
    public bool isMyDice { get { return owner == GameData.player.me; } }

    // �ּ� ����
    [SerializeField]
    float minHeight = 1.9f;

    // �ִ� ����
    [SerializeField]
    float maxHeight = 20.0f;

    // �ִ� ����
    [SerializeField]
    Vector3 diceDistance = new Vector3(0, 3.1f, -2);

    // ȸ�� �ӵ�
    [Header("spin")]
    float _rotSpeed = 1.00f;
    float rotSpeed { get { return _rotSpeed * rotAccel; } }
    [SerializeField]
    float rotAccelMax = 100f;
    [SerializeField]
    float rotAccel = 0.00f;

    // ��� �ϰ� �ӵ�
    [Header("rise")]
    float _posSpeed = 1.00f;
    float posSpeed { get { return _posSpeed * posAccel; } }
    [SerializeField]
    float posAccelMax = 100f;
    [SerializeField]
    float posAccel = 0.00f;


    // ���� ����
    public bool isInputBlock { get { return !GameData.worldManager.camMover.isInputBlock; } }

    // �ֻ��� �� �� ������
    Quaternion eye1 = Quaternion.Euler(-90, 0, 0);
    Quaternion eye2 = Quaternion.Euler(0, 0, 0);
    Quaternion eye3 = Quaternion.Euler(0, 0, 90);
    Quaternion eye4 = Quaternion.Euler(0, 0, -90);
    Quaternion eye5 = Quaternion.Euler(0, 0, 180);
    Quaternion eye6 = Quaternion.Euler(90, 0, 0);


    [Header("Action")]
    // �׼� ����
    public DiceAction action = DiceAction.Wait;
    // �׼� ���൵
    public ActionProgress actionProgress = ActionProgress.Ready;
    float elapsedTime = 0.00f;
    public bool isTimeCountWork = false;

    // ���� �ƿ�ǲ
    public bool isFree { get { return action == DiceAction.Wait && actionProgress == ActionProgress.Ready; } }
    public bool isBusy { get { return !isFree && !isFinish; } }
    public bool isFinish { get { return action == DiceAction.Finish && actionProgress == ActionProgress.Finish; } }



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
        ActUpdate();
        test();

        // ���� ���۽� �ð����� ����
        if (isTimeCountWork && !owner.isAutoPlay)
            isTimeCountWork = false;
    }

    void test()
    {
        if (!testRun)
            return;

        Debug.Log("�׽�Ʈ ��û��");
        testRun = false;

        if (GameData.player.me == null)
            GameData.player.me = new Player(Player.Type.User, 1, false, "�׽�Ʈ");

        CallDice(GameData.player.me, testObject);

    }

    void ActUpdate()
    {
        if(action == DiceAction.Wait)
        {
            if (actionProgress == ActionProgress.Ready)
            {

                // ��ŵ
                //actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // ��ŵ
                actionProgress = ActionProgress.Working;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                // ���� ����
                GameData.worldManager.camMover.CamMoveTo(owner.avatar.transform, TouchCameraMover.CamAngle.Middle);

                // ��ŵ
                actionProgress = ActionProgress.Finish;
            }
            else if (actionProgress == ActionProgress.Finish)
            {

                // ��ŵ
                actionProgress = ActionProgress.Ready;
                action = DiceAction.Hovering;
            }
            return;
        }
        else if (action == DiceAction.Hovering)
        {
            if (actionProgress == ActionProgress.Ready)
            {
                // ȸ�� ���� �ʱ�ȭ
                rotAccel = 1.0f;

                // �ֻ��� ���̵� �� ����??

                // ��ŵ
                actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {            
                // ��ŵ
                actionProgress = ActionProgress.Working;
                return;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                if (!isInputBlock)
                {
                    // �� ��������
                    if (Input.GetMouseButton(0))
                    {
                        // ���ӵ�
                        if (rotAccel < rotAccelMax)
                            rotAccel += Time.deltaTime * 5.0f + rotAccel * Time.deltaTime * 0.5f;
                        else if (rotAccel > rotAccelMax)
                            rotAccel = rotAccelMax;
                    }
                    // Ŭ�� ����� ��
                    else if (Input.GetMouseButtonUp(0))
                    {
                        actionProgress = ActionProgress.Finish;
                    }
                }

                // �ּ�, �ִ� ���� ����
                Tool.HeightLimit(transform, minHeight, maxHeight);

                // ����
                Tool.Spin(transform, rotSpeed);


                // �ð� ����
                if (elapsedTime > 15.0f)
                {
                    actionProgress = ActionProgress.Finish;

                    Debug.Log("ȣ���� Ÿ�� ����");
                }

                // �ð� ī��Ʈ
                if(isTimeCountWork)
                    elapsedTime += Time.deltaTime;
            }
            else if (actionProgress == ActionProgress.Finish)
            {
                // �ð� �ʱ�ȭ
                elapsedTime = 0f;

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
                // �̵� ���ӵ� �ʱ�ȭ
                posAccel = 0f;

                // ��ŵ
                actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // ���� ����
                GameData.worldManager.camMover.CamMoveTo(owner.avatar.transform, TouchCameraMover.CamAngle.Top);

                // ��ǥ ����
                _posSpeed = transform.position.y;

                // ��ŵ
                actionProgress = ActionProgress.Working;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                // ����
                Tool.Spin(transform, rotSpeed);

                // ��� ó��
                if (transform.position.y < maxHeight)
                {
                    // ȸ�� ���ӵ� -  �Ѱ�ġ����
                    if (rotAccel < rotAccelMax)
                        rotAccel += Time.deltaTime * 5.0f + rotAccel * Time.deltaTime * 1f;
                    else if (rotAccel > rotAccelMax)
                        rotAccel = rotAccelMax;

                    // ���
                    //transform.position = new Vector3(transform.position.x, posSpeed, transform.position.y);
                    transform.position = Vector3.Lerp(
                        transform.position, 
                        new Vector3(transform.position.x, transform.position.y + posSpeed, transform.position.z), 
                        Time.deltaTime * 5.00f
                        );


                    // �̵� ���ӵ�
                    if (posAccel < posAccelMax)
                        posAccel += Time.deltaTime * 5.0f + posAccel * Time.deltaTime * 0.5f;
                    else if (posAccel > posAccelMax)
                        posAccel = posAccelMax;
                }
                else
                {
                    // �ּ�, �ִ� ���� ����
                    Tool.HeightLimit(transform, minHeight, maxHeight);

                    // ��� ����
                    actionProgress = ActionProgress.Finish;
                }
            }
            else if (actionProgress == ActionProgress.Finish)
            {
                // ��ŵ
                actionProgress = ActionProgress.Ready;
                action = DiceAction.Spinning;
            }
            return;
        }
        else if (action == DiceAction.Spinning)
        {
            if (actionProgress == ActionProgress.Ready)
            {
                // ��ŵ
                actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // ������ �� ����
                dice.Rolling();

                // ��ŵ
                actionProgress = ActionProgress.Working;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                // ����
                Tool.Spin(transform, rotSpeed);

                // ȸ�� ���ӵ� -  �Ѱ�ġ����
                if (rotAccel < rotAccelMax)
                    rotAccel += Time.deltaTime * 5.0f + rotAccel * Time.deltaTime * 0.5f;
                else if (rotAccel > rotAccelMax)
                    rotAccel = rotAccelMax;

                // ���� �����ϰ� ��������
                else if (rotAccel == rotAccelMax)
                {
                    actionProgress = ActionProgress.Finish;
                }

            }
            else if (actionProgress == ActionProgress.Finish)
            {
                // ��ŵ
                actionProgress = ActionProgress.Ready;
                action = DiceAction.Landing;
            }
            return;
        }
        else if (action == DiceAction.Landing)
        {
            if (actionProgress == ActionProgress.Ready)
            {
                // �̵� ���ӵ� �ʱ�ȭ
                posAccel = 0f;

                // ��ŵ
                actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // �ֻ��� �� ����
                dice.Rolling();
                Debug.Log("�ֻ��� �� :: " + dice.value);

                // ��ŵ
                actionProgress = ActionProgress.Working;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                // ����
                Tool.Spin(transform, rotSpeed);

                // ȸ�� ����
                if (rotAccel > 0.0f)
                    rotAccel -= Time.deltaTime * 5.0f - rotAccel * Time.deltaTime * 0.5f;
                else if (rotAccel < 0.0f)
                    rotAccel = 0.0f;

                // �ϰ� ó��
                if (transform.position.y >  minHeight)
                {
                    // �ϰ�
                    transform.position = Vector3.Lerp(
                        transform.position,
                        new Vector3(transform.position.x, transform.position.y - posSpeed, transform.position.z),
                        Time.deltaTime * 5.00f
                        );

                    // ���ӵ�
                    if (posAccel < posAccelMax)
                        posAccel += Time.deltaTime * 5.0f + posAccel * Time.deltaTime * 0.5f;
                    else if (posAccel > posAccelMax)
                        posAccel = posAccelMax;
                }
                else 
                {
                    // �ּ�, �ִ� ���� ����
                    Tool.HeightLimit(transform, minHeight, maxHeight);

                    // �ϰ� ����
                    actionProgress = ActionProgress.Finish;
                }

            }
            else if (actionProgress == ActionProgress.Finish)
            {
                // ��ŵ
                actionProgress = ActionProgress.Ready;
                action = DiceAction.Finish;
            }
            return;
        }
        else if (action == DiceAction.Finish)
        {
            if (actionProgress == ActionProgress.Ready)
            {
                // ��ŵ
                actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // ���� ���� ���� ����
                Quaternion lastRot;
                if (dice.value == 1)
                    lastRot = eye1;
                else if (dice.value == 2)
                    lastRot = eye2;
                else if (dice.value == 3)
                    lastRot = eye3;
                else if (dice.value == 4)
                    lastRot = eye4;
                else if (dice.value == 5)
                    lastRot = eye5;
                else //(dice.value == 6)
                    lastRot = eye6;

                // ���� ȸ�� ó��
                if (
                    Mathf.Abs(transform.rotation.x) != Mathf.Abs(lastRot.x) &&
                    Mathf.Abs(transform.rotation.y) != Mathf.Abs(lastRot.y) &&
                    Mathf.Abs(transform.rotation.z) != Mathf.Abs(lastRot.z) &&
                    Mathf.Abs(transform.rotation.w) != Mathf.Abs(lastRot.w)
                    )
                {
                    // ȸ���� ��� (���� ����)
                    transform.rotation = Quaternion.Lerp(transform.rotation, lastRot, Time.deltaTime * Mathf.Abs(_rotSpeed -rotAccel));
                }
                // �ֵ� ȸ�� �Ϸ�� ��ŵ
                else
                {
                    // �ֻ��� ���� ����
                    dice.count--;
                    Debug.Log("�ֻ��� ���� :: -1 =>" + dice.count);

                    actionProgress = ActionProgress.Working;
                }
            }
            else if (actionProgress == ActionProgress.Working)
            {
                if (dice.value == 1)
                    Tool.SpinZ(transform, _rotSpeed);
                else if (dice.value == 2)
                    Tool.SpinY(transform, _rotSpeed);
                else if (dice.value == 3)
                    Tool.SpinX(transform, _rotSpeed);
                else if (dice.value == 4)
                    Tool.SpinX(transform, _rotSpeed);
                else if (dice.value == 5)
                    Tool.SpinY(transform, _rotSpeed);
                else // (dice.value == 6)
                    Tool.SpinZ(transform, _rotSpeed);


                // �ð� ��� �� �ѱ�
                if (elapsedTime > 3.5f)
                {
                    elapsedTime = 0f;
                    actionProgress = ActionProgress.Finish;
                }

                // �ð� ī��Ʈ
                elapsedTime += Time.deltaTime;

            }
            else if (actionProgress == ActionProgress.Finish)
            {
            }
            return;
        }
    }





    
    /// <summary>
    /// ���� �ʱ�ȭ ����
    /// </summary>
    public void ResetDice()
    {
        // �ֻ��� ������ �ʱ�ȭ
        owner = null;
        dice = null;

        // �׼ǻ��� �ʱ�ȭ
        action = DiceAction.Wait;
        actionProgress = ActionProgress.Ready;

        // �ð� ���ð� ����
        rotAccel = 0f;
        posAccel = 0f;
        elapsedTime = 0f;


        // �ֻ��� ���� ����
        transform.parent = transform.root;

        // �Ⱥ��̵��� ���ӿ� ����
        transform.position = new Vector3(0, -10, 0) ;
        
        // ������Ʈ ��Ȱ��
        transform.GetComponent<MeshRenderer>().enabled = false;
        //gameObject.SetActive(false);


    }


    public void CallDice(Player __owner, Transform obj) {  CallDice(__owner, obj, diceDistance); }
    public void CallDice(Player __owner, Transform obj, Vector3 distance)
    {
        // �ֻ��� ������̸� �ߴ�
        if (action != DiceAction.Wait)
            if (actionProgress != ActionProgress.Ready)
                return;

        // �ֻ��� �������� ����
        owner = __owner;
        dice = owner.dice;
        Debug.LogError("����� :: " + owner.name);

        // �ֻ��� ������ �����ϸ� �ߴ�
        if (dice.count < 1)
        {
            Debug.Log("�ֻ��� ���� ���� :: " + dice.count);

            // �׼� ����
            action = DiceAction.Finish;
            actionProgress = ActionProgress.Working;

            Debug.Break();
            return;
        }

        // ���� ����
        dice.isRolling = true;

        // �׼� ����
        action = DiceAction.Wait;
        actionProgress = ActionProgress.Start;

        // ������Ʈ �̵�
        Vector3 pos = new Vector3(obj.position.x, obj.position.y, obj.position.z);
        pos += distance;
        transform.position = pos;


        // �ֻ��� ����
        //transform.parent = obj;

        // ������Ʈ Ȱ��
        transform.GetComponent<MeshRenderer>().enabled = true;
        //gameObject.SetActive(true);


    }

    /// <summary>
    ///  �ֻ����� ���� �������� �ʱ�ȭ ����
    /// </summary>
    /// <returns></returns>
    public int UseDice()
    {
        int result = 0;

        // �ֻ��� �������� �߰� ����
        if (dice.count > 1)
        {
            Debug.Log("�ֻ��� ���� :: �ٽ� ����");

            // �ٽ� ó������ �ʱ�ȭ
            action = DiceAction.Wait;
            actionProgress = ActionProgress.Start;

            // �ð� ���ð� ����
            rotAccel = 0f;
            posAccel = 0f;
            elapsedTime = 0f;
        }
        else
        {
            Debug.Log("�ֻ��� ���� :: ����");

            // ���� ����
            dice.isRolling = false;
            dice.isRolled = true;

            // ����� ���
            result = dice.value;

            // �ʱ�ȭ ����
            ResetDice();

            // ���� ����
            GameData.worldManager.camMover.CamMoveTo(GameData.worldManager.camMover.cam, TouchCameraMover.CamAngle.Top);
        }


        return result;
    }




    /// <summary>
    /// �ֻ��� �� ����
    /// </summary>
    //void HeightFix()
    //{
    //    // �ּҰ� ����
    //    if (transform.position.y < minHeight)
    //    {
    //        transform.position = new Vector3(
    //            transform.position.x,
    //            minHeight,
    //            transform.position.z
    //            );
    //        Debug.Log("�ּҰ� ����");
    //        Debug.Log(transform.position.ToString());
    //    }

    //    // �ִ밪 ����
    //    if (transform.position.y > maxHeight)
    //    {
    //        transform.position = new Vector3(
    //                transform.position.x,
    //                maxHeight,
    //                transform.position.z
    //                );
    //        Debug.Log("�ִ밪 ����");
    //        Debug.Log(transform.position.ToString());
    //    }
    //}
}
