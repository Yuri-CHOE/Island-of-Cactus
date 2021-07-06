using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class act_Score : MonoBehaviour
{
    public enum Type
    {
        System,
        User,
        AI,
    }

    // 플레이어 타입
    Type _type = Type.System;
    public Type type { get { return _type; } }

    // 캐릭터 클래스 포함
    Character _character = null;
    public Character character { get { return _character; } }

    // 오토 플레이 여부
    bool _isAutoPlay = false;
    public bool isAutoPlay { get { return _isAutoPlay; } }

    // 플레이어 이름
    string _name = null;
    public string name { get { return _name; } }

    // 플레이어 아이콘
    public Sprite face = null;

    //미니게임 점수
    public int score = 0;

    //미니게임 등수
    public int rank = 0;

    //미니게임 참가 여부
    public bool active = false;

    void Awake()
    {
        //메임게임으로부터 플레이어 정보 세팅 코드 작성 필요
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (active)
        {
            gameObject.SetActive(false);
        }
    }

    public void LoadFace()
    {
        // 아이콘 로드
        Debug.Log(@"Data/Character/Face/Face" + character.index.ToString("D4"));
        Sprite temp = Resources.Load<Sprite>(@"Data/Character/Face/Face" + character.index.ToString("D4"));

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
}
