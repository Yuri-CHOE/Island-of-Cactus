using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MainGame;

namespace CustomAI
{
    namespace MainGame
    {
        // AI - ����
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
                // ���� ���� üũ
                if (isStart)
                {
                    if (isDone)
                        return true;
                    else
                        return false;
                }

                // �ֻ��� �۵� �ܰ� üũ
                if (Turn.turnAction != Turn.TurnAction.Block)
                    return false;

                // ���� Ȱ��ȭ üũ
                if (!ShortcutManager.script.gameObject.activeSelf)
                    return false;
                // ���� ���� �� üũ
                if (ShortcutManager.script.customer == null || ShortcutManager.script.customer != owner)
                    return false;

                // �� ���� üũ
                if (owner != Turn.now)
                    return false;


                // �۵� �㰡
                return true;
            }

            public override IEnumerator Work()
            {
                // ��ŸƮ ������
                WaitForSeconds waiter = new WaitForSeconds(element.latency.value / 4 + 0.5f);
                yield return waiter;

                if (ShortcutManager.script.canUse)
                    ShortcutManager.script.Use();
                else
                {
                    ShortcutManager.script.Clear();

                    // UI �ݱ�
                    GameMaster.script.messageBox.PopUp(MessageBox.Type.Close);
                }

            }
        }


    }
}
