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
            Debug.LogError(_total);
        }

        //public int Share(int value)
        //{
        //    return total * value / (rank_1 + rank_2 + rank_3 + rank_4);
        //}

        public int GetRank(int rank)
        {
            switch (rank)
            {
                case 1:
                    Debug.LogWarning("미니게임 :: 보상 지분 -> " + rank + "등 = 포인트 " + rank_1);
                    return rank_1;
                case 2:
                    Debug.LogWarning("미니게임 :: 보상 지분 -> " + rank + "등 = 포인트 " + rank_2);
                    return rank_2;
                case 3:
                    Debug.LogWarning("미니게임 :: 보상 지분 -> " + rank + "등 = 포인트 " + rank_3);
                    return rank_3;
                case 4:
                    Debug.LogWarning("미니게임 :: 보상 지분 -> " + rank + "등 = 포인트 " + rank_4);
                    return rank_4;
                default:
                    return 0;
            }
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

    // 최소 참여자 수
    int _playerMin = -1;
    public int playerMin { get { return _playerMin; } }

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
        if (strList.Count != 10)
            return;
        if (loaclList.Count != 3)
            return;
        
        // 테이블 읽어오기

        // 인덱스
        _index = int.Parse(strList[0]);

        // 이름
        _name = loaclList[1];

        // 최소 참여자 수
        _playerMin = int.Parse(strList[2]);

        // 씬 번호
        _sceneNum = int.Parse(strList[3]);

        // 보상 및 지분률
        _reward.Set(
            int.Parse(strList[4]),
            int.Parse(strList[5]),
            int.Parse(strList[6]),
            int.Parse(strList[7]),
            int.Parse(strList[8])
            );

        // 정보
        _info = loaclList[2].Replace("\\n", "\n");
    }



    /// <summary>
    /// 테이블 생성
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("테이블 셋팅 : 미니게임");

        // 중복 실행 방지
        if (_isReady)
            return;

        // 테이블 읽어오기
        CSVReader miniReader = new CSVReader(null, "Minigame.csv");
        CSVReader local = new CSVReader(null, "Minigame_local.csv", true, false);

        // 더미 생성
        table.Add(new Minigame());

        // 테이블로 리스트 셋팅
        Minigame current = null;
        for (int i = 1; i < miniReader.table.Count; i++)
        {
            current = new Minigame(miniReader.table[i], local.table[i]);
            table.Add(current);
        }

        // 준비완료
        _isReady = true;
    }


    public static Minigame RandomGame()
    {
        return RandomGame(1);
    }

    public static Minigame RandomGame(int entryCount)
    {
        // 테이블 누락 방지
        if (!isReady)
            return null;


        // 랜덤 값
        int indexer = Random.Range(1, table.Count);
        while (table[indexer].playerMin < entryCount)
            indexer = Random.Range(1, table.Count);

        Debug.LogWarning("미니게임 :: 랜덤 인덱스 -> " + indexer);

        return table[indexer];
    }
}
