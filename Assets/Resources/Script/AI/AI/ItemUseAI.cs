using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MainGame;

namespace CustomAI
{
    namespace MainGame
    {
        // AI - �ֻ���
        public class ItemUseAI : AI
        {
            // �ֻ��� AI�� �Ϸ� �÷���
            public bool canDice = false;

            // ĿƮ����
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
                // ���� ���� üũ
                if (isStart)
                {
                    if (isDone)
                        return true;
                    else
                        return false;
                }

                // ���� �ܰ� üũ
                if(GameData.gameFlow != GameMaster.Flow.Cycling)
                    return false;

                // �ֻ��� �۵� �ܰ� üũ
                if (GameMaster.script.diceController.action != DiceController.DiceAction.Hovering)
                    return false;
                if (GameMaster.script.diceController.actionProgress != ActionProgress.Working)
                    return false;

                // �� ���� üũ
                if (owner != Turn.now)
                    return false;
                
                // ������ ���� üũ
                if (owner.inventoryCount <= 0)
                {
                    canDice = true;
                    return false;
                }

                // �۵� �㰡
                return true;
            }

            public override IEnumerator Work()
            {
                // �۾� ����

                // ��ŸƮ ������
                WaitForSeconds waiter = new WaitForSeconds(element.latency.value / 4 + 0.5f);
                yield return waiter;

                // ȣ��
                yield return CheckEfficiency(_cutline * element.intelligence.value * 2);

                // �Ϸ� ó��
                isDone = true;

                // �Ϸ� ������
                //waiter = new WaitForSeconds(element.latency.value / 2);
                yield return waiter;
                canDice = true;
            }

            /// <summary>
            /// ������ ȿ�� üũ
            /// </summary>
            IEnumerator CheckEfficiency(float cutline)
            {
                // Ÿ��
                Player userOrTarget = null;

                // ȿ�� �� �� ������ ���
                for (int i = 0; i < owner.inventoryCount; i++)
                {
                    // ȿ�� ��
                    if (Item.Efficiency(owner.inventory[i].item, owner) >= cutline)
                    {
                        // ���� ����
                        owner.inventory[i].count--;

                        // Ÿ����
                        if (owner.inventory[i].effect.target == IocEffect.Target.SelectedPlayer)
                            userOrTarget = Item.AutoTargeting(owner.inventory[i].item, owner);
                        // �׿�
                        else
                            userOrTarget = owner;

                        // ���
                        Debug.LogError("AI Run :: item [" + owner.inventory[i].item.name + "]�� ��� -> " + userOrTarget.name);
                        yield return owner.inventory[i].item.Effect(userOrTarget);

                        // ������ ����
                        if (owner.inventory[i].count <= 0)
                            owner.RemoveItem(owner.inventory[i]);
                        else
                            Debug.LogError("AI Run :: item [" + owner.inventory[i].item.name + "] �ܿ� ����-> " + owner.inventory[i].count);
                    }
                }

                {
                    /*
                Debug.LogWarning("AI Rue :: dice -> "+DownSec + " after " + UpSec);
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

                // �Ϸ� ó��
                isDone = true;
                    */
                }
            }
        }


    }
}