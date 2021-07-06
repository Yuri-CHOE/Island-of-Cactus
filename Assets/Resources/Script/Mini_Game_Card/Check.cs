using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Check : MonoBehaviour
{
    public Scenes_mini num_player;  //미니게임에 참가한 플레이어 수
    public Text score_p1, score_p2, score_p3, score_p4; //player1의score Txt오브젝트, player2의score Txt오브젝트,  player3의score Txt오브젝트,  player4의score Txt오브젝트
    public set_Ranking ranking;
    public Mouse_Click mouse_Click1;    //첫번째 마우스 클릭
    public Mouse_Click mouse_Click2;    //두번째 마우스 클릭
    //첫번째 카드 숫자, 두번째 카드 숫자, 첫번째 클릭한 오브젝트 이름, 두번째로 클릭한 오브젝트 이름
    public string num1, num2, name1, name2;
    //참여하는 플레이어 수, 몇번 클릭했는지, 뒤집힌 카드짝 수, 누구 차례로 바뀌어야 하는지, 전 차례 플레이어, player1의score, player2의score,  player3의score,  player4의score
    public int player,x, z = 0, number, ex_number, score1,score2,score3,score4;

    void Awake()
    {
        num_player = GameObject.Find("Test").GetComponent<Scenes_mini>();   //매인게임에서 미니게임을 플레이할 플레이어 수를 받아옴    
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
    
    //플레이어 수에 맞게 오브젝트 변수 불러옴
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
            Debug.Log("확인" + num1 + " " + num2);
            if(num1 == num2)
            {
                mouse_Click2.i = 4;
                mouse_Click1.i = 4;
                z += 1;
                score(ex_number);
                if (z == 9)         //카드짝이 다 맞추어짐
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
        //첫번째 카드 숫자와 카드 번호
        num1 = number;
        name1 = card_name;
    }

    public void num2_set(string number, string card_name)
    {
        //두번째 카드 숫자와 카드 번호
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
        // 자신의 차례인 플레이어의 경우 death 비활성화 그외 플레이어는 death를 활성화 시켜 누구 차례인지 구분할 수 있도록 함
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
