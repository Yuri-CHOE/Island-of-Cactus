using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Load_Ranking : MonoBehaviour
{
    public Scenes_mini DontD_ob_playernum;
    public Check chek_score;
    public bool obj_delete;
    public int player_num;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        obj_delete = false;

        DontD_ob_playernum = GameObject.Find("Test").GetComponent<Scenes_mini>();
        chek_score = GameObject.Find("Game").GetComponent<Check>();
        player_num = DontD_ob_playernum.member_num;

    }

    void Update()
    {
        if (chek_score.z == 9)
        {
            DontD_ob_playernum.ob_delete = true;
        }
    }
}
