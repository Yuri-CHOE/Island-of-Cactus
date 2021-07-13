using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI;
using CustomAI.MainGame;

public class AIWorker : MonoBehaviour
{
    // 소유자
    public Player owner = null;
    public bool isAuto { get { if (owner == null) return false; else return owner.isAutoPlay; } }

    List<AI> aiList = new List<AI>();

    // 메인게임 AI
    public MainGameAI mainGame = new MainGameAI();
    public class MainGameAI
    {
        // 주사위 굴리기
        public AI dice = new DiceAI();
    }

    public void SetUp(Player _owner)
    {
        // 소유자 지정
        owner = _owner;

        // AI 목록 등록
        aiList.Add(mainGame.dice);

        // AI 소유자 지정
        for (int i = 0; i < aiList.Count; i++)
        {
            aiList[i].owner = _owner;
        }
    }


    void Update()
    {
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