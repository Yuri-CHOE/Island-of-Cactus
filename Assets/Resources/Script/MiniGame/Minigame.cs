using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame
{
    public struct MinigameReward
    {
        // 전체 보상
        public int total;

        // 등수별 지분률
        public int rank_1;
        public int rank_2;
        public int rank_3;
        public int rank_4;

        public void Set(int _total, int _rank_1, int _rank_2, int _rank_3, int _rank_4)
        {
            total = _total;
            rank_1 = _rank_1;
            rank_2 = _rank_2;
            rank_3 = _rank_3;
            rank_4 = _rank_4;
        }
    }

    // 테이블
    static List<Minigame> _table = new List<Minigame>();
    public static List<Minigame> table { get { return _table; } }


    // 테이블 확인용
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }





    // 미니게임 번호
    int _index = -1;
    public int index { get { return _index; } }

    // 미니게임 이름
    string _name = null;
    public string name { get { return _name; } }

    // 미니게임 번호
    int _sceneNum = -1;
    public int sceneNum { get { return _sceneNum; } }    

    // 미니게임 보상
    MinigameReward _reward = new MinigameReward();
    public MinigameReward reward { get { return _reward; } }

    // 미니게임 정보
    string _info = null;
    public string info { get { return _info; } }



    // 생성 제한
    protected Minigame()
    {

    }
    /// <summary>
    /// 테이블 정보를 입력받아 셋팅
    /// </summary>
    /// <param name="strList">테이블 리스트로 읽기</param>
    protected Minigame(List<string> strList, List<string> loaclList)
    {
        // out of range 방지
        if (strList.Count != 8)
            return;
        if (loaclList.Count != 3)
            return;

        // 테이블 읽어오기

        // 인덱스
        _index = int.Parse(strList[0]);

        // 이름
        _name = loaclList[1];

        // 씬 번호
        _sceneNum = int.Parse(strList[2]);

        // 보상 및 지분률
        reward.Set(
            int.Parse(strList[3]),
            int.Parse(strList[4]),
            int.Parse(strList[5]),
            int.Parse(strList[6]),
            int.Parse(strList[7])
            );

        // 정보
        _info = loaclList[2].Replace("\\n", "\n").Replace("value", strList[8]);
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
        CSVReader miniReader = new CSVReader(null, "Minigame.csv");
        CSVReader local = new CSVReader(null, "Minigame_local.csv", true, false);

        // 더미 생성
        table.Add(new Minigame());

        // 테이블로 리스트 셋팅
        for (int i = 1; i < miniReader.table.Count; i++)
        {
            table.Add(new Minigame(miniReader.table[i], local.table[i]));
        }

        // 준비완료
        _isReady = true;
    }

}
