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
        // 타겟 플레이어
        public enum Target
        {
            Self,
            AllPlayer,
            OthersPlayer,
            DesignatedPlayer,
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

        // 효과 카운트(개수)
        int _count = -1;
        public int count { get { return _count; } }

        // 효과 타겟 플레이어
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
        /// <param name="__count"></param>
        /// <param name="__target"></param>
        /// <param name="__where"></param>
        /// <param name="__what"></param>
        /// <param name="__value"></param>
        public void Set(int __count, Target __target, int __where, What __what, int __value)
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
    iocEventEffect _effect = new iocEventEffect();
    public iocEventEffect effect { get { return _effect; } }



    // 생성자
    protected IocEvent()
    {
        // 사용 금지
    }

    protected IocEvent(List<string> strList, List<string> loaclList)
    {
        // out of range 방지
        if (strList.Count != 10)
            return;
        if (loaclList.Count != 3)
            return;

        // 테이블 읽어오기
        Set(
            int.Parse(strList[0]),
            (ActiveType)int.Parse(strList[1]),
            loaclList[1],
            int.Parse(strList[3]),

            int.Parse(strList[4]),
            (iocEventEffect.Target)(int.Parse(strList[5])),
            int.Parse(strList[6]),
            (iocEventEffect.What)(int.Parse(strList[7])),
            int.Parse(strList[8]),

            loaclList[2].Replace("\\n", "\n").Replace("value", strList[8])
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
    

    void Set(int __index, ActiveType __activeType, string __name, int __modelIndex, int __count, iocEventEffect.Target __target, int __where, iocEventEffect.What __what, int __value, string __info)
    {
        SetIndex(__index);
        SetActiveType(__activeType);
        SetName(__name);
        effect.Set(__count, __target, __where, __what, __value);
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
    /// 이벤트 효과
    /// </summary>
    /// <param name="__iocEvent">작동할 이벤트</param>
    /// <param name="targetPlayer_Or_null">작동시킨 플레이어</param>
    public static void Effect(IocEvent __iocEvent, Player targetPlayer_Or_null)
    {
        switch (__iocEvent.index)
        {
            case 0:
                // 0번은 없음
                break;

            case 1:
                // 효과
                break;

            case 2:
                // 효과
                break;

                // 이하 추가 필요========================

        }
    }

}
