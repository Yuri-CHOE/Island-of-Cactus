using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster script = null;

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
    public LoadingManager loadingManager = null;

    // 사이클 UI 관리 스크립트
    public CycleManager cycleManager = null;

    // 메시지 박스 스크립트
    public MessageBox messageBox = null;

    // 주사위 컨트롤러 스크립트
    public DiceController diceController = null;

    // 아이템 관리 스크립트
    public ItemManager itemManager = null;

    // 이벤트 관리 스크립트
    public EventManager eventManager = null;

    // 캐릭터 오브젝트 부모
    [SerializeField]
    Transform characterParent = null;

    // PlayerInfo UI 오브젝트
    [Space]
    public List<PlayerInfoUI> playerInfoUI = new List<PlayerInfoUI>();
    public CanvasGroup MainUI = null;

    // 플레이어 선택기
    public List<Transform> playerSelecter = new List<Transform>();


    // 아이템 사용 명령
    public static bool useItemOrder = false;

    // 씬 재로드 제어용
    public static Flow flowCopy = Flow.Wait;


    // 트로피 오브젝트
    public GameObject Trophy1st = null;
    public GameObject Trophy2nd = null;
    public GameObject Trophy3rd = null;


    // 게임 정지
    public static bool isBlock = false;

    // 자동저장
    public static bool useAutoSave = true;


    private void Awake()
    {
        // 퀵 등록
        script = this;

        // 재로드 제어용 백업 및 리셋
        flowCopy = GameData.gameFlow;
        GameData.gameFlow = Flow.Wait;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.gameFlow > Flow.Start && isBlock)
            return;

        DoFlowWork();

        //if(GameData.gameFlow == Flow.Cycling)
        //Debug.LogError(Turn.now.name);

        // 첫프레임 제어
        Turn.isFirstFrame = false;
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

                    // 세이브 파일 로드
                    if (GameSaver.useLoad)
                        GameSaver.LoadGameInfo();

                    // 세이브파일 작성
                    // 미구현================
                    // 원본에서 복사할것


                    // 중력 설정
                    if (flowCopy == Flow.Wait)
                        Physics.gravity = Physics.gravity * 10;


                    // 플레이어 구성 초기화
                    {
                        if (GameData.gameMode == GameMode.Mode.None)
                            return;
                        else if (GameData.gameMode == GameMode.Mode.Online)
                        {
                            // 미구현 : 별도 지정 필요===========
                        }
                        else if (Player.allPlayer.Count == 0)
                        {
                            // 중복 방지 작업
                            List<int> picked = Tool.RandomNotCross(1, Character.table.Count, 4);
                            if (picked.Contains(Player.me.character.index))
                                picked.Remove(Player.me.character.index);

                            // 플레이어 초기화 및 리스트 구성
                            Player.player_1 = Player.me;
                            Player.allPlayer.Add(Player.player_1);

                            if (GameRule.playerCount >= 2)
                            {
                                Player.player_2 = new Player(Player.Type.AI, picked[0], true, "Player02");
                                Player.allPlayer.Add(Player.player_2);
                            }

                            if (GameRule.playerCount >= 3)
                            {
                                Player.player_3 = new Player(Player.Type.AI, picked[1], true, "Player03");
                                Player.allPlayer.Add(Player.player_3);
                            }

                            if (GameRule.playerCount >= 4)
                            {
                                Player.player_4 = new Player(Player.Type.AI, picked[2], true, "Player04");
                                Player.allPlayer.Add(Player.player_4);
                            }

                            // 시스템 플레이어 - 다른 플레이어 설정
                            Player.system.Monster.otherPlayers = new List<Player>(Player.allPlayer);
                        }
                    }


                    // 캐릭터 생성 및 캐릭터 아이콘 로드
                    for (int i = 0; i < Player.allPlayer.Count; i++)
                    {
                        // 대상 플레이어
                        Player current = Player.allPlayer[i];

                        // 캐릭터 아이콘 로드
                        current.LoadFace();

                        // "다른 플레이어" 구성
                        {
                            // 모든 플레이어 등록
                            for (int j = 0; j < Player.allPlayer.Count; j++)
                                current.otherPlayers.Add(Player.allPlayer[j]);

                            // 등록자 본인 제외
                            current.otherPlayers.Remove(current);
                        }

                        // 캐릭터 생성
                        if (Player.allPlayer[i].avatar == null)
                        {

                            // 캐릭터 생성
                            current.CreateAvatar(characterParent);
                            current.avatar.name = "p" + (i + 1);

                            // 캐릭터 이동
                            if (current.movement.location == -1)
                                current.avatar.transform.position = GameData.blockManager.startBlock.position;
                            else
                                current.avatar.transform.position = GameData.blockManager.GetBlock(current.movement.location).transform.position;
                        }

                        // 미니게임 정보 초기화
                        if (Player.allPlayer[i].miniInfo == null)
                        {
                            Player.allPlayer[i].miniInfo = new MiniScore();
                        }
                        Player.system.Monster.miniInfo = new MiniScore();

                    }

                    
                    // PlayerInfo UI 비활성
                    MainUI.GetComponent<CanvasGroup>().alpha = 0f;
                    MainUI.blocksRaycasts = false;


                    // 숏컷 등록
                    ShortcutManager.script.SetUp();

                    // 사이클 설정
                    Cycle.goal = GameRule.cycleMax;


                    // 세이브 파일 로드
                    if (GameSaver.useLoad)
                    {
                        GameSaver.LoadPlayer();
                        GameSaver.LoadItemObject();
                        GameSaver.LoadEventObject();
                    }

                    // 캐릭터 겹침 해소
                    //Player.me.movement.AvatarOverFix();
                    CharacterMover.AvatarOverFixAll();

                    // 종료절차 진입 초기화
                    EndManager.Reset();


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
                    // 씬 재로드 제어
                    if (flowCopy <= Flow.Ordering && !GameSaver.useLoad)
                    {
                        //Debug.Log("게임 플로우 :: 순서 주사위 확인중");

                        // 주사위를 아무도 굴리지 않을때
                        if (diceController.isFree)
                        {

                            // 플레이어별 체크
                            for (int i = 0; i < Player.allPlayer.Count; i++)
                            {
                                Player current = Player.allPlayer[i];

                                // 이미 굴렸으면 다음 플레이어 처리
                                if (current.dice.isRolled)
                                    continue;

                                // 해당 플레이어가 굴리고 있지 않으면 주사위 호출
                                if (!current.dice.isRolling)
                                {
                                    Debug.Log(string.Format("게임 플로우 :: Player{0} 주사위 굴리는중", i + 1));

                                    // 주사위 지급
                                    Player.allPlayer[i].dice.count = 1;

                                    // 주사위 기능 호출
                                    diceController.CallDice(
                                        current,
                                        current.avatar.transform
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
                        for (int i = 0; i < Player.allPlayer.Count; i++)
                            if (!Player.allPlayer[i].dice.isRolled)
                                return;

                        Debug.Log("게임 플로우 :: 모든 플레이어 주사위 굴림 완료 =>" + Player.allPlayer.Count);

                    }
                    else
                    {
                        // 씬 재로드시 Opening 스킵
                        // 아래 PlayerInfo UI 초기화 문제로 return하면 안됨
                        GameData.gameFlow = Flow.Cycling;
                    }

                    // 순서큐 셋팅
                    if (Turn.origin.Count == 0)
                    {
                        // 순서용 리스트 복사
                        List<Player> pOrderList = new List<Player>(Player.allPlayer);
                        //Debug.Log("게임 플로우 :: 리스트 복사 체크 =>" + pOrderList.Count + " = " + Player.allPlayer.Count);

                        // 순서큐 셋팅 및 리스트 순차 정리
                        Turn.SetUp(pOrderList);
                    }

                    // 모든 플레이어 주사위 굴림완료 상태 초기화
                    for (int i = 0; i < Player.allPlayer.Count; i++)
                        Player.allPlayer[i].dice.Clear();

                    // PlayerInfo UI 초기화
                    //for (int i = 0; i < pOrderList.Count; i++)
                    for (int i = 0; i < Player.order.Count; i++)
                    {
                        //pOrderList[i].infoUI = playerInfoUI[i];
                        //pOrderList[i].infoUI.SetPlayer(pOrderList[i]);
                        Player.order[i].infoUI = playerInfoUI[i];
                        Player.order[i].infoUI.SetPlayer(Player.order[i]);
                    }

                    // 미할당 PlayerInfo UI 제거
                    for (int i = Player.allPlayer.Count; i < playerInfoUI.Count; i++)
                        playerInfoUI[i].gameObject.SetActive(false);

                    // 세이브 파일 로드
                    if (GameSaver.useLoad)
                    {
                        GameSaver.LoadPlayerInventory();

                        // 스타트 플레이어 강제 호출
                        TurnWork();

                        GameSaver.LoadTurn();
                    }
                    GameSaver.Clear();

                    //// 순서큐 셋팅 및 리스트 순차 정리
                    //Turn.SetUp(pOrderList);

                    // PlayerInfo UI 활성
                    StartCoroutine(Tool.CanvasFade(MainUI, true, 1.5f));
                    MainUI.blocksRaycasts = true;


                    //GameData.gameFlow = Flow.CycleStart;
                    GameData.gameFlow = Flow.Opening;
                    break;
                }


            // 사이클링 시작 전 오프닝 단계
            case Flow.Opening:
                {
                    // 연출 일단 생략


                    // 턴 시작
                    if(Cycle.now == 0)
                        Cycle.now = 1;

                    GameData.gameFlow = Flow.Cycling;
                    break;
                }


            // 게임 진행
            case Flow.Cycling:
                if (Cycle.isEnd())
                {
                    Debug.Log("게임 플로우 :: 종료 조건 달성 확인됨 => by " + Turn.now.name);

                    // 종료 연출
                    // 미구현=========================

                    cycleManager.Refresh();
                    GameData.gameFlow = Flow.End;
                }
                else
                {
                    TurnWork();

                    // 자동저장
                    AutoSave(true);
                }
                break;


            /*
        case Flow.Turn:
            if (Turn.now == Player.system.Minigame)
                GameData.gameFlow = Flow.MiniGameStart;
            else if (Turn.now == Player.system.Ender)
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
            if(Cycle.isEnd())
                GameData.gameFlow = Flow.End;
            else
                GameData.gameFlow = Flow.CycleStart;
            break;
            */


            // 게임 종료됨
            case Flow.End:
                {
                    // 모든 플레이어 시작점 소환
                    if (EndManager.endProgress == ActionProgress.Ready)
                    {
                        if (EndManager.coroutine == null)
                            EndManager.coroutine = StartCoroutine(EndManager.CallCenter());

                        return;
                    }
                    //bool check = false;
                    //for (int i = 0; i < Player.allPlayer.Count; i++)
                    //{
                    //    // 위치 체크
                    //    if (Player.allPlayer[i].movement.location != -1)
                    //    {
                    //        // 이동중이 아닐 경우 이동
                    //        if (!Player.allPlayer[i].movement.isBusy)
                    //            Player.allPlayer[i].movement.Tleport(-1, 1f);
                    //    }
                    //    else
                    //    {
                    //        // 이동 종료 체크
                    //        check = check || Player.allPlayer[i].movement.isBusy;
                    //    }
                    //}

                    //// 모든 플레이어 이동 대기
                    //if (check)
                    //    return;

                    // 연출 종료 처리
                    GameData.gameFlow = Flow.Trophy;
                    Debug.LogWarning("게임 정산 :: 종료절차 수행");
                }
                break;


            // 트로피 지급 및 우승자 발표
            case Flow.Trophy:
                {
                    // 트로피 지급 및 우승자 선정
                    if (EndManager.endProgress == ActionProgress.Ready)
                        return;
                    else if (EndManager.endProgress == ActionProgress.Start)
                    {

                        // 트로피 지급 및 유저 데이터 기록, 세이브 제거
                        if (EndManager.coroutine == null)
                            EndManager.coroutine = StartCoroutine(EndManager.Trophy());

                        return;
                    }

                    GameData.gameFlow = Flow.Finish;
                }
                break;


            // 게임 종료됨
            case Flow.Finish:
                // 타이틀 화면으로
                // 미구현================

                // 타이틀 씬 로딩
                if(!loadingManager.isLoading)
                    loadingManager.LoadAsync("Title");
                break;
        }
    }


    void TurnWork()
    {
        // 로그 기록
        //Debug.Log("플레이어 호출 :: " + Turn.now.name);

        // 종료 조건 체크

        // 라이프 체크
        CheckLife();
        //for (int i = 0; i < Player.allPlayer.Count; i++)
        //{
        //    // 라이프 0 또는 음수일 경우
        //    if (Player.allPlayer[i].life.Value < 1)
        //    {
        //        // 이미 감옥일 경우 중단
        //        if (Player.allPlayer[i].isDead)
        //        {
        //            //Debug.LogError("라이프 체크 :: 이미 수감됨 => " + Player.allPlayer[i].name);
        //            continue;
        //        }

        //        //Debug.LogError("라이프 체크 :: 감지됨 => " + Player.allPlayer[i].name);
        //        Player.allPlayer[i].movement.GotoJail();
        //    }
        //}

        // 시스템 플레이어 - 스타터
        if (Turn.now == Player.system.Starter)
        {
            // 시작 연출 및 각종 초기화

            // 모든 플레이어 대상
            for (int i = 0; i < Player.allPlayer.Count; i++)
            {
                //주사위 초기화
                Player.allPlayer[i].dice.Clear();

                //미니게임 기록 초기화
                Player.allPlayer[i].miniInfo.Reset();

                //// 부활 체크
                //if (Player.allPlayer[i].isDead)
                //    if (Player.allPlayer[i].stunCount <= 0)
                //        Player.allPlayer[i].Resurrect();

                // 행동 불가능 체크
                if (Player.allPlayer[i].isStun)
                {
                    // 남은 대기 턴 감소
                    Player.allPlayer[i].stunCount--;
                }
            }

            // 사이클 UI 갱신
            cycleManager.Refresh();

            // 턴 종료 처리
            Turn.Next();
        }
        // 시스템 플레이어 - 미니게임
        else if(Turn.now == Player.system.Minigame)
        {
            // 미니게임 선정
            if (MiniGameManager.progress == ActionProgress.Ready)
            {
                // 미니게임 알림 연출
                // 미구현=============================
                
                // 미니게임 로드
                loadingManager.LoadAsyncMiniGameRandom();

                Turn.Next();
            }

        }
        // 시스템 플레이어 - 미니게임 엔더
        else if (Turn.now == Player.system.MinigameEnder)
        {
            // 미니게임 로딩 대기
            if (MiniGameManager.progress != ActionProgress.Finish)
            {
                //Debug.LogError(MiniGameManager.progress);
                return;
            }

            //// 미니게임 정산 페이지 삭제
            //MiniReportManager report = FindObjectOfType<MiniReportManager>();
            //Transform.Destroy(report.transform.root);


            // 모든 플레이어 대상
            for (int i = 0; i < Player.allPlayer.Count; i++)
            {
                // 미니게임 정산

                // 불참자 제외
                if (!Player.allPlayer[i].miniInfo.join)
                    continue;

                // 코인 지급
                Player.allPlayer[i].coin.Add(Player.allPlayer[i].miniInfo.reward);

                // 기록
                Player.allPlayer[i].miniInfo.Record();

                // 초기화
                Player.allPlayer[i].miniInfo.Reset();
            }

            // 미니게임 보상 및 지분 초기화
            MiniScore.totalReward = 0;
            MiniScore.totalRewardRatio = 0;
            MiniGameManager.minigameNow = null;
            MiniGameManager.progress = ActionProgress.Ready;


            // 턴 종료 처리
            Turn.Next();
        }
        // 시스템 플레이어 - 엔더
        else if (Turn.now == Player.system.Ender)
        {
            // 종료 분기 체크

            // 모든 플레이어 대상
            for (int i = 0; i < Player.allPlayer.Count; i++)
            {
                // 아이템 변화 체크
                for (int j = 0; j < Player.allPlayer[i].inventoryCount; j++)
                {
                    if (Player.allPlayer[i].inventory[j].item.index == 10)
                    {
                        // 변화 확률
                        int percentage = 5 * Player.allPlayer[i].inventory[j].effect.value;
                        Debug.Log("디버그 :: 변화 확률 = " + percentage);

                        // 변화 여부
                        bool isTrans = false;

                        if (percentage >= 100)
                        {
                            // 무조건 변화
                            isTrans = true;
                        }
                        else if (percentage > 0)
                        {
                            // 확률 적중시 변화
                            int rand = Random.Range(0, 100);
                            if (percentage >= rand)
                            {
                                Debug.Log("디버그 :: 변화 확률 = " + percentage);
                                isTrans = true;
                            }
                        }

                        // 변화
                        if (isTrans)
                        {
                            Player.allPlayer[i].inventory[j].item = Item.table[8];
                            Debug.Log("아이템 :: 변화 -> " + Player.allPlayer[i].inventory[j].item.name);
                        }
                    }
                }
            }



            // 사이클 증가
            Cycle.NextCycle();

            // 턴 종료 처리
            Turn.Next();
            //Debug.Break();
        }
        // 실제 플레이어
        else
        {
            PlayerWork();
        }
    }


    void PlayerWork()
    {
        // 초기화
        if (Turn.turnAction == Turn.TurnAction.Wait)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화

                // 로그 기록
                Debug.Log("턴 진행 :: " + Turn.turnAction + " & " + Turn.actionProgress + " :: " + Turn.now.name);


                // 스킵
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                // 각종 체크

                // 부활 체크
                if (Turn.now.isDead)
                    if (Turn.now.stunCount <= 0)
                        Turn.now.Resurrect();

                // 행동 불가능 체크
                if (Turn.now.isStun)
                {
                    //// 남은 대기 턴 감소
                    //Turn.now.stunCount--;

                    // 턴 중단 및 종료 연출로 이동
                    Turn.turnAction = Turn.TurnAction.Ending;
                    Turn.actionProgress = ActionProgress.Ready;
                    return;
                }

                // 주사위 지급
                Turn.now.dice.count = 1;
                Debug.Log("주사위 수량 :: " + Turn.now.dice.count);


                // 카메라 강제 포커스
                if (Player.me == Turn.now)
                    GameData.worldManager.cameraManager.LockBtn.isOn = true;


                // 스킵
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {
                // 메인 작업


                // 스킵
                Turn.actionProgress = ActionProgress.Finish;
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {
                // 종료 연출


                // 스킵
                Turn.turnAction = Turn.TurnAction.Opening;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // 시작 연출
        else if (Turn.turnAction == Turn.TurnAction.Opening)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화


                // 스킵
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                // 연출 준비


                // 스킵
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {
                // 시작 연출


                // 스킵
                Turn.actionProgress = ActionProgress.Finish;
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {


                // 스킵
                Turn.turnAction = Turn.TurnAction.DiceRolling;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // 주사위 굴리기
        else if (Turn.turnAction == Turn.TurnAction.DiceRolling)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화


                // 스킵
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {

                // 스킵
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {

                // 아이템 사용시
                if (useItemOrder)
                {
                    // 시간 정지
                    diceController.isTimeCountWork = false;

                    // 아이템 사용 단계로 워프
                    Turn.turnAction = Turn.TurnAction.Item;
                    Turn.actionProgress = ActionProgress.Ready;
                }

                // 주사위를 굴리는중 중단
                else if (diceController.isBusy)
                    return;

                // 해당 플레이어가 굴리고 있지 않으면 주사위 호출
                else if (diceController.isFree)
                {
                    Debug.Log(string.Format("게임 플로우 :: Player({0}) 주사위 굴리는중", Turn.now.name));

                    // 주사위 기능 호출
                    diceController.CallDice(
                        Turn.now,
                        Turn.now.avatar.transform
                        );
                }

                // 주사위 마무리
                else if (diceController.isFinish)
                {
                    // 사용 처리
                    diceController.UseDice();

                    // 주사위 더이상 없을시 스킵
                    if (diceController.owner == null)   // 주사위 0개일 경우 소유권 박탈됨
                        Turn.actionProgress = ActionProgress.Finish;
                }
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {


                // 스킵
                Turn.turnAction = Turn.TurnAction.Plan;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // 아이템 사용 단계
        else if (Turn.turnAction == Turn.TurnAction.Item)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화


                // 스킵
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                Debug.LogWarning("아이템 사용됨");

                // 사용 준비 연출



                // 아이템 사용
                itemManager.ItemUse(itemManager.selected);
                //itemManager.ItemUseByUI();

                // 스킵
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {
                // 효과 적용



                // 아이템 사용중 대기처리
                if (useItemOrder)
                    return;


                // 스킵
                Debug.LogWarning("아이템 사용 종료");
                Turn.actionProgress = ActionProgress.Finish;
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {

                // 사용 종료 연출





                // 다시 시간 흐름
                diceController.isTimeCountWork = true;

                // 다시 주사위 마저 진행
                Turn.turnAction = Turn.TurnAction.DiceRolling;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // 액션 계획 단계
        else if (Turn.turnAction == Turn.TurnAction.Plan)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화

                // 스킵
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                // 시작 연출

                // 스킵
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {
                // 액션 스케줄링
                Debug.LogWarning( "액션 스케줄링 :: 총 이동력 => " +Turn.now.dice.valueTotal);
                Turn.now.movement.PlanMoveBy(
                    Turn.now.dice.valueTotal
                    );

                // 스킵
                Turn.actionProgress = ActionProgress.Finish;
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {
                // 종료 연출

                
                // 다음으로 스킵
                Turn.turnAction = Turn.TurnAction.Action;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // 액션 단계
        else if (Turn.turnAction == Turn.TurnAction.Action)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화

                // 스킵
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                // 시작 연출

                // 카메라 부착
                GameData.worldManager.cameraManager.CamMoveTo(Turn.now.avatar.transform, CameraManager.CamAngle.Top);

                // 스킵
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {
                // 스크립트 퀵 등록
                CharacterMover movement = Turn.now.movement;

                // 액션 미수행 경우
                if (movement.actNow.type == Action.ActionType.None)
                {
                    // 잔여 액션 없음
                    if (movement.actionsQueue.Count == 0)
                    {
                        //// 카메라 탈착
                        //GameData.worldManager.cameraManager.CamFree();

                        // 주사위 값 제거
                        Turn.now.dice.SetValueTotal(0);

                        // 스킵
                        Turn.actionProgress = ActionProgress.Finish;
                    }
                }

            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {
                // 종료 연출

                // 다음으로 스킵
                Turn.turnAction = Turn.TurnAction.Block;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // 블록 기능 수행 단계
        else if (Turn.turnAction == Turn.TurnAction.Block)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화

                // 블록 기능 초기화
                BlockWork.Clear();

                // 스킵
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {

                // 스킵
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {
                // 블록 기능 수행
                if (!BlockWork.isWork)
                    BlockWork.Work(Turn.now);

                // 스킵
                if (BlockWork.isEnd)
                {
                    if (messageBox.gameObject.activeSelf)
                    {
                        Debug.LogError("왜 턴이 멋대로 넘어갈까?");
                        Debug.Break();
                    }
                    Turn.actionProgress = ActionProgress.Finish;
                }
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {
                // 종료 연출


                // 다음으로 스킵
                Turn.turnAction = Turn.TurnAction.Ending;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // 종료 연출 단계
        else if (Turn.turnAction == Turn.TurnAction.Ending)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화

                // 카메라 탈착
                GameData.worldManager.cameraManager.CamFree();

                // 스킵
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                // 시작 연출

                // 스킵
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {

                // 스킵
                Turn.actionProgress = ActionProgress.Finish;
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {
                // 종료 연출


                // 다음으로 스킵
                Turn.turnAction = Turn.TurnAction.Finish;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // 종료 단계
        else if (Turn.turnAction == Turn.TurnAction.Finish)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // 각종 초기화

                // 스킵
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                // 시작 연출

                // 스킵
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {

                // 스킵
                Turn.actionProgress = ActionProgress.Finish;
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {
                // 종료 연출

                // 진행 초기화
                Turn.turnAction = Turn.TurnAction.Wait;
                Turn.actionProgress = ActionProgress.Ready;


                // 턴 종료 처리
                Turn.Next();
            }

        }
    }



    /// <summary>
    /// 라이프를 체크하여 감옥으로 보냄
    /// </summary>
    public void CheckLife()
    {
        // 체크
        for (int i = 0; i < Player.allPlayer.Count; i++)
            if (Player.allPlayer[i].life.checkMin(1))
                if (!Player.allPlayer[i].isDead)
                    GotoJail(Player.allPlayer[i]);
    }

    /// <summary>
    /// 특정 플레이어를 감옥으로 보냄
    /// </summary>
    /// <param name="targetPlayer"></param>
    public void GotoJail(Player targetPlayer)
    {
        targetPlayer.movement.GotoJail();
    }


    /// <summary>
    /// 테스트 전용
    /// </summary>
    public void SaveGame()
    {
        AutoSave(false);
    }

    /// <summary>
    /// 테스트 전용
    /// </summary>
    public void StopMove()
    {
        Turn.now.movement.MoveStop();
    }

    /// <summary>
    /// 테스트 전용
    /// </summary>
    /// <param name="minigameNum"></param>
    public void StartMiniGame(int minigameNum)
    {

        //// 참가자 설정
        //// 추후 플레이어 리스트를 인자로 받을것
        //// 테스트용 전원 참가
        //for (int i = 0; i < Player.allPlayer.Count; i++)
        //{
        //    Player.allPlayer[i].miniInfo.join = true;
        //}

        //// 로드
        //loadingManager.LoadAsync(sceneName);

        // 호출
        loadingManager.LoadAsyncMiniGame(Minigame.table[minigameNum], 100, Player.allPlayer);
    }

    public void AutoSave(bool isSmoothSave)
    {
        // 턴의 처음이면
        if (Turn.isFirstFrame)
            // 자동저장 활성화 일경우
            if (useAutoSave)
                // 부드러운 저장
                if (isSmoothSave)
                {
                    // 저장
                    if (GameData.saveControl == null)
                        GameData.saveControl = StartCoroutine(AutoSaveForced());
                }
                else
                {
                    // 저장
                    GameSaver.GameSave();
                }

    }
    IEnumerator AutoSaveForced()
    {
        // 저장
        GameSaver.GameSave();
        yield return null;

        // 완료 처리
        GameData.saveControl = null;
    }
}
