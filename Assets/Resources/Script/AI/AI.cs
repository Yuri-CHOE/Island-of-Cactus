using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MainGame;

namespace CustomAI
{
    public abstract class AI
    {
        public Player owner = null;

        // �׼� ����
        public Coroutine coroutine = null;
        public bool isStart = false;
        public bool isStop = false;
        public bool isDone = false;

        // AI ���ΰ�
        public AIElement element = AIElement.New();

        // AI �۵����
        public abstract bool CheckStart();
        public abstract bool CheckReset();
        public abstract IEnumerator Work();

        public void Aging()
        {
            // �ð� ��� ó��
            //if (isStart && !isStop)
            if (!isStop)
            {
                element.Aging();
            }
        }

        /// <summary>
        /// �غ�� �ʱ�ȭ
        /// </summary>
        public void Ready()
        {
            // ����¡ ����
            element.AgeSet(0f);

            // �׼� ���� ����
            isStart = false;
            isStop = false;
            isDone = false;
        }        
    }

    namespace MainGame
    {
        // AI - �ֻ���
        public class DiceAI : AI
        {
            public override bool CheckReset()
            {
                if (isDone)
                    return true;
                else
                    return false;

            }

            public override bool CheckStart()
            {
                // ���� ���� üũ
                if (isStart)
                {
                    if (isDone)
                        return true;
                    else
                        return false;
                }
                
                // �ֻ��� �������� ���� ���
                if (GameMaster.script.diceController.owner != owner)
                    return false;

                // �ֻ��� �۵� �ܰ� üũ
                if(GameMaster.script.diceController.action != DiceController.DiceAction.Hovering)
                    return false;
                if (GameMaster.script.diceController.actionProgress != ActionProgress.Working)
                    return false;


                // �۵� �㰡
                return true;
            }

            public override IEnumerator Work()
            {
                {
                    //// �۾��� ó��
                    //Ready();
                    //isStart = true;

                    //if (coroutine != null)
                    //    StopCoroutine(coroutine);
                }

                // �۾� ����
                return ClickBySec(
                         element.intelligence.value,
                         element.intelligence.value + element.latency.value
                         );
            }

            IEnumerator ClickBySec(float DownSec, float UpSec)
            {
                Debug.LogWarning("AI Rue :: dice -> "+DownSec + " after " + UpSec);

                // ����¡ ����
                element.AgeSet(0f);

                // ���콺 �ٿ� �� ���� �ð���ŭ ���
                while (element.CheckTime(DownSec))
                {
                    yield return null;
                }

                // ���� Ŭ��
                GameMaster.script.diceController.doForceClick = true;

                // 1������ ��ŵ
                yield return null;

                // ����¡ ����
                element.AgeSet(0f);

                // ���콺 �� ���� �ð���ŭ ���
                while ( element.CheckTime(UpSec))
                    yield return null;

                // ���� ���콺 ��
                GameMaster.script.diceController.doForceClick = false;
                GameMaster.script.diceController.doForceClickUp = true;

                // �Ϸ� ó��
                isDone = true;
            }
        }


    }
}