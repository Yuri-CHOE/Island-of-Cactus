using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Check : MonoBehaviour
{
    public Scenes_mini num_player;  //�̴ϰ��ӿ� ������ �÷��̾� ��
    public manage_Player managePlayer;

    public Mouse_Click mouse_Click1;    //ù��° ���콺 Ŭ��
    public Mouse_Click mouse_Click2;    //�ι�° ���콺 Ŭ��
    //ù��° ī�� ����, �ι�° ī�� ����, ù��° Ŭ���� ������Ʈ �̸�, �ι�°�� Ŭ���� ������Ʈ �̸�
    public string num1, num2, name1, name2;
    //�����ϴ� �÷��̾� ��, ��� Ŭ���ߴ���, ������ ī��¦ ��
    public int player, x, z = 0;
    public bool turn;

    void Awake()
    {
        num_player = GameObject.Find("Test").GetComponent<Scenes_mini>();   //���ΰ��ӿ��� �̴ϰ����� �÷����� �÷��̾� ���� �޾ƿ�    
        managePlayer = GameObject.Find("Game").GetComponent<manage_Player>();

        player = num_player.member_num;     
        num1 = "";
        num2 = "";
        x = 1;
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
                managePlayer.plusScore = true;

                if (z == 9)         //ī��¦�� �� ���߾���
                {
                    GameObject.Find("Canvas").transform.Find("Ending").gameObject.SetActive(true);
                    managePlayer.ranking = true;
                }
            }
            else
            {
                mouse_Click2.i = 2;
                mouse_Click1.i = 2;
                managePlayer.plusScore = false;
            }
            managePlayer.turn = true;
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
}
