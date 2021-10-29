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
        Rich,       // ���� ������
        Runner,     // �̵� �Ÿ� 
        Mini,    // �̴ϰ��� ���ھ�
    }



    // �۵� ����
    public static ActionProgress endProgress = ActionProgress.Ready;

    public static Coroutine coroutine = null;


    // ������
    static Vector3 size = new Vector3(0.7f, 1, 0.7f);
    static Vector3 sizeUp = size * 1.5f;

    // ������
    static Vector3 hidePoint = new Vector3(0, -10, 0);



    /// <summary>
    /// ���� ���� �ʱ�ȭ
    /// </summary>
    public static void Reset()
    {
        endProgress = ActionProgress.Ready;
    }


    public static IEnumerator CallCenter()
    {
        // �۵� ���� üũ
        if(endProgress != ActionProgress.Ready)
        {
            Debug.LogError("error :: ���� ������ Ready ���°� �ƴ� -> " + endProgress);
        }
        else
        {
            // ī�޶� ���� ����
            GameData.worldManager.cameraManager.controller.isFreeMode = false;

            // ī�޶� ���� ����
            //GameData.worldManager.cameraManager.CamMoveTo(BlockManager.script.startBlock, CameraManager.CamAngle.Top);
            //GameData.worldManager.cameraManager.CamMoveTo(new CameraManager.CamPoint(BlockManager.script.startBlock, CameraManager.CamAngle.Top));
            GameData.worldManager.cameraManager.CamFreeStartPoint();

            // Ư�� UI ����
            GameMaster.script.MainUI.transform.Find("btn_Cam").gameObject.SetActive(false);

            // ��� �÷��̾� ������ ��ȯ
            for (int i = 0; i < Player.allPlayer.Count; i++)
            {
                // ���ۺ�� ��ġ �÷��̾� ��ŵ
                if (Player.allPlayer[i].location == -1)
                    continue;

                yield return Player.allPlayer[i].movement.Tleport(-1, 1f);
                Debug.Log("���� ���� :: �߾� ��ȯ -> " + Player.allPlayer[i].name);
            }

            // ��ȯ �Ϸ� ���
            bool check = true;
            while (check)
            {
                // ���� ��� ����
                check = false;

                // ��� �÷��̾� üũ
                for (int i = 0; i < Player.allPlayer.Count; i++)
                {
                    check = check || Player.allPlayer[i].movement.isBusy;
                    check = check || Player.allPlayer[i].location != -1;
                    Debug.Log(string.Format("���� ���� :: ��ȯ ���� -> {0} = ��� {1} (�̵��� = {2})" , Player.allPlayer[i].name, Player.allPlayer[i].location, Player.allPlayer[i].movement.isBusy));
                }

                yield return null;
            }

            // ��ħ ������
            CharacterMover.AvatarOverFixAll();

            // �۵� ���� ����
            coroutine = null;
            endProgress = ActionProgress.Start;
        }
    }

    public static IEnumerator Trophy()
    {
        // �۵� ���� üũ
        if (endProgress != ActionProgress.Start)
        {
            Debug.LogError("error :: ���� ������ Start ���°� �ƴ� -> " + endProgress);
        }
        else
        {
            List<Player> order = new List<Player>();
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f);

            // Ʈ���� ���� - ����
            {
                // ���� ����
                order.Clear();

                // ����
                List<Player> temp = new List<Player>(Player.allPlayer);
                order = temp.OrderBy(x => x.coin.Value).Reverse().ToList();

                // Ʈ���� ����
                yield return Trophy(TrophyType.Rich, order);

                // ��ü ȸ��
                Hide();

                // ���
                yield return wait;
                wait.Reset();
            }

            // Ʈ���� ���� - �̵���
            {
                // ���� ����
                order.Clear();

                // ����
                List<Player> temp = new List<Player>(Player.allPlayer);
                order = temp.OrderBy(x => x.dice.valueRecord).Reverse().ToList();

                // Ʈ���� ����
                yield return Trophy(TrophyType.Runner, order);

                // ��ü ȸ��
                Hide();

                // ���
                yield return wait;
                wait.Reset();
            }

            // Ʈ���� ���� - �̴ϰ��� ����
            {
                // ���� ����
                order.Clear();

                // ����
                List<Player> temp = new List<Player>(Player.allPlayer);
                order = temp.OrderBy(x => x.miniInfo.recordScore).Reverse().ToList();

                // Ʈ���� ����
                yield return Trophy(TrophyType.Mini, order);

                // ��ü ȸ��
                Hide();

                // ���
                yield return wait;
                wait.Reset();
            }

            // Ʈ���� ���� - ����
            {
                // ���� ����
                order.Clear();

                // ����
                List<Player> temp = new List<Player>(Player.allPlayer);
                order = temp.OrderBy(x => x.trophy.score).Reverse().ToList();

                // Ʈ���� ����
                yield return Trophy(TrophyType.Win, order);

                // ��ü ȸ��
                //Hide();
            }

            // �۵� ���� ����
            coroutine = null;
            endProgress = ActionProgress.Working;
        }
    }

    static IEnumerator Trophy(TrophyType trophyType, List<Player> order)
    {
        if (trophyType != TrophyType.None)
        {
            // Ʈ���� ���� - ����
            if (trophyType == TrophyType.Rich)
            {
                // �˸�
                Debug.Log("Ʈ���� : ���� ������");
                GameMaster.script.messageBox.PopUpText("Trophy", "���� : ���� ������");

                // �˸� Ȯ�� ���
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // Ʈ���� ���� - 3��
                if(Player.allPlayer.Count >= 3)
                {
                    yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                    order[2].trophy.rich = 3;
                }

                // Ʈ���� ���� - 2��
                if (Player.allPlayer.Count >= 2)
                {
                    yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                    order[1].trophy.rich = 2;
                }

                // Ʈ���� ���� - 1��
                if (Player.allPlayer.Count >= 1)
                {
                    yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                    order[0].trophy.rich = 1;
                }
            }
            // Ʈ���� ���� - �̵���
            else if (trophyType == TrophyType.Runner)
            {
                // �˸�
                Debug.Log("Ʈ���� : �̵� �Ÿ�");
                GameMaster.script.messageBox.PopUpText("Trophy", "���� : �̵� �Ÿ�");

                // �˸� Ȯ�� ���
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // Ʈ���� ���� - 3��
                if (Player.allPlayer.Count >= 3)
                {
                    yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                    order[2].trophy.runner = 3;
                }

                // Ʈ���� ���� - 2��
                if (Player.allPlayer.Count >= 2)
                {
                    yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                    order[1].trophy.runner = 2;
                }

                // Ʈ���� ���� - 1��
                if (Player.allPlayer.Count >= 1)
                {
                    yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                    order[0].trophy.runner = 1;
                }
            }
            // Ʈ���� ���� - �̴ϰ��� ����
            else if (trophyType == TrophyType.Mini)
            {
                // �˸�
                Debug.Log("Ʈ���� : �̴ϰ��� ����");
                GameMaster.script.messageBox.PopUpText("Trophy", "���� : �̴ϰ��� ����");

                // �˸� Ȯ�� ���
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // Ʈ���� ���� - 3��
                if (Player.allPlayer.Count >= 3)
                {
                    yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                    order[2].trophy.mini = 3;
                }

                // Ʈ���� ���� - 2��
                if (Player.allPlayer.Count >= 2)
                {
                    yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                    order[1].trophy.mini = 2;
                }

                // Ʈ���� ���� - 1��
                if (Player.allPlayer.Count >= 1)
                {
                    yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                    order[0].trophy.mini = 1;
                }
            }

            // Ʈ���� ���� - ����
            else if (trophyType == TrophyType.Win)
            {
                // �˸�
                Debug.Log("Ʈ���� : ���� �����");
                GameMaster.script.messageBox.PopUpText("Winner", "���� ����� Ȯ����...");

                // �˸� Ȯ�� ���
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // ���
                WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f);
                yield return wait;

                // Ʈ���� ���� - 3��
                if (Player.allPlayer.Count >= 3)
                {
                    //yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                    order[2].trophy.final = 3;
                }

                // Ʈ���� ���� - 2��
                if (Player.allPlayer.Count >= 2)
                {
                    //yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                    order[1].trophy.final = 2;
                }

                // Ʈ���� ���� - 1��
                if (Player.allPlayer.Count >= 1)
                {
                    yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                    order[0].trophy.final = 1;
                }

                // ���� ������ ���
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
                // UserData.playTime : �̱���====================
                UserData.playCount++;
                UserData.SaveData();

                // ���̺� �ı�
                GameSaveStream.SaveRemove();

                // �˸�
                GameMaster.script.messageBox.PopUpText("Winner", "����ڴ� " + order[0].name + " �Դϴ� !!");

                // �˸� Ȯ�� ���
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // Ŭ���� ���� ���
                wait.Reset();
                yield return wait;

                
                // �۵� ���� ����
                coroutine = null;
                endProgress = ActionProgress.Finish;
                GameData.gameFlow = GameMaster.Flow.Finish;
            }

        }
    }

    /// <summary>
    /// Ʈ���� ���� ����
    /// </summary>
    /// <param name="trophy">������ Ʈ����</param>
    /// <param name="target">���� ���</param>
    /// <returns></returns>
    static IEnumerator TrophyGive(Transform trophy, Player target)
    {
        Debug.Log("Ʈ���� :: ������ -> " + target.name + " ���� " + trophy.name);
        
        // ���
        trophy.localScale = Vector3.zero;

        // ��ȯ
        trophy.position = target.movement.transform.position + Vector3.up * 8;

        // ������ ����
        int flow = 0;
        float speed = 5.0f;

        // ������ ��
        while (flow == 0)
        {
            trophy.localScale = Vector3.Lerp(trophy.localScale, sizeUp, Time.deltaTime * speed * 1.5f);
            if (trophy.localScale.y >= sizeUp.y * 0.999)
                flow++;
            yield return null;
        }
        trophy.localScale = sizeUp;

        // ������ �ٿ�
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
        // ��ü ȸ��
        GameMaster.script.Trophy1st.transform.position = hidePoint;
        GameMaster.script.Trophy2nd.transform.position = hidePoint;
        GameMaster.script.Trophy3rd.transform.position = hidePoint;
    }
}
