using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MainGame;

namespace CustomAI
{
    public abstract class AI
    {
        public Player owner = null;

        // 액션 제어
        public Coroutine coroutine = null;
        public bool isStart = false;
        public bool isStop = false;
        public bool isDone = false;

        // AI 세부값
        public AIElement element = AIElement.New();

        // AI 작동방식
        public abstract bool CheckStart();
        public abstract bool CheckReset();
        public abstract IEnumerator Work();

        public void Aging()
        {
            // 시간 경과 처리
            //if (isStart && !isStop)
            if (!isStop)
            {
                element.Aging();
            }
        }

        /// <summary>
        /// 준비용 초기화
        /// </summary>
        public void Ready()
        {
            // 에이징 리셋
            element.AgeSet(0f);

            // 액션 제어 리셋
            isStart = false;
            isStop = false;
            isDone = false;
        }        
    }
}