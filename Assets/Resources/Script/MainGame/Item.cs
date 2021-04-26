using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum Type
    {
        None,
        Target,
        Installation,
        Consumable,
        WideArea,
    }

    public class ItemEffect
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

        // 효과 유효기간
        Expiration _expiration = Expiration.Never;
        public Expiration expiration { get { return _expiration; } }

        // 효과 카운트(개수)
        int _count = -1;
        public int count { get { return _count; } }

        // 효과 타겟 플레이어
        Target _target = Target.Self;
        public Target target { get { return _target; } }

        // 효과 대상
        What _what = What.None;
        public What what { get { return _what; } }

        // 효과 값
        int _value = -1;
        public int value { get { return _value; } }



        /// <summary>
        /// 재설정 함수, 별도 사용하지 말것
        /// </summary>
        /// <param name="__expiration"></param>
        /// <param name="__count"></param>
        /// <param name="__target"></param>
        /// <param name="__what"></param>
        /// <param name="__value"></param>
        public void Set(Expiration __expiration, int __count, Target __target, What __what, int __value)
        {
            SetExpiration(__expiration);
            SetCount(__count);
            SetTarget(__target);
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

        void SetWhat(What __what)
        {
            _what = __what;
        }

        void SetValue(int __value)
        {
            _value = __value;
        }
    }


    // 아이템 번호
    int _index = -1;
    public int index { get { return _index; } }

    // 아이템 카테고리
    Type _type = Type.None;
    public Type type { get { return _type; } }

    // 아이템 이름
    string _name = null;
    public string name { get { return _name; } }

    // 아이콘 번호
    int _icon = -1;
    public int icon { get { return _icon; } }

    // 아이템 레어도 (드랍률)
    int _rare = -1;
    public int rare { get { return _rare; } }

    // 아이템 가격
    int _cost = 0;
    public int cost { get { return _cost; } }

    // 럭키박스 획득여부
    bool _isLuckyBoxGet = false;
    public bool isLuckyBoxGet { get { return _isLuckyBoxGet; } }

    // 아이템 정보
    string _info = null;
    public string info { get { return _info; } }

    // 효과
    public ItemEffect effect = new ItemEffect();



    // 생성자
    /// <summary>
    /// 사용 금지
    /// </summary>
    protected Item()
    {
        // 사용 금지
    }
    /// <summary>
    /// 인덱스로 테이블 참조하여 셋팅
    /// </summary>
    /// <param name="__index">테이블 인덱스</param>
    public Item(int __index)
    {
        Set( __index);
    }


    /// <summary>
    /// 재설정 함수, 테이블 읽어서 사용할것
    /// </summary>
    /// <param name="__index"></param>
    public void Set(int __index)
    {
        // 인덱스로 테이블 읽어오기=================
        //Set( __index,  __type,  __name,  __icon,  __rare, __cost,  __isLuckyBoxGet, __expiration,  __count, __target, __what, __value, __info);
    }


    /// <summary>
    /// 재설정 함수, 테이블 읽어서 사용할것
    /// </summary>
    /// <param name="__index"></param>
    /// <param name="__type"></param>
    /// <param name="__name"></param>
    /// <param name="__icon"></param>
    /// <param name="__rare"></param>
    /// <param name="__isLuckyBoxGet"></param>
    /// <param name="__expiration"></param>
    /// <param name="__count"></param>
    /// <param name="__target"></param>
    /// <param name="__what"></param>
    /// <param name="__value"></param>
    public void Set(int __index, Type __type, string __name, int __icon, int __rare, int __cost, bool __isLuckyBoxGet, ItemEffect.Expiration __expiration, int __count, ItemEffect.Target __target, ItemEffect.What __what, int __value, string __info)
    {
        SetIndex(__index);
        SetType(__type);
        SetName(__name);
        SetIcon(__icon);
        SetRare(__rare);
        SetCost(__cost);
        SetLuckyBoxGet(__isLuckyBoxGet);
        effect.Set(__expiration, __count, __target, __what, __value);
        SetInfo(__info);
    }

    void SetIndex(int __index)
    {
        _index = __index;
    }

    void SetType(Type __type)
    {
        _type = __type;
    }

    void SetName(string __name)
    {
        _name = __name;
    }

    void SetIcon(int __icon)
    {
        _icon = __icon;
    }

    void SetRare(int __rare)
    {
        _rare = __rare;
    }

    void SetCost(int __cost)
    {
        _cost = __cost;
    }

    void SetInfo(string __info)
    {
        _info = __info;
    }

    void SetLuckyBoxGet(bool __isLuckyBoxGet)
    {
        _isLuckyBoxGet = __isLuckyBoxGet;
    }
}
