using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy
{
    // ���� ������
    public int rich = 0;

    // ���� �̵��Ÿ�
    public int runner = 0;

    // �̴ϰ��� ����
    public int mini = 0;

    // ����
    public int score { get { return /*5 * 3 -*/ (rich + runner + mini); } }

    // ���� ����
    public int final = 0;
}
