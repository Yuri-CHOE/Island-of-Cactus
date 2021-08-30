using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DropTable
{
    // ���(�����) ����Ʈ
    public List<int> rare;

    

    public int totalRare { get { int sum = 0; for (int i = 0; i < rare.Count; i++) sum += rare[i]; return sum; } }

    public int Drop()
    {
        if (rare == null)
            return -1;

        if (rare.Count > 0)
        {
            // ���� �� ��������
            int randomValue = Random.Range(0, totalRare);

            Debug.LogWarning("��� ���̺�(�� "+ rare.Count + ") :: 0 ~ " + totalRare + " �� ���õ� -> " + randomValue);

            int result = 0;
            for (int i = 0; i < rare.Count; i++)
            {
                // ���� �ߴ�
                if (randomValue < rare[i])
                    break;

                // ���� �� ����
                randomValue -= rare[i];

                // ��� �ε��� ��������
                result++;
            }

            return result;

        }
        else
            return -1;
    }

    public List<int> Drop(int count)
    {
        // �����
        List<int> result = new List<int>();

        // ����Ʈ �Է� �ȵǸ� �ߴ�
        if (rare.Count == 0)
            return result;


        // �ε��� ����Ʈ �ʱ�ȭ
        List<int> index = new List<int>();

        // �ε��� ����Ʈ ����
        for (int i = 0; i < rare.Count; i++)
            index.Add(i);

        // �ݺ� ����
        for (int i = 0; i < count; i++)
        {
            // ������ �ε���
            int select = Drop();

            // ������ �ε��� Ȯ��
            result.Add(index[select]);

            // ���� ó��
            //rare.RemoveAt(select);
            index.RemoveAt(select);
        }

        return result;
    }
}
