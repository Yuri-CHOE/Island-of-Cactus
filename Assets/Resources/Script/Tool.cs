using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tool
{
    public static List<int> RandomNotCross(int minInclusive, int maxExclusive, int requiredCount)
    {
        // �������� ���� ����
        if (minInclusive >= maxExclusive)
            return null;

        // �������� �䱸�� ����
        if (maxExclusive - minInclusive <= requiredCount)
            return null;

        // ���� �� ���̺�
        List<int> table = new List<int>();
        for (int i = minInclusive; i < maxExclusive; i++)
            table.Add(i);

        // ����� �����
        List<int> result = new List<int>();

        // �䱸 ������ŭ ���� ȣ��
        for (int i = 0; i < requiredCount; i++)
        {
            // ���� �� ���̺��� ������ �ε��� ȣ��
            int randIndex = Random.Range(0, table.Count);

            // �ε����� ���� ��
            result.Add(table[randIndex]);

            // �ȵ� �� ����
            table.RemoveAt(randIndex);
        }

        // ����� ���� Ȯ�� => ����ġ ���̽� ���� ������
        //while(result.Count < requiredCount)
        //    result.Add(0);

        return result;
    }

}
