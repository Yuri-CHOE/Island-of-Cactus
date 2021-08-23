using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct IocEffect
{
    // 활성화 이펙트 목록
    public static List<IocEffect> activeEffects = new List<IocEffect>();


    // 유효기간
    public enum Expiration
    {
        Never,
        Forever,
        Cycle,
        Turn,
        Moment,
        Invalid,
    }

    // 타겟 플레이어
    public enum Target
    {
        Self,
        AllPlayer,
        OthersPlayer,
        SelectedPlayer,
        World,
    }

    // 대상 필드
    public enum What
    {
        None,
        Character,
        Move,
        Block,
        Dice,
        Life,
        Coin,
        Item,
        Minigame,
        Event,
    }




    // 효과 유효기간
    Expiration _expiration ;
    public Expiration expiration { get { return _expiration; } }
    public bool isInvalid { get { return expiration == Expiration.Invalid; } }

    // 효과 카운트(개수)
    int _count;
    public int count { get { return _count; } }

    // 효과 타겟 플레이어 (사용자와의 관계)
    Target _target;
    public Target target { get { return _target; } }

    // 효과 시작점
    int _where;
    public int where { get { return _where; } }

    // 효과 대상
    What _what;
    public What what { get { return _what; } }

    // 효과 값
    int _value;
    public int value { get { return _value; } }




    /// <summary>
    /// 재설정 함수, 별도 사용하지 말것
    /// </summary>
    /// <param name="__expiration">잔존 기간</param>
    /// <param name="__count">발동 횟수</param>
    /// <param name="__target">발동자에 의해 가공된 효과 적용 대상</param>
    /// <param name="__where">발동 지점으로부터 거리</param>
    /// <param name="__what">효과 종류</param>
    /// <param name="__value">효과 값</param>
    public void Set(Expiration __expiration, int __count, Target __target, int __where, What __what, int __value)
    {
        SetExpiration(__expiration);
        SetCount(__count);
        SetTarget(__target);
        SetWhere(__where);
        SetWhat(__what);
        SetValue(__value);
    }

    public void SetExpiration(Expiration __expiration)
    {
        _expiration = __expiration;
    }

    void SetCount(int __count)
    {
        if (__count < 0)
            return;

        _count = __count;
    }

    void SetTarget(Target __target)
    {
        _target = __target;
    }

    void SetWhere(int __where)
    {
        _where = __where;
    }

    void SetWhat(What __what)
    {
        _what = __what;
    }

    void SetValue(int __value)
    {
        _value = __value;
    }




    public static IocEffect New() { IocEffect ef = new IocEffect(); ef.SetExpiration(IocEffect.Expiration.Invalid); return ef; }











    /// <summary>
    /// 효과를 적용받을 플레이어 리스트
    /// SelectedPlayer 는 빈 리스트 반환
    /// World 는 null 반환
    /// </summary>
    /// <param name="__iocEvent">이벤트</param>
    /// <param name="targetPlayer_Or_null">발동자 또는 null</param>
    /// <returns></returns>
    public static List<Player> TargetFiltering(Target target, Player targetPlayer_Or_null)
    {
        // 결과물
        List<Player> pl = new List<Player>();

        // 자기 자신
        if (target == Target.Self)
            pl.Add(targetPlayer_Or_null);

        // 모든 플레이어
        else if (target == Target.AllPlayer)
            pl.AddRange(Player.allPlayer);

        // 다른 플레이어
        else if (target == Target.OthersPlayer)
            pl.AddRange(targetPlayer_Or_null.otherPlayers);

        // 다른 플레이어
        // 사용 시점에서 타겟 지정하기 때문에 궂이 타겟팅 호출 할 이유 없음
        else if (target == Target.SelectedPlayer)
            pl.Add(targetPlayer_Or_null);

        // 맵 광역
        else if (target == Target.World)
            return null;

        return pl;
    }


    /// <summary>
    /// 실제 효과 발동
    /// </summary>
    /// <param name="filteredTarget">효과를 받을 플레이어들</param>
    /// <param name="__blockIndex">위치</param>
    public IEnumerator GeneralEffect(Player user, List<Player> filteredTarget)
    {
        // 월드 이벤트 호출
        if (filteredTarget == null)
        {
            // 호출            
            yield return WorldEffect(this);
        }
        else
        {
            // 선택형 선택
            if (filteredTarget.Count == 0)
            {
                //=========== 미구현
                //yield return ;
            }


            Player current = null;
            int blockIndex;
            bool isExecute;

            for (int i = 0; i < filteredTarget.Count; i++)
            {
                // 퀵등록
                current = filteredTarget[i];

                // 이동 포인트 확보
                blockIndex = current.location;

                // 효과 적용 여부
                isExecute = true;



                // 사용자 불일치 경우
                if (current != user)
                {
                    // 인벤토리 스캔
                    for (int j = 0; j < current.inventoryCount; j++)
                    {
                        // 실드 체크
                        if (current.inventory[j].item.index == 19)
                        {
                            // 실드 자동 사용
                            //GameMaster.script.itemManager.ItemUse(current.inventory[j]);
                            current.inventory[j].count--;

                            // 차단 적용
                            isExecute = false;

                            // 스캔 중단
                            break;
                        }
                    }
                }



                // 효과 차단
                if (!isExecute)
                    continue;

                // 대상 없음
                if (what == What.None)
                {
                    Debug.Log("효과 :: 통합 효과 없음");
                    break;
                }

                // 캐릭터 (플레이어 아바타)
                else if (what == What.Character)
                {
                    // 미구현 ===================================
                }

                // 이동
                else if (what == What.Move)
                {
                    // 중단
                    current.movement.MoveStop();

                    // 이동 포인트 가공
                    if (where == -3)
                    {
                        //blockIndex *= where;
                        current.dice.SetValueTotal(current.dice.valueTotal * value);

                        // 이동 호출
                        current.movement.PlanMoveBy(current.dice.valueTotal);
                    }
                    else if (where == -2)
                    {
                        //blockIndex += where;
                        current.dice.SetValueTotal(current.dice.valueTotal + value);

                        // 이동 호출
                        current.movement.PlanMoveBy(current.dice.valueTotal);
                    }
                    else
                    {
                        blockIndex = where;
                        current.dice.SetValueTotal(0);
                        Debug.LogError(blockIndex);


                        // 오아시스 입장
                        if (blockIndex == -1)
                            current.movement.GotoJail();
                        // 이동 호출
                        else
                            current.movement.PlanMoveTo(blockIndex);
                    }

                }

                // 블록 타입 변경
                else if (what == What.Block)
                {
                    // 퀵등록
                    DynamicBlock dBlock = BlockManager.script.GetBlock(where).GetComponent<DynamicBlock>();

                    // 설정
                    dBlock.blockTypeDetail = (BlockType.TypeDetail)value;
                    dBlock.blockType = BlockType.GetTypeByDetail(dBlock.blockTypeDetail);
                    dBlock.Refresh();
                }

                // 주사위 제어
                else if (what == What.Dice)
                {
                    if (value != 0)
                    {
                        // 주사위 추가
                        current.dice.count += value;

                        Debug.Log("주사위 :: " + value + " 개 추가 -> 총 " + current.dice.count);
                    }
                    else
                    {
                        // 주사위 변경
                        current.dice.type = (Dice.SpecialDice)where;

                        Debug.Log("주사위 :: 타입 변경 -> " + current.dice.type);
                    }
                }

                // 라이프 획득
                else if (what == What.Life)
                {
                    current.life.Add(value);
                }

                // 코인 획득
                else if (what == What.Coin)
                {
                    current.coin.Add(value);
                }

                // 아이템 획득
                else if (what == What.Item)
                {
                    if(value > 1)
                        if (value < Item.table.Count)
                            current.AddItem(Item.table[value], count);
                }

                // 미니게임 수행
                else if (what == What.Minigame)
                {
                    // 미니게임 호출
                    // 임시구현======================================= 미니게임 카드는 솔로플레이 불가능이므로 수량 미기입으로 우회중
                    // 주석처리된 스크립트로 교체할것
                    GameMaster.script.loadingManager.LoadAsyncMiniGame(Minigame.RandomGame(), value, filteredTarget);
                    //GameMaster.script.loadingManager.LoadAsyncMiniGame(
                    //    Minigame.RandomGame(filteredTarget.Count), 
                    //    value, filteredTarget
                    //    );

                    // 반복 차단
                    break;
                }

                // 미니게임 수행
                else if (what == What.Event)
                {
                    // 생성 위치
                    blockIndex = current.location + where;

                    // 이벤트 생성
                    // 캐릭터 현재위치에서 where 떨어진곳에 value번째 이벤트를 count 만큼 current가 설치한다
                    GameMaster.script.eventManager.CreateEventObject(blockIndex, value, count, current);
                }
            }

        }


        // 사용된 효과 등록
        if (expiration == Expiration.Forever || expiration == Expiration.Cycle || expiration == Expiration.Turn)
        {
            // 소모처리
            _count--;
            
            // 효과 등록
            if (!activeEffects.Contains(this))
                activeEffects.Add(this);
        }
        // 즉시 만료
        else
            _expiration = Expiration.Invalid;

        Debug.Log("ioc effect :: 효과 작동됨");

        yield return null;
    }



    public static IEnumerator WorldEffect(IocEffect __iocEffect)
    {
        // 개별 특수 효과
        switch ((int)__iocEffect.what)
        {
            // 필요시 추가

            case 0:
                // 0번은 없음
                Debug.LogError("error :: 존재하지 않는 월드 이벤트(0)의 효과 호출됨");
                Debug.Break();
                break;

            case 1:
                // 마이너스 블록 강화

                // 연출
                // 미구현=========

                // 마이너스 블록 강화
                BlockWork.minusBlockValue++;

                break;

            case 2:
                // 플러스 블록 강화

                // 연출
                // 미구현=========

                // 플러스 블록 강화
                BlockWork.plusBlockValue++;

                break;

            case 3:
                // 노말 블록 초기화

                // 연출
                // 미구현=========

                // 노말 블록 강화 초기화
                BlockWork.plusBlockValue = 0;
                BlockWork.minusBlockValue = 0;

                break;
        }

        yield return null;
    }
}
