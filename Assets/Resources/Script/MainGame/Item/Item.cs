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

    // 아이템 테이블
    static List<Item> _table = new List<Item>();
    public static List<Item> table { get { return _table; } }

    // 아이템 테이블 확인용
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }


    // 공백 아이템
    static string[] emptyItem = { "0", "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "" };
    static string[] emptyItem_local = { "0", "empty", "is empty" };
    public static Item empty = new Item(new List<string>(emptyItem), new List<string>(emptyItem_local));


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
    /// 테이블 정보를 입력받아 셋팅
    /// </summary>
    /// <param name="strList">테이블 리스트로 읽기</param>
    protected Item(List<string> strList, List<string> loaclList)
    {
        // out of range 방지
        if (strList.Count != 13)
            return;
        if (loaclList.Count != 3)
            return;


        // 테이블 읽어오기
        Set(
            int.Parse(strList[0]),
            (Type)int.Parse(strList[1]),
            //strList[2],
            loaclList[1],
            int.Parse(strList[3]),
            int.Parse(strList[4]),
            int.Parse(strList[5]),
            System.Convert.ToBoolean(int.Parse(strList[6])),
            (ItemEffect.Expiration)int.Parse(strList[7]),
            int.Parse(strList[8]),
            (ItemEffect.Target)(int.Parse(strList[9])),
            (ItemEffect.What)(int.Parse(strList[10])),
            int.Parse(strList[11]),
            //strList[12].Replace("value", strList[11])
            loaclList[2].Replace("\\n","\n").Replace("value", strList[11])
        );
    }

    /// <summary>
    /// 테이블 생성
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("테이블 셋팅 : 아이템");

        // 중복 실행 방지
        if (_isReady)
            return;

        // 테이블 읽어오기
        CSVReader itemReader = new CSVReader(null, "Item.csv");
        CSVReader local = new CSVReader(null, "Item_local.csv", true, false);

        // 더미 생성
        table.Add(new Item());

        // 테이블로 리스트 셋팅
        for (int i = 1; i < itemReader.table.Count; i++)
        {
            table.Add(new Item(itemReader.table[i], local.table[i]));
        }

        // 준비완료
        _isReady = true;
    }


    /// <summary>
    /// 재설정 함수, 테이블 읽어서 사용할것
    /// </summary>
    /// <param name="__index"></param>
    /// <param name="__type"></param>
    /// <param name="__name"></param>
    /// <param name="__icon"></param>
    /// <param name="__rare"></param>
    /// <param name="__cost"></param>
    /// <param name="__isLuckyBoxGet"></param>
    /// <param name="__expiration"></param>
    /// <param name="__count"></param>
    /// <param name="__target"></param>
    /// <param name="__what"></param>
    /// <param name="__value"></param>
    /// <param name="__info"></param>
    void Set(int __index, Type __type, string __name, int __icon, int __rare, int __cost, bool __isLuckyBoxGet, ItemEffect.Expiration __expiration, int __count, ItemEffect.Target __target, ItemEffect.What __what, int __value, string __info)
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

    void SetLuckyBoxGet(bool __isLuckyBoxGet)
    {
        _isLuckyBoxGet = __isLuckyBoxGet;
    }

    void SetInfo(string __info)
    {
        _info = __info;
    }


    /// <summary>
    /// 아이템 효과
    /// </summary>
    /// <param name="___index">작동할 아이템</param>
    /// <param name="targetPlayer_Or_null">작동시킨 플레이어</param>
    public static void Effect(Item __item, Player targetPlayer_Or_null)
    {
        switch (__item.index)
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
