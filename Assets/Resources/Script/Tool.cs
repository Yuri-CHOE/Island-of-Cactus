using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    /// <summary>
    /// ȸ���ӵ� 1�� 3�� ���� ����
    /// </summary>
    /// <param name="obj"></param>
    public static void Spin(Transform obj) { Spin(obj, 1.0f); }
    /// <summary>
    /// 3�� ���� ����
    /// </summary>
    /// <param name="obj"></param>
    public static void Spin(Transform obj, float speed)
    {
        Quaternion spin = Quaternion.Euler(
            obj.rotation.eulerAngles.x + Random.Range(144, 170),
            obj.rotation.eulerAngles.y + Random.Range(144, 170),
            obj.rotation.eulerAngles.z + Random.Range(144, 170)
            );

        // ȸ���� ��� (���� ����)
        obj.rotation = Quaternion.Lerp(obj.rotation, spin, Time.deltaTime * speed);
    }


    /// <summary>
    /// Ŭ���� ������Ʈ ��������
    /// </summary>
    public static GameObject Targeting()
    {
        // Ŭ�� üũ
        if (!Input.GetMouseButtonUp(0))
            return null;

        // UI Ŭ�� ����ó��
        if (EventSystem.current.currentSelectedGameObject != null)
            return null;


        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GameObject clickObj = null;

        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            clickObj = hit.transform.gameObject;
            Debug.Log(clickObj.name);
        }

        return clickObj;
    }


    //public static float SpeedAcceleration()
    //{

    //}
}
