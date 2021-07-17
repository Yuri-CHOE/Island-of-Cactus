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
        // 미완성==================
        Ending,
        Finish,
    }

    // 턴 진행 상태
    public static TurnAction turnAction = TurnAction.Wait;
    public static ActionProgress actionProgress = ActionProgress.Ready;


    // 턴 원본
    static List<Player> _origin = new List<Player>();
    public static List<Player> origin { get { return _origin; } }

    // 턴 사본
    public static Queue<Player> queue = new Queue<Player>();

    // 현재 턴
    public static Player now { get { return queue.Peek(); } }

    public static Player Next()
    {
        // 하나 꺼내서 재입력 (순환)
        queue.Enqueue(queue.Dequeue());

        return now;
    }

    public static void Skip(Player skipStop)
    {
        // 잘못된 플레이어 입력시 중단
        if(!queue.Contains(skipStop))
            if (!origin.Contains(skipStop))
            {
                Debug.LogError("error :: 존재하지 않는 플레이어");
                Debug.Break();
                return;
            }

        while (now != skipStop)
            Next();
    }

    public static void Clear()
    {
        // 초기화
        TurnAction turnAction = TurnAction.Wait;
        ActionProgress actionProgress = ActionProgress.Ready;
        origin.Clear();
        queue.Clear();
    }

    public static void Add(Player player)
    {
        // 등록
        origin.Add(player);
        queue.Enqueue(player);
    }

    public static void SetUp(List<Player> order)
    {
        // 오류 방지
        if (order == null)
            return;

        // 초기화
        Clear();

        // p스타터 등록
        Add(Player.system.Starter);

        // p1~4 등록
        List<Player> orderCopy = new List<Player>(order);
        if (orderCopy.Count > 0)
        {
            // 정렬
            if (orderCopy.Count >= 2)
                while (orderCopy.Count > 0)
                {
                    int temp = 0;
                    for (int j = 0; j < orderCopy.Count; j++)
                    {
                        if (orderCopy[j].dice.value > orderCopy[temp].dice.value)
                        {
                            temp = j;
                        }
                    }

                    // 등록
                    Debug.Log(string.Format("순서 = ({0}) {1}", orderCopy[temp].dice.value, orderCopy[temp].name));
                    Add(orderCopy[temp]);

                    // 정렬
                    order.Remove(orderCopy[temp]);
                    order.Add(orderCopy[temp]);

                    //제외
                    orderCopy.RemoveAt(temp);
                }
        }

        // p미니게임 등록
        Add(Player.system.Minigame);

        // p미니게임 엔더 등록
        Add(Player.system.MinigameEnder);

        // p엔더 등록
        Add(Player.system.Ender);
    }

    /// <summary>
    /// 현재 플레이어의 플레이어 리스트 인덱스를 반환
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
