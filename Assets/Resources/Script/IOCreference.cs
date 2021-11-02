using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IocReference
{
    public enum Type
    {
        None,
        Skybox,
        DesertObject,
        Mummy,
        Character,
        Trophy,
        BGM,
        SFX,
        Icon,
    }

    // 테이블
    static List<IocReference> _table = new List<IocReference>();
    public static List<IocReference> table { get { return _table; } }


    // 테이블 확인용
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }




    // 번호
    int _index = -1;
    public int index { get { return _index; } }

    // 카테고리
    Type _type = Type.None;
    public Type type { get { return _type; } }

    // 이름
    string _name = null;
    public string name { get { return _name; } }

    // 링크
    string _url = null;
    public string url { get { return _url; } }



    // 생성자

    /// <summary>
    /// 사용 금지
    /// </summary>
    protected IocReference()
    {
        // 사용 금지
    }
    /// <summary>
    /// 테이블 정보를 입력받아 셋팅
    /// </summary>
    /// <param name="strList">테이블 리스트로 읽기</param>
    protected IocReference(List<string> strList)
    {
        // out of range 방지
        if (strList.Count != 4)
            return;

        // 테이블 읽어오기

        // 인덱스
        _index = int.Parse(strList[0]);

        // 카테고리
        _type = (Type)int.Parse(strList[1]);

        // 이름
        _name = strList[2];

        // 링크
        _url = strList[3]; ;
    }






    public static void SetUp(TextAsset dataAsset)
    {
        Debug.Log("테이블 셋팅 : " + dataAsset.name);

        // 중복 실행 방지
        if (_isReady)
            return;

        // 테이블 읽어오기
        CSVReader reader = new CSVReader(dataAsset);
        Debug.Log(reader.table.Count);

        // 더미 생성
        table.Add(new IocReference());

        // 테이블로 리스트 셋팅
        for (int i = 1; i < reader.table.Count; i++)
        {
            table.Add(new IocReference(reader.table[i]));
        }

        // 준비완료
        _isReady = true;

        Debug.Log("테이블 셋팅 : " + dataAsset.name + " -> 완료 , table.count=" + table.Count);
    }

}
