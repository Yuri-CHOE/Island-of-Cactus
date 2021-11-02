using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MainGame;

namespace CustomAI
{
    namespace MainGame
    {
        // AI - 아이템 구매
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
                // 시작 여부 체크
                if (isStart)
                {
                    if (isDone)
                        return true;
                    else
                        return false;
                }

                //// 주사위 작동 단계 체크
                //if(GameMaster.script.diceController.action != DiceController.DiceAction.Hovering)
                //    return false;
                //if (GameMaster.script.diceController.actionProgress != ActionProgress.Working)
                //    return false;

                // 주사위 작동 단계 체크
                if (Turn.turnAction != Turn.TurnAction.Block)
                    return false;

                // 아이템 상점 활성화 체크
                if (!ItemShop.script.gameObject.activeSelf)
                    return false;
                // 아이템 상점 고객 체크
                if (ItemShop.script.customer == null  ||  ItemShop.script.customer != owner)
                    return false;

                // 턴 제어 체크
                if (owner != Turn.now)
                    return false;


                // 작동 허가
                return true;
            }

            public override IEnumerator Work()
            {
                // 작업 수행
                Debug.Log("AI Run :: ItemBuy");

                // 스타트 딜레이
                WaitForSeconds waiter = new WaitForSeconds(element.latency.value / 4 + 0.5f);
                yield return waiter;

                // 호츌
                yield return CheckBundle(Player.inventoryMax - owner.inventoryCount);

                // 구매 결정
                ItemShop.script.Buy();

                // 완료 처리
                isDone = true;
            }

            /// <summary>
            /// 아이템 효율 체크
            /// </summary>
            IEnumerator CheckBundle(int buyCount)
            {
                // 스타트 딜레이
                WaitForSeconds waiter = new WaitForSeconds(element.latency.value / 4 + 0.25f);
                yield return waiter;

                // 가상 잔고
                int _coin = owner.coin.Value;

                // 위험 감수 - 저
                if (owner.ai.risk == AIWorker.Risk.Low)
                {
                    // 평균가
                    while (buyCount > 0)
                    {
                        int bill = SelectRare(_coin / buyCount);
                        Debug.Log("AI :: 아이템 구매 -> 선택 가격 = " + bill);

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
                // 위험 감수 - 중
                else if (owner.ai.risk == AIWorker.Risk.Middle)
                {
                    // 최고가 1회
                    {
                        int bill = SelectRare(_coin / buyCount);
                        Debug.Log("AI :: 아이템 구매 -> 선택 가격 = " + bill);

                        if (bill == 0)
                            buyCount = 0;
                        else
                        {
                            _coin -= bill;
                            buyCount--;
                        }

                        yield return waiter;
                    }
                    // 평균가
                    while (buyCount > 0)
                    {
                        int bill = SelectRare(_coin / buyCount);
                        Debug.Log("AI :: 아이템 구매 -> 선택 가격 = " + bill);

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
                // 위험 감수 - 고
                else if (owner.ai.risk == AIWorker.Risk.High)
                {
                    // 최고가
                    while(buyCount > 0)
                    {
                        int bill = SelectRare(_coin);
                        Debug.Log("AI :: 아이템 구매 -> 선택 가격 " + bill);

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
                Debug.Log("AI :: 아이템 구매 -> 단일품목 예산 = " + _coin);

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