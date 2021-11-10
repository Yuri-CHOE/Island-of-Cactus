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

    // �� �����
    Queue<Player> turn = null;
    public Player turnNow { get { return turn.Peek(); } }    
    public bool isFirstFrame = true;           // �� ȹ�� ���� ~ ù������ ���� ��

    // ������ ���
    public static List<Player> entryPlayer = null;
    public static List<Player> entryAI = new List<Player>();


    int entryCount { get { return turn.Count; } }


    void Awake()
    {
        // ���� �ʱ�ȭ
        turn = new Queue<Player>(Player.order);
    }
    public void Init()
    {
        // �� ���
        //script = this;

        // ������ ����
        SetEntry();
    }

    void Start()
    {
    }

    void Update()
    {
        // �÷��̾� �غ� ������ üũ
        //if (MiniGameManager.progress < ActionProgress.Working)
        if (!MiniGameManager.script.isGameStart)
        {
            if (entryPlayer != null)
            {
                bool allReady = true;

                // �Ѹ��̶� �غ� �ȵǸ� false
                for (int i = 0; i < entryPlayer.Count; i++)
                {
                    allReady = allReady && entryPlayer[i].miniInfo.isReady;
                }

                // ���� ���� �ݿ�
                MiniGameManager.script.isGameStart = allReady;
                if (allReady)
                {
                    // �غ� ��ư ��Ȱ��ȭ
                    MiniGameManager.script.readyText.transform.parent.gameObject.SetActive(false);

                    MiniGameManager.progress = ActionProgress.Working;
                }
            }
        }

        // ���� ù �������� ���
        if (isFirstFrame)
        {
            Debug.Log("�̴ϰ��� :: �� ���۵� -> " + turnNow.name);

            // AI �÷��̾� �۵�
            if (turnNow.type == Player.Type.AI || turnNow == Player.system.Monster)
            {
                // AI �۵� ��û
                if (turnNow.miniAi.workControl == null)
                {
                    Debug.Log("�̴� AI :: �۵��� -> " + turnNow.name);
                    turnNow.miniAi.workControl = StartCoroutine(turnNow.miniAi.Work(MiniGameManager.minigameNow));
                }
            }
        }

        // ���� ù ������ �Ҹ�
        isFirstFrame = false;
    }

    public void NextTurn()
    {
        // �� ������
        Player end = turn.Dequeue();

        // ���� �ڿ� �ٽ� �ֱ�
        turn.Enqueue(end);

        // �� ǥ�� ����
        end.miniPlayerUI.BlinkOff();

        // ���� ù ������ ����
        isFirstFrame = true;

        // AI ��� ����
        MiniGameManager.answer = CustomAI.MiniGame.MiniAI.Answer.none;
        MiniGameManager.isAnswerSubmit = false;
    }

    void SetEntry()
    {
        Player temp = Player.system.Monster;

        // ���� ����
        {
            // �ε� �Ϸ� ǥ��
            MiniGameManager.progress = ActionProgress.Start;

            // UI ������ ���
            monster.SetOwner(temp);

            // ����
            if (temp.miniInfo.join)
            {
                // �� ���
                turn.Enqueue(temp);

                // UI Ȱ��
                monster.gameObject.SetActive(true);

                Debug.Log("�̴ϰ��� :: �÷��̾� ���� -> " + temp.name);
            }
            else
            {

                Debug.Log("�̴ϰ��� :: �÷��̾� ���� ���� -> " + temp.name);
                //// UI ��Ȱ��
                //scoreList[i].gameObject.SetActive(false);
            }

            // AI ����
            if (Player.system.Monster.miniAi == null)
                Player.system.Monster.miniAi = new CustomAI.MiniGame.MiniAI(Player.system.Monster);

            // �� ǥ�� ����
            Player.system.Monster.miniPlayerUI.BlinkOff();

            // AI �ڵ� �غ�
            temp.miniInfo.isReady = true;
        }
        // �÷��̾� ����
        for (int i = 0; i < Player.order.Count; i++)
        {
            // �÷��̾� ����
            temp = turn.Dequeue();

            // UI ������ ���
            scoreList[i].SetOwner(temp);

            // ����
            if (temp.miniInfo.join)
            {
                // �� ���
                turn.Enqueue(temp);

                // UI Ȱ��
                scoreList[i].gameObject.SetActive(true);

                Debug.Log("�̴ϰ��� :: �÷��̾� ���� -> " + temp.name);
            }
            else
            {

                Debug.Log("�̴ϰ��� :: �÷��̾� ���� ���� -> " + temp.name);
                //// UI ��Ȱ��
                //scoreList[i].gameObject.SetActive(false);
            }

            // �� ǥ�� ����
            temp.miniPlayerUI.BlinkOff();
        }

        // ���� �÷��̾� ���
        entryPlayer = new List<Player>(turn);

        // AI ���
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

        // ����
        if (sort.Count > 0)
        {
            // ���� ���� ����Ʈ
            List<Player> temp = new List<Player>();
            Player tempPlayer;

            // ���
            int c = 1;

            // ����
            while (sort.Count > 0)
            {
                //// ��� �߰�
                //c++;

                Debug.Log("��ũ :: ��ũ ����� -> " + c  + " ��");

                // ���� ���� ����Ʈ �ʱ�ȭ
                temp.Clear();
                temp.Add(sort[0]);

                // �ְ� ������ Ȯ��
                for (int i = 1; i < sort.Count; i++)
                {
                    // Ŭ ��� �ʱ�ȭ �� Ȯ��
                    if (sort[i].miniInfo.score > temp[0].miniInfo.score)
                    {
                        temp.Clear();
                        temp.Add(sort[i]);
                    }
                    // ���� ��� �߰� Ȯ��
                    else if (sort[i].miniInfo.score == temp[0].miniInfo.score)
                    {
                        temp.Add(sort[i]);
                    }
                }

                // ���
                for (int j = 0; j < temp.Count; j++)
                {
                    // �� ��� ������� �ߴ�
                    if (sort.Count == 0)
                        break;

                    // ���
                    tempPlayer = temp[j];

                    //tempPlayer.miniInfo.rank = c;
                    //Debug.Log(string.Format("rank :: [{0}] {1} -> {2}������ {3}��", c, tempPlayer.name, tempPlayer.miniInfo.score, c));

                    // ������
                    if (tempPlayer.miniInfo.join)
                    {
                        // ��ũ �Է�
                        tempPlayer.miniInfo.rank = c;
                        Debug.Log(string.Format("rank :: [{0}] {1} -> {2}������ {3}��", c, tempPlayer.name, tempPlayer.miniInfo.score, c));

                        // ���� ����
                        //tempPlayer.miniInfo.rewardRatio = Minigame.table[MiniScore.index].reward.GetRank(tempPlayer.miniInfo.rank);
                        tempPlayer.miniInfo.rewardRatio = MiniGameManager.minigameNow.reward.GetRank(tempPlayer.miniInfo.rank);

                        // ��ü ���з� �ݿ�
                        MiniScore.totalRewardRatio += tempPlayer.miniInfo.rewardRatio;

                        Debug.Log("�̴ϰ��� :: ���� ���� -> " + tempPlayer.miniInfo.rewardRatio + " by �÷��̾� " + tempPlayer.name);
                    }
                    // ������
                    else
                    {
                        // ��ũ ��Ż
                        tempPlayer.miniInfo.rank = 0;

                        // ���� ��Ż
                        tempPlayer.miniInfo.rewardRatio = 0;
                    }

                    // ����
                    sort.Remove(tempPlayer);
                }

                // ��� �ݿ�
                c += temp.Count;
            }
        }
    }



    // �ڵ� �ʾ� - �ۼ��� : ������
    //public static List<MiniGamePlayer> scoreList = new List<MiniGamePlayer>();
    //public MiniGamePlayer player1, player2, player3, player4;
    //public Scenes_mini num_player;                          //�̴ϰ��ӿ� ������ �÷��̾� ���� �޾ƿ��� ��ũ��Ʈ
    //public int player, turnNum, rank, turnNumB, turnCheck;                                     //�̴ϰ��� ���� �ο�, ������ ����
    //public bool plusScore, minusScore, turn, ranking, playerSet, scoreSetCheck;
    //private Queue<int> queue = new Queue<int>();

    //void Awake()
    //{
    //    num_player = GameObject.Find("Test").GetComponent<Scenes_mini>();   //���ΰ��ӿ��� �̴ϰ����� �÷����� �÷��̾� ���� �޾ƿ�
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
    //        Debug.Log("�������� Ȯ����2........");
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
    //    Debug.Log("�������� Ȯ����1........" + num_player.turn[turnCheck - 2]);

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
