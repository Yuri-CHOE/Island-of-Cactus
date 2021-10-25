using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MainGame;

namespace CustomAI
{
    namespace MainGame
    {
        // AI - 주사위
        public class ItemUseAI : AI
        {
            // 주사위 AI용 완료 플래그
            public bool canDice = false;

            // 커트라인
            float _cutline = 0.5f;

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

                // 게임 단계 체크
                if(GameData.gameFlow != GameMaster.Flow.Cycling)
                    return false;

                // 주사위 작동 단계 체크
                if (GameMaster.script.diceController.action != DiceController.DiceAction.Hovering)
                    return false;
                if (GameMaster.script.diceController.actionProgress != ActionProgress.Working)
                    return false;

                // 턴 제어 체크
                if (owner != Turn.now)
                    return false;
                
                // 아이템 수량 체크
                if (owner.inventoryCount <= 0)
                {
                    canDice = true;
                    return false;
                }

                // 작동 허가
                return true;
            }

            public override IEnumerator Work()
            {
                // 작업 수행

                // 스타트 딜레이
                WaitForSeconds waiter = new WaitForSeconds(element.latency.value / 4 + 0.5f);
                yield return waiter;

                // 호츌
                yield return CheckEfficiency(_cutline * element.intelligence.value * 2);

                // 완료 처리
                isDone = true;

                // 완료 딜레이
                //waiter = new WaitForSeconds(element.latency.value / 2);
                yield return waiter;
                canDice = true;
            }

            /// <summary>
            /// 아이템 효율 체크
            /// </summary>
            IEnumerator CheckEfficiency(float cutline)
            {
                // 타겟
                Player userOrTarget = null;

                // 효율 비교 및 아이템 사용
                for (int i = 0; i < owner.inventoryCount; i++)
                {
                    // 효율 비교
                    if (Item.Efficiency(owner.inventory[i].item, owner) >= cutline)
                    {
                        // 개수 차감
                        owner.inventory[i].count--;

                        // 타겟팅
                        if (owner.inventory[i].effect.target == IocEffect.Target.SelectedPlayer)
                            userOrTarget = Item.AutoTargeting(owner.inventory[i].item, owner);
                        // 그외
                        else
                            userOrTarget = owner;

                        // 사용
                        Debug.LogError("AI Run :: item [" + owner.inventory[i].item.name + "]을 사용 -> " + userOrTarget.name);
                        yield return owner.inventory[i].item.Effect(userOrTarget);

                        // 아이템 제거
                        if (owner.inventory[i].count <= 0)
                            owner.RemoveItem(owner.inventory[i]);
                        else
                            Debug.LogError("AI Run :: item [" + owner.inventory[i].item.name + "] 잔여 수량-> " + owner.inventory[i].count);
                    }
                }

                {
                    /*
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
                    */
                }
            }
        }


    }
}