using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_mini : MonoBehaviour
{
    // 플레이어
    public Player owner = null;

    // 플레이어 아이콘
    public Sprite face = null;

    //미니게임 점수
    public int score = 0;
    public bool plusScore = false;
    public bool minusScore = false;
    public bool scoreSetCheck = false;

    //미니게임 등수
    public int rank = 0;

    //미니게임 참가 여부
    public bool join = false;

    //현재 자신의 차례인지 확인
    public bool myTurn = false;

    //플레이어 death 오브젝트
    public GameObject deathUi;

    //플레이어 스코어 텍스트 오브젝트
    public Text txtScore;

    void Start()
    {
        score = 0;
        txtScore.text = score.ToString();
    }

    void Update()
    {
        if (join)
        {
            // 자체 활성화 혹은 비활성화
            gameObject.SetActive(false);
        }

        turnCehck();
        scoreCheck();
    }

    public void LoadFace()
    {
        // 아이콘 로드
        Debug.Log(@"Data/Character/Face/Face" + owner.character.index.ToString("D4"));
        Sprite temp = Resources.Load<Sprite>(@"Data/Character/Face/Face" + owner.character.index.ToString("D4"));

        // 이미지 유효 검사
        if (temp == null)
        {
            // 기본 아이콘 대체 처리
            Debug.Log(@"UI/playerInfo/player");
            temp = Resources.Load<Sprite>(@"UI/playerInfo/player");
        }

        // 최종 실패 처리
        if (temp == null)
            Debug.Log("로드 실패 :: UI/playerInfo/player");
        // 아이콘 변경
        else
            face = temp;
    }

    void turnCehck()
    {
        if (myTurn)
        {
            deathUi.gameObject.SetActive(false);
        }
        else
        {
            deathUi.gameObject.SetActive(true);
        }
    }

    void scoreCheck()
    {
        scoreSetCheck = false;

        if (plusScore)
        {
            score += 100;
            txtScore.text = score.ToString();
            scoreSetCheck = true;
            plusScore = false;
        }
    }
}