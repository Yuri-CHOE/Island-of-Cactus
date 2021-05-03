using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public enum Flow
    {
        Wait,
        Start,
        Ordering,
        Cycling,
        //CycleStart,
        //Turn,
        //MiniGameStart,
        //MiniGame,
        //MiniGameEnd,
        //CycleEnd,
        End,
        Trophy,
        Finish,
    }

    // 로딩 매니저 스크립트
    [SerializeField]
    LoadingManager loadingManager = null;

    // 캐릭터 오브젝트 부모 스크립트
    [SerializeField]
    GameObject characterParent = null;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoFlowWork()
    {
        switch (GameData.gameFlow)
        {
            // 아직 게임 시작 안됨
            case Flow.Wait:
                // 로딩 완료 대기
                if (loadingManager.isFinish)    return;

                // 캐릭터 생성
                // 미구현================

                // 세이브파일 작성
                // 미구현================
                // 원본에서 복사할것

                Debug.Log("게임 플로우 :: 새 게임 호출 확인됨");
                GameData.gameFlow = Flow.Start;
                break;


            // 게임 시작됨
            case Flow.Start:

                // 플레이어 구성 초기화
                if (GameData.gameMode == GameMode.Mode.None)
                    return;
                else if (GameData.gameMode == GameMode.Mode.Online)
                {
                    // 미구현 : 별도 지정 필요===========
                }
                else
                {
                    // 중복 방지 작업
                    List<int> picked = Tool.RandomNotCross(1, Character.table.Count, 4);
                    if (picked.Contains(GameData.player.me.character.index))
                        picked.Remove(GameData.player.me.character.index);

                    // 초기화 진행
                    GameData.SetPlayer(
                        GameData.player.me,
                        new Player(Player.Type.AI, picked[0], false, null),
                        new Player(Player.Type.AI, picked[1], false, null),
                        new Player(Player.Type.AI, picked[2], false, null)
                        );
                }


                // 맵 둘러보기
                // 미구현==================                

                Debug.Log("게임 플로우 :: 게임 시작 확인됨 => by ");
                GameData.gameFlow = Flow.Ordering;
                break;


            // 순서주사위
            case Flow.Ordering:
                // 순서주사위 굴리기
                // 미구현==================

                // 순서큐 셋팅
                GameData.turn.SetUp();      // 순서 주사위 배정 이후 턴 초기화

                //GameData.gameFlow = Flow.CycleStart;
                GameData.gameFlow = Flow.Cycling;
                break;


            // 게임 진행
            case Flow.Cycling:
                if (GameData.cycle.isEnd())
                {
                    Debug.Log("게임 플로우 :: 종료 조건 달성 확인됨 => by " + GameData.turn.now.name);
                    GameData.gameFlow = Flow.End;
                }
                else
                {
                    TurnWork();
                }
                break;


            /*
        case Flow.Turn:
            if (GameData.turn.now == GameData.player.system.Minigame)
                GameData.gameFlow = Flow.MiniGameStart;
            else if (GameData.turn.now == GameData.player.system.Ender)
                GameData.gameFlow = Flow.CycleEnd;
            break;
        case Flow.MiniGameStart:
            GameData.gameFlow = Flow.MiniGame;
            break;
        case Flow.MiniGame:
            GameData.gameFlow = Flow.MiniGameEnd;
            break;
        case Flow.MiniGameEnd:
            GameData.gameFlow = Flow.Turn;      // 주의 : 다시 턴으로 돌아가서 시스템 플레이어 엔더 역할 수행됨
            break;
        case Flow.CycleEnd:
            if(GameData.cycle.isEnd())
                GameData.gameFlow = Flow.End;
            else
                GameData.gameFlow = Flow.CycleStart;
            break;
            */


            // 게임 종료됨
            case Flow.End:
                GameData.gameFlow = Flow.Trophy;
                break;


            // 트로피 지급 및 우승자 발표
            case Flow.Trophy:
                GameData.gameFlow = Flow.Finish;
                break;


            // 게임 종료됨
            case Flow.Finish:
                break;
        }
    }


    public void TurnWork()
    {
        // 로그 기록
        Debug.Log("플레이어 호출 :: " + GameData.turn.now.name);

        // 시스템 플레이어 - 스타터
        if (GameData.turn.now == GameData.player.system.Starter)
        {

        }
        // 시스템 플레이어 - 미니게임
        else if(GameData.turn.now == GameData.player.system.Minigame)
        {

        }
        // 시스템 플레이어 - 엔더
        else if (GameData.turn.now == GameData.player.system.Ender)
        {

        }
        // 실제 플레이어
        else
        {

        }
    }


    /// <summary>
    /// 라이프를 체크하여 감옥으로 보냄
    /// </summary>
    public void CheckLife()
    {
        // 1p 체크
        if (GameData.player.player_1 != null)
            if (GameData.player.player_1.life.Value <= 0)
                GotoPrison(GameData.player.player_1);

        // 2p 체크
        if (GameData.player.player_2 != null)
            if (GameData.player.player_2.life.Value <= 0)
                GotoPrison(GameData.player.player_2);

        // 3p 체크
        if (GameData.player.player_3 != null)
            if (GameData.player.player_3.life.Value <= 0)
                GotoPrison(GameData.player.player_3);

        // 4p 체크
        if (GameData.player.player_4 != null)
            if (GameData.player.player_4.life.Value <= 0)
                GotoPrison(GameData.player.player_4);
    }

    /// <summary>
    /// 특정 플레이어를 감옥으로 보냄
    /// </summary>
    /// <param name="targetPlayer"></param>
    public void GotoPrison(Player targetPlayer)
    {
        // 미구현================
    }
}
