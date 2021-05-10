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
        Opening,
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

    [Header("script")]
    // 로딩 매니저 스크립트
    [SerializeField]
    LoadingManager loadingManager = null;

    // 주사위 컨트롤러 스크립트
    [SerializeField]
    DiceController diceController = null;

    // 캐릭터 오브젝트 부모 스크립트
    [SerializeField]
    Transform characterParent = null;

    // PlayerInfo UI 오브젝트
    [Space]
    public List<PlayerInfoUI> playerInfoUI = new List<PlayerInfoUI>();
    public CanvasGroup MainUI = null;


    // 아이템 사용 명령
    public bool useItemOrder = false;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DoFlowWork();
    }

    public void DoFlowWork()
    {
        switch (GameData.gameFlow)
        {
            // 아직 게임 시작 안됨
            case Flow.Wait:
                {
                    // 로딩 완료 대기
                    if (loadingManager.isFinish) return;

                    // 세이브파일 작성
                    // 미구현================
                    // 원본에서 복사할것



                    // 플레이어 구성 초기화
                    {
                        if (GameData.gameMode == GameMode.Mode.None)
                            return;
                        else if (GameData.gameMode == GameMode.Mode.Online)
                        {
                            // 미구현 : 별도 지정 필요===========
                        }
                        else if (GameData.player.allPlayer.Count == 0)
                        {
                            // 중복 방지 작업
                            List<int> picked = Tool.RandomNotCross(1, Character.table.Count, 4);
                            if (picked.Contains(GameData.player.me.character.index))
                                picked.Remove(GameData.player.me.character.index);

                            // 초기화 진행
                            GameData.player.player_1 = GameData.player.me;
                            GameData.player.player_2 = new Player(Player.Type.AI, picked[0], false, "Player02");
                            GameData.player.player_3 = new Player(Player.Type.AI, picked[1], false, "Player03");
                            GameData.player.player_4 = new Player(Player.Type.AI, picked[2], false, "Player04");

                            // 플레이어 리스트 구성
                            GameData.player.allPlayer.Add(GameData.player.player_1);
                            GameData.player.allPlayer.Add(GameData.player.player_2);
                            GameData.player.allPlayer.Add(GameData.player.player_3);
                            GameData.player.allPlayer.Add(GameData.player.player_4);

                            // 임시 큐 구성
                            //for(int i = 0; i < GameData.player.allPlayer.Count; i++)
                            //    GameData.turn.queue.Enqueue(GameData.player.allPlayer[i]);
                        }
                    }


                    // 캐릭터 생성 및 캐릭터 아이콘 로드
                    for (int i = 0; i < GameData.player.allPlayer.Count; i++)
                        if (GameData.player.allPlayer[i].avatar == null)
                        {
                            // 캐릭터 생성
                            GameData.player.allPlayer[i].CreateAvatar(characterParent);
                            GameData.player.allPlayer[i].avatar.name = "p" + (i + 1);

                            // 캐릭터 아이콘 로드
                            GameData.player.allPlayer[i].LoadFace();
                        }


                    Debug.Log("게임 플로우 :: 새 게임 호출 확인됨");
                    GameData.gameFlow = Flow.Start;
                    break;
                }


            // 게임 시작됨
            case Flow.Start:
                {
                    // 로딩중일 경우 대기
                    if (!loadingManager.isFinish || !loadingManager.isFadeFinish)
                        return;


                    // 맵 둘러보기
                    // 미구현==================                

                    Debug.Log("게임 플로우 :: 게임 시작 확인됨");
                    GameData.gameFlow = Flow.Ordering;
                    break;
                }


            // 순서주사위
            case Flow.Ordering:
                // 순서주사위 굴리기
                {

                    //Debug.Log("게임 플로우 :: 순서 주사위 확인중");

                    // 주사위를 아무도 굴리지 않을때
                    if (diceController.isFree)
                    {

                        // 플레이어별 체크
                        for (int i = 0; i < GameData.player.allPlayer.Count; i++)
                        {
                            // 이미 굴렸으면 다음 플레이어 처리
                            if (GameData.player.allPlayer[i].dice.isRolled)
                                continue;

                            // 해당 플레이어가 굴리고 있지 않으면 주사위 호출
                            if (!GameData.player.allPlayer[i].dice.isRolling)
                            {
                                Debug.Log(string.Format("게임 플로우 :: Player{0} 주사위 굴리는중", i+1));
                                // 주사위 기능 호출
                                diceController.CallDice(
                                    GameData.player.allPlayer[i],
                                    GameData.player.allPlayer[i].avatar.transform
                                    );

                                // 다른 플레이어 무시
                                break;
                            }
                        }
                    }

                    // 주사위 마무리
                    if (diceController.isFinish)
                        diceController.UseDice();

                    // 모두가 주사위 굴리지 않으면 중단
                    for (int i = 0; i < GameData.player.allPlayer.Count; i++)
                        if (!GameData.player.allPlayer[i].dice.isRolled)
                            return;
                    
                    Debug.Log("게임 플로우 :: 모든 플레이어 주사위 굴림 완료 =>" + GameData.player.allPlayer.Count);

                    // 모든 플레이어 주사위 굴림완료 상태 초기화
                    for (int i = 0; i < GameData.player.allPlayer.Count; i++)
                        GameData.player.allPlayer[i].dice.isRolled = false;


                    // 순서용 리스트 복사
                    List<Player> pOrderList = new List<Player>(GameData.player.allPlayer);
                    Debug.Log("게임 플로우 :: 리스트 복사 체크 =>" + pOrderList.Count + " = " + GameData.player.allPlayer.Count);

                    // 순서큐 셋팅 및 리스트 순차 정리
                    GameData.turn.SetUp(pOrderList);

                    // PlayerInfo UI 초기화
                    for (int i = 0; i < pOrderList.Count; i++)
                    {
                        Debug.Log("게임 플로우 :: 디버그 =>" + pOrderList[i].name);
                        pOrderList[i].infoUI = playerInfoUI[i];
                        Debug.Log("게임 플로우 :: 디버그 =>" + pOrderList[i].infoUI.transform.name);
                        Debug.Log("게임 플로우 :: 디버그 =>" + pOrderList[i].infoUI.transform.name);
                        pOrderList[i].infoUI.SetPlayer(pOrderList[i]);
                    }

                    // PlayerInfo UI 활성
                    StartCoroutine(Tool.CanvasFade(MainUI, true, 1.5f));


                    //GameData.gameFlow = Flow.CycleStart;
                    GameData.gameFlow = Flow.Opening;
                    break;
                }


            // 사이클링 시작 전 오프닝 단계
            case Flow.Opening:
                {
                    // 연출 일단 생략


                    GameData.gameFlow = Flow.Cycling;
                    break;
                }


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


    void TurnWork()
    {
        // 로그 기록
        Debug.Log("플레이어 호출 :: " + GameData.turn.now.name);

        // 종료 조건 체크


        // 시스템 플레이어 - 스타터
        if (GameData.turn.now == GameData.player.system.Starter)
        {
            // 시작 연출 및 각종 초기화

            // 턴 종료 처리
            GameData.turn.Next();
        }
        // 시스템 플레이어 - 미니게임
        else if(GameData.turn.now == GameData.player.system.Minigame)
        {
            // 미니게임 선정

            // 턴 종료 처리
            GameData.turn.Next();

            // 미니게임 로드
        }
        // 시스템 플레이어 - 미니게임 엔더
        else if (GameData.turn.now == GameData.player.system.MinigameEnder)
        {
            // 미니게임 정산

            // 턴 종료 처리
            GameData.turn.Next();
        }
        // 시스템 플레이어 - 엔더
        else if (GameData.turn.now == GameData.player.system.Ender)
        {
            // 종료 분기 체크
            
            // 사이클 증가

            // 턴 종료 처리
            GameData.turn.Next();
            Debug.Break();
        }
        // 실제 플레이어
        else
        {
            PlayerWork();
        }
    }


    void PlayerWork()
    {
        // 로그 기록
        Debug.Log("턴 진행 :: " + GameData.turn.now.name);

        // 초기화
        if (GameData.turn.turnAction == Turn.TurnAction.Wait)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화


                // 스킵
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // 시작 연출


                // 스킵
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {
                // 메인 작업


                // 스킵
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // 종료 연출


                // 스킵
                GameData.turn.turnAction = Turn.TurnAction.Opening;
                GameData.turn.actionProgress = ActionProgress.Ready;
            }

        }
        // 시작 연출
        else if (GameData.turn.turnAction == Turn.TurnAction.Opening)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화


                // 스킵
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // 각종 체크

                // 모든 플레이어 대상 라이프 체크
                CheckLife();

                // 행동 불가능 체크
                if (GameData.turn.now.isStun)
                {
                    // 턴 중단 및 종료 연출로 이동
                    GameData.turn.turnAction = Turn.TurnAction.Ending;
                    GameData.turn.actionProgress = ActionProgress.Ready;
                    return;
                }


                // 스킵
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {
                // 시작 연출


                // 스킵
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {


                // 스킵
                GameData.turn.turnAction = Turn.TurnAction.Item;
                GameData.turn.actionProgress = ActionProgress.Ready;
            }

        }
        // 주사위 굴리기
        else if (GameData.turn.turnAction == Turn.TurnAction.DiceRolling)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화


                // 스킵
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {
                // 아이템 아이콘 누르면 사용 버튼 활성

                // 주사위를 굴리는중 중단
                if (diceController.isBusy)
                    return;

                // 해당 플레이어가 굴리고 있지 않으면 주사위 호출
                else if (diceController.isFree)
                {
                    Debug.Log(string.Format("게임 플로우 :: Player({0}) 주사위 굴리는중", GameData.turn.now.name));

                    // 주사위 기능 호출
                    diceController.CallDice(
                        GameData.turn.now,
                        GameData.turn.now.avatar.transform
                        );
                }

                // 아이템 사용시
                else if(useItemOrder)
                {
                    // 시간 정지
                    diceController.isTimeCountWork = false;

                    // 아이템 사용 단계로 워프
                    GameData.turn.turnAction = Turn.TurnAction.Item;
                    GameData.turn.actionProgress = ActionProgress.Ready;
                }

                // 주사위 마무리
                else if (diceController.isFinish)
                {
                    // 사용 처리
                    diceController.UseDice();

                    // 주사위 더이상 없을시 스킵
                    if (diceController.owner.dice.count <= 0)
                        GameData.turn.actionProgress = ActionProgress.Finish;
                }
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {


                // 스킵
                GameData.turn.turnAction = Turn.TurnAction.Plan;
                GameData.turn.actionProgress = ActionProgress.Ready;
            }

        }
        // 아이템 사용 단계
        else if (GameData.turn.turnAction == Turn.TurnAction.Item)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화


                // 소모처리

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                Debug.LogWarning("아이템 사용됨");


                // 연출

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {
                // 효과 적용

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                Debug.LogWarning("아이템 사용 종료");

                // 사용 종료 연출


                // 다시 시간 흐름
                diceController.isTimeCountWork = true;

                // 다시 주사위 마저 진행
                GameData.turn.turnAction = Turn.TurnAction.DiceRolling;
                GameData.turn.actionProgress = ActionProgress.Working;
            }

        }
        // 액션 계획 단계
        else if (GameData.turn.turnAction == Turn.TurnAction.Plan)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // 시작 연출

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // 종료 연출

                
                // 다음으로 스킵
                GameData.turn.turnAction = Turn.TurnAction.Action;
                GameData.turn.actionProgress = ActionProgress.Working;
            }

        }
        // 액션 단계
        else if (GameData.turn.turnAction == Turn.TurnAction.Action)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // 시작 연출

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // 종료 연출


                // 다음으로 스킵
                GameData.turn.turnAction = Turn.TurnAction.Block;
                GameData.turn.actionProgress = ActionProgress.Working;
            }

        }
        // 블록 기능 수행 단계
        else if (GameData.turn.turnAction == Turn.TurnAction.Block)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // 시작 연출

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {
                // 블록 기능 수행===================미구현
                BlockWork();

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // 종료 연출


                // 다음으로 스킵
                GameData.turn.turnAction = Turn.TurnAction.Ending;
                GameData.turn.actionProgress = ActionProgress.Working;
            }

        }
        // 종료 연출 단계
        else if (GameData.turn.turnAction == Turn.TurnAction.Ending)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // 시작 연출

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // 종료 연출


                // 다음으로 스킵
                GameData.turn.turnAction = Turn.TurnAction.Finish;
                GameData.turn.actionProgress = ActionProgress.Working;
            }

        }
        // 종료 단계
        else if (GameData.turn.turnAction == Turn.TurnAction.Finish)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // 시작 연출

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {

                // 스킵
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // 종료 연출


                // 턴 종료 처리
                GameData.turn.Next();
            }

        }
    }


    void BlockWork()
    {
    }



    /// <summary>
    /// 라이프를 체크하여 감옥으로 보냄
    /// </summary>
    public void CheckLife()
    {
        // 체크
        for (int i = 0; i < GameData.player.allPlayer.Count; i++)
            if (GameData.player.allPlayer[i].life.checkMin(1))
                GotoPrison(GameData.player.allPlayer[i]);
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
