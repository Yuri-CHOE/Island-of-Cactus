using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public enum Flow
    {
        Wait,
        Start,
        Ordering,
        Cycling,
        //CycleStart,
        //Turn,
        //MiniGameStart,
        //MiniGame,
        //MiniGameEnd,
        //CycleEnd,
        End,
        Trophy,
        Finish,
    }

    // �ε� �Ŵ��� ��ũ��Ʈ
    [SerializeField]
    LoadingManager loadingManager = null;

    // ĳ���� ������Ʈ �θ� ��ũ��Ʈ
    [SerializeField]
    GameObject characterParent = null;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoFlowWork()
    {
        switch (GameData.gameFlow)
        {
            // ���� ���� ���� �ȵ�
            case Flow.Wait:
                // �ε� �Ϸ� ���
                if (loadingManager.isFinish)    return;

                // ĳ���� ����
                // �̱���================

                // ���̺����� �ۼ�
                // �̱���================
                // �������� �����Ұ�

                Debug.Log("���� �÷ο� :: �� ���� ȣ�� Ȯ�ε�");
                GameData.gameFlow = Flow.Start;
                break;


            // ���� ���۵�
            case Flow.Start:

                // �÷��̾� ���� �ʱ�ȭ
                if (GameData.gameMode == GameMode.Mode.None)
                    return;
                else if (GameData.gameMode == GameMode.Mode.Online)
                {
                    // �̱��� : ���� ���� �ʿ�===========
                }
                else
                {
                    // �ߺ� ���� �۾�
                    List<int> picked = Tool.RandomNotCross(1, Character.table.Count, 4);
                    if (picked.Contains(GameData.player.me.character.index))
                        picked.Remove(GameData.player.me.character.index);

                    // �ʱ�ȭ ����
                    GameData.SetPlayer(
                        GameData.player.me,
                        new Player(Player.Type.AI, picked[0], false, null),
                        new Player(Player.Type.AI, picked[1], false, null),
                        new Player(Player.Type.AI, picked[2], false, null)
                        );
                }


                // �� �ѷ�����
                // �̱���==================                

                Debug.Log("���� �÷ο� :: ���� ���� Ȯ�ε� => by ");
                GameData.gameFlow = Flow.Ordering;
                break;


            // �����ֻ���
            case Flow.Ordering:
                // �����ֻ��� ������
                // �̱���==================

                // ����ť ����
                GameData.turn.SetUp();      // ���� �ֻ��� ���� ���� �� �ʱ�ȭ

                //GameData.gameFlow = Flow.CycleStart;
                GameData.gameFlow = Flow.Cycling;
                break;


            // ���� ����
            case Flow.Cycling:
                if (GameData.cycle.isEnd())
                {
                    Debug.Log("���� �÷ο� :: ���� ���� �޼� Ȯ�ε� => by " + GameData.turn.now.name);
                    GameData.gameFlow = Flow.End;
                }
                else
                {
                    TurnWork();
                }
                break;


            /*
        case Flow.Turn:
            if (GameData.turn.now == GameData.player.system.Minigame)
                GameData.gameFlow = Flow.MiniGameStart;
            else if (GameData.turn.now == GameData.player.system.Ender)
                GameData.gameFlow = Flow.CycleEnd;
            break;
        case Flow.MiniGameStart:
            GameData.gameFlow = Flow.MiniGame;
            break;
        case Flow.MiniGame:
            GameData.gameFlow = Flow.MiniGameEnd;
            break;
        case Flow.MiniGameEnd:
            GameData.gameFlow = Flow.Turn;      // ���� : �ٽ� ������ ���ư��� �ý��� �÷��̾� ���� ���� �����
            break;
        case Flow.CycleEnd:
            if(GameData.cycle.isEnd())
                GameData.gameFlow = Flow.End;
            else
                GameData.gameFlow = Flow.CycleStart;
            break;
            */


            // ���� �����
            case Flow.End:
                GameData.gameFlow = Flow.Trophy;
                break;


            // Ʈ���� ���� �� ����� ��ǥ
            case Flow.Trophy:
                GameData.gameFlow = Flow.Finish;
                break;


            // ���� �����
            case Flow.Finish:
                break;
        }
    }


    public void TurnWork()
    {
        // �α� ���
        Debug.Log("�÷��̾� ȣ�� :: " + GameData.turn.now.name);

        // �ý��� �÷��̾� - ��Ÿ��
        if (GameData.turn.now == GameData.player.system.Starter)
        {

        }
        // �ý��� �÷��̾� - �̴ϰ���
        else if(GameData.turn.now == GameData.player.system.Minigame)
        {

        }
        // �ý��� �÷��̾� - ����
        else if (GameData.turn.now == GameData.player.system.Ender)
        {

        }
        // ���� �÷��̾�
        else
        {

        }
    }


    /// <summary>
    /// �������� üũ�Ͽ� �������� ����
    /// </summary>
    public void CheckLife()
    {
        // 1p üũ
        if (GameData.player.player_1 != null)
            if (GameData.player.player_1.life.Value <= 0)
                GotoPrison(GameData.player.player_1);

        // 2p üũ
        if (GameData.player.player_2 != null)
            if (GameData.player.player_2.life.Value <= 0)
                GotoPrison(GameData.player.player_2);

        // 3p üũ
        if (GameData.player.player_3 != null)
            if (GameData.player.player_3.life.Value <= 0)
                GotoPrison(GameData.player.player_3);

        // 4p üũ
        if (GameData.player.player_4 != null)
            if (GameData.player.player_4.life.Value <= 0)
                GotoPrison(GameData.player.player_4);
    }

    /// <summary>
    /// Ư�� �÷��̾ �������� ����
    /// </summary>
    /// <param name="targetPlayer"></param>
    public void GotoPrison(Player targetPlayer)
    {
        // �̱���================
    }
}
