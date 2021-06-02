using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{

    Player_score score;

    private void Awake()
    {
        score = GameObject.Find("Result").GetComponent<Player_score>();
    }

    // 출력값 테스트용
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            score.player_1 += 10;
            score.player_2 += 20;
            score.player_3 += 30;
            score.player_4 += 40;
        }
    }
}
