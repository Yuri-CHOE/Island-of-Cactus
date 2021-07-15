using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    // ��ġ (��� �ε���)
    public int location = -2;

    // ����
    public int count = 0;

    // ��� �غ�
    public bool isReady = false;


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
        CharacterMover.barricade[loc].Add(this);
        Debug.LogWarning(string.Format("�ٸ�����Ʈ :: {0} ĭ�� �߰���(�� ���� => {1})", location, CharacterMover.barricade[loc].Count));
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
        if (!CharacterMover.barricade[loc].Contains(this))
        {
            Debug.LogError("error : ��ϵ��� ���� DynamicObject ���� => RemoveBarricade()");
            Debug.Break();
            return;
        }

        // ��ֹ� ���
        CharacterMover.barricade[loc].Remove(this);
    }
}
