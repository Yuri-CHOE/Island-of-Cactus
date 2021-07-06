using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes_mini : MonoBehaviour
{
    public bool player1, player2, player3, player4, ob_delete;
    public int member_num;
    public int random;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ob_delete = false;
    }

    // Update is called once per frame
    void Update()
    {
        //space 입력시 씬 변경
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            member_num = 0; //플레이어 수 초기화
            player_set(player1);
            player_set(player2);
            player_set(player3);
            player_set(player4);

            if(member_num == 1) //1인용 미니게임
            {
                // 미니게임 개발 후 범위 확장 필요
                random = Random.Range(4, 5);
            }
            else
            {
                if (member_num == 2)    //2인용 미니게임
                {
                    // 미니게임 개발 후 범위 확장 필요
                    random = Random.Range(4, 5);
                }
                else
                {
                    if (member_num == 3)    //3인용 미니게임
                    {
                        // 미니게임 개발 후 범위 확장 필요
                        random = Random.Range(4, 5);
                    }
                    else
                    {
                        // 미니게임 개발 후 범위 확장 필요
                        random = Random.Range(4, 5);
                    }
                }
            }

            SceneManager.LoadScene(random);
        }

        if (ob_delete)
        {
            Destroy(gameObject);
        }
        
    }

    // 게임에 참여하는 플레이어 수 확인
    void player_set(bool player)
    {
        if (player)
        {
            // 플레이어의 캐릭터를 설정하는 코딩 필요, 추후 수정할 예정
            member_num += 1;
        }
    }
}
