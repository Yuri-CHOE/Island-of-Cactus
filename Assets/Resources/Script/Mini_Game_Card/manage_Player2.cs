using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manage_Player2 : MonoBehaviour
{
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

        //�÷��̾� ����� ������ Ȱ��ȭ
        for (int i = 4; i > player; i-- ){
            if(i == 2)
            {
                score2.active = true;
            }
            if (i == 3)
            {
                score3.active = true;
            }
            if (i == 4)
            {
                score4.active = true;
            }
        }
    }
    
}
