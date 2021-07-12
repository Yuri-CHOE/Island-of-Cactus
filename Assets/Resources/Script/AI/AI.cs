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
        Coroutine coroutine = null;
        public bool isStart = false;
        public bool isStop = false;
        public bool isDone = false;

        // AI 세부값
        public AIElement element = AIElement.New();

        // AI 작동방식
        public abstract void Work();

        void Update()
        {
            // 시간 경과 처리
            if (isStart && !isStop)
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

    namespace MainGame
    {
        // AI - 주사위
        public class DiceAI : AI
        {
            public override void Work()
            {
                // 작업중 처리
                Ready();
                isStart = true;

                //if (coroutine != null)
                //    StopCoroutine(coroutine);

                // 작업 수행
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
                // 에이징 리셋
                element.AgeSet(0f);

                // 마우스 다운 전 지정 시간만큼 대기
                while (!element.CheckTime(DownSec))
                {
                    yield return null;
                }

                // 강제 클릭
                GameMaster.script.diceController.doForceClick = true;

                // 1프레임 스킵
                yield return null;

                // 에이징 리셋
                element.AgeSet(0f);

                // 마우스 업 지정 시간만큼 대기
                while ( !element.CheckTime(UpSec))
                {
                    yield return null;
                }

                // 강제 마우스 업
                GameMaster.script.diceController.doForceClick = false;
                GameMaster.script.diceController.doForceClickUp = true;

                // 완료 처리
                isDone = true;
            }
        }


    }
}