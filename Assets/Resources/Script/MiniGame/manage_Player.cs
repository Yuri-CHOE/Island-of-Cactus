using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class manage_Player : MonoBehaviour
{
    public static List<miniGamePlayer> scoreList = new List<miniGamePlayer>();
    public miniGamePlayer player1, player2, player3, player4;
    public Scenes_mini num_player;                          //�̴ϰ��ӿ� ������ �÷��̾� ���� �޾ƿ��� ��ũ��Ʈ
    public int player, turnNum, rank, turnNumB, turnCheck;                                     //�̴ϰ��� ���� �ο�, ������ ����
    public bool plusScore, minusScore, turn, ranking, playerSet, scoreSetCheck;
    private Queue<int> queue = new Queue<int>();

    void Awake()
    {
        num_player = GameObject.Find("Test").GetComponent<Scenes_mini>();   //���ΰ��ӿ��� �̴ϰ����� �÷����� �÷��̾� ���� �޾ƿ�
    }
    void Start()
    {
        player = num_player.member_num;
        turnNum = 0;
        turnCheck = 0;
        plusScore = false;
        minusScore = false;
        turn = true;
        ranking = false;
        playerSet = true;
        scoreSetCheck = false;
        setPlayer();
    }

    void Update()
    {
        if (playerSet)
        {
            playerSet = false;
            setQue();
        }

        if (turn)
        {
            if (plusScore)
            {
                setScorePlus();
                plusScore = false;
            }

            Setturn();
            turn = false;
        }

        if (ranking)
        {
            Debug.Log("�������� Ȯ����2........");
            scoreSetChecking();

            if (scoreSetCheck)
            {
                if (num_player.player01 == true)
                {
                    player1.rank = setRanking(player1);
                    Debug.Log("player1 : " + player1.rank + ", " + player1.score);
                }
                if (num_player.player02 == true)
                {
                    player2.rank = setRanking(player2);
                    Debug.Log("player2 : " + player2.rank + ", " + player2.score);
                }
                if (num_player.player03 == true)
                {
                    player3.rank = setRanking(player3);
                    Debug.Log("player3 : " + player3.rank + ", " + player3.score);
                }
                if (num_player.player04 == true)
                {
                    player4.rank = setRanking(player4);
                    Debug.Log("player4 : " + player4.rank + ", " + player4.score);
                }
                ranking = false;
                SceneManager.LoadScene("Mini_game");
            }
        }
    }

    public void scoreSetChecking()
    {
        Debug.Log("�������� Ȯ����1........" + num_player.turn[turnCheck - 2]);

        if (num_player.turn[turnCheck - 2] == 1)
        {
            scoreSetCheck = player1.scoreSetCheck;
        }
        if (num_player.turn[turnCheck - 2] == 2)
        {
            scoreSetCheck = player2.scoreSetCheck;
        }
        if (num_player.turn[turnCheck - 2] == 3)
        {
            scoreSetCheck = player3.scoreSetCheck;
        }
        if (num_player.turn[turnCheck - 2] == 4)
        {
            scoreSetCheck = player4.scoreSetCheck;
        }
    }


    void setPlayer()
    {
        if (num_player.player01 == true)
        {
            scoreList.Add(player1);
            player1.join = false;
        }
        else
        {
            player1.join = true;
        }

        if (num_player.player02 == true)
        {
            scoreList.Add(player2);
        }
        else
        {
            player2.join = true;
        }

        if (num_player.player03 == true)
        {
            scoreList.Add(player3);
        }
        else
        {
            player3.join = true;
        }

        if (num_player.player04 == true)
        {
            scoreList.Add(player4);
        }
        else
        {
            player4.join = true;
        }
    }

    void setScorePlus()
    {
        if (turnNumB == 1)
        {
            player1.plusScore = plusScore;
        }
        else if (turnNumB == 2)
        {
            player2.plusScore = plusScore;
        }
        else if (turnNumB == 3)
        {
            player3.plusScore = plusScore;
        }
        else
        {
            player4.plusScore = plusScore;
        }
    }

    void setQue()
    {
        for (int i = 0; i < player; i++)
        {
            queue.Enqueue(num_player.turn[i]);
        }
        turnNum = (int)queue.Dequeue();
    }

    void setDead()
    {
        if (turnNumB == 1)
        {
            player1.myTurn = false;
        }
        if (turnNumB == 2)
        {
            player2.myTurn = false;
        }
        if (turnNumB == 3)
        {
            player3.myTurn = false;
        }
        if (turnNumB == 4)
        {
            player4.myTurn = false;
        }
    }

    void Setturn()
    {
        if (queue.Count == 0 && turnCheck == player)
        {
            setQue();
            turnCheck = 0;
        }

        if (turnNum == 1)
        {
            setDead();
            player1.myTurn = true;
            turnCheck += 1;
        }
        if (turnNum == 2)
        {
            setDead();
            player2.myTurn = true;
            turnCheck += 1;
        }
        if (turnNum == 3)
        {
            setDead();
            player3.myTurn = true;
            turnCheck += 1;
        }
        if (turnNum == 4)
        {
            setDead();
            player4.myTurn = true;
            turnCheck += 1;
        }

        turnNumB = turnNum;
        if (queue.Count != 0)
        {
            turnNum = (int)queue.Dequeue();
        }
    }

    int setRanking(miniGamePlayer player)
    {
        int ranking = 1;

        if (num_player.player01 == true)
        {
            if (player.score < player1.score)
            {
                ranking += 1;
            }
        }
        if (num_player.player02 == true)
        {
            if (player.score < player2.score)
            {
                ranking += 1;
            }
        }
        if (num_player.player03 == true)
        {
            if (player.score < player3.score)
            {
                ranking += 1;
            }
        }
        if (num_player.player04 == true)
        {
            if (player.score < player4.score)
            {
                ranking += 1;
            }
        }

        return ranking;

    }
}
