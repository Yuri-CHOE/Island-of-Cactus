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
}