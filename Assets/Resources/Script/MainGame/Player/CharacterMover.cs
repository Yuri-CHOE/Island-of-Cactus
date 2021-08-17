using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    // ��ֹ� ���
    //public static List<int> barricade = new List<int>();
    //public static List<List<DynamicObject>> barricade = new List<List<DynamicObject>>();

    // �÷��̾�
    public Player owner = null;

    // �׼� ť
    public Queue<Action> actionsQueue = new Queue<Action>();

    // ���� �׼�
    public Action actNow = new Action();


    // ȸ�� �ӵ�
    public static float ActTurnSpeed = 5f;


    // ��ǥ �̵� ��ǥ
    Vector3 movePoint = Vector3.zero;

    // �̵� �����
    Coroutine moveCoroutine = null;
    public bool isBusy = false;



    // ���� ����Ʈ
    List<Player> attackTarget = new List<Player>();
    bool attackNow = false;


    
    // ������Ʈ ���� �����
    Coroutine objectPickUp = null;
    List<DynamicObject> objectPickUpList = new List<DynamicObject>();
    ActionProgress objectPickUpStep = ActionProgress.Ready;



    // ��ġ �ε���
    int _location = -1;
    //public int location { get { return _location; } set { _location = GameData.blockManager.indexLoop(_location, value); } }
    public int location { get { return _location; } set { _location = GameData.blockManager.indexLoop(value, 0); owner.MirrorLoaction(); } }

    // �̵���
    public int moveCount = 0;

    // ���� ��ġ ��� ������Ʈ
    public Transform locateBlock { get { if (GameData.isMainGameScene) { if (location >= 0) return GameData.blockManager.GetBlock(location).transform; else return GameData.blockManager.startBlock; } else return null; } }



    // �̵� �ӵ�
    float moveSpeed = 2.00f;


    [SerializeField]
    float posMinY = 1.9f;           // ĳ���� �ּ� ����
    [SerializeField]
    float posMaxY = 20f;            // ĳ���� �ִ� ����


    // ȸ���� ���� ������Ʈ
    public Transform bodyObject = null;


    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        //// �Ϸ�� �̵���ǥ ����
        //if (movePoint != Vector3.zero && moveCoroutine == null)
        //    movePoint = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ����
        Tool.HeightLimit(bodyObject, posMinY - transform.position.y, posMaxY - transform.position.y);

        // �׼� �ڵ� ����
        ActionCall();
    }


    public static void AvatarOverFixAll()
    {
        // �÷��̾� �˻� ����Ʈ
        List<Player> fixSearch = new List<Player>(Player.allPlayer);

        // �۾� ���
        List<Player> targetList = new List<Player>();

        // �˻�
        while (fixSearch.Count > 0)
        {
            // �����
            Player current = fixSearch[0];

            // �۾� ��� �߰�
            targetList.Clear();
            targetList.Add(current);

            // ��ħ ����
            bool isOver = false;

            // �ٸ��÷��̾�� ��ġ ��
            for (int i = 0; i < current.otherPlayers.Count; i++)
            {
                // �����
                Player other = current.otherPlayers[i];
                
                // ��ġ �ߺ� üũ
                if (current.location == other.location)
                {
                    // �ߺ� �÷��̾� �˻����� ����
                    fixSearch.Remove(other);

                    // �۾� ��� ����
                    targetList.Add(other);

                    // ��ħ ó��
                    isOver = true;
                }
                //if (current.movement.location == other.movement.location)
                //{
                //    // �ߺ� �÷��̾� �˻����� ����
                //    fixSearch.Remove(other);

                //    // �۾� ��� ����
                //    targetList.Add(other);

                //    // ��ħ ó��
                //    isOver = true;
                //}
            }

            // �˻� ���� �÷��̾� �˻� ����
            fixSearch.Remove(current);

            // �������� �ؼ� �۾� ȣ��
            //if (isOver)
                ReLocation(targetList);
        }
    }

    static void ReLocation(List<Player> playerList)
    {
        // ��� �������� �ߴ�
        if (playerList == null || playerList.Count == 0)
            return;

        // ��ģ ��ǥ
        //Vector3 crossPoint = playerList[0].movement.locateBlock.position;
        Vector3 crossPoint;

        // ��ŸƮ ���� ��ȸ
        if (playerList[0].location == -1)
            crossPoint = BlockManager.script.startBlock.position;
        else
            crossPoint = BlockManager.script.GetBlock(playerList[0].location).transform.position;

        // ����� 1���� ���
        if (playerList.Count == 1)
        {
            // �������� �̵�
            playerList[0].movement.MoveSet(crossPoint, ActTurnSpeed, true);
            return;
        }

        int count = playerList.Count;

        for (int i = 0; i < count; i++)
        {
            // �� ���
            Player current = playerList[0];

            // �ӽ� ����
            GameObject obj = new GameObject("pos");

            // �ӽ� �̵�
            //playerList[0].movement.transform.RotateAround(crossPoint, Vector3.down, (360 / playerList.Count * i));
            float angle = 180 / count + 360 / count * -i;
            obj.transform.position = crossPoint + Vector3.forward * 2;
            obj.transform.RotateAround(crossPoint, Vector3.down, angle);

            // ��ǥ ����
            Vector3 pos = obj.transform.position;
            
            // �ӽ� ������Ʈ ����
            Destroy(obj);

            // �̵� ����
            playerList[i].movement.MoveSet(pos, ActTurnSpeed, true);
        }
    }

    /// <summary>
    /// �ƹ�Ÿ ��ħ ó��
    /// </summary>
    public void AvatarOverFix()
    {
        // �ߺ� �÷��̾� ����Ʈ
        List<Player> fixTarget = new List<Player>();
        fixTarget.Add(owner);

        // ��� �÷��̾� ��ġ üũ
        for (int i = 0; i < owner.otherPlayers.Count; i++)
        {
            // �����
            Player other = owner.otherPlayers[i];

            // ��ġ �ߺ� �÷��̾� Ȯ��
            if (location == other.movement.location)
                fixTarget.Add(other);
        }

        // �������� �ؼ� �۾� ȣ��
        if (fixTarget.Count > 1)
            ReLocation(fixTarget);

        return;
    }

    /// <summary>
    /// Ư�� ��ġ�� ���� �����ٸ�
    /// </summary>
    /// <param name="moveLocation">��ġ</param>
    public void PlanMoveTo(int moveLocation)
    {
        // �̵��� ���
        int distance = moveLocation - location;

        // ���� ����
        if (distance < 0)
            distance += GameData.blockManager.blockCount;

        // �̵� ��ȹ ��û
        PlanMoveBy(distance);
    }

    /// <summary>
    /// �� ��ŭ �̵� �����ٸ�
    /// </summary>
    /// <param name="moveValue"></param>
    public void PlanMoveBy(int moveValue)
    {
        // �̵� ���Ұ��
        if(moveValue == 0)
            actionsQueue.Enqueue(new Action(Action.ActionType.Stop, owner.location, moveSpeed));

        // ���� ��ǥ
        int movePoint = GameData.blockManager.indexLoop(_location, moveValue);

        Debug.LogWarning("�̵� ���� :: "+moveValue + " ĭ �̵��Ͽ� �������� = " + movePoint);

        // �� �̵� �Ÿ�(moveValue) �̳��� ��ֹ� ���
        int counter = 0;
        int _sign = (int)Mathf.Sign(moveValue);
        for (int i = 0; i != moveValue; i += _sign)
        {
            counter += _sign;

            //int loc = GameData.blockManager.indexLoop(location, ((i) * _sign));
            //int locNext = GameData.blockManager.indexLoop(location,  ((i + 1) * _sign));
            int loc = GameData.blockManager.indexLoop(location, i);
            int locNext = GameData.blockManager.indexLoop(location, i + _sign );

            // ��ŸƮ ���� üũ
            if (location == -1 && i == 0)
            {
                Debug.Log("��ŸƮ ���� Ž�� : " + counter);

                //�����ٸ� �߰� - counter��ŭ ĭ�� ����
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));

                //�����ٸ� �߰� - ȸ�� �׼� �߰�
                //actionsQueue.Enqueue(new Action(Action.ActionType.Turn, GameData.blockManager.GetBlock(locNext).GetComponent<DynamicBlock>().GetDirection(), moveSpeed));

                // ī���� ����
                counter = 0;
            }

            // �÷��̾� üũ
            for (int p = 0; p < owner.otherPlayers.Count; p++)
            {            
                // üũ ���
                Player current = owner.otherPlayers[p];

                // ��ο� ���� ���
                if (current.movement.location == locNext)
                {
                    Debug.Log("�÷��̾� Ž�� : " + counter);

                    //�����ٸ� �߰� - counter��ŭ ĭ�� ����
                    actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));

                    //�����ٸ� �߰� - ���� ó��
                    if (Cycle.now > 5 || owner == Player.system.Monster)  
                    actionsQueue.Enqueue(new Action(Action.ActionType.Attack, locNext, moveSpeed));

                    // ī���� ����
                    counter = 0;

                    // �ߴ�
                    break;
                }
            }

            // ��ֹ� üũ
            if (DynamicObject.objectList[locNext].Count > 0)
            {
                Debug.Log("��ֹ� Ž�� : " + counter);

                //�����ٸ� �߰� - counter��ŭ ĭ�� ����
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));
                //actionsQueue.Enqueue(new Action(Action.ActionType.Move, counter, moveSpeed));

                //�����ٸ� �߰� - ��ֹ� ó�� �߰�
                actionsQueue.Enqueue(new Action(Action.ActionType.Barricade, locNext, moveSpeed));

                // ī���� ����
                counter = 0;
            }


            // ���� �� üũ
            if (i + _sign == moveValue)
            {
                Debug.Log("���� Ž�� : " + counter);

                // �̵� ������
                if (counter != 0)
                {
                    //�����ٸ� �߰� - counter��ŭ ĭ�� ����
                    actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));

                    // ī���� ����
                    counter = 0;
                }

                //�����ٸ� �߰� - ���� �׼� : ���� �� ����
                actionsQueue.Enqueue(new Action(Action.ActionType.Stop, i + _sign, moveSpeed));
            }
            // �ڳ� üũ - �������� �ڳ��� ��� �ڳ�Ž�� �ʿ� ����
            else if (BlockManager.dynamicBlockList[locNext].isCorner)
            {
                Debug.Log("�ڳ� Ž�� : " + counter);

                //�����ٸ� �߰� - counter��ŭ ĭ�� ����
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));

                // ī���� ����
                counter = 0;
            }
        }

        //�����ٸ� �߰� - ���� ����
        //actionsQueue.Enqueue(new Action(Action.ActionType.Turn, 2, moveSpeed));
    }


    public void ActionCall()
    {
        // �׼� �̼��� ���
        if (actNow.type == Action.ActionType.None)
        {
            // �ܿ� �׼� ����
            if (actionsQueue.Count > 0)
                GetAction();

            // ��� �׼� ����
            else
            {
                // ���
            }
        }
        // �׼� ����
        else if (!actNow.isFinish)
        {
            // �̵� ó��
            if (actNow.type == Action.ActionType.Move)
                MoveByAction(ref actNow);

            // ��ֹ� ó��
            else if (actNow.type == Action.ActionType.Barricade)
                CheckBarricade(ref actNow);

            // ���� ó��
            else if (actNow.type == Action.ActionType.Attack)
                AttackPlayer(ref actNow);

            // ���� ȸ�� ó��
            else if (actNow.type == Action.ActionType.Stop)
                StopByAction(ref actNow);

        }
        // �׼� ���� ó��
        else //if (actNow.isFinish)
        {
            // �׼� �Ұ�
            actNow = new Action();
        }
    }


    public ref Action GetAction()
    {
        actNow = actionsQueue.Dequeue();

        Debug.LogWarning("�׼� :: " + actNow.type.ToString());

        return ref actNow;
    }



    /// <summary>
    /// ���� �׼��� ����� ����
    /// </summary>
    /// <param name="act"></param>
    public void AttackPlayer(ref Action act)
    {
        // ��ġ
        int loc = act.count;

        if (act.progress == ActionProgress.Ready)
        {
            // ���� �ʱ�ȭ
            attackTarget.Clear();
            attackNow = false;

            // ��ŵ
            act.progress = ActionProgress.Start;
        }
        else if (act.progress == ActionProgress.Start)
        {
            Debug.LogWarning("���� ��� �ۼ�");

            // ���� ��� ��ĵ
            for (int i = 0; i < owner.otherPlayers.Count; i++)
                // �׼���ġ�� �ִ� �ٸ� �÷��̾�
                if (owner.otherPlayers[i].movement.location == loc)
                {
                    // ���
                    attackTarget.Add(owner.otherPlayers[i]);

                    // �α�
                    Debug.LogWarning("���� ��� �߰� :: " + owner.otherPlayers[i].name + "�� �߰���");
                }
                //else Debug.LogWarning("���� ��� �ƴ� :: " + owner.otherPlayers[i].name);

            // ��ŵ
            act.progress = ActionProgress.Working;
        }
        else if (act.progress == ActionProgress.Working)
        {
            // �������� �ƴҶ�
            if (!attackNow)
            {
                // ���� ����� ���� ���
                if ( attackTarget.Count > 0)
                {
                    // ���� ó��
                    attackNow = true;
                    
                    // ���鿡�� �ϰ� ����
                    for (int i = 0; i < attackTarget.Count; i++)
                    {
                        // ����
                        owner.Attack(attackTarget[i]);

                        // �α�
                        Debug.LogWarning("���� �õ� :: " + owner.name + "�� " + attackTarget[i].name + "�� ����");
                    }
                    // �ӽ� : �Ʒ��� ������������ �ű��
                    attackTarget.Clear();
                    attackNow = false;
                }
                else
                    // ���� ����� ��ŵ
                    act.progress = ActionProgress.Finish;
            }
            // �������϶�
            else
            {
                // ���� ���� ����
                // ================���� ���� �Ұ� => �ִϸ��̼� ���� �̿�
                if (false)
                    attackNow = false;
            }
        }
        else if (act.progress == ActionProgress.Finish)
        {

            // ���� ó��
            act.isFinish = true;
        }
    }


    public void CheckBarricade(ref Action act)
    {
        // ��ġ
        int loc = act.count;

        if (act.progress == ActionProgress.Ready)
        {
            Debug.LogWarning("������Ʈ ���� :: �׼� ��ġ = " + act.count);

            // ������Ʈ : �ʱ�ȭ
            objectPickUpList.Clear();
            objectPickUpStep = ActionProgress.Ready;


            //// ������ : �ʱ�ȭ
            //itemList.Clear();
            //isPickingUpItem = false;
            //itemFinish = false;

            //// �̺�Ʈ : �ʱ�ȭ
            //eventList.Clear();
            //isPickingUpEvent = false;
            //eventFinish = false;

            // ��ŵ
            act.progress = ActionProgress.Start;
        }
        else if (act.progress == ActionProgress.Start)
        {
            // �ʱ�ȭ
            objectPickUpList.Clear();

            // ��ġ�� ������Ʈ ��� ��������
            List<DynamicObject> localBarricade = DynamicObject.objectList[loc];

            // ������Ʈ : ����Ʈ �ۼ�
            for (int i = 0; i < localBarricade.Count; i++)
            {
                DynamicObject obj = localBarricade[i];

                // ȹ�� ���� ����
                if (obj.CheckCondition(owner))
                {
                    // ȹ�� ��⿭ �߰�
                    objectPickUpList.Add(obj);

                    // �α�
                    Debug.LogWarning(string.Format("{0} ������Ʈ :: ���� ��� �߰� => ", obj.type, obj.transform.name));

                    continue;
                }
                else
                    Debug.LogWarning(string.Format("{0} ������Ʈ :: ���� �ڰ� �̴� => ", obj.type, obj.transform.name));

            }

            // ��ŵ
            act.progress = ActionProgress.Working;
        }
        else if (act.progress == ActionProgress.Working)
        {
            // ������Ʈ : ����
            if (objectPickUpStep == ActionProgress.Ready)
            objectPickUp = StartCoroutine(GetObject());


            // �Ϸ� üũ
            //if (itemFinish && eventFinish)
            if (objectPickUpStep == ActionProgress.Finish)
            {
                // ��ŵ
                objectPickUpStep = ActionProgress.Ready;
                act.progress = ActionProgress.Finish;
            }
        }
        else if (act.progress == ActionProgress.Finish)
        {
            // ���� ó��
            act.isFinish = true;
        }
    }

    void SetObjectPickUpList()
    {
        // �ʱ�ȭ
        objectPickUpList.Clear();


    }

    /// <summary>
    /// ������Ʈ ȹ�� ����Ʈ ��ü ȹ��
    /// </summary>
    /// <returns></returns>
    IEnumerator GetObject()
    {
        // ���� ����
        objectPickUpStep = ActionProgress.Start;

        // ���� ȹ��
        while (objectPickUpList.Count > 0)
        {
            // ���� ���� ���
            yield return GetObject(objectPickUpList[0]);
        }

        // ���� ���� ó��
        objectPickUpStep = ActionProgress.Finish;
    }

    /// <summary>
    /// ������Ʈ ���� ����
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    IEnumerator GetObject(DynamicObject current)
    {
        // ������ ó��
        objectPickUpStep = ActionProgress.Working;

        // �������� ���
        if (current.type == DynamicObject.Type.Item)
        {
            DynamicItem di = (DynamicItem)current;

            Debug.LogWarning("������ ������Ʈ :: ���� => " + di.item.index);

            // ȹ��
            di.GetItem(owner);

            // ���� ����
            yield return null;
        }

        // �������� ���
        else if (current.type == DynamicObject.Type.Event)
        {
            DynamicEvent de = (DynamicEvent)current;

            Debug.LogWarning("�̺�Ʈ ������Ʈ :: ���� => " + de.iocEvent.index);

            // ȹ��
            //de.GetEvent(owner, de.location);
            de.GetEvent(owner);

            // ���� ����
            yield return null;
        }

        // ���� ����Ʈ���� ����
        objectPickUpList.Remove(current);

        // ������ �Ϸ� ó��
        objectPickUpStep = ActionProgress.Start;
    }




    public void MoveByAction(ref Action act)
    {
        if (act.progress == ActionProgress.Ready)
        {
            // ���� �ʱ�ȭ

            // ��ŵ
            act.progress = ActionProgress.Start;
        }
        else if (act.progress == ActionProgress.Start)
        {
            // �̹� �̵����� ��� ���
            if (isBusy)
                return;

            // ��ǥ ���� �� �̵�
            if (act.count != 0)
                MoveSet(
                    BlockManager.dynamicBlockList[BlockManager.script.indexLoop(location, act.count)].transform.position,
                    act.speed,
                    false
                    );


            // �ӽ� �̵��� ����
            owner.location = location + act.count;
            AvatarOverFixAll();

            // ��ŵ
            act.progress = ActionProgress.Working;
        }
        else if (act.progress == ActionProgress.Working)
        {

            // �Ϸ� üũ
            if (!isBusy)
            {
                // �̵���ǥ ����
                movePoint = Vector3.zero;

                // ���� ����
                // ���� �߻�! =============== �̵� �ѹ� �Ҷ����� ���� �ٶ� => ���麸�� �׼� �÷� ������ų��
                //MoveSet(transform.position, ActTurnSpeed, true);

                // ��ŵ
                act.progress = ActionProgress.Finish;
            }
        }
        else if (act.progress == ActionProgress.Finish)
        {
            // ���� ó��
            act.isFinish = true;
        }
    }




    public void StopByAction(ref Action act)
    {
        if (act.progress == ActionProgress.Ready)
        {
            // ���� �ʱ�ȭ

            // ��ŵ
            act.progress = ActionProgress.Start;
        }
        else if (act.progress == ActionProgress.Start)
        {
            // �̹� �̵����� ��� ���
            //if (isBusy)
            //    return;

            // ��ǥ ���� �� �̵�
            //MoveSet(transform.position, ActTurnSpeed, true);

            // ����
            //StopCoroutine(moveCoroutine);

            // ��ŵ
            act.progress = ActionProgress.Working;
        }
        else if (act.progress == ActionProgress.Working)
        {
            // �Ϸ� üũ
            //if (!isBusy)
            {
                // ��ǥ ����
                //location +=  owner.dice.valueTotal;
                location = owner.location;

                // �̵���ǥ ����
                movePoint = locateBlock.position;

                // ��ħ ����
                CharacterMover.AvatarOverFixAll();

                // ��ŵ
                act.progress = ActionProgress.Finish;
            }
        }
        else if (act.progress == ActionProgress.Finish)
        {
            // ���� ó��
            act.isFinish = true;
        }
    }


    /// <summary>
    /// �Էµ� ��ǥ�� ���� �� �ڷ�ƾ���� �̵� ó��
    /// </summary>
    /// <param name="pos">������</param>
    /// <param name="speed">�ӵ�</param>
    public void MoveSet(Vector3 pos, float speed, bool isTurnAfterMove)
    {
        Debug.Log("�̵� ���� :: (" + transform.name + ") -> " + pos);

        movePoint = pos;

        if (!isBusy)
            moveCoroutine = StartCoroutine(ActMove(movePoint, speed, isTurnAfterMove));
        else
            Debug.LogWarning("error :: �̹� �׼� ������ -> " + actNow.type);
    }

    /// <summary>
    /// �Էµ� ��ǥ��ŭ �߰� �� �ڷ�ƾ���� �̵� ó��
    /// </summary>
    /// <param name="pos">������</param>
    /// <param name="speed">�ӵ�</param>
    public void MoveAdd(Vector3 pos, float speed, bool isTurnAfterMove)
    {
        Debug.Log("�̵� ���� :: (" + transform.name + ") -> " + pos);

        movePoint += pos;

        if (!isBusy)
            moveCoroutine = StartCoroutine(ActMove(movePoint, speed, isTurnAfterMove));
        else
            Debug.LogWarning("error :: �̹� �׼� ������ -> " + actNow.type);
    }

    /// <summary>
    /// ���� �׼ǰ� ����(������) �׼� ������ ��� �׼� ����
    /// ���� :: ��� ������ ����
    /// </summary>
    public void MoveStop()
    {
        Debug.LogError("�׼� �ߴ� :: (" + transform.name + ") ���� ��û�� -> " + owner.location);

        int diceTemp = owner.location - location;
        Debug.LogError("�׼� �ߴ� :: �ܿ� �ֻ��� = " + diceTemp);
        owner.dice.SetValueTotal(diceTemp);

        // ��ŵ ��� ���� ���
        if (actNow.type == Action.ActionType.Stop || actionsQueue.Count == 0)
            return;
        else
        {
            // ��ŵ ��� ���� �� ���� ����
            while (actionsQueue.Peek().type != Action.ActionType.Stop)
            {
                // ����
                //    Action.ActionType aType = actionsQueue.Dequeue().type;
                //    Debug.LogError("�׼� ���� :: " + aType);
                Debug.LogError("�׼� ���� :: " + actionsQueue.Dequeue().type);
            }

            //// �׼� ��ü ����
            //while (actionsQueue.Count > 0)
            //{
            //    // ����
            //    Action.ActionType aType = actionsQueue.Dequeue().type;

            //    Debug.LogError("�׼� ���� :: " + aType);
            //}



            //// ����
            //if (moveCoroutine != null)
            //{
            //    //MoveByAction(ref actNow);
            //    Debug.LogError("�׼� �ߴ� :: " + actNow.type);
            //    StopCoroutine(moveCoroutine);
            //}

            //// ��ǥ ����
            //location = owner.location;

            //// �̵���ǥ ����
            //movePoint = locateBlock.position;

            //// ��ħ ����
            //CharacterMover.AvatarOverFixAll();

            //// ���� ȣ��
            //GetAction();
            ////StopByAction(ref actNow);

            //// �׽�Ʈ�� ����============
            //actNow.type = Action.ActionType.Idle;

        }
    }

    /// <summary>
    /// ���� : ��ǥ �ٶ󺸱� -> �̵� -> ���� �ٶ󺸱�(�ɼ�)
    /// </summary>
    /// <param name="pos">��ǥ</param>
    /// <param name="speed">�ӵ�</param>
    /// <returns></returns>
    IEnumerator ActMove(Vector3 pos, float speed, bool isTurnAfterMove)
    {
        isBusy = true;

        // ��ǥ �ٶ󺸱�
        yield return StartCoroutine(ActTurnPoint(pos, speed));

        // ��ǥ�� �̵�
        yield return StartCoroutine(ActMovePoint(pos, speed));

        // ���� �ٶ󺸱�
        if (isTurnAfterMove)
            yield return StartCoroutine(ActTurnFornt(speed));
        
        isBusy = false;
    }

    /// <summary>
    /// Ư�� ��ǥ�� ���� �̵�
    /// </summary>
    /// <param name="pos">Ư�� ��ǥ</param>
    /// <param name="speed"></param>
    /// <returns></returns>
    IEnumerator ActMovePoint(Vector3 pos, float speed)
    {
        // ��ǥ�� �̵�
        Tool.HeightLimit(bodyObject, posMinY - transform.position.y, posMaxY - transform.position.y);
        Vector3 posY = new Vector3(pos.x, transform.position.y, pos.z);

        // ��ǥ ����
        float dist = Vector3.Distance(transform.position, posY);
        float distNow;
        while ( (distNow = Vector3.Distance(transform.position, posY)) > 0.1f )
        {
            //transform.position = Vector3.Lerp(transform.position, posY, Time.deltaTime * speed);

            // ������ ��ǥ�� ���
            Vector3 targetPos = Vector3.Lerp(transform.position, posY, Time.deltaTime * speed);
                        
            // ��ȣ ����
            Vector3 signPos = new Vector3(Mathf.Sign(targetPos.x - transform.position.x), 0, Mathf.Sign(targetPos.z - transform.position.z));
            // �ִ�ġ ���
            Vector3 limitePos = transform.position + signPos * speed / 5.00f * 0.5f;


            bool isCorner = actionsQueue.Count > 0 && actionsQueue.Peek().type == Action.ActionType.Move; //&& dist / 2 < distNow;
            // �ڳ� ���� ����
            if (isCorner)
            {
                // x�� �ּ�ġ ����
                targetPos.x = limitePos.x;
                targetPos.z = limitePos.z;
            }
            else
            {
                // x�� �ִ�ġ ����
                if (Mathf.Abs(targetPos.x - transform.position.x) > Mathf.Abs(limitePos.x - transform.position.x))
                {
                    //Debug.Log("x�� ������");
                    targetPos.x = limitePos.x;
                }
                // z�� �ִ�ġ ����
                if (Mathf.Abs(targetPos.z - transform.position.z) > Mathf.Abs(limitePos.z - transform.position.z))
                {
                    //Debug.Log("z�� ������");
                    targetPos.z = limitePos.z;
                }
            }


            // x�� �ʰ� ����
            if (Mathf.Abs(posY.x - transform.position.x) < Mathf.Abs(targetPos.x - transform.position.x))
            {
                //Debug.Log("x�� ������");
                targetPos.x = posY.x;
            }
            // z�� �ʰ� ����
            if (Mathf.Abs(posY.z - transform.position.z) < Mathf.Abs(limitePos.z - transform.position.z))
            {
                //Debug.Log("z�� ������");
                targetPos.z = posY.z;
            }


            // �̵� ó��
            transform.position = targetPos;

            yield return null;
        }
        // �� ����
        transform.position = posY;
    }

    /// <summary>
    /// Ư�� ��ǥ�� ���� ȸ��
    /// </summary>
    /// <param name="pos">Ư�� ��ǥ</param>
    /// <param name="speed"></param>
    /// <returns></returns>
    IEnumerator ActTurnPoint(Vector3 pos, float speed)
    {
        // ��ǥ �ٶ󺸱�
        Tool.HeightLimit(bodyObject, posMinY - transform.position.y, posMaxY - transform.position.y);
        Vector3 posY = new Vector3(pos.x, bodyObject.position.y, pos.z);
        float elapsedTime = 0.000f;
        //while (Mathf.Abs(Quaternion.LookRotation((posY - bodyObject.position).normalized).y) - Mathf.Abs(bodyObject.rotation.y) > 0.1f)
        while (Mathf.Abs(Quaternion.LookRotation((bodyObject.position- posY).normalized).y) - Mathf.Abs(bodyObject.rotation.y) > 0.1f)
        {
            elapsedTime += Time.deltaTime;
            // ȸ��
            bodyObject.rotation = Quaternion.Lerp(
                bodyObject.rotation,
                //Quaternion.LookRotation((posY - bodyObject.position).normalized),
                Quaternion.LookRotation((bodyObject.position - posY).normalized),
                elapsedTime * speed
                );

            yield return null;
        }
        // ȸ�� ����
        bodyObject.rotation = Quaternion.Lerp(
            bodyObject.rotation,
            //Quaternion.LookRotation((posY - bodyObject.position).normalized),
            Quaternion.LookRotation((bodyObject.position- posY).normalized),
            1f
            );
    }

    /// <summary>
    /// ������ ���� ȸ��
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    IEnumerator ActTurnFornt(float speed)
    {
        // ���� ����
        //Vector3 dir = bodyObject.position;
        //dir.z += 1f;
        while (Quaternion.LookRotation(Vector3.forward.normalized).y / bodyObject.rotation.y > 0.1f)
        {
            bodyObject.rotation = Quaternion.Lerp(
                bodyObject.rotation,
                Quaternion.LookRotation(Vector3.forward.normalized),
                Time.deltaTime * speed
                );

            yield return null;
        }
        //while (Quaternion.LookRotation((dir - bodyObject.position).normalized).y / bodyObject.rotation.y > 0.1f)
        //{
        //    bodyObject.rotation = Quaternion.Lerp(
        //        bodyObject.rotation,
        //        Quaternion.LookRotation((dir - bodyObject.position).normalized),
        //        Time.deltaTime * speed
        //        );

        //    yield return null;
        //}
        // ȸ�� ����
        bodyObject.rotation = Quaternion.Lerp(
            bodyObject.rotation,
            Quaternion.LookRotation(Vector3.forward.normalized),
            1f
            );
        //bodyObject.rotation = Quaternion.Lerp(
        //    bodyObject.rotation,
        //    Quaternion.LookRotation((dir - bodyObject.position).normalized),
        //    1f
        //    );
    }


    public void GotoJail()
    {
        Debug.LogWarning("�������� �̵� :: " + transform.name);

        // �ܿ� �׼� ����
        actionsQueue.Clear();
        actNow = new Action();

        // �ൿ���� �ο�
        owner.stunCount = 3;

        // ��ġ ����
        _location = -1;
        owner.location = -1;

        // ��ħ �ؼ�
        AvatarOverFix();

        // ��ġ ����
        owner.isDead = true;

        // ��ǥ Ȯ��
        Vector3 jailPos = GameData.blockManager.startBlock.position;

        // ���� �̵� �ߴ�
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        // �̵�
        moveCoroutine = StartCoroutine(ActFly(jailPos, 2f, true));
    }

    /// <summary>
    /// �������� ����Ͽ� �̵� �� �ϰ�
    /// </summary>
    /// <param name="pos">���� ����</param>
    /// <param name="speed">�ӵ�</param>
    /// <param name="isTurnAfterMove">�̵� �� ����</param>
    /// <returns></returns>
    IEnumerator ActFly(Vector3 pos, float speed, bool isTurnAfterMove)
    {
        isBusy = true;

        float height = transform.position.y;

        // ���
        yield return StartCoroutine(ActFlyPoint(transform.position + Vector3.up * 5, speed));

        // ��ǥ ����
        Vector3 flyPoint = pos;
        flyPoint.y = transform.position.y;

        // �̵�
        yield return StartCoroutine(ActMovePoint(flyPoint, speed * 5f));

        // ��ǥ ����
        flyPoint.y = height;

        // �ϰ�
        yield return StartCoroutine(ActFlyPoint(flyPoint, speed));

        // ���� �ٶ󺸱�
        if (isTurnAfterMove)
            yield return StartCoroutine(ActTurnFornt(speed));

        isBusy = false;
    }

    /// <summary>
    /// Ư�� ��ǥ�� ���� �̵�
    /// </summary>
    /// <param name="pos">Ư�� ��ǥ</param>
    /// <param name="speed"></param>
    /// <returns></returns>
    IEnumerator ActFlyPoint(Vector3 pos, float speed)
    {
        // ��ǥ�� �̵�
        Tool.HeightLimit(bodyObject, posMinY - transform.position.y, posMaxY - transform.position.y);
        while (Vector3.Distance(transform.position, pos) > 0.1f)
        {
            //transform.position = Vector3.Lerp(transform.position, posY, Time.deltaTime * speed);

            // ������ ��ǥ�� ���
            Vector3 targetPos = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed);

            // ��ȣ ����
            Vector3 signPos = new Vector3(Mathf.Sign(targetPos.x - transform.position.x), 0, Mathf.Sign(targetPos.z - transform.position.z));
            // �ִ�ġ ���
            Vector3 limitePos = transform.position + signPos * speed / 5.00f * 0.5f;



            // x�� �ִ�ġ ����
            if (Mathf.Abs(targetPos.x - transform.position.x) > Mathf.Abs(limitePos.x - transform.position.x))
            {
                //Debug.Log("x�� ������");
                targetPos.x = limitePos.x;
            }
            // z�� �ִ�ġ ����
            if (Mathf.Abs(targetPos.z - transform.position.z) > Mathf.Abs(limitePos.z - transform.position.z))
            {
                //Debug.Log("z�� ������");
                targetPos.z = limitePos.z;
            }

            // �̵� ó��
            transform.position = targetPos;

            yield return null;
        }
        // �� ����
        transform.position = pos;
    }
}