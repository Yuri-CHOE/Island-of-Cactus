using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IocEffect
{
    // 유효기간
    public enum Expiration
    {
        Never,
        Forever,
        Cycle,
        Turn,
        Moment,
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
    }




    // 효과 유효기간
    Expiration _expiration = Expiration.Never;
    public Expiration expiration { get { return _expiration; } }

    // 효과 카운트(개수)
    int _count = -1;
    public int count { get { return _count; } }

    // 효과 타겟 플레이어 (사용자와의 관계)
    Target _target = Target.Self;
    public Target target { get { return _target; } }

    // 효과 시작점
    int _where = -1;
    public int where { get { return _where; } }

    // 효과 대상
    What _what = What.None;
    public What what { get { return _what; } }

    // 효과 값
    int _value = -1;
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

    void SetExpiration(Expiration __expiration)
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
        else if (target == Target.SelectedPlayer)
            ;

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
    public static IEnumerator GeneralEffect(Expiration __expiration, List<Player> filteredTarget, int __where, What __what, int __value)
    {
        // 대상 없음
        if (__what == What.None)
        {

        }

        // 캐릭터 (플레이어 아바타)
        else if (__what == What.Character)
        {
            // 미구현 ===================================
        }

        // 이동
        else if (__what == What.Move)
        {
            for (int i = 0; i < filteredTarget.Count; i++)
            {
                Player current = filteredTarget[i];

                /*
             기존 이동 중단    
             현위치(current.movement.location)에 중단점(current.location) 대입
             을 무브먼트에서 메소드로 처리

            여기서 호출
             */
            }
        }

        // 블록 타입 변경
        else if (__what == What.Block)
        {

        }

        // 주사위 값 제어
        else if (__what == What.Dice)
        {

        }

        // 라이프 획득
        else if (__what == What.Life)
        {

        }

        // 코인 획득
        else if (__what == What.Coin)
        {

        }

        // 아이템 획득
        else if (__what == What.Item)
        {

        }

        // 미니게임 수행
        else if (__what == What.Minigame)
        {

        }

        yield return null;
    }
}
