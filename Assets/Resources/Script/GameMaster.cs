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
    public DiceController diceController = null;

    // ������ ���� ��ũ��Ʈ
    public ItemManager itemManager = null;

    // �̺�Ʈ ���� ��ũ��Ʈ
    public EventManager eventManager = null;

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
    public static bool useItemOrder = false;

    // �� ��ε� �����
    public static Flow flowCopy = Flow.Wait;


    private void Awake()
    {
        // �� ���
        script = this;

        // ��ε� ����� ��� �� ����
        flowCopy = GameData.gameFlow;
        GameData.gameFlow = Flow.Wait;
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

                    // ���̺� ���� �ε�
                    if (GameSaver.useLoad)
                        GameSaver.LoadGameInfo();

                    // ���̺����� �ۼ�
                    // �̱���================
                    // �������� �����Ұ�


                    // �߷� ����
                    if (flowCopy == Flow.Wait)
                        Physics.gravity = Physics.gravity * 10;


                    // �÷��̾� ���� �ʱ�ȭ
                    {
                        if (GameData.gameMode == GameMode.Mode.None)
                            return;
                        else if (GameData.gameMode == GameMode.Mode.Online)
                        {
                            // �̱��� : ���� ���� �ʿ�===========
                        }
                        else if (Player.allPlayer.Count == 0)
                        {
                            // �ߺ� ���� �۾�
                            List<int> picked = Tool.RandomNotCross(1, Character.table.Count, 4);
                            if (picked.Contains(Player.me.character.index))
                                picked.Remove(Player.me.character.index);

                            // �÷��̾� �ʱ�ȭ �� ����Ʈ ����
                            Player.player_1 = Player.me;
                            Player.allPlayer.Add(Player.player_1);

                            if (GameRule.playerCount >= 2)
                            {
                                Player.player_2 = new Player(Player.Type.AI, picked[0], true, "Player02");
                                Player.allPlayer.Add(Player.player_2);
                            }

                            if (GameRule.playerCount >= 3)
                            {
                                Player.player_3 = new Player(Player.Type.AI, picked[1], true, "Player03");
                                Player.allPlayer.Add(Player.player_3);
                            }

                            if (GameRule.playerCount >= 4)
                            {
                                Player.player_4 = new Player(Player.Type.AI, picked[2], true, "Player04");
                                Player.allPlayer.Add(Player.player_4);
                            }
                        }
                    }


                    // ĳ���� ���� �� ĳ���� ������ �ε�
                    for (int i = 0; i < Player.allPlayer.Count; i++)
                    {
                        // ��� �÷��̾�
                        Player current = Player.allPlayer[i];

                        // ĳ���� ������ �ε�
                        current.LoadFace();

                        // "�ٸ� �÷��̾�" ����
                        {
                            // ��� �÷��̾� ���
                            for (int j = 0; j < Player.allPlayer.Count; j++)
                                current.otherPlayers.Add(Player.allPlayer[j]);

                            // ����� ���� ����
                            current.otherPlayers.Remove(current);
                        }

                        // ĳ���� ����
                        if (Player.allPlayer[i].avatar == null)
                        {

                            // ĳ���� ����
                            current.CreateAvatar(characterParent);
                            current.avatar.name = "p" + (i + 1);

                            // ĳ���� �̵�
                            if (current.movement.location == -1)
                                current.avatar.transform.position = GameData.blockManager.startBlock.position;
                            else
                                current.avatar.transform.position = GameData.blockManager.GetBlock(current.movement.location).transform.position;
                        }
                    }

                    
                    // PlayerInfo UI ��Ȱ��
                    MainUI.GetComponent<CanvasGroup>().alpha = 0f;
                    MainUI.blocksRaycasts = false;


                    // ���� ���
                    ShortcutManager.script.SetUp();

                    // ����Ŭ ����
                    Cycle.goal = GameRule.cycleMax;


                    // ���̺� ���� �ε�
                    if (GameSaver.useLoad)
                    {
                        GameSaver.LoadPlayer();
                        GameSaver.LoadItemObject();
                        GameSaver.LoadEventObject();
                    }

                    // ĳ���� ��ħ �ؼ�
                    //Player.me.movement.AvatarOverFix();
                    CharacterMover.AvatarOverFixAll();


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
                    // �� ��ε� ����
                    if (flowCopy <= Flow.Ordering && !GameSaver.useLoad)
                    {
                        //Debug.Log("���� �÷ο� :: ���� �ֻ��� Ȯ����");

                        // �ֻ����� �ƹ��� ������ ������
                        if (diceController.isFree)
                        {

                            // �÷��̾ üũ
                            for (int i = 0; i < Player.allPlayer.Count; i++)
                            {
                                Player current = Player.allPlayer[i];

                                // �̹� �������� ���� �÷��̾� ó��
                                if (current.dice.isRolled)
                                    continue;

                                // �ش� �÷��̾ ������ ���� ������ �ֻ��� ȣ��
                                if (!current.dice.isRolling)
                                {
                                    Debug.Log(string.Format("���� �÷ο� :: Player{0} �ֻ��� ��������", i + 1));

                                    // �ֻ��� ����
                                    Player.allPlayer[i].dice.count = 1;

                                    // �ֻ��� ��� ȣ��
                                    diceController.CallDice(
                                        current,
                                        current.avatar.transform
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
                        for (int i = 0; i < Player.allPlayer.Count; i++)
                            if (!Player.allPlayer[i].dice.isRolled)
                                return;

                        Debug.Log("���� �÷ο� :: ��� �÷��̾� �ֻ��� ���� �Ϸ� =>" + Player.allPlayer.Count);

                    }
                    else
                    {
                        // �� ��ε�� Opening ��ŵ
                        // �Ʒ� PlayerInfo UI �ʱ�ȭ ������ return�ϸ� �ȵ�
                        GameData.gameFlow = Flow.Cycling;
                    }


                    // ������ ����Ʈ ����
                    List<Player> pOrderList = new List<Player>(Player.allPlayer);
                    //Debug.Log("���� �÷ο� :: ����Ʈ ���� üũ =>" + pOrderList.Count + " = " + Player.allPlayer.Count);

                    // ����ť ���� �� ����Ʈ ���� ����
                    Turn.SetUp(pOrderList);

                    // ��� �÷��̾� �ֻ��� �����Ϸ� ���� �ʱ�ȭ
                    for (int i = 0; i < Player.allPlayer.Count; i++)
                        Player.allPlayer[i].dice.Clear();

                    // PlayerInfo UI �ʱ�ȭ
                    for (int i = 0; i < pOrderList.Count; i++)
                    {
                        pOrderList[i].infoUI = playerInfoUI[i];
                        pOrderList[i].infoUI.SetPlayer(pOrderList[i]);
                    }

                    // ���Ҵ� PlayerInfo UI ����
                    for (int i = Player.allPlayer.Count; i < playerInfoUI.Count; i++)
                        playerInfoUI[i].gameObject.SetActive(false);

                    // ���̺� ���� �ε�
                    if (GameSaver.useLoad)
                    {
                        GameSaver.LoadPlayerInventory();

                        // ��ŸƮ �÷��̾� ���� ȣ��
                        TurnWork();

                        GameSaver.LoadTurn();
                    }
                    GameSaver.Clear();

                    //// ����ť ���� �� ����Ʈ ���� ����
                    //Turn.SetUp(pOrderList);

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


                    // �� ����
                    if(Cycle.now == 0)
                        Cycle.now = 1;

                    GameData.gameFlow = Flow.Cycling;
                    break;
                }


            // ���� ����
            case Flow.Cycling:
                if (Cycle.isEnd())
                {
                    Debug.Log("���� �÷ο� :: ���� ���� �޼� Ȯ�ε� => by " + Turn.now.name);
                    GameData.gameFlow = Flow.End;
                }
                else
                {
                    TurnWork();
                }
                break;


            /*
        case Flow.Turn:
            if (Turn.now == Player.system.Minigame)
                GameData.gameFlow = Flow.MiniGameStart;
            else if (Turn.now == Player.system.Ender)
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
            if(Cycle.isEnd())
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
        //Debug.Log("�÷��̾� ȣ�� :: " + Turn.now.name);

        // ���� ���� üũ

        // ������ üũ
        for (int i = 0; i < Player.allPlayer.Count; i++)
        {
            // ������ 0 �Ǵ� ������ ���
            if (Player.allPlayer[i].life.Value < 1)
            {
                // �̹� ������ ��� �ߴ�
                if (Player.allPlayer[i].isDead)
                {
                    //Debug.LogError("������ üũ :: �̹� ������ => " + Player.allPlayer[i].name);
                    continue;
                }

                //Debug.LogError("������ üũ :: ������ => " + Player.allPlayer[i].name);
                Player.allPlayer[i].movement.GotoJail();
            }
        }

        // �ý��� �÷��̾� - ��Ÿ��
        if (Turn.now == Player.system.Starter)
        {
            // ���� ���� �� ���� �ʱ�ȭ

            // ��� �÷��̾� �ֻ��� �ʱ�ȭ
            for (int i = 0; i < Player.allPlayer.Count; i++)
                Player.allPlayer[i].dice.Clear();

            // ����Ŭ UI ����
            cycleManager.Refresh();

            // �� ���� ó��
            Turn.Next();
        }
        // �ý��� �÷��̾� - �̴ϰ���
        else if(Turn.now == Player.system.Minigame)
        {
            // �̴ϰ��� ����

            // �� ���� ó��
            Turn.Next();

            // �̴ϰ��� �ε�
        }
        // �ý��� �÷��̾� - �̴ϰ��� ����
        else if (Turn.now == Player.system.MinigameEnder)
        {
            // �̴ϰ��� ����

            // �� ���� ó��
            Turn.Next();
        }
        // �ý��� �÷��̾� - ����
        else if (Turn.now == Player.system.Ender)
        {
            // ���� �б� üũ



            // ����Ŭ ����
            Cycle.NextCycle();

            // �� ���� ó��
            Turn.Next();
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
        if (Turn.turnAction == Turn.TurnAction.Wait)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // �α� ���
                Debug.Log("�� ���� :: " + Turn.turnAction + " & " + Turn.actionProgress + " :: " + Turn.now.name);


                // ��ŵ
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                // ���� üũ

                // ��Ȱ üũ
                if (Turn.now.isDead)
                    if (Turn.now.stunCount <= 0)
                        Turn.now.Resurrect();

                // �ൿ ���� ���� üũ
                if (Turn.now.isStun)
                {
                    // ���� ��� �� ����
                    Turn.now.stunCount--;

                    // �ൿ�� ��Ż
                    Turn.turnAction = Turn.TurnAction.Ending;
                }

                // ��� �÷��̾� ��� ������ üũ
                CheckLife();

                // �ൿ �Ұ��� üũ
                if (Turn.now.isStun)
                {
                    // �� �ߴ� �� ���� ����� �̵�
                    Turn.turnAction = Turn.TurnAction.Ending;
                    Turn.actionProgress = ActionProgress.Ready;
                    return;
                }

                // �ֻ��� ����
                Turn.now.dice.count = 1;
                Debug.Log("�ֻ��� ���� :: " + Turn.now.dice.count);


                // ī�޶� ���� ��Ŀ��
                if (Player.me == Turn.now)
                    GameData.worldManager.cameraManager.LockBtn.isOn = true;


                // ��ŵ
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {
                // ���� �۾�


                // ��ŵ
                Turn.actionProgress = ActionProgress.Finish;
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����


                // ��ŵ
                Turn.turnAction = Turn.TurnAction.Opening;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // ���� ����
        else if (Turn.turnAction == Turn.TurnAction.Opening)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ


                // ��ŵ
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                // ���� �غ�


                // ��ŵ
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {
                // ���� ����


                // ��ŵ
                Turn.actionProgress = ActionProgress.Finish;
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {


                // ��ŵ
                Turn.turnAction = Turn.TurnAction.DiceRolling;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // �ֻ��� ������
        else if (Turn.turnAction == Turn.TurnAction.DiceRolling)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ


                // ��ŵ
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {

                // ��ŵ
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {

                // ������ ����
                if (useItemOrder)
                {
                    // �ð� ����
                    diceController.isTimeCountWork = false;

                    // ������ ��� �ܰ�� ����
                    Turn.turnAction = Turn.TurnAction.Item;
                    Turn.actionProgress = ActionProgress.Ready;
                }

                // �ֻ����� �������� �ߴ�
                else if (diceController.isBusy)
                    return;

                // �ش� �÷��̾ ������ ���� ������ �ֻ��� ȣ��
                else if (diceController.isFree)
                {
                    Debug.Log(string.Format("���� �÷ο� :: Player({0}) �ֻ��� ��������", Turn.now.name));

                    // �ֻ��� ��� ȣ��
                    diceController.CallDice(
                        Turn.now,
                        Turn.now.avatar.transform
                        );
                }

                // �ֻ��� ������
                else if (diceController.isFinish)
                {
                    // ��� ó��
                    diceController.UseDice();

                    // �ֻ��� ���̻� ������ ��ŵ
                    if (diceController.owner == null)   // �ֻ��� 0���� ��� ������ ��Ż��
                        Turn.actionProgress = ActionProgress.Finish;
                }
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {


                // ��ŵ
                Turn.turnAction = Turn.TurnAction.Plan;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // ������ ��� �ܰ�
        else if (Turn.turnAction == Turn.TurnAction.Item)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ


                // ��ŵ
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                Debug.LogWarning("������ ����");

                // ��� �غ� ����



                // ������ ���
                itemManager.ItemUse(itemManager.selected);
                //itemManager.ItemUseByUI();

                // ��ŵ
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {
                // ȿ�� ����



                // ������ ����� ���ó��
                if (useItemOrder)
                    return;


                // ��ŵ
                Debug.LogWarning("������ ��� ����");
                Turn.actionProgress = ActionProgress.Finish;
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {

                // ��� ���� ����





                // �ٽ� �ð� �帧
                diceController.isTimeCountWork = true;

                // �ٽ� �ֻ��� ���� ����
                Turn.turnAction = Turn.TurnAction.DiceRolling;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // �׼� ��ȹ �ܰ�
        else if (Turn.turnAction == Turn.TurnAction.Plan)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // ��ŵ
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                // ���� ����

                // ��ŵ
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {
                // �׼� �����ٸ�
                Debug.LogWarning( "�׼� �����ٸ� :: �� �̵��� => " +Turn.now.dice.valueTotal);
                Turn.now.movement.PlanMoveBy(
                    Turn.now.dice.valueTotal
                    );

                // ��ŵ
                Turn.actionProgress = ActionProgress.Finish;
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����

                
                // �������� ��ŵ
                Turn.turnAction = Turn.TurnAction.Action;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // �׼� �ܰ�
        else if (Turn.turnAction == Turn.TurnAction.Action)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // ��ŵ
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                // ���� ����

                // ī�޶� ����
                GameData.worldManager.cameraManager.CamMoveTo(Turn.now.avatar.transform, CameraManager.CamAngle.Top);

                // ��ŵ
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {
                // ��ũ��Ʈ �� ���
                CharacterMover movement = Turn.now.movement;

                // �׼� �̼��� ���
                if (movement.actNow.type == Action.ActionType.None)
                {
                    // �ܿ� �׼� ����
                    if (movement.actionsQueue.Count == 0)
                    {
                        // ī�޶� Ż��
                        GameData.worldManager.cameraManager.CamFree();

                        // ��ŵ
                        Turn.actionProgress = ActionProgress.Finish;
                    }
                }

            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����

                // �������� ��ŵ
                Turn.turnAction = Turn.TurnAction.Block;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // ��� ��� ���� �ܰ�
        else if (Turn.turnAction == Turn.TurnAction.Block)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // ��� ��� �ʱ�ȭ
                BlockWork.Clear();

                // ��ŵ
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {

                // ��ŵ
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {
                // ��� ��� ����
                if (!BlockWork.isWork)
                    BlockWork.Work(Turn.now);

                // ��ŵ
                if (BlockWork.isEnd)
                {
                    if (messageBox.gameObject.activeSelf)
                    {
                        Debug.LogError("�� ���� �ڴ�� �Ѿ��?");
                        Debug.Break();
                    }
                    Turn.actionProgress = ActionProgress.Finish;
                }
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����


                // �������� ��ŵ
                Turn.turnAction = Turn.TurnAction.Ending;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // ���� ���� �ܰ�
        else if (Turn.turnAction == Turn.TurnAction.Ending)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // ��ŵ
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                // ���� ����

                // ��ŵ
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {

                // ��ŵ
                Turn.actionProgress = ActionProgress.Finish;
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����


                // �������� ��ŵ
                Turn.turnAction = Turn.TurnAction.Finish;
                Turn.actionProgress = ActionProgress.Ready;
            }

        }
        // ���� �ܰ�
        else if (Turn.turnAction == Turn.TurnAction.Finish)
        {
            if (Turn.actionProgress == ActionProgress.Ready)
            {
                // ���� �ʱ�ȭ

                // ��ŵ
                Turn.actionProgress = ActionProgress.Start;
            }
            else if (Turn.actionProgress == ActionProgress.Start)
            {
                // ���� ����

                // ��ŵ
                Turn.actionProgress = ActionProgress.Working;
            }
            else if (Turn.actionProgress == ActionProgress.Working)
            {

                // ��ŵ
                Turn.actionProgress = ActionProgress.Finish;
            }
            else if (Turn.actionProgress == ActionProgress.Finish)
            {
                // ���� ����

                // ���� �ʱ�ȭ
                Turn.turnAction = Turn.TurnAction.Wait;
                Turn.actionProgress = ActionProgress.Ready;


                // �� ���� ó��
                Turn.Next();
            }

        }
    }



    /// <summary>
    /// �������� üũ�Ͽ� �������� ����
    /// </summary>
    public void CheckLife()
    {
        // üũ
        for (int i = 0; i < Player.allPlayer.Count; i++)
            if (Player.allPlayer[i].life.checkMin(1))
                GotoPrison(Player.allPlayer[i]);
    }

    /// <summary>
    /// Ư�� �÷��̾ �������� ����
    /// </summary>
    /// <param name="targetPlayer"></param>
    public void GotoPrison(Player targetPlayer)
    {
        targetPlayer.movement.GotoJail();
    }


    public void SaveGame()
    {
        GameSaver.GameSave();
    }
    public void StopMove()
    {
        Turn.now.movement.MoveStop();
    }
}
