using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Turn
{
    public enum TurnAction
    {
        Wait,
        Opening,
        DiceRolling,
        Item,
        Plan,
        Action,
        Block,
        //LuckyBox,
        //Shop,
        //BossGame,
        // �̿ϼ�==================
        Ending,
        Finish,
    }

    // �� ���� ����
    public static TurnAction turnAction = TurnAction.Wait;
    public static ActionProgress actionProgress = ActionProgress.Ready;


    // �� ����
    static List<Player> _origin = new List<Player>();
    public static List<Player> origin { get { return _origin; } }

    // �� �纻
    public static Queue<Player> queue = new Queue<Player>();

    // ���� ��
    public static Player now { get { return queue.Peek(); } }

    // �� ȹ�� ���� ~ ù������ ���� ��
    public static bool isFirstFrame = false;

    public static Player Next()
    {
        // �ϳ� ������ ���Է� (��ȯ)
        queue.Enqueue(queue.Dequeue());

        // ù������ Ȱ��ȭ
        isFirstFrame = true;

        // �ڵ�����
        GameMaster.script.AutoSave();

        return now;
    }

    public static void Skip(string playerName)
    {
        for (int i = 0; i < origin.Count; i++)
        {
            // �� ��ġ üũ
            if (origin[i].name.Equals(playerName))
            {
                // ��ŵ �۵�
                Skip(origin[i]);
                return;
            }
        }
    }
    public static void Skip(Player skipStop)
    {
        // �߸��� �÷��̾� �Է½� �ߴ�
        if(!queue.Contains(skipStop))
            if (!origin.Contains(skipStop))
            {
                Debug.LogError("error :: �������� �ʴ� �÷��̾�");
                Debug.Break();
                return;
            }

        while (now != skipStop)
            // �ϳ� ������ ���Է� (��ȯ)
            queue.Enqueue(queue.Dequeue());

        Debug.Log("�� ���� :: �÷��̾��� ������ ����� -> " + skipStop.name);
    }

    public static void Clear()
    {
        // �ʱ�ȭ
        turnAction = TurnAction.Wait;
        actionProgress = ActionProgress.Ready;
        origin.Clear();
        queue.Clear();
        Player.order.Clear();
        isFirstFrame = false;
    }

    public static void Add(Player player)
    {
        // ���
        origin.Add(player);
        queue.Enqueue(player);
    }

    public static void SetUp(List<Player> order)
    {
        // ���� ����
        if (order == null)
            return;

        // �ʱ�ȭ
        Clear();

        // p��Ÿ�� ���
        Add(Player.system.Starter);

        // p1~4 ���
        List<Player> orderCopy = new List<Player>(order);
        if (orderCopy.Count > 0)
        {
            // ����
            if (orderCopy.Count >= 2)
                while (orderCopy.Count > 0)
                {
                    int temp = 0;
                    for (int j = 0; j < orderCopy.Count; j++)
                    {
                        if (orderCopy[j].dice.orderDiceValue > orderCopy[temp].dice.orderDiceValue)
                        {
                            temp = j;
                        }
                    }

                    // ���
                    Debug.Log(string.Format("���� = ({0}) {1}", orderCopy[temp].dice.orderDiceValue, orderCopy[temp].name));
                    Add(orderCopy[temp]);

                    // ���� ����Ʈ ���
                    Player.order.Add(orderCopy[temp]);

                    // ����
                    order.Remove(orderCopy[temp]);
                    order.Add(orderCopy[temp]);

                    //����
                    orderCopy.RemoveAt(temp);
                }
        }

        // p�̴ϰ��� ���
        Add(Player.system.Minigame);

        // p�̴ϰ��� ���� ���
        Add(Player.system.MinigameEnder);

        // p���� ���
        Add(Player.system.Ender);
    }

    /// <summary>
    /// ���� �÷��̾��� �÷��̾� ����Ʈ �ε����� ��ȯ
    /// </summary>
    /// <returns></returns>
    public static int Index(Player current)
    {       
        for (int i = 0; i < origin.Count; i++)
            if (origin[i] == current)
                return i;

        return -1;
    }
}
