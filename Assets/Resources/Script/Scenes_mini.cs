using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes_mini : MonoBehaviour
{
    public static Scenes_mini Instance;       //DontDestroy중복생성 확인
    public int[] turn;      //플레이어 차례(메인게임) 저장
    public bool player01, player02, player03, player04;
    public int member_num;
    public int random;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //space 입력시 씬 변경
        if (Input.GetKeyDown(KeyCode.Space))
        {
            member_num = 1; //플레이어 수 초기화
            player_set(player02);
            player_set(player03);
            player_set(player04);
            turn = new int[member_num];

            turnSet();

            if (member_num == 1) //1인용 미니게임
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

        //Destroy(gameObject);
    }

    // 게임에 참여하는 플레이어 수 확인
    void player_set(bool player)
    {
        if (player)
        {
            member_num += 1;
        }
    }

    //test용
    void turnSet()
    {
        for (int i = 0; i < member_num; i++)
        {
            turn[i] += i + 1;
        }
    }
}
