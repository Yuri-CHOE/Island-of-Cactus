using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    // �� ����
    Queue<Player> _origin = new Queue<Player>();
    public Queue<Player> origin { get { return _origin; } }

    // �� �纻
    public Queue<Player> queue = new Queue<Player>();

    // ���� ��
    public Player now { get { return queue.Peek(); } }

    public Player Next()
    {
        // �ϳ� ������ ���Է� (��ȯ)
        queue.Enqueue(queue.Dequeue());

        return now;
    }

    public void Clear()
    {
        // �ʱ�ȭ
        origin.Clear();
        queue.Clear();
    }

    public void Add(Player player)
    {
        // ���
        origin.Enqueue(player);
        queue.Enqueue(player);
    }

    public void SetUp(List<Player> order)
    {
        // ���� ����
        if (order == null)
            return;

        // �ʱ�ȭ
        Clear();

        // p��Ÿ�� ���
        Add(GameData.player.system.Starter);

        // p1~4 ���
        if (order.Count > 0)
            for (int i = 0; i < order.Count; i++)
                Add(order[i]);

        // p�̴ϰ��� ���
        Add(GameData.player.system.Minigame);

        // p�̴ϰ��� ���� ���
        Add(GameData.player.system.MinigameEnder);

        // p���� ���
        Add(GameData.player.system.Ender);
    }
}
