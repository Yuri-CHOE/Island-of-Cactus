using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MainGame;

namespace CustomAI
{
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

                // 아이템 AI 체크
                if (! ((ItemAI)owner.ai.mainGame.itemUse).canDice)
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
                yield return ClickBySec(
                         element.intelligence.value,
                         element.intelligence.value + element.latency.value
                         );

                // 완료 처리
                isDone = true;
            }

            IEnumerator ClickBySec(float DownSec, float UpSec)
            {
                Debug.LogWarning("AI Run :: dice -> " + DownSec + " after " + UpSec);

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
            }
        }


    }
}