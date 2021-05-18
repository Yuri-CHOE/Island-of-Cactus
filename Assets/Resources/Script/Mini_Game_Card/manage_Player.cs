using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manage_Player : MonoBehaviour
{
    public Score score1, score2, score3, score4;
    public int player = 2;

    void Awake()
    {
        score1 = GameObject.Find("player (1)").GetComponent<Score>();
        score2 = GameObject.Find("player (2)").GetComponent<Score>();
        score3 = GameObject.Find("player (3)").GetComponent<Score>();
        score4 = GameObject.Find("player (4)").GetComponent<Score>();
    }
    void Start()
    {
        for (int i = 4; i > player; i-- ){
            if (i == 2)
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
