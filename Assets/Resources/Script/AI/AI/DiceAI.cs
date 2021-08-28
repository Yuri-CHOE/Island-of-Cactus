using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MainGame;

namespace CustomAI
{
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

                // ������ AI üũ
                if (! ((ItemAI)owner.ai.mainGame.itemUse).canDice)
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
                yield return ClickBySec(
                         element.intelligence.value,
                         element.intelligence.value + element.latency.value
                         );

                // �Ϸ� ó��
                isDone = true;
            }

            IEnumerator ClickBySec(float DownSec, float UpSec)
            {
                Debug.LogWarning("AI Run :: dice -> " + DownSec + " after " + UpSec);

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
            }
        }


    }
}