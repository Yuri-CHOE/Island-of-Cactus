using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 0 ~ 1사이의 정확도, 반응 지연, 반응 가능 여부 
/// </summary>
public struct AIProperty
{
    public struct AIElement
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
        public AIElement(float _valueBasic, float _errorRange)
        {
            basic = _valueBasic;
            if (_valueBasic < 0)
                basic = 0f;

            range = Mathf.Abs(_errorRange);
        }

        // 기본값
        public static AIElement intelligence { get { return new AIElement(0.5f, 0.05f); } }
        public static AIElement Intelligence(float _valueBasic)
        {
            AIElement prop = AIElement.intelligence;

            if (_valueBasic >= 1.0f)
                prop.basic = 1.0f;

            else if (_valueBasic < 0.0f)
                prop.basic = 0.0f;

            else
                prop.basic = _valueBasic;

            return prop;
        }

        public static AIElement latency { get { return new AIElement(2f, 1.5f); } }
        public static AIElement Latency(float _valueBasic)
        {
            AIElement prop = AIElement.latency;

            if (_valueBasic < 0.0f)
                prop.basic = 0.0f;

            else
                prop.basic = _valueBasic;

            return prop;
        }
    }

    // 정확도 0 ~ 1 (완벽한 정도)
    AIElement _intelligence;
    public AIElement intelligenceElement { get { return _intelligence; } }
    public float intelligence { get { return _intelligence.value; } }

    // 반응 지연시간 평균
    AIElement _latency;
    public AIElement latencyElement { get { return _latency; } }
    public float latency { get { return _latency.value; } }

    // 반응 지연시간 카운터
    float _elapsedTime;
    public float elapsedTime { get { return (_elapsedTime); } }
    bool _isTime;
    public bool isTime { get { if (!_isTime) _isTime = (elapsedTime >= latency); return _isTime; } }




    //생성자
    public AIProperty(AIElement __intelligence, AIElement __latency)
    {
        _intelligence = AIElement.intelligence;
        _latency = AIElement.latency;

        _elapsedTime = 0f;
        _isTime = false;

        Set(__intelligence, __latency);
    }


    /// <summary>
    /// 기본 생성자 대신 사용할것
    /// </summary>
    /// <returns></returns>
    public static AIProperty New()
    {
        AIProperty newAI = new AIProperty();

        newAI.Reset();

        return newAI;
    }

    public void Reset()
    {        
        _intelligence = AIElement.intelligence;
        _latency = AIElement.latency;

        _elapsedTime = 0f;
        _isTime = false;
    }

    public void Set(AIElement __intelligence, AIElement __latency)
    {
        _intelligence = __intelligence;
        _latency = __latency;
    }


    public void Aging(float __elapsedTime)
    {
        _elapsedTime += __elapsedTime;
    }
    public void Aging()
    {
        Aging(Time.deltaTime);
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
