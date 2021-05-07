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




    // 캐릭터 오브젝트
    public GameObject avatar = null;

    // 캐릭터가 고정될 블록 인덱스
    public int location = -1;




    // 위치 인덱스
    int locate = -1;

    // 주사위
    public Dice dice = new Dice();

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

        Debug.Log("플레이어 생성됨 :: 캐릭터 번호 = " + characterIndex);
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
        // 테이블 체크
        if (!Character.isReady)
            Character.SetUp();

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



    public void CreateAvatar(Transform parentObject)
    {
        // 부모 미지정시 중단
        if (parentObject == null)
            return;

        // 기존 아바타 있을 경우 오브젝트 제거
        if (avatar != null)
            Transform.Destroy(avatar);

        // 오브젝트 유효 검사
        Debug.Log(@"Data/Character/Character" + character.index.ToString("D4"));
        GameObject obj = Resources.Load<GameObject>(@"Data/Character/Character" + character.index.ToString("D4"));
        if (obj == null)
        {
            obj = Resources.Load<GameObject>(@"Data/Character/Character0000");
            Debug.Log(@"Data/Character/Character0000");
        }
        if (obj == null)
            Debug.Log("로드 실패 :: Data/Character/Character0000");

        // 생성 및 등록
        avatar = GameObject.Instantiate(
            obj,
            parentObject
            ) as GameObject;

        Debug.Log("캐릭터 생성 :: " + avatar.name);
    }

}

