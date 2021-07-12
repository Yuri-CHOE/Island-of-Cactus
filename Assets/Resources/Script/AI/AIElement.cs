using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 0 ~ 1������ ��Ȯ��, ���� ����, ���� ���� ���� 
/// </summary>
public struct AIElement
{
    public struct AIProperty
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
        public AIProperty(float _valueBasic, float _errorRange)
        {
            basic = _valueBasic;
            if (_valueBasic < 0)
                basic = 0f;

            range = Mathf.Abs(_errorRange);
        }

        // �⺻��
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

    // ��Ȯ�� 0 ~ 1 (�Ϻ��� ����)
    AIProperty _intelligence;
    public AIProperty intelligence { get { return _intelligence; } }
    //public float intelligence { get { return _intelligence.value; } }

    // ���� �����ð� ���
    AIProperty _latency;
    public AIProperty latency { get { return _latency; } }
    //public float latency { get { return _latency.value; } }

    // ���� �����ð� ī����
    float _elapsedTime;
    public float elapsedTime { get { return (_elapsedTime); } }
    bool _isTime;
    public bool isTime { get { if (!_isTime) _isTime = (elapsedTime >= latency.value); return _isTime; } }




    //������
    public AIElement(AIProperty __intelligence, AIProperty __latency)
    {
        _intelligence = AIProperty.intelligence;
        _latency = AIProperty.latency;

        _elapsedTime = 0f;
        _isTime = false;

        Set(__intelligence, __latency);
    }


    /// <summary>
    /// �⺻ ������ ��� ����Ұ�
    /// </summary>
    /// <returns></returns>
    public static AIElement New()
    {
        AIElement newAI = new AIElement();

        newAI.Reset();

        return newAI;
    }

    /// <summary>
    /// intelligence �缳�� (0.75�� ~ 1.25��)
    /// </summary>
    public AIElement Randomize()
    {
        return Randomize(0.75f, 1.25f);
    }
    /// <summary>
    /// intelligence ���� ������ �缳��
    /// </summary>
    /// <param name="minRatio">�ּ� ����</param>
    /// <param name="maxRatio">�ִ� ����</param>
    /// <returns></returns>
    public AIElement Randomize(float minRatio, float maxRatio )
    {
        // ���ɰ� Ȯ��
        float intel = intelligence.value;

        // �ִ� �� �ּ� ���� ����
        float min = intel * minRatio;
        float max = intel * maxRatio;

        // ����ȭ
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
