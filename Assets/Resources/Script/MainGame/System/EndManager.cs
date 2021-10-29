using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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


    // 사이즈
    static Vector3 size = new Vector3(0.7f, 1, 0.7f);
    static Vector3 sizeUp = size * 1.5f;

    // 보관소
    static Vector3 hidePoint = new Vector3(0, -10, 0);



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
            //GameData.worldManager.cameraManager.CamMoveTo(BlockManager.script.startBlock, CameraManager.CamAngle.Top);
            //GameData.worldManager.cameraManager.CamMoveTo(new CameraManager.CamPoint(BlockManager.script.startBlock, CameraManager.CamAngle.Top));
            GameData.worldManager.cameraManager.CamFreeStartPoint();

            // 특수 UI 제거
            GameMaster.script.MainUI.transform.Find("btn_Cam").gameObject.SetActive(false);

            // 모든 플레이어 시작점 소환
            for (int i = 0; i < Player.allPlayer.Count; i++)
            {
                // 시작블록 위치 플레이어 스킵
                if (Player.allPlayer[i].location == -1)
                    continue;

                yield return Player.allPlayer[i].movement.Tleport(-1, 1f);
                Debug.Log("게임 정산 :: 중앙 소환 -> " + Player.allPlayer[i].name);
            }

            // 소환 완료 대기
            bool check = true;
            while (check)
            {
                // 무한 대기 방지
                check = false;

                // 모든 플레이어 체크
                for (int i = 0; i < Player.allPlayer.Count; i++)
                {
                    check = check || Player.allPlayer[i].movement.isBusy;
                    check = check || Player.allPlayer[i].location != -1;
                    Debug.Log(string.Format("게임 정산 :: 소환 상태 -> {0} = 블록 {1} (이동중 = {2})" , Player.allPlayer[i].name, Player.allPlayer[i].location, Player.allPlayer[i].movement.isBusy));
                }

                yield return null;
            }

            // 겹침 재정렬
            CharacterMover.AvatarOverFixAll();

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
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f);

            // 트로피 지급 - 코인
            {
                // 성적 산출
                order.Clear();

                // 정렬
                List<Player> temp = new List<Player>(Player.allPlayer);
                order = temp.OrderBy(x => x.coin.Value).Reverse().ToList();

                // 트로피 지급
                yield return Trophy(TrophyType.Rich, order);

                // 전체 회수
                Hide();

                // 대기
                yield return wait;
                wait.Reset();
            }

            // 트로피 지급 - 이동량
            {
                // 성적 산출
                order.Clear();

                // 정렬
                List<Player> temp = new List<Player>(Player.allPlayer);
                order = temp.OrderBy(x => x.dice.valueRecord).Reverse().ToList();

                // 트로피 지급
                yield return Trophy(TrophyType.Runner, order);

                // 전체 회수
                Hide();

                // 대기
                yield return wait;
                wait.Reset();
            }

            // 트로피 지급 - 미니게임 성적
            {
                // 성적 산출
                order.Clear();

                // 정렬
                List<Player> temp = new List<Player>(Player.allPlayer);
                order = temp.OrderBy(x => x.miniInfo.recordScore).Reverse().ToList();

                // 트로피 지급
                yield return Trophy(TrophyType.Mini, order);

                // 전체 회수
                Hide();

                // 대기
                yield return wait;
                wait.Reset();
            }

            // 트로피 지급 - 최종
            {
                // 성적 산출
                order.Clear();

                // 정렬
                List<Player> temp = new List<Player>(Player.allPlayer);
                order = temp.OrderBy(x => x.trophy.score).Reverse().ToList();

                // 트로피 지급
                yield return Trophy(TrophyType.Win, order);

                // 전체 회수
                //Hide();
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
                Debug.Log("트로피 : 코인 보유량");
                GameMaster.script.messageBox.PopUpText("Trophy", "판정 : 코인 보유량");

                // 알림 확인 대기
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // 트로피 지급 - 3등
                if(Player.allPlayer.Count >= 3)
                {
                    yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                    order[2].trophy.rich = 3;
                }

                // 트로피 지급 - 2등
                if (Player.allPlayer.Count >= 2)
                {
                    yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                    order[1].trophy.rich = 2;
                }

                // 트로피 지급 - 1등
                if (Player.allPlayer.Count >= 1)
                {
                    yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                    order[0].trophy.rich = 1;
                }
            }
            // 트로피 지급 - 이동량
            else if (trophyType == TrophyType.Runner)
            {
                // 알림
                Debug.Log("트로피 : 이동 거리");
                GameMaster.script.messageBox.PopUpText("Trophy", "판정 : 이동 거리");

                // 알림 확인 대기
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // 트로피 지급 - 3등
                if (Player.allPlayer.Count >= 3)
                {
                    yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                    order[2].trophy.runner = 3;
                }

                // 트로피 지급 - 2등
                if (Player.allPlayer.Count >= 2)
                {
                    yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                    order[1].trophy.runner = 2;
                }

                // 트로피 지급 - 1등
                if (Player.allPlayer.Count >= 1)
                {
                    yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                    order[0].trophy.runner = 1;
                }
            }
            // 트로피 지급 - 미니게임 성적
            else if (trophyType == TrophyType.Mini)
            {
                // 알림
                Debug.Log("트로피 : 미니게임 성적");
                GameMaster.script.messageBox.PopUpText("Trophy", "판정 : 미니게임 성적");

                // 알림 확인 대기
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // 트로피 지급 - 3등
                if (Player.allPlayer.Count >= 3)
                {
                    yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                    order[2].trophy.mini = 3;
                }

                // 트로피 지급 - 2등
                if (Player.allPlayer.Count >= 2)
                {
                    yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                    order[1].trophy.mini = 2;
                }

                // 트로피 지급 - 1등
                if (Player.allPlayer.Count >= 1)
                {
                    yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                    order[0].trophy.mini = 1;
                }
            }

            // 트로피 지급 - 최종
            else if (trophyType == TrophyType.Win)
            {
                // 알림
                Debug.Log("트로피 : 최종 우승자");
                GameMaster.script.messageBox.PopUpText("Winner", "최종 우승자 확인중...");

                // 알림 확인 대기
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // 대기
                WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f);
                yield return wait;

                // 트로피 지급 - 3등
                if (Player.allPlayer.Count >= 3)
                {
                    //yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                    order[2].trophy.final = 3;
                }

                // 트로피 지급 - 2등
                if (Player.allPlayer.Count >= 2)
                {
                    //yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                    order[1].trophy.final = 2;
                }

                // 트로피 지급 - 1등
                if (Player.allPlayer.Count >= 1)
                {
                    yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                    order[0].trophy.final = 1;
                }

                // 유저 데이터 기록
                switch (Player.me.trophy.final)
                {
                    case 1:
                        UserData.rank1++;
                        break;

                    case 2:
                        UserData.rank2++;
                        break;

                    case 3:
                        UserData.rank3++;
                        break;

                    default:
                        UserData.rank4++;
                        break;
                }
                // UserData.playTime : 미구현====================
                UserData.playCount++;
                UserData.SaveData();

                // 세이브 파기
                GameSaveStream.SaveRemove();

                // 알림
                GameMaster.script.messageBox.PopUpText("Winner", "우승자는 " + order[0].name + " 입니다 !!");

                // 알림 확인 대기
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // 클리어 연출 대기
                wait.Reset();
                yield return wait;

                
                // 작동 상태 변경
                coroutine = null;
                endProgress = ActionProgress.Finish;
                GameData.gameFlow = GameMaster.Flow.Finish;
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
        Debug.Log("트로피 :: 수여됨 -> " + target.name + " 에게 " + trophy.name);
        
        // 축소
        trophy.localScale = Vector3.zero;

        // 소환
        trophy.position = target.movement.transform.position + Vector3.up * 8;

        // 사이즈 제어
        int flow = 0;
        float speed = 5.0f;

        // 사이즈 업
        while (flow == 0)
        {
            trophy.localScale = Vector3.Lerp(trophy.localScale, sizeUp, Time.deltaTime * speed * 1.5f);
            if (trophy.localScale.y >= sizeUp.y * 0.999)
                flow++;
            yield return null;
        }
        trophy.localScale = sizeUp;

        // 사이즈 다운
        while (flow == 1)
        {
            trophy.localScale = Vector3.Lerp(trophy.localScale, size, Time.deltaTime * speed);
            if (trophy.localScale.y <= size.y * 1.001)
                flow++;
            yield return null;
        }
        trophy.localScale = size;
    }

    static void Hide()
    {
        // 전체 회수
        GameMaster.script.Trophy1st.transform.position = hidePoint;
        GameMaster.script.Trophy2nd.transform.position = hidePoint;
        GameMaster.script.Trophy3rd.transform.position = hidePoint;
    }
}
