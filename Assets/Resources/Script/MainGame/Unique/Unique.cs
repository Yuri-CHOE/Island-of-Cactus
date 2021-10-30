using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unique
{
    // 유니크 테이블
    static List<Unique> _table = new List<Unique>();
    public static List<Unique> table { get { return _table; } }

    // 유니크 테이블 확인용
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }



    // 번호
    int _index = -1;
    public int index { get { return _index; } }

    // 이름
    string _name = null;
    public string name { get { return _name; } }

    // 값
    int _value = -1;
    public int value { get { return _value; } }

    // 정보
    string _info = null;
    public string info { get { return _info; } }



    /// <summary>
    /// 사용 금지
    /// </summary>
    Unique()
    {
        // 사용 금지
    }
    /// <summary>
    /// 테이블 정보를 입력받아 셋팅
    /// </summary>
    /// <param name="strList">테이블 리스트로 읽기</param>
    public Unique(List<string> strList, List<string> loaclList)
    {
        // out of range 방지
        if (strList.Count != 4)
            return;
        if (loaclList.Count != 3)
            return;

        // 테이블 읽어오기
        Set(
            int.Parse(strList[0]),
            loaclList[1],
            int.Parse(strList[2]),
            loaclList[2]
        );

    }


    /// <summary>
    /// 테이블 생성
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("테이블 셋팅 : 유니크");

        // 중복 실행 방지
        if (_isReady)
            return;

        // 테이블 읽어오기
        CSVReader uniqueReader = new CSVReader(null, "Unique.csv");
        CSVReader local = new CSVReader(null, "Unique_local.csv",true, false);

        // 더미 생성
        table.Add(new Unique());

        // 테이블로 리스트 셋팅
        for (int i = 1; i < uniqueReader.table.Count; i++)
        {
            table.Add(new Unique(uniqueReader.table[i], local.table[i]));
        }

        // 준비완료
        _isReady = true;
    }
    public static void SetUp(TextAsset dataAsset, TextAsset localAsset)
    {
        Debug.Log("테이블 셋팅 : " + dataAsset.name);

        // 중복 실행 방지
        if (_isReady)
            return;

        // 테이블 읽어오기
        CSVReader reader = new CSVReader(dataAsset);
        //CSVReader local = new CSVReader(localAsset, true, false);
        CSVReader local = new CSVReader(localAsset);
        Debug.Log(reader.table.Count);

        // 더미 생성
        table.Add(new Unique());

        // 테이블로 리스트 셋팅
        for (int i = 1; i < reader.table.Count; i++)
        {
            table.Add(new Unique(reader.table[i], local.table[i]));
        }

        // 준비완료
        _isReady = true;

        Debug.Log("테이블 셋팅 : " + dataAsset.name + " -> 완료 , table.count=" + table.Count);
    }


    void Set(int __index, string __name, int __value, string __info)
    {
        _index = __index;
        _name = __name;
        _value = __value;
        _info = __info;
    }
}
