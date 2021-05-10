using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    /*
     * < �̱��� �䱸���� >
     * ĳ���� �׼� ����
     * ĳ���� �̵� ����
     * ĳ���� �ִ� �ּ� ��ǥ ����
     */
    //


    // ��ֹ� ���
    public static List<int> barricade = new List<int>();

    // �׼� ť
    public Queue<Action> actionsQueue = new Queue<Action>();

    // ��ġ �ε���
    int _location = -1;
    public int location { get { return _location; } /*set { _location = GameData.blockManager.indexLoop(_location, value); }*/ }

    // �̵���
    public int moveCount = 0;

    // ��ġ ������Ʈ
    public Transform locateBlock { get { if (GameData.isMainGameScene) { if (location >= 0) return GameData.blockManager.GetBlock(location).transform; else return GameData.blockManager.startBlock; } else return null; } }



    // �̵� �ӵ�
    float moveSpeed = 1.00f;


    [SerializeField]
    float posMinY = 1.9f;           // ĳ���� �ּ� ����
    [SerializeField]
    float posMaxY = 20f;            // ĳ���� �ִ� ����






    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �� ����
        Tool.HeightLimit(transform, posMinY, posMaxY);
    }

    /// <summary>
    /// moveLocation ��ġ�� ���� �����ٸ�
    /// </summary>
    /// <param name="moveLocation">��ġ</param>
    void PlanMoveTo(int moveLocation)
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
    /// moveValue ��ŭ �̵� �����ٸ�
    /// </summary>
    /// <param name="moveValue"></param>
    void PlanMoveBy(int moveValue)
    {
        // ���� ��ǥ
        int movePoint = GameData.blockManager.indexLoop(_location, moveValue);

        // �� �̵� �Ÿ�(moveValue) �̳��� ��ֹ� ���
        int counter = 0;
        int _sign = (int)Mathf.Sign(moveValue);
        for (int i = 0; i < moveValue; i += _sign)
        {
            counter += _sign;

            // ��ֹ� üũ
            if (barricade[location + ((i + 1) * _sign)] > 0)
            {
                //�����ٸ� �߰� - counter��ŭ ĭ�� ����
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, counter, moveSpeed));

                // ī���� ����
                counter = 0;
            }
        }
    }
}
