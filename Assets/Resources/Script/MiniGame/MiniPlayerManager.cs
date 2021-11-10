using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniPlayerManager : MonoBehaviour
{
    //public static MiniPlayerManager script = null;

    [Header("Player UI")]
    public MiniGamePlayer monster = null;
    public List<MiniGamePlayer> scoreList = new List<MiniGamePlayer>();

    // 턴 제어용
    Queue<Player> turn = null;
    public Player turnNow { get { return turn.Peek(); } }    
    public bool isFirstFrame = true;           // 턴 획득 이후 ~ 첫프레임 종료 전

    // 참여자 목록
    public static List<Player> entryPlayer = null;
    public static List<Player> entryAI = new List<Player>();


    int entryCount { get { return turn.Count; } }


    void Awake()
    {
        // 순서 초기화
        turn = new Queue<Player>(Player.order);
    }
    public void Init()
    {
        // 퀵 등록
        //script = this;

        // 참가자 설정
        SetEntry();
    }

    void Start()
    {
    }

    void Update()
    {
        // 플레이어 준비 대기상태 체크
        //if (MiniGameManager.progress < ActionProgress.Working)
        if (!MiniGameManager.script.isGameStart)
        {
            if (entryPlayer != null)
            {
                bool allReady = true;

                // 한명이라도 준비 안되면 false
                for (int i = 0; i < entryPlayer.Count; i++)
                {
                    allReady = allReady && entryPlayer[i].miniInfo.isReady;
                }

                // 시작 여부 반영
                MiniGameManager.script.isGameStart = allReady;
                if (allReady)
                {
                    // 준비 버튼 비활성화
                    MiniGameManager.script.readyText.transform.parent.gameObject.SetActive(false);

                    MiniGameManager.progress = ActionProgress.Working;
                }
            }
        }

        // 턴의 첫 프레임일 경우
        if (isFirstFrame)
        {
            Debug.Log("미니게임 :: 턴 시작됨 -> " + turnNow.name);

            // AI 플레이어 작동
            if (turnNow.type == Player.Type.AI || turnNow == Player.system.Monster)
            {
                // AI 작동 요청
                if (turnNow.miniAi.workControl == null)
                {
                    Debug.Log("미니 AI :: 작동됨 -> " + turnNow.name);
                    turnNow.miniAi.workControl = StartCoroutine(turnNow.miniAi.Work(MiniGameManager.minigameNow));
                }
            }
        }

        // 턴의 첫 프레임 소모
        isFirstFrame = false;
    }

    public void NextTurn()
    {
        // 턴 종료자
        Player end = turn.Dequeue();

        // 가장 뒤에 다시 넣기
        turn.Enqueue(end);

        // 턴 표시 제거
        end.miniPlayerUI.BlinkOff();

        // 턴의 첫 프레임 셋팅
        isFirstFrame = true;

        // AI 답안 리셋
        MiniGameManager.answer = CustomAI.MiniGame.MiniAI.Answer.none;
        MiniGameManager.isAnswerSubmit = false;
    }

    void SetEntry()
    {
        Player temp = Player.system.Monster;

        // 몬스터 셋팅
        {
            // 로딩 완료 표시
            MiniGameManager.progress = ActionProgress.Start;

            // UI 소유자 등록
            monster.SetOwner(temp);

            // 참가
            if (temp.miniInfo.join)
            {
                // 턴 등록
                turn.Enqueue(temp);

                // UI 활성
                monster.gameObject.SetActive(true);

                Debug.Log("미니게임 :: 플레이어 참가 -> " + temp.name);
            }
            else
            {

                Debug.Log("미니게임 :: 플레이어 참가 안함 -> " + temp.name);
                //// UI 비활성
                //scoreList[i].gameObject.SetActive(false);
            }

            // AI 생성
            if (Player.system.Monster.miniAi == null)
                Player.system.Monster.miniAi = new CustomAI.MiniGame.MiniAI(Player.system.Monster);

            // 턴 표시 제거
            Player.system.Monster.miniPlayerUI.BlinkOff();

            // AI 자동 준비
            temp.miniInfo.isReady = true;
        }
        // 플레이어 셋팅
        for (int i = 0; i < Player.order.Count; i++)
        {
            // 플레이어 추출
            temp = turn.Dequeue();

            // UI 소유자 등록
            scoreList[i].SetOwner(temp);

            // 참가
            if (temp.miniInfo.join)
            {
                // 턴 등록
                turn.Enqueue(temp);

                // UI 활성
                scoreList[i].gameObject.SetActive(true);

                Debug.Log("미니게임 :: 플레이어 참가 -> " + temp.name);
            }
            else
            {

                Debug.Log("미니게임 :: 플레이어 참가 안함 -> " + temp.name);
                //// UI 비활성
                //scoreList[i].gameObject.SetActive(false);
            }

            // 턴 표시 제거
            temp.miniPlayerUI.BlinkOff();
        }

        // 참여 플레이어 목록
        entryPlayer = new List<Player>(turn);

        // AI 등록
        for(int i = 0; i < entryPlayer.Count; i++)
        {
            if (entryPlayer[i].type == Player.Type.AI || turnNow == Player.system.Monster)
                entryAI.Add(entryPlayer[i]);
        }
    }

    public void SetRanking()
    {
        //List<Player> sort = new List<Player>(Player.allPlayer);
        List<Player> sort = new List<Player>(entryPlayer);

        // 정렬
        if (sort.Count > 0)
        {
            // 공동 순위 리스트
            List<Player> temp = new List<Player>();
            Player tempPlayer;

            // 등수
            int c = 1;

            // 정렬
            while (sort.Count > 0)
            {
                //// 등수 추가
                //c++;

                Debug.Log("랭크 :: 랭크 계산중 -> " + c  + " 등");

                // 공동 순위 리스트 초기화
                temp.Clear();
                temp.Add(sort[0]);

                // 최고 점수자 확보
                for (int i = 1; i < sort.Count; i++)
                {
                    // 클 경우 초기화 후 확보
                    if (sort[i].miniInfo.score > temp[0].miniInfo.score)
                    {
                        temp.Clear();
                        temp.Add(sort[i]);
                    }
                    // 같을 경우 추가 확보
                    else if (sort[i].miniInfo.score == temp[0].miniInfo.score)
                    {
                        temp.Add(sort[i]);
                    }
                }

                // 등록
                for (int j = 0; j < temp.Count; j++)
                {
                    // 빈 대상 없을경우 중단
                    if (sort.Count == 0)
                        break;

                    // 대상
                    tempPlayer = temp[j];

                    //tempPlayer.miniInfo.rank = c;
                    //Debug.Log(string.Format("rank :: [{0}] {1} -> {2}점으로 {3}등", c, tempPlayer.name, tempPlayer.miniInfo.score, c));

                    // 참여시
                    if (tempPlayer.miniInfo.join)
                    {
                        // 랭크 입력
                        tempPlayer.miniInfo.rank = c;
                        Debug.Log(string.Format("rank :: [{0}] {1} -> {2}점으로 {3}등", c, tempPlayer.name, tempPlayer.miniInfo.score, c));

                        // 지분 배정
                        //tempPlayer.miniInfo.rewardRatio = Minigame.table[MiniScore.index].reward.GetRank(tempPlayer.miniInfo.rank);
                        tempPlayer.miniInfo.rewardRatio = MiniGameManager.minigameNow.reward.GetRank(tempPlayer.miniInfo.rank);

                        // 전체 지분량 반영
                        MiniScore.totalRewardRatio += tempPlayer.miniInfo.rewardRatio;

                        Debug.Log("미니게임 :: 보상 지분 -> " + tempPlayer.miniInfo.rewardRatio + " by 플레이어 " + tempPlayer.name);
                    }
                    // 불참시
                    else
                    {
                        // 랭크 박탈
                        tempPlayer.miniInfo.rank = 0;

                        // 지분 박탈
                        tempPlayer.miniInfo.rewardRatio = 0;
                    }

                    // 제외
                    sort.Remove(tempPlayer);
                }

                // 등수 반영
                c += temp.Count;
            }
        }
    }



    // 코드 초안 - 작성자 : 최유리
    //public static List<MiniGamePlayer> scoreList = new List<MiniGamePlayer>();
    //public MiniGamePlayer player1, player2, player3, player4;
    //public Scenes_mini num_player;                          //미니게임에 참가할 플레이어 수를 받아오는 스크립트
    //public int player, turnNum, rank, turnNumB, turnCheck;                                     //미니게임 참가 인원, 턴제어 숫자
    //public bool plusScore, minusScore, turn, ranking, playerSet, scoreSetCheck;
    //private Queue<int> queue = new Queue<int>();

    //void Awake()
    //{
    //    num_player = GameObject.Find("Test").GetComponent<Scenes_mini>();   //매인게임에서 미니게임을 플레이할 플레이어 수를 받아옴
    //}
    //void Start()
    //{



    //    player = num_player.member_num;
    //    turnNum = 0;
    //    turnCheck = 0;
    //    plusScore = false;
    //    minusScore = false;
    //    turn = true;
    //    ranking = false;
    //    playerSet = true;
    //    scoreSetCheck = false;
    //    setPlayer();
    //}

    //void Update()
    //{
    //    if (playerSet)
    //    {
    //        playerSet = false;
    //        setQue();
    //    }

    //    if (turn)
    //    {
    //        if (plusScore)
    //        {
    //            setScorePlus();
    //            plusScore = false;
    //        }

    //        Setturn();
    //        turn = false;
    //    }

    //    if (ranking)
    //    {
    //        Debug.Log("적용됬는지 확인중2........");
    //        scoreSetChecking();

    //        if (scoreSetCheck)
    //        {
    //            if (num_player.player01 == true)
    //            {
    //                player1.rank = setRanking(player1);
    //                Debug.Log("player1 : " + player1.rank + ", " + player1.score);
    //            }
    //            if (num_player.player02 == true)
    //            {
    //                player2.rank = setRanking(player2);
    //                Debug.Log("player2 : " + player2.rank + ", " + player2.score);
    //            }
    //            if (num_player.player03 == true)
    //            {
    //                player3.rank = setRanking(player3);
    //                Debug.Log("player3 : " + player3.rank + ", " + player3.score);
    //            }
    //            if (num_player.player04 == true)
    //            {
    //                player4.rank = setRanking(player4);
    //                Debug.Log("player4 : " + player4.rank + ", " + player4.score);
    //            }
    //            ranking = false;
    //            SceneManager.LoadScene("Mini_game");
    //        }
    //    }
    //}

    //public void scoreSetChecking()
    //{
    //    Debug.Log("적용됬는지 확인중1........" + num_player.turn[turnCheck - 2]);

    //    if (num_player.turn[turnCheck - 2] == 1)
    //    {
    //        scoreSetCheck = player1.scoreSetCheck;
    //    }
    //    if (num_player.turn[turnCheck - 2] == 2)
    //    {
    //        scoreSetCheck = player2.scoreSetCheck;
    //    }
    //    if (num_player.turn[turnCheck - 2] == 3)
    //    {
    //        scoreSetCheck = player3.scoreSetCheck;
    //    }
    //    if (num_player.turn[turnCheck - 2] == 4)
    //    {
    //        scoreSetCheck = player4.scoreSetCheck;
    //    }
    //}


    //void setPlayer()
    //{
    //    if (num_player.player01 == true)
    //    {
    //        scoreList.Add(player1);
    //        player1.join = false;
    //    }
    //    else
    //    {
    //        player1.join = true;
    //    }

    //    if (num_player.player02 == true)
    //    {
    //        scoreList.Add(player2);
    //    }
    //    else
    //    {
    //        player2.join = true;
    //    }

    //    if (num_player.player03 == true)
    //    {
    //        scoreList.Add(player3);
    //    }
    //    else
    //    {
    //        player3.join = true;
    //    }

    //    if (num_player.player04 == true)
    //    {
    //        scoreList.Add(player4);
    //    }
    //    else
    //    {
    //        player4.join = true;
    //    }
    //}

    //void setScorePlus()
    //{
    //    if (turnNumB == 1)
    //    {
    //        player1.plusScore = plusScore;
    //    }
    //    else if (turnNumB == 2)
    //    {
    //        player2.plusScore = plusScore;
    //    }
    //    else if (turnNumB == 3)
    //    {
    //        player3.plusScore = plusScore;
    //    }
    //    else
    //    {
    //        player4.plusScore = plusScore;
    //    }
    //}

    //void setQue()
    //{
    //    for (int i = 0; i < player; i++)
    //    {
    //        queue.Enqueue(num_player.turn[i]);
    //    }
    //    turnNum = (int)queue.Dequeue();
    //}

    //void setDead()
    //{
    //    if (turnNumB == 1)
    //    {
    //        player1.myTurn = false;
    //    }
    //    if (turnNumB == 2)
    //    {
    //        player2.myTurn = false;
    //    }
    //    if (turnNumB == 3)
    //    {
    //        player3.myTurn = false;
    //    }
    //    if (turnNumB == 4)
    //    {
    //        player4.myTurn = false;
    //    }
    //}

    //void Setturn()
    //{
    //    if (queue.Count == 0 && turnCheck == player)
    //    {
    //        setQue();
    //        turnCheck = 0;
    //    }

    //    if (turnNum == 1)
    //    {
    //        setDead();
    //        player1.myTurn = true;
    //        turnCheck += 1;
    //    }
    //    if (turnNum == 2)
    //    {
    //        setDead();
    //        player2.myTurn = true;
    //        turnCheck += 1;
    //    }
    //    if (turnNum == 3)
    //    {
    //        setDead();
    //        player3.myTurn = true;
    //        turnCheck += 1;
    //    }
    //    if (turnNum == 4)
    //    {
    //        setDead();
    //        player4.myTurn = true;
    //        turnCheck += 1;
    //    }

    //    turnNumB = turnNum;
    //    if (queue.Count != 0)
    //    {
    //        turnNum = (int)queue.Dequeue();
    //    }
    //}

    //int setRanking(MiniGamePlayer player)
    //{
    //    int ranking = 1;

    //    if (num_player.player01 == true)
    //    {
    //        if (player.score < player1.score)
    //        {
    //            ranking += 1;
    //        }
    //    }
    //    if (num_player.player02 == true)
    //    {
    //        if (player.score < player2.score)
    //        {
    //            ranking += 1;
    //        }
    //    }
    //    if (num_player.player03 == true)
    //    {
    //        if (player.score < player3.score)
    //        {
    //            ranking += 1;
    //        }
    //    }
    //    if (num_player.player04 == true)
    //    {
    //        if (player.score < player4.score)
    //        {
    //            ranking += 1;
    //        }
    //    }

    //    return ranking;

    //}

}
