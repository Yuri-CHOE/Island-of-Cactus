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

    // 페이드 제어
    bool isOpen = false;

    // 메인 게임 로딩
    AsyncOperation ao = null;


    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        // 미니게임 정산 진입
        if(Turn.now == Player.system.Minigame)
            Turn.Next();

        // 설정
        SetUp();

        // 초기화
        MiniScore.RewardReset();

        // 페이드 인
        StartCoroutine(Tool.CanvasFade(curtain, false, 0.5f));

        //// 현재 씬 캔버스 우선순위 상승
        //transform.parent.GetComponent<Canvas>().sortingOrder += 1;

        //// 본 오브젝트 파괴 방지
        //DontDestroyOnLoad(transform.root);

        //// 씬 로드
        //ao = SceneManager.LoadSceneAsync("Main_game");

        //// 메인게임 조작 차단
        ////CustomInput.isBlock = true;
        //GameMaster.isBlock = true;
    }

    // Update is called once per frame
    void Update()
    {
        // 가림 완료시
        if (isOpen && curtain.alpha >= 1f)
        {
            // 메인 게임 셋팅중일 경우 대기
            if (GameData.gameFlow < GameMaster.Flow.Start)
            {
                Debug.LogWarning(GameData.gameFlow);
                return;
            }

            // 로딩 종료시 씬 제거
            Destroy(transform.root.gameObject);

            // 조작 차단 해제
            //CustomInput.isBlock = false;
            GameMaster.isBlock = false;

            // BGM 재생
            GameMaster.script.audioManager.bgmPlayer.Play();
        }
        // 공개 완료시
        else if (curtain.alpha == 0f && !isOpen)
        {
            isOpen = true;
            
            // 현재 씬 캔버스 우선순위 상승
            transform.parent.GetComponent<Canvas>().sortingOrder += 1;

            // 본 오브젝트 파괴 방지
            DontDestroyOnLoad(transform.root);

            // 메인게임 조작 차단
            //CustomInput.isBlock = true;
            GameMaster.isBlock = true;

            // 씬 로드
            ao = SceneManager.LoadSceneAsync("Main_game");
        }

        // 페이지 준비 완료 시
        if (ao != null && ao.isDone && GameMaster.script.loadingManager.isFinish)
            // 다음씬 버튼 활성화
            endBtn.gameObject.SetActive(true);
    }

    void SetUp()
    {
        // 불참자 처리
        while (reportSlots.Count > MiniPlayerManager.entryPlayer.Count)
        {
            // 슬롯 비활성
            reportSlots[MiniPlayerManager.entryPlayer.Count].gameObject.SetActive(false);

            // 리스트 제외
            reportSlots.RemoveAt(MiniPlayerManager.entryPlayer.Count);
        }

        // 참여자 처리
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
                // 1등
                if (player.miniInfo.rank == 1)
                {
                    reportSlots[i].SetUp(player, rank1, rank1Text);
                    return;
                }

                // 2등
                else if (player.miniInfo.rank == 2)
                {
                    reportSlots[i].SetUp(player, rank2, rank2Text);
                    return;
                }

                // 3등
                else if (player.miniInfo.rank == 3)
                {
                    reportSlots[i].SetUp(player, rank3, rank3Text);
                    return;
                }

                // 4등
                else if (player.miniInfo.rank == 4)
                {
                    reportSlots[i].SetUp(player, rank4, rank4Text);
                    return;
                }

                // 2등
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
        // 미니게임 종료 처리
        if (MiniGameManager.progress != ActionProgress.Ready)
            MiniGameManager.progress = ActionProgress.Finish;

        // 페이드 아웃
        StartCoroutine(Tool.CanvasFade(curtain, true, 0.5f));
    }
}
