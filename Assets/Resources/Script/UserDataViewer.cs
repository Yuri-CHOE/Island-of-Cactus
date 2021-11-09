using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserDataViewer : MonoBehaviour
{

    // 유저 이름
    public Text username = null;

    //// 전체 플레이 시간 (초)
    //public Text playTime = null;

    // 전체 플레이 횟수
    public Text playCount = null;

    // 등수 달성 횟수
    public Text rank1 = null;
    public Text rank2 = null;
    public Text rank3 = null;
    public Text rank4 = null;

    private void Awake()
    {
        SetUp();
        gameObject.SetActive(false);
    }

    public void SetUp()
    {
        username.text = UserData.userName;
        playCount.text = UserData.playCount.ToString();
        rank1.text = UserData.rank1.ToString();
        rank2.text = UserData.rank2.ToString();
        rank3.text = UserData.rank3.ToString();
        rank4.text = UserData.rank4.ToString();
    }
}
