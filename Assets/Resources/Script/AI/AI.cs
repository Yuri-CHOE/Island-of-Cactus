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

    namespace MainGame
    {
        // AI - 주사위
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
                // 시작 여부 체크
                if (isStart)
                {
                    if (isDone)
                        return true;
                    else
                        return false;
                }
                
                // 주사위 소유권이 없을 경우
                if (GameMaster.script.diceController.owner != owner)
                    return false;

                // 주사위 작동 단계 체크
                if(GameMaster.script.diceController.action != DiceController.DiceAction.Hovering)
                    return false;
                if (GameMaster.script.diceController.actionProgress != ActionProgress.Working)
                    return false;


                // 작동 허가
                return true;
            }

            public override IEnumerator Work()
            {
                {
                    //// 작업중 처리
                    //Ready();
                    //isStart = true;

                    //if (coroutine != null)
                    //    StopCoroutine(coroutine);
                }

                // 작업 수행
                return ClickBySec(
                         element.intelligence.value,
                         element.intelligence.value + element.latency.value
                         );
            }

            IEnumerator ClickBySec(float DownSec, float UpSec)
            {
                Debug.LogWarning("AI Rue :: dice -> "+DownSec + " after " + UpSec);

                // 에이징 리셋
                element.AgeSet(0f);

                // 마우스 다운 전 지정 시간만큼 대기
                while (element.CheckTime(DownSec))
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
                while ( element.CheckTime(UpSec))
                    yield return null;

                // 강제 마우스 업
                GameMaster.script.diceController.doForceClick = false;
                GameMaster.script.diceController.doForceClickUp = true;

                // 완료 처리
                isDone = true;
            }
        }


    }
}