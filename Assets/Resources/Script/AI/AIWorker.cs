using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI;
using CustomAI.MainGame;

public class AIWorker : MonoBehaviour
{
    public enum Risk
    {
        None,
        Low,
        Middle,
        High,
    }

    // 소유자
    public Player owner = null;
    public bool isAuto { get { if (owner == null) return false; else return owner.isAutoPlay; } }

    // 위험 감수 정도
    public Risk risk = Risk.None;

    List<AI> aiList = new List<AI>();

    // 메인게임 AI
    public MainGameAI mainGame = new MainGameAI();
    public class MainGameAI
    {
        // 주사위 굴리기
        public AI itemUse = new ItemUseAI();
        public AI dice = new DiceAI();
        public AI itemBuy = new ItemBuyAI();
    }

    public void SetUp(Player _owner)
    {
        // 소유자 지정
        owner = _owner;

        // AI 목록 등록
        aiList.Add(mainGame.itemUse);
        aiList.Add(mainGame.dice);
        aiList.Add(mainGame.itemBuy);

        // AI 소유자 지정
        for (int i = 0; i < aiList.Count; i++)
        {
            aiList[i].owner = _owner;
        }

        // 위험 감수 정도 지정
        risk = (Risk)(Random.Range(1,4));
    }


    void Update()
    {
        if (GameData.gameFlow > GameMaster.Flow.Start)
        {
            // 게임 중단 반영
            if (GameMaster.isBlock)
                return;

            if (isAuto)
            {
                // 목록 순회 체크
                for (int i = 0; i < aiList.Count; i++)
                {
                    if (aiList[i].isStart)
                    {
                        // 작동 종료 체크 및 종료처리
                        if (aiList[i].CheckReset())
                        {
                            aiList[i].Ready();
                        }

                        // 시간 경과
                        aiList[i].Aging();
                    }
                    // 작동 체크 및 작동
                    else if (aiList[i].CheckStart())
                    {
                        // 작업 초기화
                        aiList[i].Ready();

                        // 작업 시작 처리
                        aiList[i].isStart = true;

                        // 작업 시작
                        aiList[i].coroutine = StartCoroutine(aiList[i].Work());

                    }
                }
            }
        }
    }

}