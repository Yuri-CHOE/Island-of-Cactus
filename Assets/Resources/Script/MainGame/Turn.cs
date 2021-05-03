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

    public void SetUp()
    {
        // 초기화
        origin.Clear();
        queue.Clear();

        // p스타터 등록
        origin.Enqueue(GameData.player.system.Starter);
        queue.Enqueue(GameData.player.system.Starter);

        // p1 등록
        if (GameData.player.player_1 != null)
        {
            origin.Enqueue(GameData.player.player_1);
            queue.Enqueue(GameData.player.player_1);
        }

        // p2 등록
        if (GameData.player.player_2 != null)
        {
            origin.Enqueue(GameData.player.player_2);
            queue.Enqueue(GameData.player.player_2);
        }

        // p3 등록
        if (GameData.player.player_3 != null)
        {
            origin.Enqueue(GameData.player.player_3);
            queue.Enqueue(GameData.player.player_3);
        }

        // p4 등록
        if (GameData.player.player_4 != null)
        {
            origin.Enqueue(GameData.player.player_4);
            queue.Enqueue(GameData.player.player_4);
        }

        // p미니게임 등록
        origin.Enqueue(GameData.player.system.Minigame);
        queue.Enqueue(GameData.player.system.Minigame);

        // p미니게임 엔더 등록
        origin.Enqueue(GameData.player.system.MinigameEnder);
        queue.Enqueue(GameData.player.system.MinigameEnder);

        // p엔더 등록
        origin.Enqueue(GameData.player.system.Ender);
        queue.Enqueue(GameData.player.system.Ender);
    }
}
