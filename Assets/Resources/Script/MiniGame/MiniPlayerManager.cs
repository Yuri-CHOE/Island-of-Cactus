using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniPlayerManager : MonoBehaviour
{
    //public static MiniPlayerManager script = null;

    [Header("Player UI")]
    public List<MiniGamePlayer> scoreList = new List<MiniGamePlayer>();

    // 턴 제어용
    private Queue<Player> turn = new Queue<Player>(Player.order);
    public Player turnNow { get { return turn.Peek(); } }


    int entryCount { get { return turn.Count; } }


    //void Awake()
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

    }

    public void NextTurn()
    {
        // 턴 종료자
        Player end = turn.Dequeue();

        // 가장 뒤에 다시 넣기
        turn.Enqueue(end);

        // 턴 표시 제거
        end.miniPlayerUI.BlinkOff();
    }

    void SetEntry()
    {
        Player temp = null;
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

                Debug.LogWarning("미니게임 :: 플레이어 참가 -> " + temp.name);
            }
            else
            {

                Debug.LogWarning("미니게임 :: 플레이어 참가 안함 -> " + temp.name);
                //// UI 비활성
                //scoreList[i].gameObject.SetActive(false);
            }

            // 턴 표시 제거
            temp.miniPlayerUI.BlinkOff();
        }
    }

    public void SetRanking()
    {
        List<Player> sort = new List<Player>(Player.allPlayer);

        // 정렬
        if (sort.Count > 0)
        {
            // 공동 순위 리스트
            List<int> temp = new List<int>();
            Player tempPlayer;

            // 등수
            int c = 0;

            // 정렬
            while (sort.Count > 0)
            {
                // 등수 추가
                c++;

                // 공동 순위 리스트 초기화
                temp.Clear();
                temp.Add(0);

                // 최고 점수자 확보
                for (int j = 0; j < sort.Count; j++)
                {
                    // 클 경우 초기화 후 확보
                    if (sort[j].miniInfo.score > sort[temp[0]].miniInfo.score)
                    {
                        temp.Clear();
                        temp.Add(j);
                    }
                    // 같을 경우 추가 확보
                    else if (sort[j].miniInfo.score == sort[temp[0]].miniInfo.score)
                    {
                        temp.Add(j);
                    }
                }

                // 등록
                Debug.LogError(temp.Count);
                for (int j = 0; j < temp.Count; j++)
                {
                    int indexer = temp[j];

                    Debug.LogError(j +" = "+ indexer);

                    // 대상
                    tempPlayer = sort[temp[j]];

                    tempPlayer.miniInfo.rank = c;
                    Debug.Log(string.Format("rank :: [{0}] {1} -> {2}점", c, tempPlayer.name, tempPlayer.miniInfo.score));

                    // 참여시 랭크 입력
                    if (tempPlayer.miniInfo.join)
                        tempPlayer.miniInfo.rank = c;
                    // 불참시 랭크 박탈
                    else
                        tempPlayer.miniInfo.rank = 0;

                    // 제외
                    sort.Remove(tempPlayer);
                }

                // 동시 등수 반영
                c += temp.Count - 1;
            }
        }
    }




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
