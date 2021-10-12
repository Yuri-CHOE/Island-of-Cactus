using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            GameData.worldManager.cameraManager.CamMoveTo(BlockManager.script.startBlock, CameraManager.CamAngle.Top);

            // Ư�� UI ����
            GameMaster.script.MainUI.transform.Find("btn_Cam").gameObject.SetActive(false);

            // ��� �÷��̾� ������ ��ȯ
            for (int i = 0; i < Player.allPlayer.Count; i++)
            {
                Player.allPlayer[i].movement.Tleport(-1, 1f);
            }

            // ��ȯ �Ϸ� ���
            bool check = true;
            while (check)
            {
                // ���� ��� ����
                check = false;

                // ��� �÷��̾� üũ
                for (int i = 0; i < Player.allPlayer.Count; i++)
                    check = check || Player.allPlayer[i].movement.isBusy;

                yield return null;
            }

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

            // Ʈ���� ���� - ����
            {
                // ���� ����
                order.Clear();

                int indexer = 0;
                List<Player> temp = new List<Player>(Player.allPlayer);
                while (temp.Count > 0)
                {
                    // 1���� Ž��
                    for (int i = 1; i < temp.Count; i++)
                    {
                        if (temp[i].coin.Value > temp[indexer].coin.Value)
                            indexer = i;
                    }

                    // ���
                    order.Add(temp[indexer]);

                    // ����
                    temp.Remove(temp[indexer]);
                }

                // Ʈ���� ����
                yield return Trophy(TrophyType.Rich, order);
            }

            // Ʈ���� ���� - �̵���
            {
                // ���� ����
                order.Clear();

                int indexer = 0;
                List<Player> temp = new List<Player>(Player.allPlayer);
                while (temp.Count > 0)
                {
                    // 1���� Ž��
                    for (int i = 1; i < temp.Count; i++)
                    {
                        if (temp[i].dice.valueRecord > temp[indexer].dice.valueRecord)
                            indexer = i;
                    }

                    // ���
                    order.Add(temp[indexer]);

                    // ����
                    temp.Remove(temp[indexer]);
                }

                // Ʈ���� ����
                yield return Trophy(TrophyType.Rich, order);
            }

            // Ʈ���� ���� - �̴ϰ��� ����
            {
                // ���� ����
                order.Clear();

                int indexer = 0;
                List<Player> temp = new List<Player>(Player.allPlayer);
                while (temp.Count > 0)
                {
                    // 1���� Ž��
                    for (int i = 1; i < temp.Count; i++)
                    {
                        if (temp[i].miniInfo.recordScore > temp[indexer].miniInfo.recordScore)
                            indexer = i;
                    }

                    // ���
                    order.Add(temp[indexer]);

                    // ����
                    temp.Remove(temp[indexer]);
                }

                // Ʈ���� ����
                yield return Trophy(TrophyType.Rich, order);
            }

            // Ʈ���� ���� - ����
            {
                // ���� ����
                order.Clear();

                int indexer = 0;
                List<Player> temp = new List<Player>(Player.allPlayer);
                while (temp.Count > 0)
                {
                    // 1���� Ž��
                    for (int i = 1; i < temp.Count; i++)
                    {
                        if (temp[i].miniInfo.recordScore > temp[indexer].miniInfo.recordScore)
                            indexer = i;
                    }

                    // ���
                    order.Add(temp[indexer]);

                    // ����
                    temp.Remove(temp[indexer]);
                }

                // Ʈ���� ����
                yield return Trophy(TrophyType.Win, order);
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
                GameMaster.script.messageBox.PopUpText("Trophy", "���� : ���� ������");

                // �˸� Ȯ�� ���
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // Ʈ���� ���� - 3��
                yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                order[2].trophy.rich = 3;

                // Ʈ���� ���� - 2��
                yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                order[1].trophy.rich = 2;

                // Ʈ���� ���� - 1��
                yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                order[0].trophy.rich = 1;
            }
            // Ʈ���� ���� - �̵���
            else if (trophyType == TrophyType.Runner)
            {
                // �˸�
                GameMaster.script.messageBox.PopUpText("Trophy", "���� : �̵� �Ÿ�");

                // �˸� Ȯ�� ���
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // Ʈ���� ���� - 3��
                yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                order[2].trophy.runner = 3;

                // Ʈ���� ���� - 2��
                yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                order[1].trophy.runner = 2;

                // Ʈ���� ���� - 1��
                yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                order[0].trophy.runner = 1;
            }
            // Ʈ���� ���� - �̴ϰ��� ����
            else if (trophyType == TrophyType.Mini)
            {
                // �˸�
                GameMaster.script.messageBox.PopUpText("Trophy", "���� : �̴ϰ��� ����");

                // �˸� Ȯ�� ���
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;

                // Ʈ���� ���� - 3��
                yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                order[2].trophy.mini = 3;

                // Ʈ���� ���� - 2��
                yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                order[1].trophy.mini = 2;

                // Ʈ���� ���� - 1��
                yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                order[0].trophy.mini = 1;
            }

            // Ʈ���� ���� - ����
            else if (trophyType == TrophyType.Mini)
            {
                // �˸�
                GameMaster.script.messageBox.PopUpText("Winner", "���� ����� Ȯ����...");

                // Ʈ���� ���� - 3��
                //yield return TrophyGive(GameMaster.script.Trophy3rd.transform, order[2]);
                order[2].trophy.final = 3;

                // Ʈ���� ���� - 2��
                //yield return TrophyGive(GameMaster.script.Trophy2nd.transform, order[1]);
                order[1].trophy.final = 2;

                // Ʈ���� ���� - 1��
                yield return TrophyGive(GameMaster.script.Trophy1st.transform, order[0]);
                order[0].trophy.final = 1;

                // �˸�
                GameMaster.script.messageBox.PopUpText("Winner", "����ڴ� " + order[0].movement.transform.name + " �Դϴ� !!");

                // �˸� Ȯ�� ���
                while (GameMaster.script.messageBox.gameObject.activeSelf) yield return null;
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
        // ����
        trophy = GameMaster.script.Trophy3rd.transform;

        // ���
        trophy.localScale = Vector3.zero;

        // ��ȯ
        trophy.position = target.movement.transform.position + Vector3.up * 2;

        // ������ ����
        int flow = 0;
        Vector3 size = new Vector3(0.7f, 1, 0.7f);

        // ������ ��
        while (flow == 0)
        {
            trophy.localScale = Vector3.Lerp(trophy.localScale, size * 2, Time.deltaTime);
            if (trophy.localScale.y >= size.y * 2 * 0.999)
                flow++;
            yield return null;
        }
        trophy.localScale = size * 2;

        // ������ �ٿ�
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
