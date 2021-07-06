using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class set_Ranking : MonoBehaviour
{
    public Scenes_mini DontD_ob_playernum;
    public act_Score player1, player2, player3, player4;
    public Check check;

    public int player_num;
    public int[] score, scoreSort, rank;
    public bool calRank = false;    //평균 계산 시작

    void Awake()
    {  
        DontD_ob_playernum = GameObject.Find("Test").GetComponent<Scenes_mini>();
        check = GameObject.Find("Game").GetComponent<Check>();

        player_num = DontD_ob_playernum.member_num;
        score = new int[player_num];
        scoreSort = new int[player_num];
        rank = new int[player_num];
    }

    void Start() { 
    }

    void Update()
    {
        if (calRank)
        {
            score[0] = check.score1;
            Debug.Log(check.score1);
            if (player_num == 2)
            {
                score[1] = check.score2;
                Debug.Log(check.score2);
            }
            else if(player_num == 3)
            {
                score[1] = check.score2;
                score[2] = check.score3;
            }
            else
            {
                score[1] = check.score2;
                score[2] = check.score3;
                score[3] = check.score4;
            }

            Array.Copy(score, scoreSort, score.Length);
            Array.Sort(scoreSort);
            Array.Reverse(scoreSort);

            for(int i = 0; i<score.Length; i++)
            {
                for(int j = 0; j<scoreSort.Length; j++)
                {
                    if(score[i] == scoreSort[j])
                    {
                        rank[i] = j+1;
                        Debug.Log(score[i] + " " + scoreSort[j] + " " + rank[i]);
                    }
                }
            }

            ob_set(player_num);

        }
    }

    //각 플레이어 스크립트에 미니게임 점수와 등수 등록
    void ob_set(int player)
    {
        player1 = GameObject.Find("player (1)").GetComponent<act_Score>();
        player1.score = score[0];
        player1.rank = rank[0];

        if (player == 2)
        {
            player2 = GameObject.Find("player (2)").GetComponent<act_Score>();
            player2.score = score[1];
            player2.rank = rank[1];
        }
        else
        {
            if (player == 3)
            {
                player2 = GameObject.Find("player (2)").GetComponent<act_Score>();
                player2.score = score[1];
                player2.rank = rank[1];
                player3 = GameObject.Find("player (3)").GetComponent<act_Score>();
                player3.score = score[2];
                player3.rank = rank[2];

            }
            else
            {
                player2 = GameObject.Find("player (2)").GetComponent<act_Score>();
                player2.score = score[1];
                player2.rank = rank[1];
                player3 = GameObject.Find("player (3)").GetComponent<act_Score>();
                player3.score = score[2];
                player3.rank = rank[2];
                player4 = GameObject.Find("player (4)").GetComponent<act_Score>();
                player4.score = score[3];
                player4.rank = rank[3];
            }
        }
    }

}
