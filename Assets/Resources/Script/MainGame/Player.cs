using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    // 카운터
    static int count = 0;               // 참여자 + 시스템 Player 카운터
    static int sysCount = 0;            // 시스템 Player 카운터

    // 캐릭터 클래스 포함
    public Character character = new Character(-1);

    // 플레이어 정보 UI 배치
    public PlayerInfoUI InfoUI = null;


    // 오토 플레이 여부
    bool _isAutoPlay = false;
    public bool isAutoPlay { get { return _isAutoPlay; } }

    // 플레이어 번호
    int _index = -1;
    public int index { get { return _index; } }

    // 플레이어 이름
    string _name = null;
    public string name { get { return _name; } }

    // 플레이어 순서
    int _order = -1;
    public int order { get { return _order; } }



    // 플레이어 자원
    public GameResource life = new GameResource(10, 10, -1);
    public GameResource coin = new GameResource(0, 999, 0);

    // 아이템 슬롯
    public List<Item> inventory = new List<Item>();



    // 생성자
    /// <summary>
    /// 사용 금지 => Player(int characterIndex) 사용
    /// </summary>
    private Player()
    {
        // 사용 방지
    }
    public Player(string playerName, int characterIndex, bool __isAutoPlay)
    {
        // 캐릭터 셋팅
        character.SetCharacter(characterIndex);


        // 오토 셋팅
        _isAutoPlay = __isAutoPlay;

        // 플레이어 번호 셋팅
        _index = count;

        // 플레이어 이름 셋팅
        _name = playerName;



        // 카운터 반영
        if (character.job == Job.JobType.System)
        {
            sysCount++;
            count++;
        }
        else
        {
            count++;
        }
    }

    // 소멸자
    ~Player()
    {
        // 카운터 반영
        if (character.job == Job.JobType.System)
        {
            sysCount--;
            count--;
        }
        else
        {
            count--;
        }
    }



    /// <summary>
    /// 플레이어 인원수 반환
    /// </summary>
    public static int Count()
    {
        return count - sysCount;
    }


    /// <summary>
    /// 시스템 플레이어 인원수 반환
    /// </summary>
    public static int CountSystem()
    {
        return sysCount;
    }



    public void SetPlayer(string playerName, int characterIndex, bool __isAutoPlay)
    {
        // 캐릭터 셋팅
        character.SetCharacter(characterIndex);


        // 오토 셋팅
        _isAutoPlay = __isAutoPlay;

        // 플레이어 번호 셋팅
        _index = count;

        // 플레이어 이름 셋팅
        _name = playerName;
    }
}

