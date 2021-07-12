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
    [SerializeField]
    CameraManager cm = null;

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
    float maxHeight = 30.0f;

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
    float posSpeed = 5.00f;
    float timeLimit = 3.00f;


    // ���� ����
    public bool isInputBlock { get { return GameData.worldManager.cameraManager.controller.isFreeMode; } }

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


    // AI ����
    public Coroutine AIWork = null;
    // ���� ��ǲ
    public bool doForceClick = false;
    public bool doForceClickUp = false;
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

        if(owner != null)
        {
            // ���� ���۽� �ð����� ����
            if (isTimeCountWork && !owner.isAutoPlay)
                isTimeCountWork = false;
        }
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
                isTimeCountWork = true;

                // AI ���� �ʱ�ȭ
                if(owner != null)
                    owner.ai.mainGame.dice.Ready();
                doForceClick = false;
                doForceClickUp = false;

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
                cm.CamMoveTo(owner.avatar.transform, CameraManager.CamAngle.Middle);

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

                // �ֻ��� ���̵� �� ���� ���� ===============

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
                // ���� �÷���
                if (owner.isAutoPlay)
                {
                    // AI �ʵ� �۵�
                    if (!owner.ai.mainGame.dice.isStart)
                    {
                        //AI Ȱ��ȭ
                        owner.ai.mainGame.dice.Work();
                    }
                }

                // �Է� ���� ���� �� UI Ŭ�� �ƴҰ�� ================= ���� �ν��ؼ� ����� �����ؾ���
                if (!isInputBlock && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
                {
                    // �� ��������
                    if (Input.GetMouseButton(0) || doForceClick)
                    {
                        // ���ӵ�
                        if (rotAccel < rotAccelMax)
                            rotAccel += 0.1f+ Time.deltaTime * 5.0f + rotAccel * Time.deltaTime * 0.5f;
                        else if (rotAccel > rotAccelMax)
                            rotAccel = rotAccelMax;
                    }
                    // Ŭ�� ����� ��
                    else if (Input.GetMouseButtonUp(0) || doForceClickUp)
                    {
                        //Debug.Break();
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
            // ����
            Tool.Spin(transform, rotSpeed);

            // ȸ�� ���ӵ� -  �Ѱ�ġ����
            if (rotAccel < rotAccelMax)
                rotAccel += 1f + Time.deltaTime * 5.0f + rotAccel * Time.deltaTime * 0.5f;
            else if (rotAccel > rotAccelMax)
                rotAccel = rotAccelMax;

            if (actionProgress == ActionProgress.Ready)
            {
                // �߷� Ȱ��ȭ
                transform.GetComponent<Rigidbody>().useGravity = true;

                // ��ŵ
                actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // ���� ����
                cm.CamMoveTo(owner.avatar.transform, CameraManager.CamAngle.Top);

                // ��� ó��
                transform.GetComponent<Rigidbody>().isKinematic = false;
                transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 50f * transform.GetComponent<Rigidbody>().mass, ForceMode.Impulse);

                // ��ǥ ����
                //_posSpeed = transform.position.y;

                // ��ŵ
                actionProgress = ActionProgress.Working;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                // ��� �ν�
                if (transform.GetComponent<Rigidbody>().velocity.y > 0)
                {
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
            // ����
            Tool.Spin(transform, rotSpeed);

            // ȸ�� ���ӵ� -  �Ѱ�ġ����
            if (rotAccel < rotAccelMax)
                rotAccel += 1f + Time.deltaTime * 5.0f + rotAccel * Time.deltaTime * 0.5f;
            else if (rotAccel > rotAccelMax)
                rotAccel = rotAccelMax;

            if (actionProgress == ActionProgress.Ready)
            {
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
                // ���� �����ϰ� ��������
                if (rotAccel == rotAccelMax)
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
                // ����
                Tool.Spin(transform, rotSpeed);

                // ȸ�� ����
                if (rotAccel > 0.0f)
                    rotAccel -= Time.deltaTime * 5.0f - rotAccel * Time.deltaTime * 0.5f;
                else if (rotAccel < 0.0f)
                    rotAccel = 0.0f;

                // �ϰ� �ν�
                if (transform.GetComponent<Rigidbody>().velocity.y < 0)
                {
                    if (transform.position.y <= minHeight)
                    {
                        // �߷� ��Ȱ��ȭ
                        transform.GetComponent<Rigidbody>().isKinematic = true;
                        transform.GetComponent<Rigidbody>().useGravity = false;

                        // �ּ�, �ִ� ���� ����
                        Tool.HeightLimit(transform, minHeight, maxHeight);

                        // �ϰ� ����
                        actionProgress = ActionProgress.Finish;
                    }
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
                // �ð� ī���� ����
                elapsedTime = 0f;

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

                    // ����
                    transform.rotation = lastRot;

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
                    Tool.SpinY(transform, _rotSpeed);
                else if (dice.value == 4)
                    Tool.SpinY(transform, _rotSpeed);
                else if (dice.value == 5)
                    Tool.SpinY(transform, _rotSpeed);
                else // (dice.value == 6)
                    Tool.SpinZ(transform, _rotSpeed);

                // �ð� ��� �� �ѱ�
                if (elapsedTime > 3.0f)
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

        // AI ���� �ʱ�ȭ
        if (owner != null)
            owner.ai.mainGame.dice.Ready();
        doForceClick = false;
        doForceClickUp = false;

        // �ð� ���ð� ����
        rotAccel = 0f;
        elapsedTime = 0f;

        // �߷� ��Ȱ��ȭ
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<Rigidbody>().useGravity = false;

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
        Debug.LogWarning("�ֻ��� :: ������ = " + owner.name);

        // �ֻ��� ������ �����ϸ� �ߴ�
        if (dice.count < 1)
        {
            Debug.Log("�ֻ��� ���� ���� :: " + dice.count);

            // �׼� ����
            action = DiceAction.Finish;
            actionProgress = ActionProgress.Working;

            //Debug.Break();
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

        // AI ���� �ʱ�ȭ
        owner.ai.mainGame.dice.Ready();
        doForceClick = false;
        doForceClickUp = false;

        // �ֻ��� �������� �߰� ����
        if (dice.count > 1)
        {
            Debug.Log("�ֻ��� ���� :: �ٽ� ����");

            // �ٽ� ó������ �ʱ�ȭ
            action = DiceAction.Wait;
            actionProgress = ActionProgress.Start;

            // �ð� ���ð� ����
            rotAccel = 0f;
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
            cm.CamMoveTo(cm.controller.cam, CameraManager.CamAngle.Top);
        }


        return result;
    }


    /// <summary>
    /// AI �۵�
    /// </summary>
    public void RunAI(IEnumerator script)
    {
        AIWork =
                // �۾� ����
                StartCoroutine(
                    script
                    );
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
