using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes_mini : MonoBehaviour
{
    public bool player2, player3, player4;
    public int member_num;
    public int random;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //space �Է½� �� ����
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            member_num = 1; //�÷��̾� �� �ʱ�ȭ
            player_set(player2);
            player_set(player3);
            player_set(player4);

            if(member_num == 1) //1�ο� �̴ϰ���
            {
                // �̴ϰ��� ���� �� ���� Ȯ�� �ʿ�
                random = Random.Range(4, 5);
            }
            else
            {
                if (member_num == 2)    //2�ο� �̴ϰ���
                {
                    // �̴ϰ��� ���� �� ���� Ȯ�� �ʿ�
                    random = Random.Range(4, 5);
                }
                else
                {
                    if (member_num == 3)    //3�ο� �̴ϰ���
                    {
                        // �̴ϰ��� ���� �� ���� Ȯ�� �ʿ�
                        random = Random.Range(4, 5);
                    }
                    else
                    {
                        // �̴ϰ��� ���� �� ���� Ȯ�� �ʿ�
                        random = Random.Range(4, 5);
                    }
                }
            }

            SceneManager.LoadScene(random);
        }

        //Destroy(gameObject);
    }

    // ���ӿ� �����ϴ� �÷��̾� �� Ȯ��
    void player_set(bool player)
    {
        if (player)
        {
            // �÷��̾��� ĳ���͸� �����ϴ� �ڵ� �ʿ�, ���� ������ ����
            member_num += 1;
        }
    }
}