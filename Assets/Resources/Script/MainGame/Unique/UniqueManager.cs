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
    }

    // �����
    public static UniqueManager script = null;

    // ȿ�� �̸�
    [SerializeField]
    Text name = null;

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
        // �޽��� �ڽ� ����
        name.text = Unique.table[(int)index].name;
        info.text = Unique.table[(int)index].info;

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
        yield return StartCoroutine(Effect(currentPlayer, index));


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

        switch (index)
        {
            case Mapcode.world_01_01:
                // �÷��� ��ϰ� ���̳ʽ� ����� ���� ����ġ�� 1 ����
                while (isWork)
                {
                    BlockWork.nomalValue++;
                    isWork = false;

                    yield return null;
                }
                break;
        }

        // ���� ����
        BlockWork.isEnd = true;
    }

}
