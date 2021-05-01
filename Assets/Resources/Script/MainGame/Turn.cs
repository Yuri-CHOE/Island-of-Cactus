using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    // 턴 원본
    Queue<Player> _origin = new Queue<Player>();
    public Queue<Player> origin { get { return _origin; } }

    // 턴 사본
    public Queue<Player> queue = new Queue<Player>();

    // 현재 턴
    public Player now { get { return queue.Peek(); } }

    public Player Next()
    {
        // 하나 꺼내서 재입력 (순환)
        queue.Enqueue(queue.Dequeue());

        return now;
    }
}
