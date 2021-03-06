using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    // 캐릭터 테이블
    static List<Character> _table = new List<Character>();
    public static List<Character> table { get { return _table; } }       // 초기화 안됬으면 초기화 후 반환

    // 캐릭터 테이블 확인용
    static bool _isReady = false;
    public static bool isReady { get { return _isReady; } }



    // 캐릭터 번호
    int _index = -1;
    public int index { get { return _index; } }

    // 캐릭터 이름
    string _name = null;
    public string name { get { return _name; } }

    // 캐릭터 이름
    Job.JobType _job = Job.JobType.None;
    public Job.JobType job { get { return _job; } }

    // 캐릭터 설명
    string _info = null;
    public string info { get { return _info; } }



    // 생성자
    protected Character()
    {
        // 사용 금지
    }

    protected Character(List<string> strList, List<string> loaclList)
    {
        // out of range 방지
        if (strList.Count != 4)
            return;
        if (loaclList.Count != 3)
            return;

        // 테이블 읽어오기
        SetCharacter(
            int.Parse(strList[0]),
            //strList[1],
            loaclList[1],
            (Job.JobType)int.Parse(strList[2]),
            //strList[3]
            loaclList[2].Replace("\\n", "\n")
            );
    }


    /// <summary>
    /// 테이블 생성
    /// </summary>
    public static void SetUp()
    {
        Debug.Log("테이블 셋팅 : 캐릭터");

        // 중복 실행 방지
        if (_isReady)
            return;

        // 테이블 읽어오기
        CSVReader characterReader = new CSVReader(null, "Character.csv");
        CSVReader local = new CSVReader(null, "Character_local.csv", true, false);
        Debug.Log(characterReader.table.Count);

        // 더미 생성
        table.Add(new Character());

        // 테이블로 리스트 셋팅
        for (int i = 1; i < characterReader.table.Count; i++)
        {
            table.Add(new Character(characterReader.table[i], local.table[i]));
        }

        // 준비완료
        _isReady = true;
    }

    /// <summary>
    /// 테이블 인덱스값으로 캐릭터 정보입력
    /// </summary>
    /// <param name="characterIndex">테이블 인덱스 값</param>
    public void SetCharacter(int characterIndex)
    {        
        if (characterIndex < 0)
            SetCharacter(-1, null, Job.JobType.None, null);     // 잘못된 값 초기화
        else
        {
            // 테이블 입출력 기능이 없으므로 보류==========================================
            //SetCharacter(
            //    테이블[characterIndex][0], 
            //    테이블[characterIndex][1], 
            //    (Job.JobType)테이블[characterIndex][2], 
            //    테이블[characterIndex][3]
            //    );     // 캐릭터 설정
        }

    }
    void SetCharacter(int __index, string __name, Job.JobType __job, string __info)
    {
        // 잘못된 값 초기화
        if (__index < 0)
        {
            SetIndex(-1);
            SetName(null);
            SetJob(Job.JobType.None);
            SetInfo(null);
        }

        SetIndex(__index);
        SetName(__name);
        SetJob(__job);
        SetInfo(__info);
    }

    void SetIndex(int __index)
    {
        _index = __index;
    }

    void SetName(string __name)
    {
        _name = __name;
    }

    void SetJob(Job.JobType __job)
    {
        _job = __job;
    }

    void SetInfo(string __info)
    {
        _info = __info;
    }




    public Sprite GetIcon()
    {
        // 아이콘 로드
        Debug.Log(@"Data/Character/Face/Face" + index.ToString("D4"));
        Sprite temp = Resources.Load<Sprite>(@"Data/Character/Face/Face" + index.ToString("D4"));

        // 이미지 유효 검사
        if (temp == null)
        {
            // 기본 아이콘 대체 처리
            Debug.Log(@"UI/playerInfo/player");
            temp = Resources.Load<Sprite>(@"UI/playerInfo/player");
        }

        // 최종 실패 처리
        if (temp == null)
            Debug.Log("로드 실패 :: UI/playerInfo/player");

        return temp;
    }
}
