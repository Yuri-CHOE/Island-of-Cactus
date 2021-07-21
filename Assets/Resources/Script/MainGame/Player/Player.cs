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

    public class PlayerSystem
    {
        public Player Starter = new Player(Player.Type.System, 1, true, "Starter");
        public Player Minigame = new Player(Player.Type.System, 1, true, "Minigame");
        public Player MinigameEnder = new Player(Player.Type.System, 1, true, "MinigameEnder");
        public Player Ender = new Player(Player.Type.System, 1, true, "Ender");
    }

    // 시스템 플레이어
    public static PlayerSystem system = new PlayerSystem();

    // 시스템 제외 모든 플레이어
    public static List<Player> allPlayer = new List<Player>();

    // 플레이어 자신
    public static Player me = null;

    // 턴 진행중 플레이어
    public static Player isTurn { get { return Turn.now; } }

    // p1~4 플레이어
    public static Player player_1 = null;
    public static Player player_2 = null;
    public static Player player_3 = null;
    public static Player player_4 = null;

    // 초기화
    public static void Clear()
    {
        //system = new PlayerSystem()
        allPlayer.Clear();
        me = null;
        player_1 = null;
        player_2 = null;
        player_3 = null;
        player_4 = null;
    }

    /// <summary>
    /// 특정 장소에 위치한 플레이어 리턴
    /// </summary>
    /// <param name="blockIndex"></param>
    /// <returns></returns>
    public List<Player> LocatedPlayer(int blockIndex)
    {
        List<Player> result = new List<Player>();

        for (int i = 0; i > allPlayer.Count; i++)
            if (allPlayer[i].movement.location == blockIndex)
                result.Add(allPlayer[i]);

        return result;
    }

    /// <summary>
    /// 특정 플레이어의 플레이어 인덱스 반환
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    public static int Index(Player current)
    {
        for (int i = 0; i < allPlayer.Count; i++)
            if (allPlayer[i] == current)
                return i;

        return -1;
    }



    // 다른 플레이어
    public List<Player> otherPlayers = new List<Player>();



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



    // 플레이어 정보 UI
    public PlayerInfoUI infoUI = null;

    // 플레이어 아이콘
    public Sprite face = null;

    // 캐릭터 오브젝트
    public GameObject avatar = null;
    public Transform avatarBody = null;

    //// 위치 인덱스
    //int _locate = -1;
    //public int locate { get { return _locate; } set { _locate = GameData.blockManager.indexLoop(_locate, value); } }

    //// 위치 오브젝트
    //public GameObject locateBlock { get { if (GameData.isMainGameScene) { if (locate >= 0) return GameData.blockManager.gol[locate]; else return GameData.blockManager.startBlock.gameObject; } else return null; } }

    // 이동제어 스크립트
    int _location = -1;
    public int location { get { return _location; } set { int i = BlockManager.script.indexLoop(value,0); dice.valueRecord += i - location; _location = i; } }
    //public CharacterMover movementMirror = new CharacterMover();
    //public CharacterMover movement { get { if (avatar == null) return null; else return avatar.GetComponent<CharacterMover>(); } }
    public CharacterMover movement = null;

    // AI 제어 스크립트
    public AIWorker ai { get { if (avatar == null) return null; else return avatar.GetComponent<AIWorker>(); } }




    // 주사위
    public Dice dice = new Dice();

    // 플레이어 자원
    //public GameResource life = new GameResource(5, 10, -1);
    //public GameResource coin = new GameResource(0, 999, 0);
    public GameResource life = new GameResource(5, 10, -1);
    public GameResource coin = new GameResource(110, 999, 0);

    // 아이템 슬롯
    public List<ItemSlot> inventory = new List<ItemSlot>();
    public int inventoryCount { get { int c = 0; for (int i = 0; i < inventory.Count; i++) { if (!inventory[i].isEmpty) c++; } return c; } }
    public static int inventoryMax = 3;

    // 행동 불가능 여부
    public bool isDead = false;
    public bool isStun { get { return (stunCount > 0); } }
    public int stunCount = 0;

    // 전투 속성
    public Battle battle = new Battle(1f, 0f);




       

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
        
        Debug.Log("플레이어 생성됨 :: 캐릭터 번호 = " + characterIndex);
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
        avatarBody = avatar.transform.Find("BodyObject");

        // 이동제어 셋업
        movement = avatar.GetComponent<CharacterMover>();
        movement.owner = this;
        //if (movementMirror.location != -1)
        //    movement.CopyByMirror(movementMirror);
        //movementMirror = movement;
        if (location != -1)
            movement.location = location;

        // AI 소유자 지정
        ai.SetUp(this);

        Debug.Log("캐릭터 생성 :: " + avatar.name);
    }


    public void LoadFace()
    {
        //// 아이콘 로드
        //Debug.Log(@"Data/Character/Face/Face" + character.index.ToString("D4"));
        //Sprite temp = Resources.Load<Sprite>(@"Data/Character/Face/Face" + character.index.ToString("D4"));

        //// 이미지 유효 검사
        //if (temp == null)
        //{
        //    // 기본 아이콘 대체 처리
        //    Debug.Log(@"UI/playerInfo/player");
        //    temp = Resources.Load<Sprite>(@"UI/playerInfo/player");
        //}

        //// 최종 실패 처리
        //if (temp == null)
        //    Debug.Log("로드 실패 :: UI/playerInfo/player");
        //// 아이콘 변경
        //else
        //    face = temp;

        face = character.GetIcon();
    }


    public void AddItem(ItemSlot itemSlot, int count)
    {
        AddItem(itemSlot.item, count);
    }
    public void AddItem(Item _item, int count)
    {
        // 코인 아이템일 경우
        if(_item.index == 1)
        {
            coin.Add(count);
            return;
        }

        // 잔여 슬롯 부족 시 버림 === 시간 여유 된다면 소거할 아이템 선택하여 버리게 바꿀것
        if (inventoryCount >= Player.inventoryMax)
            return;

        for (int i = 0; i < inventory.Count; i++)
        {
            // 빈칸일경우 넣고 종료
            if (inventory[i].isEmpty)
            {
                inventory[i].item = _item;
                inventory[i].count = count;
                break;
            }
        }
    }

    public void RemoveItem(ItemSlot currentSlot)
    {
        // 인벤토리 순회 체크
        for (int i = 0; i < inventory.Count; i++)
        {
            // 일치하는 슬롯 검색
            if (inventory[i] == currentSlot)
                inventory[i].Clear();
        }

        // 인벤토리 재정렬
        SortInventory();
    }

    public void SortInventory()
    {
        // 마지막을 제외한 모든 슬롯 순회
        for (int i = 0; i < inventory.Count - 1; i++)
        {
            // 빈 슬롯 검색
            if (inventory[i].isEmpty)
            {
                // 당겨오기 수행
                for (int j = i + 1; j < inventory.Count; j++)
                {
                    // 당겨올 슬롯에 아이템 있을 경우
                    if (!inventory[j].isEmpty)
                    {
                        // 복사
                        inventory[i].CopyByMirror(inventory[j]);

                        // 당겨온 슬롯 말소
                        inventory[j].Clear();
                    }
                }
            }
        }
    }


    public void MirrorLoaction()
    {
         _location = movement.location;
    }


    /// <summary>
    /// 공격
    /// </summary>
    /// <param name="target">공격 대상</param>
    public void Attack(Player target)
    {
        // 공격 애니메이션
        // 미구현=================

        // 데미지 요청
        target.Hit(this, battle.atk.value);
    }

    /// <summary>
    /// 피격
    /// </summary>
    /// <param name="Attacker">공격자</param>
    /// <param name="rawDamage">미가공 데미지</param>
    public void Hit(Player Attacker, float rawDamage)
    {
        // 피격 애니메이션
        // 미구현=================

        // 데미지 계산
        float finalDamage = battle.Damage(rawDamage);

        // 체력 반영
        life.subtract((int)finalDamage);
    }

    /// <summary>
    /// 부활
    /// </summary>
    public void Resurrect()
    {
        // 초기화
        stunCount = 0;
        isDead = false;

        // 라이프 지급
        life.Add(5);

        // UI 제거
        infoUI.dead.gameObject.SetActive(false);
    }

}

