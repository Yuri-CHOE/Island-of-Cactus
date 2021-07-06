using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class act_Score : MonoBehaviour
{
    public enum Type
    {
        System,
        User,
        AI,
    }

    // �÷��̾� Ÿ��
    Type _type = Type.System;
    public Type type { get { return _type; } }

    // ĳ���� Ŭ���� ����
    Character _character = null;
    public Character character { get { return _character; } }

    // ���� �÷��� ����
    bool _isAutoPlay = false;
    public bool isAutoPlay { get { return _isAutoPlay; } }

    // �÷��̾� �̸�
    string _name = null;
    public string name { get { return _name; } }

    // �÷��̾� ������
    public Sprite face = null;

    //�̴ϰ��� ����
    public int score = 0;

    //�̴ϰ��� ���
    public int rank = 0;

    //�̴ϰ��� ���� ����
    public bool active = false;

    void Awake()
    {
        //���Ӱ������κ��� �÷��̾� ���� ���� �ڵ� �ۼ� �ʿ�
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (active)
        {
            gameObject.SetActive(false);
        }
    }

    public void LoadFace()
    {
        // ������ �ε�
        Debug.Log(@"Data/Character/Face/Face" + character.index.ToString("D4"));
        Sprite temp = Resources.Load<Sprite>(@"Data/Character/Face/Face" + character.index.ToString("D4"));

        // �̹��� ��ȿ �˻�
        if (temp == null)
        {
            // �⺻ ������ ��ü ó��
            Debug.Log(@"UI/playerInfo/player");
            temp = Resources.Load<Sprite>(@"UI/playerInfo/player");
        }

        // ���� ���� ó��
        if (temp == null)
            Debug.Log("�ε� ���� :: UI/playerInfo/player");
        // ������ ����
        else
            face = temp;
    }
}
