using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EndManager
{
    public enum TrophyType
    {
        None,
        Win,
        Rich,       // 코인 보유량
        Runner,     // 이동 거리 
        Mini,    // 미니게임 스코어
    }



    // 작동 상태
    public static ActionProgress endProgress = ActionProgress.Ready;

    public static Coroutine coroutine = null;

    /// <summary>
    /// 엔딩 절차 초기화
    /// </summary>
    public static void Reset()
    {
        endProgress = ActionProgress.Ready;
    }


    public static IEnumerator CallCenter()
    {
        // 작동 상태 체크
        if(endProgress != ActionProgress.Ready)
        {
            Debug.LogError("error :: 엔딩 절차가 Ready 상태가 아님 -> " + endProgress);
        }
        else
        {
            // 카메라 조작 차단
            GameData.worldManager.cameraManager.controller.isFreeMode = false;

            // 카메라 시점 변경
            GameData.worldManager.cameraManager.CamMoveTo(BlockManager.script.startBlock, CameraManager.CamAngle.Top);

            // 특수 UI 제거
            GameMaster.script.MainUI.transform.Find("btn_Cam").gameObject.SetActive(false);

            // 모든 플레이어 시작점 소환
            for (int i = 0; i < Player.allPlayer.Count; i++)
            {
                Player.allPlayer[i].movement.Tleport(-1, 1f);
            }

            // 소환 완료 대기
            bool check = true;
            while (check)
            {
                // 무한 대기 방지
                check = false;

                // 모든 플레이어 체크
                for (int i = 0; i < Player.allPlayer.Count; i++)
                    check = check || Player.allPlayer[i].movement.isBusy;

                yield return null;
            }

            // 작동 상태 변경
            coroutine = null;
            endProgress = ActionProgress.Start;
        }
    }

    public static IEnumerator Trophy()
    {
        // 작동 상태 체크
        if (endProgress != ActionProgress.Start)
        {
            Debug.LogError("error :: 엔딩 절차가 Start 상태가 아님 -> " + endProgress);
        }
        else
        {
            List<Player> order = new List<Player>();

            // 트로피 지급 - 코인
            {
                // 성적 산출
                order.Clear();

                int indexer = 0;
                List<Player> temp = new List<Player>(Player.allPlayer);
                while (temp.Count > 0)
                {
                    // 1순위 탐색
                    for (int i = 1; i < temp.Count; i++)
                    {
                        if (temp[i].coin.Value > temp[indexer].coin.Value)
                            indexer = i;
                    }

                    // 등록
                    order.Add(temp[indexer]);

                    // 제외
                    temp.Remove(temp[indexer]);
                }

                // 트로피 지급
                yield return Trophy(TrophyType.Rich, order);
            }

            // 트로피 지급 - 이동량
            {
                // 성적 산출
                order.Clear();

                int indexer = 0;
                List<Player> temp = new List<Player>(Player.allPlayer);
                while (temp.Count > 0)
                {
                    // 1순위 탐색
                    for (int i = 1; i < temp.Count; i++)
                    {
                        if (temp[i].dice.valueRecord > temp[indexer].dice.valueRecord)
                            indexer = i;
                    }

                    // 등록
                    order.Add(temp[indexer]);

                    // 제외
                    temp.Remove(temp[indexer]);
                }

                // 트로피 지급
                yield return Trophy(TrophyType.Rich, order);
            }

            // 트로피 지급 - 미니게임 성적
            {
                // 성적 산출
                order.Clear();

                int indexer = 0;
                List<Player> temp = new List<Player>(Player.allPlayer);
                while (temp.Count > 0)
                {
                    // 1순위 탐색
                    for (int i = 1; i < temp.Count; i++)
                    {
                        if (temp[i].miniInfo.recordScore > temp[indexer].miniInfo.recordScore)
                            indexer = i;
                    }

                    // 등록
                    order.Add(temp[indexer]);

                    // 제외
                    temp.Remove(temp[indexer]);
                }

                // 트로피 지급
                yield return Trophy(TrophyType.Rich, order);
            }

            // 트로피 지급 - 최종
            {
                // 성적 산출
                order.Clear();

                int indexer = 0;
                List<Player> temp = new List<Player>(Player.allPlayer);
                while (temp.Count > 0)
                {
                    // 1순위 탐색
                    for (int i = 1; i < temp.Count; i++)
                    {
                        if (temp[i].miniInfo.recordScore > temp[indexer].miniInfo.recordScore)
                            indexer = i;
                    }

                    // 등록
                    order.Add(temp[indexer]);

                    // 제외
                    temp.Remove(temp[indexer]);
                }

                // 트로피 지급
                yield return Trophy(TrophyType.Win, order);
            }

            // 작동 상태 변경
            coroutine = null;
            endProgress = ActionProgress.Working;
        }
    }

    static IEnumerator Trophy(TrophyType trophyType, List<Player> order)
    {
        if (trophyType != TrophyType.None)
        {
            // 트로피 지급 - 코인
            if (trophyType == TrophyType.Rich)
            {
                // 알림
                GameMaster.script.messageBox.PopUpText("Trophy", "판정 : 코인 보유량");

                // 알림 확인 대기
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // 트로피 지급 - 3등
                yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                order[2].trophy.rich = 3;

                // 트로피 지급 - 2등
                yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                order[1].trophy.rich = 2;

                // 트로피 지급 - 1등
                yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                order[0].trophy.rich = 1;
            }
            // 트로피 지급 - 이동량
            else if (trophyType == TrophyType.Runner)
            {
                // 알림
                GameMaster.script.messageBox.PopUpText("Trophy", "판정 : 이동 거리");

                // 알림 확인 대기
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // 트로피 지급 - 3등
                yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                order[2].trophy.runner = 3;

                // 트로피 지급 - 2등
                yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                order[1].trophy.runner = 2;

                // 트로피 지급 - 1등
                yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                order[0].trophy.runner = 1;
            }
            // 트로피 지급 - 미니게임 성적
            else if (trophyType == TrophyType.Mini)
            {
                // 알림
                GameMaster.script.messageBox.PopUpText("Trophy", "판정 : 미니게임 성적");

                // 알림 확인 대기
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // 트로피 지급 - 3등
                yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                order[2].trophy.mini = 3;

                // 트로피 지급 - 2등
                yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                order[1].trophy.mini = 2;

                // 트로피 지급 - 1등
                yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                order[0].trophy.mini = 1;
            }

            // 트로피 지급 - 최종
            else if (trophyType == TrophyType.Mini)
            {
                // 알림
                GameMaster.script.messageBox.PopUpText("Winner", "최종 우승자 확인중...");

                // 트로피 지급 - 3등
                //yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                order[2].trophy.final = 3;

                // 트로피 지급 - 2등
                //yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                order[1].trophy.final = 2;

                // 트로피 지급 - 1등
                yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                order[0].trophy.final = 1;

                // 알림
                GameMaster.script.messageBox.PopUpText("Winner", "우승자는 " + order[0].movement.transform.name + " 입니다 !!");

                // 알림 확인 대기
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;
            }

        }
    }

    /// <summary>
    /// 트로피 지급 연출
    /// </summary>
    /// <param name="trophy">지급할 트로피</param>
    /// <param name="target">지급 대상</param>
    /// <returns></returns>
    static IEnumerator TrophyGive(Transform trophy, Player target)
    {
        // 지정
        trophy = GameMaster.script.Trophy3rd.transform;

        // 축소
        trophy.localScale = Vector3.zero;

        // 소환
        trophy.position = target.movement.transform.position + Vector3.up * 2;

        // 사이즈 제어
        int flow = 0;
        Vector3 size = new Vector3(0.7f, 1, 0.7f);

        // 사이즈 업
        while (flow == 0)
        {
            trophy.localScale = Vector3.Lerp(trophy.localScale, size * 2, Time.deltaTime);
            if (trophy.localScale.y >= size.y * 2 * 0.999)
                flow++;
            yield return null;
        }
        trophy.localScale = size * 2;

        // 사이즈 다운
        while (flow == 1)
        {
            trophy.localScale = Vector3.Lerp(trophy.localScale, size, Time.deltaTime);
            if (trophy.localScale.y <= size.y * 1.001)
                flow++;
            yield return null;
        }
        trophy.localScale = size;
    }
}
