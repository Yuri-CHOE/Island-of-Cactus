using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyBox
{
    public enum Type
    {
        None,
        Move,
        WorldEvent,
        MonsterWave,
        MiniGame,
        GetItem,
        StealItem
    }

    // 테이블
    static List<LuckyBox> _table = new List<LuckyBox>();
    public static List<LuckyBox> table { get { return _table; } }


    // 테이블 확인용
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }




    // 럭키박스 번호
    int _index = -1;
    public int index { get { return _index; } }

    // 럭키박스 카테고리
    Type _type = Type.None;
    public Type type { get { return _type; } }

    // 럭키박스 이름
    string _name = null;
    public string name { get { return _name; } }

    // 럭키박스 레어도 (드랍률)
    int _rare = -1;
    public int rare { get { return _rare; } }

    // 효과 타겟 플레이어
    Item.ItemEffect.Target _target = Item.ItemEffect.Target.Self;
    public Item.ItemEffect.Target target { get { return _target; } }

    // 효과 대상
    Item.ItemEffect.What _what = Item.ItemEffect.What.None;
    public Item.ItemEffect.What what { get { return _what; } }

    // 효과 범위
    int _where = -1;
    public int where { get { return _where; } }

    // 효과 값
    int _value = -1;
    public int value { get { return _value; } }

    // 럭키박스 정보
    string _info = null;
    public string info { get { return _info; } }



    // 생성자

    /// <summary>
    /// 사용 금지
    /// </summary>
    protected LuckyBox()
    {
        // 사용 금지
    }
    /// <summary>
    /// 테이블 정보를 입력받아 셋팅
    /// </summary>
    /// <param name="strList">테이블 리스트로 읽기</param>
    protected LuckyBox(List<string> strList, List<string> loaclList)
    {
        // out of range 방지
        if (strList.Count != 9)
            return;
        if (loaclList.Count != 3)
            return;

        // 테이블 읽어오기

        // 인덱스
        _index = int.Parse(strList[0]);

        // 카테고리
        _type = (Type)int.Parse(strList[1]);

        // 이름
        //_name = strList[2];
        _name = loaclList[1];

        // 레어도
        _rare = int.Parse(strList[3]);

        // 타겟(플레이어)
        _target = (Item.ItemEffect.Target)(int.Parse(strList[4]));

        // 대상
        _what = (Item.ItemEffect.What)(int.Parse(strList[5]));

        // 위치
        _where = int.Parse(strList[6]);

        // 값
        _value = int.Parse(strList[7]);

        // 정보
        //_info = strList[8];
        _info = loaclList[2].Replace("\\n", "\n").Replace("value", strList[7]);
    }



    /// <summary>
    /// 테이블 생성
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("테이블 셋팅 : 럭키박스");

        // 중복 실행 방지
        if (_isReady)
            return;

        // 테이블 읽어오기
        CSVReader luckyReader = new CSVReader(null, "LuckyBox.csv");
        CSVReader local = new CSVReader(null, "LuckyBox_local.csv", true, false);

        // 더미 생성
        table.Add(new LuckyBox());

        // 테이블로 리스트 셋팅
        for (int i = 1; i < luckyReader.table.Count; i++)
        {
            table.Add(new LuckyBox(luckyReader.table[i], local.table[i]));
        }

        // 준비완료
        _isReady = true;
    }













    /// <summary>
    /// 럭키박스 효과
    /// </summary>
    /// <param name="___index">작동할 럭키박스 인덱스</param>
    public static void Effect(int __index)
    {
        switch (__index)
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
