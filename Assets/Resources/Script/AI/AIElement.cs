using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 0 ~ 1������ ��Ȯ��, ���� ����, ���� ���� ���� 
/// </summary>
public struct AIProperty
{
    public struct AIElement
    {
        // �⺻��
        public float basic;

        // ������
        public float range;


        // ������
        public float value { get { return basic + (range * Random.Range(-1.00f, 1.00f)); } }


        /// <summary>
        /// AI �Ӽ� ���� 
        /// </summary>
        /// <param name="_valueBasic">������ ��� �Ұ���</param>
        /// <param name="_errorRange">������ ��� �Ұ���</param>
        public AIElement(float _valueBasic, float _errorRange)
        {
            basic = _valueBasic;
            if (_valueBasic < 0)
                basic = 0f;

            range = Mathf.Abs(_errorRange);
        }

        // �⺻��
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

    // ��Ȯ�� 0 ~ 1 (�Ϻ��� ����)
    AIElement _intelligence;
    public AIElement intelligenceElement { get { return _intelligence; } }
    public float intelligence { get { return _intelligence.value; } }

    // ���� �����ð� ���
    AIElement _latency;
    public AIElement latencyElement { get { return _latency; } }
    public float latency { get { return _latency.value; } }

    // ���� �����ð� ī����
    float _elapsedTime;
    public float elapsedTime { get { return (_elapsedTime); } }
    bool _isTime;
    public bool isTime { get { if (!_isTime) _isTime = (elapsedTime >= latency); return _isTime; } }




    //������
    public AIProperty(AIElement __intelligence, AIElement __latency)
    {
        _intelligence = AIElement.intelligence;
        _latency = AIElement.latency;

        _elapsedTime = 0f;
        _isTime = false;

        Set(__intelligence, __latency);
    }


    /// <summary>
    /// �⺻ ������ ��� ����Ұ�
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
    /// ������ �߿��� �ϳ��� �����ϸ� Ȯ���� ��� �������� ����
    /// </summary>
    /// <param name="optionList">������</param>
    public static Transform SelectObject(List<Transform> optionList)
    {
        // ���� ����
        if (optionList == null || optionList.Count < 1)
        {
            Debug.LogError("Error :: �������� 0���̰ų� ����");
            Debug.Break();
            return null;
        }

        // ������ ��÷
        int select = Random.Range(0, optionList.Count-1);

        // ���õ� ������Ʈ ��ȯ
        return optionList[select];
    }
    /// <summary>
    /// ������ �߿��� �ϳ��� �����ϸ� ���ɼ�ġ�� ���� ������ ��
    /// </summary>
    /// <param name="answer">����</param>
    /// <param name="optionList">������</param>
    /// <param name="intelligence">0 ~ 1 -> 0 = ������ ����, 1 = ������ ����</param>
    public static Transform SelectObject(List<Transform> optionList, Transform answer, float intelligence)
    {
        // ���� ����
        if (answer == null)
        {
            Debug.LogError("Error :: ���� ������");
            Debug.Break();
            return null;
        }
        if (optionList == null || optionList.Count < 1)
        {
            Debug.LogError("Error :: �������� 0���̰ų� ����");
            Debug.Break();
            return null;
        }

        // Ȯ�� ���� ����
        float myint = Random.Range(0.00f, 1.00f);
        bool hit = (intelligence >= myint);

        // ���߽�
        if (hit)
        {
            // ���� ��ȯ
            return answer;
        }
        // �����߽�
        else
        {
            // ���������� ���� ����
            if(optionList.Contains(answer))
                for (int i = 0; i < optionList.Count; i++)
                {
                    if (optionList[i] == answer)
                    {
                        optionList.RemoveAt(i);
                        break;
                    }
                }

            // ���õ� ���� ��ȯ
            return SelectObject(optionList);
        }
    }
}
