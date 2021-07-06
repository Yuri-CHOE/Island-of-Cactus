using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manage_Player : MonoBehaviour
{
    public Text score_p1, score_p2, score_p3, score_p4;
    public act_Score score1, score2, score3, score4;
    public Scenes_mini num_player;
    public int player;

    void Awake()
    {
        score1 = GameObject.Find("player (1)").GetComponent<act_Score>();
        score2 = GameObject.Find("player (2)").GetComponent<act_Score>();
        score3 = GameObject.Find("player (3)").GetComponent<act_Score>();
        score4 = GameObject.Find("player (4)").GetComponent<act_Score>();
    }
    void Start()
    {
        num_player = GameObject.Find("Test").GetComponent<Scenes_mini>();   //���ΰ��ӿ��� �̴ϰ����� �÷����� �÷��̾� ���� �޾ƿ�
        player = num_player.member_num;

        //�� �÷��̾� ���ھ��ǿ� Ȱ��ȭ ����
        if (player == 1)
        {
            score2.active = true;
            score3.active = true;
            score4.active = true;
        }
        else
        {
            if (player == 2)
            {
                score3.active = true;
                score4.active = true;
            }
            else
            {
                if (player == 3)
                {
                    score4.active = true;
                }
            }
        }
        Text_ob_set(player);
    }

    //�ؽ�Ʈ ������Ʈ ���� 0���� ����
    void Text_ob_set(int player)
    {
        score_p1 = GameObject.Find("player (1)").transform.Find("Text").GetComponent<Text>();
        score_p1.text = "0";
        if (player == 2)
        {
            score_p2 = GameObject.Find("player (2)").transform.Find("Text").GetComponent<Text>();
            score_p2.text = "0";
        }
        else
        {
            if (player == 3)
            {
                score_p2 = GameObject.Find("player (2)").transform.Find("Text").GetComponent<Text>();
                score_p3 = GameObject.Find("player (3)").transform.Find("Text").GetComponent<Text>();
                score_p2.text = "0";
                score_p3.text = "0";
            }
            else
            {
                score_p2 = GameObject.Find("player (2)").transform.Find("Text").GetComponent<Text>();
                score_p3 = GameObject.Find("player (3)").transform.Find("Text").GetComponent<Text>();
                score_p4 = GameObject.Find("player (4)").transform.Find("Text").GetComponent<Text>();
                score_p2.text = "0";
                score_p3.text = "0";
                score_p4.text = "0";
            }
        }
    }

}
