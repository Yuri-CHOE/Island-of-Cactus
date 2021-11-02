using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MainGame;

namespace CustomAI
{
    namespace MainGame
    {
        // AI - 숏컷
        public class ShortcutAI : AI
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

                // 주사위 작동 단계 체크
                if (Turn.turnAction != Turn.TurnAction.Block)
                    return false;

                // 숏컷 활성화 체크
                if (!ShortcutManager.script.gameObject.activeSelf)
                    return false;
                // 숏컷 상점 고객 체크
                if (ShortcutManager.script.customer == null || ShortcutManager.script.customer != owner)
                    return false;

                // 턴 제어 체크
                if (owner != Turn.now)
                    return false;


                // 작동 허가
                return true;
            }

            public override IEnumerator Work()
            {
                // 스타트 딜레이
                WaitForSeconds waiter = new WaitForSeconds(element.latency.value / 4 + 0.5f);
                yield return waiter;

                if (ShortcutManager.script.canUse)
                    ShortcutManager.script.Use();
                else
                {
                    ShortcutManager.script.Clear();

                    // UI 닫기
                    GameMaster.script.messageBox.PopUp(MessageBox.Type.Close);
                }

            }
        }


    }
}
