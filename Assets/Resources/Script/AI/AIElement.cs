using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 0 ~ 1사이의 정확도, 반응 지연, 반응 가능 여부 
/// </summary>
public struct AIElement
{
    public struct AIProperty
    {
        // 기본값
        public float basic;

        // 오차값
        public float range;


        // 최종값
        public float value { get { return basic + (range * Random.Range(-1.00f, 1.00f)); } }


        /// <summary>
        /// AI 속성 생성 
        /// </summary>
        /// <param name="_valueBasic">음수값 사용 불가능</param>
        /// <param name="_errorRange">음수값 사용 불가능</param>
        public AIProperty(float _valueBasic, float _errorRange)
        {
            basic = _valueBasic;
            if (_valueBasic < 0)
                basic = 0f;

            range = Mathf.Abs(_errorRange);
        }

        // 기본값
        public static AIProperty intelligence { get { return new AIProperty(0.5f, 0.05f); } }
        public static AIProperty Intelligence(float _valueBasic)
        {
            AIProperty prop = AIProperty.intelligence;

            if (_valueBasic >= 1.0f)
                prop.basic = 1.0f;

            else if (_valueBasic < 0.0f)
                prop.basic = 0.0f;

            else
                prop.basic = _valueBasic;

            return prop;
        }

        public static AIProperty latency { get { return new AIProperty(2f, 1.5f); } }
        public static AIProperty Latency(float _valueBasic)
        {
            AIProperty prop = AIProperty.latency;

            if (_valueBasic < 0.0f)
                prop.basic = 0.0f;

            else
                prop.basic = _valueBasic;

            return prop;
        }
    }

    // 정확도 0 ~ 1 (완벽한 정도)
    AIProperty _intelligence;
    public AIProperty intelligence { get { return _intelligence; } }
    //public float intelligence { get { return _intelligence.value; } }

    // 반응 지연시간 평균
    AIProperty _latency;
    public AIProperty latency { get { return _latency; } }
    //public float latency { get { return _latency.value; } }

    // 반응 지연시간 카운터
    float _elapsedTime;
    public float elapsedTime { get { return (_elapsedTime); } }
    bool _isTime;
    public bool isTime { get { if (!_isTime) _isTime = (elapsedTime >= latency.value); return _isTime; } }




    //생성자
    public AIElement(AIProperty __intelligence, AIProperty __latency)
    {
        _intelligence = AIProperty.intelligence;
        _latency = AIProperty.latency;

        _elapsedTime = 0f;
        _isTime = false;

        Set(__intelligence, __latency);
    }


    /// <summary>
    /// 기본 생성자 대신 사용할것
    /// </summary>
    /// <returns></returns>
    public static AIElement New()
    {
        AIElement newAI = new AIElement();

        newAI.Reset();

        return newAI;
    }

    /// <summary>
    /// intelligence 재설정 (0.75배 ~ 1.25배)
    /// </summary>
    public AIElement Randomize()
    {
        return Randomize(0.75f, 1.25f);
    }
    /// <summary>
    /// intelligence 랜덤 비율로 재설정
    /// </summary>
    /// <param name="minRatio">최소 비율</param>
    /// <param name="maxRatio">최대 비율</param>
    /// <returns></returns>
    public AIElement Randomize(float minRatio, float maxRatio )
    {
        // 지능값 확보
        float intel = intelligence.value;

        // 최대 및 최소 비율 설정
        float min = intel * minRatio;
        float max = intel * maxRatio;

        // 랜덤화
        _intelligence.basic = Random.Range(min, max);

        return this;
    }

    public void Reset()
    {        
        _intelligence = AIProperty.intelligence;
        _latency = AIProperty.latency;

        _elapsedTime = 0f;
        _isTime = false;
    }

    public void Set(AIProperty __intelligence, AIProperty __latency)
    {
        _intelligence = __intelligence;
        _latency = __latency;
    }



    public bool CheckTime(float time)
    {
        return (time >= elapsedTime);
    }

    public void Aging(float __elapsedTime)
    {
        _elapsedTime += __elapsedTime;
    }
    public void Aging()
    {
        Aging(Time.deltaTime);
    }

    public void AgeSet()
    {
        AgeSet(0f);
    }

    public void AgeSet(float sec)
    {
        _elapsedTime = sec;
    }


    /// <summary>
    /// 선택지 중에서 하나를 선택하며 확률은 모든 선택지가 같음
    /// </summary>
    /// <param name="optionList">선택지</param>
    public static Transform SelectObject(List<Transform> optionList)
    {
        // 오류 차단
        if (optionList == null || optionList.Count < 1)
        {
            Debug.LogError("Error :: 선택지가 0개이거나 없음");
            Debug.Break();
            return null;
        }

        // 선택지 추첨
        int select = Random.Range(0, optionList.Count-1);

        // 선택된 오브젝트 반환
        return optionList[select];
    }
    /// <summary>
    /// 선택지 중에서 하나를 선택하며 지능수치에 따라 정답을 고름
    /// </summary>
    /// <param name="answer">정답</param>
    /// <param name="optionList">선택지</param>
    /// <param name="intelligence">0 ~ 1 -> 0 = 무조건 오답, 1 = 무조건 정답</param>
    public static Transform SelectObject(List<Transform> optionList, Transform answer, float intelligence)
    {
        // 오류 차단
        if (answer == null)
        {
            Debug.LogError("Error :: 정답 미지정");
            Debug.Break();
            return null;
        }
        if (optionList == null || optionList.Count < 1)
        {
            Debug.LogError("Error :: 선택지가 0개이거나 없음");
            Debug.Break();
            return null;
        }

        // 확률 적중 여부
        float myint = Random.Range(0.00f, 1.00f);
        bool hit = (intelligence >= myint);

        // 적중시
        if (hit)
        {
            // 정답 반환
            return answer;
        }
        // 비적중시
        else
        {
            // 선택지에서 정답 제거
            if(optionList.Contains(answer))
                for (int i = 0; i < optionList.Count; i++)
                {
                    if (optionList[i] == answer)
                    {
                        optionList.RemoveAt(i);
                        break;
                    }
                }

            // 선택된 오답 반환
            return SelectObject(optionList);
        }
    }
}
