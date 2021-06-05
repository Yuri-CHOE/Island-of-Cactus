using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    // ��ֹ� ���
    public static List<int> barricade = new List<int>();

    // �׼� ť
    public Queue<Action> actionsQueue = new Queue<Action>();

    // ���� �׼�
    public Action actNow = new Action();


    public static float ActTurnSpeed = 5f;


    // ��ǥ �̵� ��ǥ
    Vector3 movePoint = Vector3.zero;

    // �̵� �����
    Coroutine moveCoroutine = null;
    public bool isBusy = false;



    // ��ġ �ε���
    int _location = -1;
    //public int location { get { return _location; } set { _location = GameData.blockManager.indexLoop(_location, value); } }
    public int location { get { return _location; } set { _location = GameData.blockManager.indexLoop(value, 0); } }

    // �̵���
    public int moveCount = 0;

    // ���� ��ġ ��� ������Ʈ
    public Transform locateBlock { get { if (GameData.isMainGameScene) { if (location >= 0) return GameData.blockManager.GetBlock(location).transform; else return GameData.blockManager.startBlock; } else return null; } }



    // �̵� �ӵ�
    float moveSpeed = 1.00f;


    [SerializeField]
    float posMinY = 1.9f;           // ĳ���� �ּ� ����
    [SerializeField]
    float posMaxY = 20f;            // ĳ���� �ִ� ����


    // ȸ���� ���� ������Ʈ
    [SerializeField]
    Transform bodyObject = null;



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
        // �� ����
        Tool.HeightLimit(bodyObject, posMinY - transform.position.y, posMaxY - transform.position.y);
    }


    /// <summary>
    /// ĳ���� ��ħ ó��
    /// </summary>
    public void AvatarOverFix()
    {

        // �ߺ� �÷��̾� ����Ʈ
        List<Player> fixTarget = new List<Player>();

        // ��� �÷��̾� ��ġ üũ
        for (int i = 0; i < GameData.player.allPlayer.Count; i++)
        {
            // ���� ����
            //if (GameData.player.allPlayer[i].avatar == transform)
            //    continue;

            // ��ġ �ߺ� üũ
            if (location == GameData.player.allPlayer[i].avatar.GetComponent<CharacterMover>().location)
                // �ߺ� �÷��̾� Ȯ��
                fixTarget.Add(GameData.player.allPlayer[i]);
        }

        // ���ƴ� ������ �����̼� �״�� =========== ū ������ �ƴ�

        // ��ģ ���
        Vector3 corssPoint = new Vector3();
        if (location == -1)     // ��ŸƮ ���
            corssPoint = BlockManager.script.startBlock.transform.position;
        else                    // �� ��
            corssPoint = BlockManager.script.GetBlock(location).transform.position;

        // 4�� �ߺ�
        if (fixTarget.Count >= 4)
        {
            fixTarget[0].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(-2, 0, 2), ActTurnSpeed, true);
            fixTarget[1].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(2, 0, 2), ActTurnSpeed, true);
            fixTarget[2].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(2, 0, -2), ActTurnSpeed, true);
            fixTarget[3].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(-2, 0, -2), ActTurnSpeed, true);

            //fixTarget[0].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[0].avatar.transform.position + new Vector3(-2, 0, 2), ActTurnSpeed, true);
            //fixTarget[1].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[1].avatar.transform.position + new Vector3(2, 0, 2), ActTurnSpeed, true);
            //fixTarget[2].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[2].avatar.transform.position + new Vector3(2, 0, -2), ActTurnSpeed, true);
            //fixTarget[3].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[3].avatar.transform.position + new Vector3(-2, 0, -2), ActTurnSpeed, true);
        }
        // 3�� �ߺ�
        else if (fixTarget.Count == 3)
        {
            fixTarget[0].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(-2, 0, 2), ActTurnSpeed, true);
            fixTarget[1].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(2, 0, 2), ActTurnSpeed, true);
            fixTarget[2].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(0, 0, -2), ActTurnSpeed, true);

            //fixTarget[0].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[0].avatar.transform.position + new Vector3(-2, 0, 2), ActTurnSpeed, true);
            //fixTarget[1].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[1].avatar.transform.position + new Vector3(2, 0, 2), ActTurnSpeed, true);
            //fixTarget[2].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[2].avatar.transform.position + new Vector3(0, 0, -2), ActTurnSpeed, true);
        }
        // 2�� �ߺ�
        else if (fixTarget.Count == 2)
        {
            fixTarget[0].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(-2, 0, 0), ActTurnSpeed, true);
            fixTarget[1].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(+2, 0, 0), ActTurnSpeed, true);

            //fixTarget[0].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[0].avatar.transform.position + new Vector3(-2, 0, 0), ActTurnSpeed, true);
            //fixTarget[1].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[1].avatar.transform.position + new Vector3(+2, 0, 0), ActTurnSpeed, true);
        }
        // �ߺ� ����
        else if (fixTarget.Count == 1)
        {

        }
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
        // ���� ��ǥ
        int movePoint = GameData.blockManager.indexLoop(_location, moveValue);

        Debug.LogError(moveValue + " = " + movePoint);

        // �� �̵� �Ÿ�(moveValue) �̳��� ��ֹ� ���
        int counter = 0;
        int _sign = (int)Mathf.Sign(moveValue);
        for (int i = 0; i != moveValue; i += _sign)
        {
            counter += _sign;

            int loc = GameData.blockManager.indexLoop(location, ((i) * _sign));
            int locNext = GameData.blockManager.indexLoop(location,  ((i + 1) * _sign));


            // ��ŸƮ ��� üũ
            if(location == -1 && i == 0)
            {
                Debug.Log("��ŸƮ ��� Ž�� : " + counter);

                //�����ٸ� �߰� - counter��ŭ ĭ�� ����
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));

                //�����ٸ� �߰� - ȸ�� �׼� �߰�
                //actionsQueue.Enqueue(new Action(Action.ActionType.Turn, GameData.blockManager.GetBlock(locNext).GetComponent<DynamicBlock>().GetDirection(), moveSpeed));

                // ī���� ����
                counter = 0;
            }

            // ��ֹ� üũ
            if (barricade[locNext] > 0)
            {
                Debug.Log("��ֹ� Ž�� : " + counter);

                //�����ٸ� �߰� - counter��ŭ ĭ�� ����
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));
                //actionsQueue.Enqueue(new Action(Action.ActionType.Move, counter, moveSpeed));

                //�����ٸ� �߰� - ���� �߰�
                //actionsQueue.Enqueue(new Action(Action.ActionType.Stop, 1, moveSpeed));

                // ī���� ����
                counter = 0;
            }

            // �ڳ� üũ
            if (
            GameData.blockManager.GetBlock(locNext).GetComponent<DynamicBlock>().GetDirection()
            !=
            GameData.blockManager.GetBlock(GameData.blockManager.indexLoop(locNext, - 1)).GetComponent<DynamicBlock>().GetDirection()
            // ��ŸƮ��Ͽ��� �����ϰ� Ž������ ��ŸƮ ��� Ȥ�� �� ���� ĭ�� ��� ���� ó��
            && !(location == -1 && (loc == 0 || locNext == 0))
            )
            {
                Debug.Log("�ڳ� Ž�� : " + counter);

                //�����ٸ� �߰� - counter��ŭ ĭ�� ����
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i+ _sign, moveSpeed));
                //actionsQueue.Enqueue(new Action(Action.ActionType.Move, counter, moveSpeed));

                //�����ٸ� �߰� - ȸ�� �׼� �߰�
                //actionsQueue.Enqueue(new Action(Action.ActionType.Turn, GameData.blockManager.GetBlock(locNext).GetComponent<DynamicBlock>().GetDirection(), moveSpeed));

                // ī���� ����
                counter = 0;
            }

            // ���� �� üũ
            if (counter > 0 && (i + _sign == moveValue))
            {
                Debug.Log("���� Ž�� : " + counter);

                //�����ٸ� �߰� - counter��ŭ ĭ�� ����
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i+ _sign, moveSpeed));
                //actionsQueue.Enqueue(new Action(Action.ActionType.Move, counter, moveSpeed));

                // ī���� ����
                counter = 0;

                break;
            }

            Debug.Log(counter + " , " + (i + _sign == moveValue));
        }

        //�����ٸ� �߰� - ���� ����
        //actionsQueue.Enqueue(new Action(Action.ActionType.Turn, 2, moveSpeed));
    }


    public ref Action GetAction()
    {
        actNow = actionsQueue.Dequeue();
        return ref actNow;
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
            if (act.count > 0)
                MoveSet(
                    GameData.blockManager.GetBlock(GameData.blockManager.indexLoop(location, act.count)).transform.position,
                    act.speed,
                    false
                    );

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
                MoveSet(transform.position, ActTurnSpeed, true);

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

        // �� �ڵ� ���
        {
            //// ��ǥ �ٶ󺸱�
            //Vector3 dir = transform.position;
            //dir.y = pos.y - transform.position.y;
            //float elapsedTime = 0.000f;
            //while (Quaternion.LookRotation(dir.normalized).y / transform.rotation.y > 0.1f)
            //{
            //    elapsedTime += Time.deltaTime;
            //    // ȸ��
            //    transform.rotation = Quaternion.Lerp(
            //        transform.rotation,
            //        Quaternion.LookRotation(dir.normalized),
            //        elapsedTime * speed
            //        );

            //    yield return null;
            //}
            //// ȸ�� ����
            //transform.rotation = Quaternion.Lerp(
            //    transform.rotation,
            //    Quaternion.LookRotation(dir.normalized),
            //    1f
            //    );


            //// ��ǥ�� �̵�
            //Vector3 posY = new Vector3(pos.x, transform.position.y, pos.z);
            //while (Vector3.Distance(transform.position, posY) > 0.1f)
            //{
            //    transform.position = Vector3.Lerp(transform.position, posY, Time.deltaTime * speed);

            //    yield return null;
            //}
            //// �� ����
            //transform.position = posY;
        }
        
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
        while (Vector3.Distance(transform.position, posY) > 0.1f)
        {
            //transform.position = Vector3.Lerp(transform.position, posY, Time.deltaTime * speed);

            // ������ ��ǥ�� ���
            Vector3 targetPos = Vector3.Lerp(transform.position, posY, Time.deltaTime * speed);
                        
            // ��ȣ ����
            Vector3 signPos = new Vector3(Mathf.Sign(targetPos.x - transform.position.x), 0, Mathf.Sign(targetPos.z - transform.position.z));
            // �ִ�ġ ���
            Vector3 limitePos = transform.position + signPos * speed / 5.00f;



            // x�� �ִ�ġ ����
            if (Mathf.Abs(targetPos.x - transform.position.x) > Mathf.Abs(limitePos.x - transform.position.x))
            {
                Debug.Log("x�� ������");
                targetPos.x = limitePos.x;
            }
            // z�� �ִ�ġ ����
            if (Mathf.Abs(targetPos.z - transform.position.z) > Mathf.Abs(limitePos.z - transform.position.z))
            {
                Debug.Log("z�� ������");
                targetPos.z = limitePos.z;
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
}
