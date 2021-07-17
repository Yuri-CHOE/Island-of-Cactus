using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manage_Player : MonoBehaviour
{
    //public static List<player_mini> scoreList = new List<player_mini>();

    public player_mini player1, player2, player3, player4;         
    public Scenes_mini num_player;                          //�̴ϰ��ӿ� ������ �÷��̾� ���� �޾ƿ��� ��ũ��Ʈ
    public int player, turnNum, rank;                                      //�̴ϰ��� ���� �ο�, ������ ����
    public bool plusScore, minusScore, turn, ranking, playerSet;

    void Awake()
    {
        //scoreList.Add(score1);
        //scoreList.Add(score2);
        //scoreList.Add(score3);
        //scoreList.Add(score4);
    }
    void Start()
    {
        num_player = GameObject.Find("Test").GetComponent<Scenes_mini>();   //���ΰ��ӿ��� �̴ϰ����� �÷����� �÷��̾� ���� �޾ƿ�
        player = num_player.member_num;
        turnNum = 1;
        plusScore = false;
        minusScore = false;
        turn = false;
        ranking = false;
        playerSet = true;
    }

    void Update()
    {
        if (playerSet)
        {
            setPlayer();
            playerSet = false;
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
            setRanking();
        }
    }

    void setPlayer()
    {
        //�� �÷��̾� ���ھ��ǿ� Ȱ��ȭ ����
        if (player == 1)
        {
            player2.join = false;
            player3.join = false;
            player4.join = false;
        }
        else
        {
            if (player == 2)
            {
                player3.join = false;
                player4.join = false;
            }
            else
            {
                if (player == 3)
                {
                    player4.join = false;
                }
            }
        }
    }

    void setScorePlus()
    {
        if (turnNum == 1)
        {
            player1.plusScore = plusScore;
        }else if(turnNum == 2)
        {
            player2.plusScore = plusScore;
        }else if(turnNum == 3)
        {
            player3.plusScore = plusScore;
        }
        else
        {
            player4.plusScore = plusScore;
        }
    }


    void Setturn()
    {
        // �ڽ��� ������ �÷��̾��� ��� death ��Ȱ��ȭ �׿� �÷��̾�� death�� Ȱ��ȭ ���� ���� �������� ������ �� �ֵ��� ��
        if (player == 2)
        {
            if (turnNum == 1)
            {
                player1.myTurn = true;
                player2.myTurn = false;
                turnNum = 2;
            }
            if (turnNum == 2)
            {
                player1.myTurn = false;
                player2.myTurn = true;
                turnNum = 1;
            }

        }
        if (player == 3)
        {
            if (turnNum == 1)
            {
                player1.myTurn = true;
                player2.myTurn = false;
                player3.myTurn = false;
                turnNum = 2;
            }
            if (turnNum == 2)
            {
                player1.myTurn = false;
                player2.myTurn = true;
                turnNum = 3;
            }
            if (turnNum == 3)
            {
                player2.myTurn = false;
                player3.myTurn = true;
                turnNum = 1;
            }

        }
        if (player == 4)
        {
            if (turnNum == 1)
            {
                player1.myTurn = true;
                player2.myTurn = false;
                player3.myTurn = false;
                player4.myTurn = false;
                turnNum = 2;
            }
            if (turnNum == 2)
            {
                player1.myTurn = false;
                player2.myTurn = true;
                turnNum = 3;
            }
            if (turnNum == 3)
            {
                player2.myTurn = false;
                player3.myTurn = true;
                turnNum = 4;
            }
            if (turnNum == 3)
            {
                player3.myTurn = false;
                player4.myTurn = true;
                turnNum = 1;
            }
        }

    }

    void setRanking()
    {
        int[] rankingList = new int[player];
        int i, j;
        rankingList[0] = player1.score;
        if (player == 2)
        {
            rankingList[1] = player2.score;
        }
        else if (player == 3)
        {
            rankingList[1] = player2.score;
            rankingList[2] = player3.score;
        }
        else
        {
            rankingList[1] = player2.score;
            rankingList[2] = player3.score;
            rankingList[3] = player4.score;
        }

        for (i = 0; i < player; i++) {
            rank = 1;
            for (j = 0; j < player; j++){
                if (i != j)
                {
                    if (rankingList[i] < rankingList[j])
                    {
                        rank += 1;
                    }
                }
            }

            if (i == 0)
            {
                player1.rank = rank;
            }
            else if (i == 1)
            {
                player2.rank = rank;
            }
            else if (i == 2)
            {
                player3.rank = rank;
            }
            else
            {
                player4.rank = rank;
            }
        }
    }
}
