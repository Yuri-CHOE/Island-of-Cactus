using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public enum Type {
        System,
        User,
        AI,
    }

    // 카운터
    static int count = 0;               // 시스템 제외 Player 카운터



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



    // 플레이어 자원
    public GameResource life = new GameResource(10, 10, -1);
    public GameResource coin = new GameResource(0, 999, 0);

    // 아이템 슬롯
    public List<Item> inventory = new List<Item>();



    // 생성자
    /// <summary>
    /// 사용 금지
    /// </summary>
    protected Player()
    {
        // 사용 방지
    }
    public Player(Type __type, int characterIndex, bool __isAutoPlay, string playerName)
    {
        SetPlayer(__type, characterIndex, __isAutoPlay, playerName);

        // 카운터 반영
        if (_type != Type.System)
            count++;
    }

    // 소멸자
    ~Player()
    {
        // 카운터 반영
        if (_type != Type.System)
            count--;
    }



    /// <summary>
    /// 플레이어 인원수 반환
    /// </summary>
    public static int Count()
    {
        return count;
    }



    public void SetPlayer(Type __type, int characterIndex, bool __isAutoPlay, string playerName)
    {
        // 캐릭터 타입
        _type = __type;

        // 캐릭터 셋팅
        Debug.Log(characterIndex + " => " + Character.table.Count);
        _character = Character.table[characterIndex];

        // 오토 셋팅
        _isAutoPlay = __isAutoPlay;

        // 플레이어 이름 셋팅
        _name = playerName;
    }

    /// <summary>
    /// 자동 플레이 설정
    /// </summary>
    /// <param name="__isAutoPlay"></param>
    public void SetAutoPlay(bool __isAutoPlay)
    {
        _isAutoPlay = __isAutoPlay;
    }

}

