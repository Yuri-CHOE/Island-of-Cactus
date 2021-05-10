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
        Opening,
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

    [Header("script")]
    // �ε� �Ŵ��� ��ũ��Ʈ
    [SerializeField]
    LoadingManager loadingManager = null;

    // �ֻ��� ��Ʈ�ѷ� ��ũ��Ʈ
    [SerializeField]
    DiceController diceController = null;

    // ĳ���� ������Ʈ �θ� ��ũ��Ʈ
    [SerializeField]
    Transform characterParent = null;

    // PlayerInfo UI ������Ʈ
    [Space]
    public List<PlayerInfoUI> playerInfoUI = new List<PlayerInfoUI>();
    public CanvasGroup MainUI = null;


    // ������ ��� ���
    public bool useItemOrder = false;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DoFlowWork();
    }

    public void DoFlowWork()
    {
        switch (GameData.gameFlow)
        {
            // ���� ���� ���� �ȵ�
            case Flow.Wait:
                {
                    // �ε� �Ϸ� ���
                    if (loadingManager.isFinish) return;

                    // ���̺����� �ۼ�
                    // �̱���================
                    // �������� �����Ұ�



                    // �÷��̾� ���� �ʱ�ȭ
                    {
                        if (GameData.gameMode == GameMode.Mode.None)
                            return;
                        else if (GameData.gameMode == GameMode.Mode.Online)
                        {
                            // �̱��� : ���� ���� �ʿ�===========
                        }
                        else if (GameData.player.allPlayer.Count == 0)
                        {
                            // �ߺ� ���� �۾�
                            List<int> picked = Tool.RandomNotCross(1, Character.table.Count, 4);
                            if (picked.Contains(GameData.player.me.character.index))
                                picked.Remove(GameData.player.me.character.index);

                            // �ʱ�ȭ ����
                            GameData.player.player_1 = GameData.player.me;
                            GameData.player.player_2 = new Player(Player.Type.AI, picked[0], false, "Player02");
                            GameData.player.player_3 = new Player(Player.Type.AI, picked[1], false, "Player03");
                            GameData.player.player_4 = new Player(Player.Type.AI, picked[2], false, "Player04");

                            // �÷��̾� ����Ʈ ����
                            GameData.player.allPlayer.Add(GameData.player.player_1);
                            GameData.player.allPlayer.Add(GameData.player.player_2);
                            GameData.player.allPlayer.Add(GameData.player.player_3);
                            GameData.player.allPlayer.Add(GameData.player.player_4);

                            // �ӽ� ť ����
                            //for(int i = 0; i < GameData.player.allPlayer.Count; i++)
                            //    GameData.turn.queue.Enqueue(GameData.player.allPlayer[i]);
                        }
                    }


                    // ĳ���� ���� �� ĳ���� ������ �ε�
                    for (int i = 0; i < GameData.player.allPlayer.Count; i++)
                        if (GameData.player.allPlayer[i].avatar == null)
                        {
                            // ĳ���� ����
                            GameData.player.allPlayer[i].CreateAvatar(characterParent);
                            GameData.player.allPlayer[i].avatar.name = "p" + (i + 1);

                            // ĳ���� ������ �ε�
                            GameData.player.allPlayer[i].LoadFace();
                        }


                    Debug.Log("���� �÷ο� :: �� ���� ȣ�� Ȯ�ε�");
                    GameData.gameFlow = Flow.Start;
                    break;
                }


            // ���� ���۵�
            case Flow.Start:
                {
                    // �ε����� ��� ���
                    if (!loadingManager.isFinish || !loadingManager.isFadeFinish)
                        return;


                    // �� �ѷ�����
                    // �̱���==================                

                    Debug.Log("���� �÷ο� :: ���� ���� Ȯ�ε�");
                    GameData.gameFlow = Flow.Ordering;
                    break;
                }


            // �����ֻ���
            case Flow.Ordering:
                // �����ֻ��� ������
                {

                    //Debug.Log("���� �÷ο� :: ���� �ֻ��� Ȯ����");

                    // �ֻ����� �ƹ��� ������ ������
                    if (diceController.isFree)
                    {

                        // �÷��̾ üũ
                        for (int i = 0; i < GameData.player.allPlayer.Count; i++)
                        {
                            // �̹� �������� ���� �÷��̾� ó��
                            if (GameData.player.allPlayer[i].dice.isRolled)
                                continue;

                            // �ش� �÷��̾ ������ ���� ������ �ֻ��� ȣ��
                            if (!GameData.player.allPlayer[i].dice.isRolling)
                            {
                                Debug.Log(string.Format("���� �÷ο� :: Player{0} �ֻ��� ��������", i+1));
                                // �ֻ��� ��� ȣ��
                                diceController.CallDice(
                                    GameData.player.allPlayer[i],
                                    GameData.player.allPlayer[i].avatar.transform
                                    );

                                // �ٸ� �÷��̾� ����
                                break;
                            }
                        }
                    }

                    // �ֻ��� ������
                    if (diceController.isFinish)
                        diceController.UseDice();

                    // ��ΰ� �ֻ��� ������ ������ �ߴ�
                    for (int i = 0; i < GameData.player.allPlayer.Count; i++)
                        if (!GameData.player.allPlayer[i].dice.isRolled)
                            return;
                    
                    Debug.Log("���� �÷ο� :: ��� �÷��̾� �ֻ��� ���� �Ϸ� =>" + GameData.player.allPlayer.Count);

                    // ��� �÷��̾� �ֻ��� �����Ϸ� ���� �ʱ�ȭ
                    for (int i = 0; i < GameData.player.allPlayer.Count; i++)
                        GameData.player.allPlayer[i].dice.isRolled = false;


                    // ������ ����Ʈ ����
                    List<Player> pOrderList = new List<Player>(GameData.player.allPlayer);
                    Debug.Log("���� �÷ο� :: ����Ʈ ���� üũ =>" + pOrderList.Count + " = " + GameData.player.allPlayer.Count);

                    // ����ť ���� �� ����Ʈ ���� ����
                    GameData.turn.SetUp(pOrderList);

                    // PlayerInfo UI �ʱ�ȭ
                    for (int i = 0; i < pOrderList.Count; i++)
                    {
                        Debug.Log("���� �÷ο� :: ����� =>" + pOrderList[i].name);
                        pOrderList[i].infoUI = playerInfoUI[i];
                        Debug.Log("���� �÷ο� :: ����� =>" + pOrderList[i].infoUI.transform.name);
                        Debug.Log("���� �÷ο� :: ����� =>" + pOrderList[i].infoUI.transform.name);
                        pOrderList[i].infoUI.SetPlayer(pOrderList[i]);
                    }

                    // PlayerInfo UI Ȱ��
                    StartCoroutine(Tool.CanvasFade(MainUI, true, 1.5f));


                    //GameData.gameFlow = Flow.CycleStart;
                    GameData.gameFlow = Flow.Opening;
                    break;
                }


            // ����Ŭ�� ���� �� ������ �ܰ�
            case Flow.Opening:
                {
                    // ���� �ϴ� ����


                    GameData.gameFlow = Flow.Cycling;
                    break;
                }


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


    void TurnWork()
    {
        // �α� ���
        Debug.Log("�÷��̾� ȣ�� :: " + GameData.turn.now.name);

        // ���� ���� üũ


        // �ý��� �÷��̾� - ��Ÿ��
        if (GameData.turn.now == GameData.player.system.Starter)
        {
            // ���� ���� �� ���� �ʱ�ȭ

            // �� ���� ó��
            GameData.turn.Next();
        }
        // �ý��� �÷��̾� - �̴ϰ���
        else if(GameData.turn.now == GameData.player.system.Minigame)
        {
            // �̴ϰ��� ����

            // �� ���� ó��
            GameData.turn.Next();

            // �̴ϰ��� �ε�
        }
        // �ý��� �÷��̾� - �̴ϰ��� ����
        else if (GameData.turn.now == GameData.player.system.MinigameEnder)
        {
            // �̴ϰ��� ����

            // �� ���� ó��
            GameData.turn.Next();
        }
        // �ý��� �÷��̾� - ����
        else if (GameData.turn.now == GameData.player.system.Ender)
        {
            // ���� �б� üũ
            
            // ����Ŭ ����

            // �� ���� ó��
            GameData.turn.Next();
            Debug.Break();
        }
        // ���� �÷��̾�
        else
        {
            PlayerWork();
        }
    }


    void PlayerWork()
    {
        // �α� ���
        Debug.Log("�� ���� :: " + GameData.turn.now.name);

        // �ʱ�ȭ
        if (GameData.turn.turnAction == Turn.TurnAction.Wait)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ


                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // ���� ����


                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {
                // ���� �۾�


                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����


                // ��ŵ
                GameData.turn.turnAction = Turn.TurnAction.Opening;
                GameData.turn.actionProgress = ActionProgress.Ready;
            }

        }
        // ���� ����
        else if (GameData.turn.turnAction == Turn.TurnAction.Opening)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ


                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // ���� üũ

                // ��� �÷��̾� ��� ������ üũ
                CheckLife();

                // �ൿ �Ұ��� üũ
                if (GameData.turn.now.isStun)
                {
                    // �� �ߴ� �� ���� ����� �̵�
                    GameData.turn.turnAction = Turn.TurnAction.Ending;
                    GameData.turn.actionProgress = ActionProgress.Ready;
                    return;
                }


                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {
                // ���� ����


                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {


                // ��ŵ
                GameData.turn.turnAction = Turn.TurnAction.Item;
                GameData.turn.actionProgress = ActionProgress.Ready;
            }

        }
        // �ֻ��� ������
        else if (GameData.turn.turnAction == Turn.TurnAction.DiceRolling)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ


                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {
                // ������ ������ ������ ��� ��ư Ȱ��

                // �ֻ����� �������� �ߴ�
                if (diceController.isBusy)
                    return;

                // �ش� �÷��̾ ������ ���� ������ �ֻ��� ȣ��
                else if (diceController.isFree)
                {
                    Debug.Log(string.Format("���� �÷ο� :: Player({0}) �ֻ��� ��������", GameData.turn.now.name));

                    // �ֻ��� ��� ȣ��
                    diceController.CallDice(
                        GameData.turn.now,
                        GameData.turn.now.avatar.transform
                        );
                }

                // ������ ����
                else if(useItemOrder)
                {
                    // �ð� ����
                    diceController.isTimeCountWork = false;

                    // ������ ��� �ܰ�� ����
                    GameData.turn.turnAction = Turn.TurnAction.Item;
                    GameData.turn.actionProgress = ActionProgress.Ready;
                }

                // �ֻ��� ������
                else if (diceController.isFinish)
                {
                    // ��� ó��
                    diceController.UseDice();

                    // �ֻ��� ���̻� ������ ��ŵ
                    if (diceController.owner.dice.count <= 0)
                        GameData.turn.actionProgress = ActionProgress.Finish;
                }
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {


                // ��ŵ
                GameData.turn.turnAction = Turn.TurnAction.Plan;
                GameData.turn.actionProgress = ActionProgress.Ready;
            }

        }
        // ������ ��� �ܰ�
        else if (GameData.turn.turnAction == Turn.TurnAction.Item)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ


                // �Ҹ�ó��

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                Debug.LogWarning("������ ����");


                // ����

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {
                // ȿ�� ����

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                Debug.LogWarning("������ ��� ����");

                // ��� ���� ����


                // �ٽ� �ð� �帧
                diceController.isTimeCountWork = true;

                // �ٽ� �ֻ��� ���� ����
                GameData.turn.turnAction = Turn.TurnAction.DiceRolling;
                GameData.turn.actionProgress = ActionProgress.Working;
            }

        }
        // �׼� ��ȹ �ܰ�
        else if (GameData.turn.turnAction == Turn.TurnAction.Plan)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // ���� ����

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����

                
                // �������� ��ŵ
                GameData.turn.turnAction = Turn.TurnAction.Action;
                GameData.turn.actionProgress = ActionProgress.Working;
            }

        }
        // �׼� �ܰ�
        else if (GameData.turn.turnAction == Turn.TurnAction.Action)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // ���� ����

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����


                // �������� ��ŵ
                GameData.turn.turnAction = Turn.TurnAction.Block;
                GameData.turn.actionProgress = ActionProgress.Working;
            }

        }
        // ��� ��� ���� �ܰ�
        else if (GameData.turn.turnAction == Turn.TurnAction.Block)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // ���� ����

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {
                // ��� ��� ����===================�̱���
                BlockWork();

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����


                // �������� ��ŵ
                GameData.turn.turnAction = Turn.TurnAction.Ending;
                GameData.turn.actionProgress = ActionProgress.Working;
            }

        }
        // ���� ���� �ܰ�
        else if (GameData.turn.turnAction == Turn.TurnAction.Ending)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // ���� ����

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����


                // �������� ��ŵ
                GameData.turn.turnAction = Turn.TurnAction.Finish;
                GameData.turn.actionProgress = ActionProgress.Working;
            }

        }
        // ���� �ܰ�
        else if (GameData.turn.turnAction == Turn.TurnAction.Finish)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // ���� ����

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����


                // �� ���� ó��
                GameData.turn.Next();
            }

        }
    }


    void BlockWork()
    {
    }



    /// <summary>
    /// �������� üũ�Ͽ� �������� ����
    /// </summary>
    public void CheckLife()
    {
        // üũ
        for (int i = 0; i < GameData.player.allPlayer.Count; i++)
            if (GameData.player.allPlayer[i].life.checkMin(1))
                GotoPrison(GameData.player.allPlayer[i]);
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
