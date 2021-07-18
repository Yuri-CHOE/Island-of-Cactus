using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    // ��ֹ� ���
    public static List<List<DynamicObject>> objectList = new List<List<DynamicObject>>();

    public enum Type
    {
        None,
        Item,
        Event,
    }

    // ������Ʈ Ÿ��
    public Type type = Type.None;


    // ��ġ (��� �ε���)
    public int location = -2;

    // ����
    public int count = 0;

    // ��� �غ�
    public bool isReady = false;



    public virtual bool CheckCondition(Player current)
    {
        Debug.LogError("error :: ȹ�� ���� üũ ���� -> DynamicObject");

        return false;
    }



    /// <summary>
    /// ��ֹ� ����
    /// </summary>
    public void CreateBarricade()
    {
        // �غ� �̴޽� �ߴ�
        if (!isReady)
        {
            Debug.LogError("error : �¾����� ���� DynamicObject => CreateBarricade()");
            Debug.Break();
            return;
        }

        // ��ȿ���� ���͸�
        int loc = GameData.blockManager.indexLoop(location,0);

        // ��ֹ� ���
        objectList[loc].Add(this);
        Debug.LogWarning(string.Format("�ٸ�����Ʈ :: {0} ĭ�� �߰���(�� ���� => {1})", location, objectList[loc].Count));
    }


    /// <summary>
    /// ��ֹ� ����
    /// </summary>
    public void RemoveBarricade()
    {
        // �غ� �̴޽� �ߴ�
        if (!isReady)
        {
            Debug.LogError("error : �¾����� ���� DynamicObject => CreateBarricade()");
            Debug.Break();
            return;
        }

        // ��ȿ���� ���͸�
        int loc = GameData.blockManager.indexLoop(location, 0);

        // ��ϵ��� ���� ��� �ߴ�
        if (!objectList[loc].Contains(this))
        {
            Debug.LogError("error : ��ϵ��� ���� DynamicObject ���� => RemoveBarricade()");
            Debug.Break();
            return;
        }

        // ��ֹ� ���
        objectList[loc].Remove(this);
    }
}
