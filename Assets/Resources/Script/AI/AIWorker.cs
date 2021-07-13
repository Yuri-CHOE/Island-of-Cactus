using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI;
using CustomAI.MainGame;

public class AIWorker : MonoBehaviour
{
    // ������
    public Player owner = null;
    public bool isAuto { get { if (owner == null) return false; else return owner.isAutoPlay; } }

    List<AI> aiList = new List<AI>();

    // ���ΰ��� AI
    public MainGameAI mainGame = new MainGameAI();
    public class MainGameAI
    {
        // �ֻ��� ������
        public AI dice = new DiceAI();
    }

    public void SetUp(Player _owner)
    {
        // ������ ����
        owner = _owner;

        // AI ��� ���
        aiList.Add(mainGame.dice);

        // AI ������ ����
        for (int i = 0; i < aiList.Count; i++)
        {
            aiList[i].owner = _owner;
        }
    }


    void Update()
    {
        if (isAuto)
        {
            // ��� ��ȸ üũ
            for (int i = 0; i < aiList.Count; i++)
            {
                if (aiList[i].isStart)
                {
                    // �۵� ���� üũ �� ����ó��
                    if (aiList[i].CheckReset())
                    {
                        aiList[i].Ready();
                    }

                    // �ð� ���
                    aiList[i].Aging();
                }
                // �۵� üũ �� �۵�
                else if (aiList[i].CheckStart())
                {
                    // �۾� �ʱ�ȭ
                    aiList[i].Ready();

                    // �۾� ���� ó��
                    aiList[i].isStart = true;

                    // �۾� ����
                    aiList[i].coroutine = StartCoroutine(aiList[i].Work());

                }
            }
        }
    }

    

}