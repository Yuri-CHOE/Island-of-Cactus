using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
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
        // 미완성==================
        Ending,
        Finish,
    }

    // 턴 진행 상태
    public TurnAction turnAction = TurnAction.Wait;
    public ActionProgress actionProgress = ActionProgress.Ready;


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

    public void Clear()
    {
        // 초기화
        origin.Clear();
        queue.Clear();
    }

    public void Add(Player player)
    {
        // 등록
        origin.Enqueue(player);
        queue.Enqueue(player);
    }

    public void SetUp(List<Player> order)
    {
        // 오류 방지
        if (order == null)
            return;

        // 초기화
        Clear();

        // p스타터 등록
        Add(GameData.player.system.Starter);

        // p1~4 등록
        if (order.Count > 0)
        {
            // 정렬
            if (order.Count > 1)
                for (int i = 0; i < order.Count; i++)
                {
                    if (order[0].dice.value > order[1].dice.value)
                    {
                        // 가장 뒤에 추가
                        order.Add(order[0]);
                        order.RemoveAt(0);
                    }
                }

            // 등록
            for (int i = 0; i < order.Count; i++)
                Add(order[i]);
        }

        // p미니게임 등록
        Add(GameData.player.system.Minigame);

        // p미니게임 엔더 등록
        Add(GameData.player.system.MinigameEnder);

        // p엔더 등록
        Add(GameData.player.system.Ender);
    }
}
