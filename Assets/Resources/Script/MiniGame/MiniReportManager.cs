using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniReportManager : MonoBehaviour
{
    public List<MiniReportSlot> reportSlots = new List<MiniReportSlot>();

    public CanvasGroup curtain = null;
    public Button endBtn = null;

    [Header("Rank 1")]
    public Color rank1 = new Color();
    public string rank1Text = "1st";

    [Header("Rank 2")]
    public Color rank2 = new Color();
    public string rank2Text = "2nd";

    [Header("Rank 3")]
    public Color rank3 = new Color();
    public string rank3Text = "3rd";

    [Header("Rank 4")]
    public Color rank4 = new Color();
    public string rank4Text = "4th";

    [Header("Rank Out")]
    public Color rankOut = new Color();
    public string rankOutText = "fail";

    // ���̵� ����
    bool isOpen = false;

    // ���� ���� �ε�
    AsyncOperation ao = null;


    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        // �̴ϰ��� ���� ����
        if(Turn.now == Player.system.Minigame)
            Turn.Next();

        // ����
        SetUp();

        // �ʱ�ȭ
        MiniScore.RewardReset();

        // ���̵� ��
        StartCoroutine(Tool.CanvasFade(curtain, false, 0.5f));

        //// ���� �� ĵ���� �켱���� ���
        //transform.parent.GetComponent<Canvas>().sortingOrder += 1;

        //// �� ������Ʈ �ı� ����
        //DontDestroyOnLoad(transform.root);

        //// �� �ε�
        //ao = SceneManager.LoadSceneAsync("Main_game");

        //// ���ΰ��� ���� ����
        ////CustomInput.isBlock = true;
        //GameMaster.isBlock = true;
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �Ϸ��
        if (isOpen && curtain.alpha >= 1f)
        {
            // ���� ���� �������� ��� ���
            if (GameData.gameFlow < GameMaster.Flow.Start)
            {
                Debug.LogWarning(GameData.gameFlow);
                return;
            }

            // �ε� ����� �� ����
            Destroy(transform.root.gameObject);

            // ���� ���� ����
            //CustomInput.isBlock = false;
            GameMaster.isBlock = false;

            // BGM ���
            GameMaster.script.audioManager.bgmPlayer.Play();
        }
        // ���� �Ϸ��
        else if (curtain.alpha == 0f && !isOpen)
        {
            isOpen = true;
            
            // ���� �� ĵ���� �켱���� ���
            transform.parent.GetComponent<Canvas>().sortingOrder += 1;

            // �� ������Ʈ �ı� ����
            DontDestroyOnLoad(transform.root);

            // ���ΰ��� ���� ����
            //CustomInput.isBlock = true;
            GameMaster.isBlock = true;

            // �� �ε�
            ao = SceneManager.LoadSceneAsync("Main_game");
        }

        // ������ �غ� �Ϸ� ��
        if (ao != null && ao.isDone && GameMaster.script.loadingManager.isFinish)
            // ������ ��ư Ȱ��ȭ
            endBtn.gameObject.SetActive(true);
    }

    void SetUp()
    {
        // ������ ó��
        while (reportSlots.Count > MiniPlayerManager.entryPlayer.Count)
        {
            // ���� ��Ȱ��
            reportSlots[MiniPlayerManager.entryPlayer.Count].gameObject.SetActive(false);

            // ����Ʈ ����
            reportSlots.RemoveAt(MiniPlayerManager.entryPlayer.Count);
        }

        // ������ ó��
        for (int rank = 1; rank <= reportSlots.Count; rank++)
        {
            for (int i = 0; i < reportSlots.Count; i++)
            {
                if (MiniPlayerManager.entryPlayer[i].miniInfo.rank == rank)
                {
                    SetUpSlot(MiniPlayerManager.entryPlayer[i]);
                }
            }
        }
    }

    void SetUpSlot(Player player)
    {
        for (int i = 0; i < reportSlots.Count; i++)
        {
            if (reportSlots[i].owner == null)
            {
                // 1��
                if (player.miniInfo.rank == 1)
                {
                    reportSlots[i].SetUp(player, rank1, rank1Text);
                    return;
                }

                // 2��
                else if (player.miniInfo.rank == 2)
                {
                    reportSlots[i].SetUp(player, rank2, rank2Text);
                    return;
                }

                // 3��
                else if (player.miniInfo.rank == 3)
                {
                    reportSlots[i].SetUp(player, rank3, rank3Text);
                    return;
                }

                // 4��
                else if (player.miniInfo.rank == 4)
                {
                    reportSlots[i].SetUp(player, rank4, rank4Text);
                    return;
                }

                // 2��
                else
                {
                    reportSlots[i].SetUp(player, rankOut, rankOutText);
                    return;
                }
            }
        }
    }

    public void ReportFinish()
    {
        // �̴ϰ��� ���� ó��
        if (MiniGameManager.progress != ActionProgress.Ready)
            MiniGameManager.progress = ActionProgress.Finish;

        // ���̵� �ƿ�
        StartCoroutine(Tool.CanvasFade(curtain, true, 0.5f));
    }
}
