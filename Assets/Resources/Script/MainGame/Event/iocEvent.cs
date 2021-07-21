using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IocEvent
{
    public enum ActiveType
    {
        none,
        arrive,     // 도착 (이동 종료)
        contact,    // 접촉
    }

    public class iocEventEffect
    {

        // 효과 카운트(개수)
        int _count = -1;
        public int count { get { return _count; } }

        // 효과 타겟 플레이어
        IocEffect.Target _target = IocEffect.Target.Self;
        public IocEffect.Target target { get { return _target; } }

        // 효과 시작점
        int _where = -1;
        public int where { get { return _where; } }

        // 효과 대상
        IocEffect.What _what = IocEffect.What.None;
        public IocEffect.What what { get { return _what; } }

        // 효과 값
        int _value = -1;
        public int value { get { return _value; } }



        /// <summary>
        /// 재설정 함수, 별도 사용하지 말것
        /// </summary>
        /// <param name="__count"></param>
        /// <param name="__target"></param>
        /// <param name="__where"></param>
        /// <param name="__what"></param>
        /// <param name="__value"></param>
        public void Set(int __count, IocEffect.Target __target, int __where, IocEffect.What __what, int __value)
        {
            SetCount(__count);
            SetTarget(__target);
            SetWhere(__where);
            SetWhat(__what);
            SetValue(__value);
        }

        void SetCount(int __count)
        {
            if (__count < 0)
                return;

            _count = __count;
        }

        void SetTarget(IocEffect.Target __target)
        {
            _target = __target;
        }

        void SetWhere(int __where)
        {
            _where = __where;
        }

        void SetWhat(IocEffect.What __what)
        {
            _what = __what;
        }

        void SetValue(int __value)
        {
            _value = __value;
        }
    }

    // 이벤트 테이블
    static List<IocEvent> _table = new List<IocEvent>();
    public static List<IocEvent> table { get { return _table; } }       // 초기화 안됬으면 초기화 후 반환

    // 이벤트 확인용
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }



    // 이벤트 번호
    int _index = -1;
    public int index { get { return _index; } }

    // 이벤트 작동 방식
    ActiveType _activeType = ActiveType.none;
    public ActiveType activeType { get { return _activeType; } }

    // 이벤트 이름
    string _name = null;
    public string name { get { return _name; } }

    // 이벤트 모델 인덱스
    int _modelIndex = -1;
    public int modelIndex { get { return _modelIndex; } }

    // 이벤트 설명
    string _info = null;
    public string info { get { return _info; } }

    // 이벤트 설명
    IocEffect _effect = new IocEffect();
    public IocEffect effect { get { return _effect; } }



    // 생성자
    protected IocEvent()
    {
        // 사용 금지
    }

    protected IocEvent(List<string> strList, List<string> loaclList)
    {
        // out of range 방지
        if (strList.Count != 11)
            return;
        if (loaclList.Count != 3)
            return;

        // 테이블 읽어오기
        Set(
            int.Parse(strList[0]),
            (ActiveType)int.Parse(strList[1]),
            loaclList[1],
            int.Parse(strList[3]),

            (IocEffect.Expiration)(int.Parse(strList[4])),
            int.Parse(strList[5]),
            (IocEffect.Target)(int.Parse(strList[6])),
            int.Parse(strList[7]),
            (IocEffect.What)(int.Parse(strList[8])),
            int.Parse(strList[9]),

            loaclList[2].Replace("\\n", "\n").Replace("value", strList[9])
            );
    }


    /// <summary>
    /// 테이블 생성
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("테이블 셋팅 : 이벤트");

        // 중복 실행 방지
        if (_isReady)
            return;

        // 테이블 읽어오기
        CSVReader reader = new CSVReader(null, "Event.csv");
        CSVReader local = new CSVReader(null, "Event_local.csv", true, false);
        Debug.Log(reader.table.Count);

        // 더미 생성
        table.Add(new IocEvent());

        // 테이블로 리스트 셋팅
        for (int i = 1; i < reader.table.Count; i++)
        {
            table.Add(new IocEvent(reader.table[i], local.table[i]));
        }

        // 준비완료
        _isReady = true;
    }
    

    void Set(int __index, ActiveType __activeType, string __name, int __modelIndex, IocEffect.Expiration __expiration, int __count, IocEffect.Target __target, int __where, IocEffect.What __what, int __value, string __info)
    {
        SetIndex(__index);
        SetActiveType(__activeType);
        SetName(__name);
        effect.Set(__expiration, __count, __target, __where, __what, __value);
        SetInfo(__info);
    }

    void SetIndex(int __index)
    {
        _index = __index;
    }

    void SetActiveType(ActiveType __activeType)
    {
        _activeType = __activeType;
    }

    void SetName(string __name)
    {
        _name = __name;
    }

    void SetInfo(string __info)
    {
        _info = __info;
    }


    /// <summary>
    /// 이벤트 작동 조건
    /// </summary>
    /// <param name="__iocEvent">작동할 이벤트</param>
    /// <param name="current">작동시킨 플레이어</param>
    /// <param name="creator">이벤트를 생성한 플레이어</param>
    public static bool Condition(IocEvent __iocEvent, Player current, Player creator)
    {
        bool result = false ;

        // 발동자 자격 체크 - 현재 DB에 발동자 자격 필드 없음
        //if(__iocEvent.effect.target == iocEventEffect.Target.Self)
        //    result = true;

        result = true;

        return result;
    }


    /// <summary>
    /// 이벤트 효과
    /// </summary>
    /// <param name="targetPlayer_Or_null">작동시킨 플레이어</param>
    //public static IEnumerator Effect(IocEvent __iocEvent, Player targetPlayer_Or_null, int __blockIndex)
    public IEnumerator Effect(Player targetPlayer_Or_null)
    {
        // 타겟 리스트
        List<Player> pl = IocEffect.TargetFiltering(effect.target, targetPlayer_Or_null);

        // 통합 효과
        yield return effect.GeneralEffect(targetPlayer_Or_null, pl);

        // 개별 특수 효과
        yield return EachEffect(this);
    }

    public static IEnumerator EachEffect(IocEvent __iocEvent)
    {
        // 개별 특수 효과
        switch (__iocEvent.index)
        {
            case 0:
                // 0번은 없음
                Debug.LogError("error :: 존재하지 않는 이벤트(0)의 효과 호출됨");
                Debug.Break();
                break;

            case 1:
                // 효과
                break;

            case 2:
                // 효과
                break;

            case 3:
                // 효과
                break;

            case 4:
                // 효과
                break;

            case 5:
                // 효과
                break;

        }

        yield return null;
    }

}
