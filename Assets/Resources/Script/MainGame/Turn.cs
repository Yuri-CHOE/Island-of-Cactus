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

    // 턴 획득 이후 ~ 첫프레임 종료 전
    public static bool isFirstFrame = false;

    public static Player Next()
    {
        // 하나 꺼내서 재입력 (순환)
        queue.Enqueue(queue.Dequeue());

        // 첫프레임 활성화
        isFirstFrame = true;

        // 자동저장
        GameMaster.script.AutoSave();

        return now;
    }

    public static void Skip(string playerName)
    {
        for (int i = 0; i < origin.Count; i++)
        {
            // 값 일치 체크
            if (origin[i].name.Equals(playerName))
            {
                // 스킵 작동
                Skip(origin[i]);
                return;
            }
        }
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
            // 하나 꺼내서 재입력 (순환)
            queue.Enqueue(queue.Dequeue());

        Debug.Log("턴 제어 :: 플레이어의 턴으로 변경됨 -> " + skipStop.name);
    }

    public static void Clear()
    {
        // 초기화
        turnAction = TurnAction.Wait;
        actionProgress = ActionProgress.Ready;
        origin.Clear();
        queue.Clear();
        Player.order.Clear();
        isFirstFrame = false;
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
                        if (orderCopy[j].dice.orderDiceValue > orderCopy[temp].dice.orderDiceValue)
                        {
                            temp = j;
                        }
                    }

                    // 등록
                    Debug.Log(string.Format("순서 = ({0}) {1}", orderCopy[temp].dice.orderDiceValue, orderCopy[temp].name));
                    Add(orderCopy[temp]);

                    // 순서 리스트 등록
                    Player.order.Add(orderCopy[temp]);

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
