using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Check : MonoBehaviour
{
    public Scenes_mini num_player;  //�̴ϰ��ӿ� ������ �÷��̾� ��
    public Text score_p1, score_p2, score_p3, score_p4; //player1��score Txt������Ʈ, player2��score Txt������Ʈ,  player3��score Txt������Ʈ,  player4��score Txt������Ʈ
    public set_Ranking ranking;
    public Mouse_Click mouse_Click1;    //ù��° ���콺 Ŭ��
    public Mouse_Click mouse_Click2;    //�ι�° ���콺 Ŭ��
    //ù��° ī�� ����, �ι�° ī�� ����, ù��° Ŭ���� ������Ʈ �̸�, �ι�°�� Ŭ���� ������Ʈ �̸�
    public string num1, num2, name1, name2;
    //�����ϴ� �÷��̾� ��, ��� Ŭ���ߴ���, ������ ī��¦ ��, ���� ���ʷ� �ٲ��� �ϴ���, �� ���� �÷��̾�, player1��score, player2��score,  player3��score,  player4��score
    public int player,x, z = 0, number, ex_number, score1,score2,score3,score4;

    void Awake()
    {
        num_player = GameObject.Find("Test").GetComponent<Scenes_mini>();   //���ΰ��ӿ��� �̴ϰ����� �÷����� �÷��̾� ���� �޾ƿ�    
        player = num_player.member_num;     
        num1 = "";
        num2 = "";
        x = 1;
        ex_number = 0;
        number = 1;
        score1 = 0;
        score2 = 0;
        score3 = 0;
        score4 = 0;
        ob_set(player);
        turn(player, 1);
    }
    
    //�÷��̾� ���� �°� ������Ʈ ���� �ҷ���
    void ob_set(int player)
    {
        score_p1 = GameObject.Find("player (1)").transform.Find("Text").GetComponent<Text>();
        if (player == 2)
        {
            score_p2 = GameObject.Find("player (2)").transform.Find("Text").GetComponent<Text>();
        }
        else
        {
            if (player == 3)
            {
                score_p2 = GameObject.Find("player (2)").transform.Find("Text").GetComponent<Text>();
                score_p3 = GameObject.Find("player (3)").transform.Find("Text").GetComponent<Text>();
            }
            else
            {
                score_p2 = GameObject.Find("player (2)").transform.Find("Text").GetComponent<Text>();
                score_p3 = GameObject.Find("player (3)").transform.Find("Text").GetComponent<Text>();
                score_p4 = GameObject.Find("player (4)").transform.Find("Text").GetComponent<Text>();
            }
        }
    }

    void Update()
    {
        if(x == 3)
        {
            mouse_Click1 = GameObject.Find(name1).GetComponent<Mouse_Click>();
            mouse_Click2 = GameObject.Find(name2).GetComponent<Mouse_Click>();
            Debug.Log("Ȯ��" + num1 + " " + num2);
            if(num1 == num2)
            {
                mouse_Click2.i = 4;
                mouse_Click1.i = 4;
                z += 1;
                score(ex_number);
                if (z == 9)         //ī��¦�� �� ���߾���
                {
                    GameObject.Find("Canvas").transform.Find("Ending").gameObject.SetActive(true);
                    ranking = GameObject.Find("Ending").GetComponent<set_Ranking>();
                    ranking.calRank = true;
                }
            }
            else
            {
                mouse_Click2.i = 2;
                mouse_Click1.i = 2;
            }
            turn(player, number);
            x = 1;
        }
    }

    public void num1_set(string number, string card_name)
    {
        //ù��° ī�� ���ڿ� ī�� ��ȣ
        num1 = number;
        name1 = card_name;
    }

    public void num2_set(string number, string card_name)
    {
        //�ι�° ī�� ���ڿ� ī�� ��ȣ
        num2 = number;
        name2 = card_name;
    }

    public void score(int num)
    {
        if(num == 1)
        {
            score1 += 100;
            score_p1.text = score1.ToString();
        }
        else
        {
            if (num == 2)
            {
                score2 += 100;
                score_p2.text = score2.ToString();
            }
            else
            {
                if (num == 3)
                {
                    score3 += 100;
                    score_p3.text = score3.ToString();
                }
                else
                {
                    score4 += 100;
                    score_p4.text = score4.ToString();
                }
            }
        }
    }

    public void turn(int player_num, int num)
    {
        // �ڽ��� ������ �÷��̾��� ��� death ��Ȱ��ȭ �׿� �÷��̾�� death�� Ȱ��ȭ ���� ���� �������� ������ �� �ֵ��� ��
        if (player_num == 2)
        {
            if(num == 1)
            {
                GameObject.Find("player (1)").transform.Find("death").gameObject.SetActive(false);
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(true);
                ex_number = num;
                number = 2;
            }
            if(num == 2)
            {
                GameObject.Find("player (1)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(false);
                ex_number = num;
                number = 1;
            }
           
        }
        if (player_num == 3)
        {
            if (num == 1)
            {
                GameObject.Find("player (1)").transform.Find("death").gameObject.SetActive(false);
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (3)").transform.Find("death").gameObject.SetActive(true);
                ex_number = num;
                number = 2;
            }
            if (num == 2)
            {
                GameObject.Find("player (1)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(false);
                ex_number = num;
                number = 3;
            }
            if (num == 3)
            {
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (3)").transform.Find("death").gameObject.SetActive(false);
                ex_number = num;
                number = 1;
            }

        }
        if (player_num == 4)
        {
            if (num == 1)
            {
                GameObject.Find("player (1)").transform.Find("death").gameObject.SetActive(false);
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (3)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (4)").transform.Find("death").gameObject.SetActive(true);
                ex_number = num;
                number = 2;
            }
            if (num == 2)
            {
                GameObject.Find("player (1)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(false);
                ex_number = num;
                number = 3;
            }
            if (num == 3)
            {
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (3)").transform.Find("death").gameObject.SetActive(false);
                ex_number = num;
                number = 4;
            }
            if (num == 3)
            {
                GameObject.Find("player (3)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (4)").transform.Find("death").gameObject.SetActive(false);
                ex_number = num;
                number = 1;
            }
        }

    }
}
