using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MainGame;

namespace CustomAI
{
    namespace MainGame
    {
        // AI - ������ ����
        public class ItemBuyAI : AI
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

                //// �ֻ��� �۵� �ܰ� üũ
                //if(GameMaster.script.diceController.action != DiceController.DiceAction.Hovering)
                //    return false;
                //if (GameMaster.script.diceController.actionProgress != ActionProgress.Working)
                //    return false;

                // �ֻ��� �۵� �ܰ� üũ
                if (Turn.turnAction != Turn.TurnAction.Block)
                    return false;

                // ������ ���� Ȱ��ȭ üũ
                if (!ItemShop.script.gameObject.activeSelf)
                    return false;
                // ������ ���� �� üũ
                if (ItemShop.script.customer == null  ||  ItemShop.script.customer != owner)
                    return false;

                // �� ���� üũ
                if (owner != Turn.now)
                    return false;


                // �۵� �㰡
                return true;
            }

            public override IEnumerator Work()
            {
                // �۾� ����
                Debug.Log("AI Run :: ItemBuy");

                // ��ŸƮ ������
                WaitForSeconds waiter = new WaitForSeconds(element.latency.value / 4 + 0.5f);
                yield return waiter;

                // ȣ��
                yield return CheckBundle(Player.inventoryMax - owner.inventoryCount);

                // ���� ����
                ItemShop.script.Buy();

                // �Ϸ� ó��
                isDone = true;
            }

            /// <summary>
            /// ������ ȿ�� üũ
            /// </summary>
            IEnumerator CheckBundle(int buyCount)
            {
                // ��ŸƮ ������
                WaitForSeconds waiter = new WaitForSeconds(element.latency.value / 4 + 0.25f);
                yield return waiter;

                // ���� �ܰ�
                int _coin = owner.coin.Value;

                // ���� ���� - ��
                if (owner.ai.risk == AIWorker.Risk.Low)
                {
                    // ��հ�
                    while (buyCount > 0)
                    {
                        int bill = SelectRare(_coin / buyCount);
                        Debug.Log("AI :: ������ ���� -> ���� ���� = " + bill);

                        if (bill == 0)
                            buyCount = 0;
                        else
                        {
                            _coin -= bill;
                            buyCount--;
                        }

                        yield return waiter;
                    }
                }
                // ���� ���� - ��
                else if (owner.ai.risk == AIWorker.Risk.Middle)
                {
                    // �ְ� 1ȸ
                    {
                        int bill = SelectRare(_coin / buyCount);
                        Debug.Log("AI :: ������ ���� -> ���� ���� = " + bill);

                        if (bill == 0)
                            buyCount = 0;
                        else
                        {
                            _coin -= bill;
                            buyCount--;
                        }

                        yield return waiter;
                    }
                    // ��հ�
                    while (buyCount > 0)
                    {
                        int bill = SelectRare(_coin / buyCount);
                        Debug.Log("AI :: ������ ���� -> ���� ���� = " + bill);

                        if (bill == 0)
                            buyCount = 0;
                        else
                        {
                            _coin -= bill;
                            buyCount--;
                        }

                        yield return waiter;
                    }
                }
                // ���� ���� - ��
                else if (owner.ai.risk == AIWorker.Risk.High)
                {
                    // �ְ�
                    while(buyCount > 0)
                    {
                        int bill = SelectRare(_coin);
                        Debug.Log("AI :: ������ ���� -> ���� ���� " + bill);

                        if (bill == 0)
                            buyCount = 0;
                        else
                        {
                            _coin -= bill;
                            buyCount--;
                        }

                        yield return waiter;
                    }
                }
            }

            int SelectRare(int _coin)
            {
                Debug.Log("AI :: ������ ���� -> ����ǰ�� ���� = " + _coin);

                int indexer = -1;

                for (int i = 0; i < ItemShop.script.bundle.Count; i++)
                    if (ItemShop.script.bundle[i].canBuy)
                        if (!ItemShop.script.bundle[i].toggle.isOn)
                            if (ItemShop.script.bundle[i].priceValue <= _coin)
                            {
                                if (indexer == -1 || ItemShop.script.bundle[i].priceValue > ItemShop.script.bundle[indexer].priceValue)
                                    indexer = i;
                            }

                if (indexer >= 0)
                {
                    ItemShop.script.bundle[indexer].Select();
                    return ItemShop.script.bundle[indexer].priceValue;
                }
                else
                    return 0;
            }
        }


    }
}