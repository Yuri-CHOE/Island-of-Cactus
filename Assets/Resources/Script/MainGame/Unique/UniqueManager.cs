using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniqueManager : MonoBehaviour
{
    public enum Mapcode
    {
        none,
        world_01_01 = 1,
        world_01_02 = 2,
        world_01_03 = 3,
    }

    // �����
    public static UniqueManager script = null;

    // ȿ�� �̸�
    [SerializeField]
    Text nameText = null;

    // ȿ�� ����
    [SerializeField]
    Text info = null;

    // �۵� ����
    public  Coroutine coroutineWork = null;
    public  bool isWork = false;


    void Awake()
    {
        // �����
        script = this;
    }


    public  void Active(Player currentPlayer)
    {
        Active(currentPlayer, GameData.worldFileName);
    }
    public  void Active(Player currentPlayer, string worldFileName)
    {
        // ���� ���� �ߴ�
        if (coroutineWork != null)
            StopCoroutine(coroutineWork);

        // ������ ���Է½� ����
        if (worldFileName == null)
        {
            Debug.LogError("Error :: ������ �̸� ����");
            Debug.Break();
            return;
        }

        // ������ �ε���ȭ
        Mapcode index = Mapcode.none;
        Enum.TryParse(worldFileName.Replace(".iocw",""), out index);

        // ��ȿ���� ���� ������ �Է½� ����
        if (index == Mapcode.none)
        {
            Debug.LogError("Error :: �߸��� ������ �̸� -> " + worldFileName);
            Debug.Break();
            return;
        }

        // �۵� ����
        isWork = true;
        coroutineWork = StartCoroutine( Work(currentPlayer, index) );
    }


    IEnumerator Work(Player currentPlayer, Mapcode index)
    {
        int _index = (int)index;

        // �޽��� �ڽ� ����
        nameText.text = Unique.table[_index].name;
        info.text = Unique.table[_index].info;

        // ȿ�� ���� ���
        MessageBox mb = GameData.gameMaster.messageBox;
        mb.PopUp(MessageBox.Type.UniqueBlock);


        // �޽��� �ڽ� Ȯ�� ���
        while (mb.gameObject.activeSelf)
        {
            //Debug.LogWarning("����ũ ��� :: �޽��� �ڽ� Ȯ�� �����");
            yield return null;
        }


        // ���� �� ȿ�� ����
        yield return Effect(currentPlayer, index);


        // �۵� ����
        while (isWork)
        {
            //Debug.LogWarning("����ũ ��� :: �۵� ������");
            yield return null;
        }

    }


    public IEnumerator Effect(Player currentPlayer, Mapcode index)
    {
        isWork = true;

        Unique temp = Unique.table[(int)index];

        switch (index)
        {
            case Mapcode.world_01_01:
                // �÷��� ��ϰ� ���̳ʽ� ����� ���� ����ġ�� 1 ����
                while (isWork)
                {
                    BlockWork.plusBlockPlus += temp.value;
                    BlockWork.minusBlockPlus += temp.value;
                    isWork = false;

                    yield return null;
                }
                break;
            case Mapcode.world_01_02:
                // �÷��� ��ϰ� ���̳ʽ� ����� ���� ����ġ�� 2�� ����
                while (isWork)
                {
                    BlockWork.plusBlockMultiple *= temp.value;
                    BlockWork.minusBlockMultiple *= temp.value;
                    isWork = false;

                    yield return null;
                }
                break;
            case Mapcode.world_01_03:
                // ���� - �߰��� ���̺� ���� �߰��ؾ���
                while (isWork)
                {                    
                    yield return null;
                }
                break;
        }

        // ���� ����
        BlockWork.isEnd = true;
    }

}
