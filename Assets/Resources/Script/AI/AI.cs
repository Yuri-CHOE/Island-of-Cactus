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
        Coroutine coroutine = null;
        public bool isStart = false;
        public bool isStop = false;
        public bool isDone = false;

        // AI ���ΰ�
        public AIElement element = AIElement.New();

        // AI �۵����
        public abstract void Work();

        void Update()
        {
            // �ð� ��� ó��
            if (isStart && !isStop)
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
            public override void Work()
            {
                // �۾��� ó��
                Ready();
                isStart = true;

                //if (coroutine != null)
                //    StopCoroutine(coroutine);

                // �۾� ����
                GameMaster.script.diceController.RunAI(
                    ClickBySec(
                        element.latency.value,
                        element.intelligence.value + element.latency.value
                        )
                    );
                //StartCoroutine(
                //    ClickBySec(
                //        element.latency.value, 
                //        element.intelligence.value + element.latency.value
                //        )
                //    );
            }

            IEnumerator ClickBySec(float DownSec, float UpSec)
            {
                // ����¡ ����
                element.AgeSet(0f);

                // ���콺 �ٿ� �� ���� �ð���ŭ ���
                while (!element.CheckTime(DownSec))
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
                while ( !element.CheckTime(UpSec))
                {
                    yield return null;
                }

                // ���� ���콺 ��
                GameMaster.script.diceController.doForceClick = false;
                GameMaster.script.diceController.doForceClickUp = true;

                // �Ϸ� ó��
                isDone = true;
            }
        }


    }
}