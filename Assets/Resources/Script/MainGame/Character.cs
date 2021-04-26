using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    //static 테이블 table; // 예시

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

    public Character(int characterIndex)
    {
        SetCharacter(characterIndex);
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
    public void SetCharacter(int __index, string __name, Job.JobType __job, string __info)
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

    public void SetIndex(int __index)
    {
        _index = __index;
    }

    public void SetName(string __name)
    {
        _name = __name;
    }

    public void SetJob(Job.JobType __job)
    {
        _job = __job;
    }

    public void SetInfo(string __info)
    {
        _info = __info;
    }
}
