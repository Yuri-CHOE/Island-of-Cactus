using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    public Mouse_Click mouse_Click1;
    public Mouse_Click mouse_Click2;
    public Scenes_mini num_player;
    public string num1, num2, name1, name2;
    public int player,x, z = 0, number;

    void Awake()
    {
<<<<<<< Updated upstream
        num_player = GameObject.Find("Test").GetComponent<Scenes_mini>();   //���ΰ��ӿ��� �̴ϰ����� �÷����� �÷��̾� ���� �޾ƿ�
=======
        num_player = GameObject.Find("Test").GetComponent<Scenes_mini>();   //���ΰ��ӿ��� �̴ϰ����� �÷����� �÷��̾� ���� �޾ƿ�    
        managePlayer = GameObject.Find("Game").GetComponent<manage_Player>();
    }

    void Start()
    {
>>>>>>> Stashed changes
        player = num_player.member_num;
        num1 = "";
        num2 = "";
        x = 1;
        number = 1;
        turn(player, 1);
    }
    
    // Update is called once per frame
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
<<<<<<< Updated upstream
                if (z == 9)
                {
                    GameObject.Find("Canvas").transform.Find("Ending").gameObject.SetActive(true);
                }
=======
                managePlayer.plusScore = true;
>>>>>>> Stashed changes
            }
            else
            {
                mouse_Click2.i = 2;
                mouse_Click1.i = 2;
            }
            turn(player, number);
            x = 1;
        }

        if (z == 9)         //ī��¦�� �� ���߾���
        {
            managePlayer.scoreSetChecking();
            managePlayer.ranking = true;
            z = 0;
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
                number = 2;
            }
            if(num == 2)
            {
                GameObject.Find("player (1)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(false);
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
                number = 2;
            }
            if (num == 2)
            {
                GameObject.Find("player (1)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(false);
                number = 3;
            }
            if (num == 3)
            {
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (3)").transform.Find("death").gameObject.SetActive(false);
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
                number = 2;
            }
            if (num == 2)
            {
                GameObject.Find("player (1)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(false);
                number = 3;
            }
            if (num == 3)
            {
                GameObject.Find("player (2)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (3)").transform.Find("death").gameObject.SetActive(false);
                number = 4;
            }
            if (num == 3)
            {
                GameObject.Find("player (3)").transform.Find("death").gameObject.SetActive(true);
                GameObject.Find("player (4)").transform.Find("death").gameObject.SetActive(false);
                number = 1;
            }
        }

    }
}
