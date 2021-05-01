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
}
