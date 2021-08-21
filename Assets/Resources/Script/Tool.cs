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
        // ������ ���ȭ
        Quaternion rot = Quaternion.Euler(
            obj.rotation.eulerAngles.x + 360f,
            obj.rotation.eulerAngles.y + 360f,
            obj.rotation.eulerAngles.z + 360f
            );

        Quaternion spin = Quaternion.Euler(
            rot.eulerAngles.x + Random.Range(144, 170),
            rot.eulerAngles.y + Random.Range(144, 170),
            rot.eulerAngles.z + Random.Range(144, 170)
            );

        // ȸ���� ��� (���� ����)
        obj.rotation = Quaternion.Lerp(rot, spin, Time.deltaTime * speed);

        ////Quaternion spin = Quaternion.Euler(
        //    obj.rotation.eulerAngles.x + Random.Range(144, 170),
        //    obj.rotation.eulerAngles.y + Random.Range(144, 170),
        //    obj.rotation.eulerAngles.z + Random.Range(144, 170)
        //    );

        //// ȸ���� ��� (���� ����)
        //obj.rotation = Quaternion.Lerp(obj.rotation, spin, Time.deltaTime * speed);
    }
    public static void SpinX(Transform obj, float speed)
    {
        // ������ ���ȭ
        Quaternion rot = Quaternion.Euler(
            obj.rotation.eulerAngles.x + 360f,
            obj.rotation.eulerAngles.y,
            obj.rotation.eulerAngles.z
            );

        Quaternion spin = Quaternion.Euler(
            rot.eulerAngles.x + Random.Range(144, 170),
            rot.eulerAngles.y,
            rot.eulerAngles.z
            );

        // ȸ���� ��� (���� ����)
        obj.rotation = Quaternion.Lerp(rot, spin, Time.deltaTime * speed);
    }
    public static void SpinY(Transform obj, float speed)
    {
        // ������ ���ȭ
        Quaternion rot = Quaternion.Euler(
            obj.rotation.eulerAngles.x,
            obj.rotation.eulerAngles.y + 360f,
            obj.rotation.eulerAngles.z
            );

        Quaternion spin = Quaternion.Euler(
            rot.eulerAngles.x,
            rot.eulerAngles.y + Random.Range(144, 170),
            rot.eulerAngles.z
            );

        // ȸ���� ��� (���� ����)
        obj.rotation = Quaternion.Lerp(rot, spin, Time.deltaTime * speed);
    }
    public static void SpinZ(Transform obj, float speed)
    {
        // ������ ���ȭ
        Quaternion rot = Quaternion.Euler(
            obj.rotation.eulerAngles.x,
            obj.rotation.eulerAngles.y,
            obj.rotation.eulerAngles.z + 360f
            );

        Quaternion spin = Quaternion.Euler(
            rot.eulerAngles.x,
            rot.eulerAngles.y,
            rot.eulerAngles.z + Random.Range(144, 170)
            );

        // ȸ���� ��� (���� ����)
        obj.rotation = Quaternion.Lerp(rot, spin, Time.deltaTime * speed);
    }



    /// <summary>
    /// �� ����
    /// </summary>
    public static void HeightLimit(Transform target, float min, float max)
    {
        // �ּҰ� ����
        if (target.position.y < min)
        {
            target.position = new Vector3(
                target.position.x,
                min,
                target.position.z
                );
            Debug.Log("�ּҰ� ����");
            Debug.Log(target.position.ToString());
        }

        // �ִ밪 ����
        if (target.position.y > max)
        {
            target.position = new Vector3(
                    target.position.x,
                    max,
                    target.position.z
                    );
            Debug.Log("�ִ밪 ���� :: " + target.position.ToString());
        }
    }


    /// <summary>
    /// ���̵� ó�� - �ڷ�ƾ �ʼ�
    /// </summary>
    /// <param name="canvasGroup">���� ���</param>
    /// <param name="isFadeIn">f=���̵� �ƿ�, t=���̵� ��</param>
    /// <param name="timer">��ǥ �ð�</param>
    /// <returns></returns>
    /// </summary>
    public static IEnumerator CanvasFade(CanvasGroup canvasGroup, bool isFadeIn, float timer)
    {
        // ���� ��� ó��
        if (isFadeIn)
        {
            // Ŭ�� ���� ��� Ȱ��ȭ
            canvasGroup.blocksRaycasts = true;

            while (canvasGroup.alpha < 1f)
            {
                // ���̵� ó��
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.deltaTime / timer); ;

                // ����
                if (canvasGroup.alpha > 0.9999f)
                    canvasGroup.alpha = 1f;

                yield return null;
            }
        }
        else
        {
            // Ŭ�� ���� ��� ��Ȱ��ȭ
            canvasGroup.blocksRaycasts = false;

            while (canvasGroup.alpha > 0f)
            {
                // ���̵� ó��
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, Time.deltaTime / timer); ;

                // ����
                if (canvasGroup.alpha < 0.0001f)
                    canvasGroup.alpha = 0f;

                yield return null;
            }
        }
    }


    /// <summary>
    /// ������Ʈ�� ��ǥ ��ǥ�� ������ ������
    /// BoxCollider, Rigidbody�� �߷� ��� �ʼ�
    /// </summary>
    /// <param name="obj">���� ������Ʈ</param>
    /// <param name="destination">������ ��ǥ</param>
    /// <param name="height">������ ����</param>
    /// <param name="time">���� �ð�</param>
    public static void ThrowParabola(Transform obj, Vector3 destination, float height, float time)
    {
        // �߸��� ������Ʈ �ߴ�
        if (obj == null)
            return;

        // �� ���
        Rigidbody objRigidbody = obj.GetComponent<Rigidbody>();

        // Rigidbody �̻��� �ߴ�
        if (objRigidbody == null)
            return;

        // ĳ���� ��ħ ���� �浹 ����
        obj.GetComponent<Collider>().isTrigger = true;


        // ���� ���� ���

        // �Ÿ� Ȯ��
        Vector3 distance = destination - obj.position;

        // ���� ���ŵ� �Ÿ�
        Vector3 distanceXZ = new Vector3(distance.x, 0, distance.z);

        // �ӵ�
        float spdY = height / time + (Mathf.Abs(Physics.gravity.y) * time / 2f);
        float spdXZ = distanceXZ.magnitude / time;

        // �����
        Vector3 result = distanceXZ.normalized * spdXZ;
        result.y = spdY;


        // ������
        objRigidbody.velocity = result;

        /*
         ���� : https://foo897.tistory.com/24
        */
    }







}
