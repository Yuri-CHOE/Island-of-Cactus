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
        // ����Ʈ �Է� �ȵǸ� �ߴ�
        if (rare.Count == 0)
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
}
