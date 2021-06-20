using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster script = null;

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

    // ����Ŭ UI ���� ��ũ��Ʈ
    public CycleManager cycleManager = null;

    // �޽��� �ڽ� ��ũ��Ʈ
    public MessageBox messageBox = null;

    // �ֻ��� ��Ʈ�ѷ� ��ũ��Ʈ
    [SerializeField]
    DiceController diceController = null;

    // ������ ���� ��ũ��Ʈ
    public ItemManager itemManager = null;

    // ĳ���� ������Ʈ �θ�
    [SerializeField]
    Transform characterParent = null;

    // PlayerInfo UI ������Ʈ
    [Space]
    public List<PlayerInfoUI> playerInfoUI = new List<PlayerInfoUI>();
    public CanvasGroup MainUI = null;

    // �÷��̾� ���ñ�
    public List<Transform> playerSelecter = new List<Transform>();


    // ������ ��� ���
    public bool useItemOrder = false;


    private void Awake()
    {
        // �� ���
        script = this;
    }

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


                    // �߷� ����
                    Physics.gravity = Physics.gravity *10;


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

                            // �÷��̾ "�ٸ� �÷��̾�" ����
                            for(int i = 0; i < GameData.player.allPlayer.Count; i++)
                            {
                                // �� ���� (�����)
                                Player temp = GameData.player.allPlayer[i];

                                // ��� �÷��̾� ���
                                for (int j = 0; j < GameData.player.allPlayer.Count; j++)
                                    temp.otherPlayers.Add(GameData.player.allPlayer[j]);

                                // ����� ���� ����
                                temp.otherPlayers.Remove(temp);
                            }

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

                            // ĳ���� �̵�
                            GameData.player.allPlayer[i].avatar.transform.position = GameData.blockManager.startBlock.position;
                                                       
                            // ĳ���� ������ �ε�
                            GameData.player.allPlayer[i].LoadFace();
                        }

                    // ĳ���� ��ħ �ؼ�
                    GameData.player.allPlayer[0].avatar.GetComponent<CharacterMover>().AvatarOverFix();


                    // PlayerInfo UI Ȱ��
                    MainUI.GetComponent<CanvasGroup>().alpha = 0f;
                    MainUI.blocksRaycasts = false;


                    // ���� ���
                    ShortcutManager.script.SetUp();


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

                    // PlayerInfo UI �ʱ�ȭ
                    for (int i = 0; i < pOrderList.Count; i++)
                    {
                        Debug.Log("���� �÷ο� :: ����� =>" + pOrderList[i].name);
                        pOrderList[i].infoUI = playerInfoUI[i];
                        Debug.Log("���� �÷ο� :: ����� =>" + pOrderList[i].infoUI.transform.name);
                        Debug.Log("���� �÷ο� :: ����� =>" + pOrderList[i].infoUI.transform.name);
                        pOrderList[i].infoUI.SetPlayer(pOrderList[i]);
                    }

                    // ����ť ���� �� ����Ʈ ���� ����
                    GameData.turn.SetUp(pOrderList);

                    // PlayerInfo UI Ȱ��
                    StartCoroutine(Tool.CanvasFade(MainUI, true, 1.5f));
                    MainUI.blocksRaycasts = true;


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
        //Debug.Log("�÷��̾� ȣ�� :: " + GameData.turn.now.name);

        // ���� ���� üũ

        // ������ üũ
        for (int i = 0; i < GameData.player.allPlayer.Count; i++)
        {
            // ������ 0 �Ǵ� ������ ���
            if (GameData.player.allPlayer[i].life.Value < 1)
            {
                // �̹� ������ ��� �ߴ�
                if (GameData.player.allPlayer[i].isDead)
                {
                    //Debug.LogError("������ üũ :: �̹� ������ => " + GameData.player.allPlayer[i].name);
                    continue;
                }

                //Debug.LogError("������ üũ :: ������ => " + GameData.player.allPlayer[i].name);
                GameData.player.allPlayer[i].movement.GotoJail();
            }
        }

        // �ý��� �÷��̾� - ��Ÿ��
        if (GameData.turn.now == GameData.player.system.Starter)
        {
            // ���� ���� �� ���� �ʱ�ȭ

            // ��� �÷��̾� �ֻ��� �ʱ�ȭ
            for (int i = 0; i < GameData.player.allPlayer.Count; i++)
                GameData.player.allPlayer[i].dice.Clear();

            // ����Ŭ UI ����
            cycleManager.Refresh();

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
            GameData.cycle.NextCycle();

            // �� ���� ó��
            GameData.turn.Next();
            //Debug.Break();
        }
        // ���� �÷��̾�
        else
        {
            PlayerWork();
        }
    }


    void PlayerWork()
    {
        // �ʱ�ȭ
        if (GameData.turn.turnAction == Turn.TurnAction.Wait)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // �α� ���
                Debug.Log("�� ���� :: " + GameData.turn.turnAction + " & " + GameData.turn.actionProgress + " :: " + GameData.turn.now.name);


                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Start;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Start)
            {
                // ���� üũ

                // ��Ȱ üũ
                if (GameData.turn.now.isDead)
                    if (GameData.turn.now.stunCount <= 0)
                        GameData.turn.now.Resurrect();

                // �ൿ ���� ���� üũ
                if (GameData.turn.now.isStun)
                {
                    // ���� ��� �� ����
                    GameData.turn.now.stunCount--;

                    // �ൿ�� ��Ż
                    GameData.turn.turnAction = Turn.TurnAction.Ending;
                }

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

                // �ֻ��� ����
                GameData.turn.now.dice.count = 1;
                Debug.Log("�ֻ��� ���� :: " + GameData.turn.now.dice.count);


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
                // ���� �غ�


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
                GameData.turn.turnAction = Turn.TurnAction.DiceRolling;
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
                    if (diceController.owner == null)   // �ֻ��� 0���� ��� ������ ��Ż��
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
                GameData.turn.actionProgress = ActionProgress.Ready;
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
                // �׼� �����ٸ�
                Debug.LogError(GameData.turn.now.dice.valueTotal);
                GameData.turn.now.movement.PlanMoveBy(
                    GameData.turn.now.dice.valueTotal
                    );

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����

                
                // �������� ��ŵ
                GameData.turn.turnAction = Turn.TurnAction.Action;
                GameData.turn.actionProgress = ActionProgress.Ready;
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

                // ī�޶� ����
                GameData.worldManager.cameraManager.CamMoveTo(GameData.turn.now.avatar.transform, CameraManager.CamAngle.Top);

                // ��ŵ
                GameData.turn.actionProgress = ActionProgress.Working;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Working)
            {
                // ��ũ��Ʈ �� ���
                CharacterMover movement = GameData.turn.now.movement;

                // �׼� �̼��� ���
                if (movement.actNow.type == Action.ActionType.None)
                {
                    // �ܿ� �׼� ����
                    if (movement.actionsQueue.Count > 0)
                        movement.GetAction();

                    // ��� �׼� ����
                    else
                    {
                        // ī�޶� Ż��
                        GameData.worldManager.cameraManager.CamFree();

                        // ��ǥ ����
                        movement.location = movement.location+GameData.turn.now.dice.valueTotal;

                        // ��ħ ����
                        movement.AvatarOverFix();

                        // �ʱ�ȭ
                        movement.actNow = new Action();

                        // ��ŵ
                        GameData.turn.actionProgress = ActionProgress.Finish;
                    }
                }
                // �׼� ������
                else if (!movement.actNow.isFinish)
                {
                    //Debug.LogWarning("�׼� ������");

                    // �̵� ó��
                    if (movement.actNow.type == Action.ActionType.Move)
                        movement.MoveByAction(ref movement.actNow);

                    // ��ֹ� ó��
                    if (movement.actNow.type == Action.ActionType.Barricade)
                        movement.CheckBarricade(ref movement.actNow);

                    // ���� ó��
                    if (movement.actNow.type == Action.ActionType.Attack)
                        movement.AttackPlayer(ref movement.actNow);

                }
                // �׼� ����
                else if (movement.actNow.isFinish)
                {
                    //Debug.LogWarning("�׼� �����");

                    // �׼� �Ұ�
                    movement.actNow = new Action();
                }

            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����

                // �������� ��ŵ
                GameData.turn.turnAction = Turn.TurnAction.Block;
                GameData.turn.actionProgress = ActionProgress.Ready;
            }

        }
        // ��� ��� ���� �ܰ�
        else if (GameData.turn.turnAction == Turn.TurnAction.Block)
        {
            if (GameData.turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // ��� ��� �ʱ�ȭ
                BlockWork.Clear();

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
                // ��� ��� ����===================������
                if (!BlockWork.isWork)
                    BlockWork.Work(GameData.turn.now);

                // ��ŵ
                if (BlockWork.isEnd)
                    GameData.turn.actionProgress = ActionProgress.Finish;
            }
            else if (GameData.turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����


                // �������� ��ŵ
                GameData.turn.turnAction = Turn.TurnAction.Ending;
                GameData.turn.actionProgress = ActionProgress.Ready;
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
                GameData.turn.actionProgress = ActionProgress.Ready;
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

                // ���� �ʱ�ȭ
                GameData.turn.turnAction = Turn.TurnAction.Wait;
                GameData.turn.actionProgress = ActionProgress.Ready;


                // �� ���� ó��
                GameData.turn.Next();
            }

        }
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
