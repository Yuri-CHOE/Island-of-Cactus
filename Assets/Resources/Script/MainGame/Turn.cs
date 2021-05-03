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

    public void SetUp()
    {
        // �ʱ�ȭ
        origin.Clear();
        queue.Clear();

        // p��Ÿ�� ���
        origin.Enqueue(GameData.player.system.Starter);
        queue.Enqueue(GameData.player.system.Starter);

        // p1 ���
        if (GameData.player.player_1 != null)
        {
            origin.Enqueue(GameData.player.player_1);
            queue.Enqueue(GameData.player.player_1);
        }

        // p2 ���
        if (GameData.player.player_2 != null)
        {
            origin.Enqueue(GameData.player.player_2);
            queue.Enqueue(GameData.player.player_2);
        }

        // p3 ���
        if (GameData.player.player_3 != null)
        {
            origin.Enqueue(GameData.player.player_3);
            queue.Enqueue(GameData.player.player_3);
        }

        // p4 ���
        if (GameData.player.player_4 != null)
        {
            origin.Enqueue(GameData.player.player_4);
            queue.Enqueue(GameData.player.player_4);
        }

        // p�̴ϰ��� ���
        origin.Enqueue(GameData.player.system.Minigame);
        queue.Enqueue(GameData.player.system.Minigame);

        // p�̴ϰ��� ���� ���
        origin.Enqueue(GameData.player.system.MinigameEnder);
        queue.Enqueue(GameData.player.system.MinigameEnder);

        // p���� ���
        origin.Enqueue(GameData.player.system.Ender);
        queue.Enqueue(GameData.player.system.Ender);
    }
}
