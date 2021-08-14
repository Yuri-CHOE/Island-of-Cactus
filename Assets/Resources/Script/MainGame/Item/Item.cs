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


    // 아이템 테이블
    static List<Item> _table = new List<Item>();
    public static List<Item> table { get { return _table; } }

    // 아이템 테이블
    static List<Item> _tableLuckyDrop = new List<Item>();
    public static List<Item> tableLuckyDrop { get { return _tableLuckyDrop; } }

    // 아이템 테이블 확인용
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }


    // 공백 아이템
    static string[] emptyItem = { "0", "0", "", "0", "0", "0", "0", "0", "0", "0", "", "0", "0", "0" };
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

    // 아이템 정보
    string _info = null;
    public string info { get { return _info; } }

    // 효과
    public IocEffect effect = new IocEffect();

    // 아이템 레어도 (드랍률)
    int _rare = -1;
    public int rare { get { return _rare; } }

    // 아이템 가격
    int _cost = 0;
    public int cost { get { return _cost; } }

    // 럭키박스 획득여부
    bool _isLuckyBoxGet = false;
    public bool isLuckyBoxGet { get { return _isLuckyBoxGet; } }



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
        if (strList.Count != 14)
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

            (IocEffect.Expiration)(int.Parse(strList[4])),
            int.Parse(strList[5]),
            (IocEffect.Target)(int.Parse(strList[6])),
            int.Parse(strList[7]),
            (IocEffect.What)(int.Parse(strList[8])),
            int.Parse(strList[9]),
            //strList[12].Replace("value", strList[11])
            loaclList[2].Replace("\\n","\n").Replace("value", strList[9]),

            int.Parse(strList[11]),
            int.Parse(strList[12]),
            System.Convert.ToBoolean(int.Parse(strList[13]))
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
            Item current = new Item(itemReader.table[i], local.table[i]);
            table.Add(current);

            // 럭키박스 등장 아이템 테이블
            if (current.isLuckyBoxGet)
                tableLuckyDrop.Add(current);
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
    void Set(int __index, Type __type, string __name, int __icon, IocEffect.Expiration __expiration, int __count, IocEffect.Target __target, int __where, IocEffect.What __what, int __value, string __info, int __rare, int __cost, bool __isLuckyBoxGet)
    {
        SetIndex(__index);
        SetType(__type);
        SetName(__name);
        SetIcon(__icon);
        effect.Set(__expiration, __count, __target, __where, __what, __value);
        SetInfo(__info);
        SetRare(__rare);
        SetCost(__cost);
        SetLuckyBoxGet(__isLuckyBoxGet);
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
    /// 아이콘 로드
    /// </summary>
    public Sprite GetIcon()
    {
        // 아이콘 로드
        Debug.Log(@"Data/Item/icon/item" + index.ToString("D4"));
        Sprite temp = Resources.Load<Sprite>(@"Data/Item/icon/item" + index.ToString("D4"));

        // 이미지 유효 검사
        if (temp == null)
        {
            // 기본 아이콘 대체 처리
            Debug.Log(@"Data/Item/icon/item0000");
            temp = Resources.Load<Sprite>(@"Data/Item/icon/item0000");
        }

        //// 최종 실패 처리
        //if (temp == null)
        //    Debug.Log("로드 실패 :: Data/Item/icon/item0000");
        //// 아이콘 리턴
        //else
        //    _icon.sprite = temp;


        // 아이콘 리턴
        if (temp != null)
            return temp;

        // 최종 실패 처리
        Debug.Log("로드 실패 :: Data/Item/icon/item0000");
        return null;
    }



    /// <summary>
    /// 이벤트 효과
    /// </summary>
    /// <param name="targetPlayer_Or_null">작동시킨 플레이어</param>
    //public static void Effect(Item __item, Player targetPlayer_Or_null)
    public IEnumerator Effect(Player targetPlayer_Or_null)
    {
        // 타겟 리스트
        List<Player> pl = IocEffect.TargetFiltering(effect.target, targetPlayer_Or_null);

        // 통합 효과
        yield return effect.GeneralEffect(targetPlayer_Or_null, pl);

        // 개별 특수 효과
        yield return EachEffect(this, targetPlayer_Or_null, pl);

        // 메인스트림 대기 해제
        GameMaster.useItemOrder = false;
    }

    public static IEnumerator EachEffect(Item __item, Player user, List<Player> filteredTarget)
    {
        switch (__item.index)
        {
            // 필요시 추가

            case 0:
                // 0번은 없음
                break;

            case 19:
                // 실제 효과는 IocEffect.GeneralEffect() 에서 호출됨

                // 이펙트만 나오면 됨 ====================== 미구현
                break;

            case 20:
                // 효과
                for (int i = 0; i < filteredTarget.Count; i++)
                {
                    // 연출
                    // 미구현

                    // 제거
                    filteredTarget[i].RemoveItem(filteredTarget[i].inventory[0]);
                }
                break;

            case 21:
                // 효과

                // 잘못된 인원수 차단
                if (filteredTarget.Count != 0)
                    break;

                // 아이템 없으면 중단
                if (filteredTarget[0].inventoryCount <= 0)
                    break;

                // 가져올 아이템 지정
                ItemSlot slot = filteredTarget[0].inventory[0];

                // 강탈
                filteredTarget[0].RemoveItem(slot);

                // 연출
                // 미구현=============

                // 획득
                user.AddItem(slot, slot.count);

                // 연출
                // 미구현=============

                break;
        }

        yield return null;
    }
}
