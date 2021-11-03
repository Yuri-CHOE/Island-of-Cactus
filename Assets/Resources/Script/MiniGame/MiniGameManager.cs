using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MiniGame;

public class MiniGameManager : MonoBehaviour
{
    // 퀵 등록
    public static MiniGameManager script = null;

    // 현재 진행중인 게임
    public static Minigame minigameNow = null;

    // 진행 상태
    public static ActionProgress progress = ActionProgress.Ready;

    //// 미니게임 로더
    //static AsyncOperation loader = null;


    /// <summary>
    /// 미니게임 플레이어 매니저
    /// </summary>
    public MiniPlayerManager mpm = null;

    // 커튼 오브젝트
    [SerializeField] CanvasGroup curtain = null;

    // 준비 텍스트
    public UnityEngine.UI.Text readyText = null;


    /// <summary>
    /// 미니게임 메인 매니저
    /// </summary>
    public CardManager manager = null;

    // 점수 배율
    public int scoreRiseValue = 10;


    // 게임 시작 여부
    public bool isGameStart = false;

    // 정산 페이지 사전 로딩
    AsyncOperation ao = null;


    // AI 제출용 답안지
    public static MiniAI.AnswerType answerType = MiniAI.AnswerType.none;
    public static MiniAI.Answer answer = MiniAI.Answer.none;
    public static bool isAnswerSubmit = false;



    private void Awake()
    {
        // 퀵 등록
        script = this;

        // 초기 설정 호출
        mpm.Init();
        manager.Init();

        // 메인게임 씬 제거
        GameObject deleteObj = FindObjectsOfType<LoadingManager>()[0].transform.root.gameObject;
        Destroy(deleteObj);

        // 커튼 페이드 인
        Starting();


        // 정산 페이지 사전 로딩
        ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MiniGameReporter");

        // 로딩 완료시 자동 씬전환 비활성
        ao.allowSceneActivation = false;
    }


    private void Update()
    {

    }



    public void ScoreAdd(int addCount)
    {
        //ScoreAdd(MiniPlayerManager.script.turnNow, addCount);
        ScoreAdd(mpm.turnNow, addCount);
    }
    public void ScoreAdd(Player target, int addCount)
    {
        target.miniPlayerUI.scorePlus+= addCount * scoreRiseValue;
    }

    public void Starting()
    {
        //// 커튼 활성화
        //curtain.gameObject.SetActive(true);

        // 커튼 페이드 아웃
        StartCoroutine(Tool.CanvasFade(curtain, false, 1f));
    }

    public void Ready()
    {
        // 준비 상태
        //isGameStart = true;

        // 준비 전환
        Player.me.miniInfo.isReady = true;

        // 텍스트 변경
        readyText.text = "wait for other player . . .";
    }

    public void Ending()
    {
        //// 커튼 활성화
        //curtain.gameObject.SetActive(true);

        // 커튼 페이드 인
        StartCoroutine(Tool.CanvasFade(curtain, true, 0.5f));

        // 등수 산정
        mpm.SetRanking();

        // 종료 처리
        progress = ActionProgress.Finish;

        // 보상
        MiniScore.GiveRewardAll();

        // 정산 씬 로딩
        StartCoroutine(DelayedScoreScene());
    }

    IEnumerator DelayedScoreScene()
    {
        // 커튼 대기
        while (curtain.alpha != 1)
            yield return null;


        //// 씬 이동
        //AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main_game");
        //ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MiniGameReporter");

        // 로딩 완료시 자동 씬전환 활성
        ao.allowSceneActivation = true;
    }



    ///// <summary>
    ///// 미니게임 호출
    ///// </summary>
    ///// <param name="minigameName"></param>
    ///// <param name="entryPlayer"></param>
    //public AsyncOperation LoadMiniGame(string minigameName, int reward, List<Player> entryPlayer)
    //{
    //    // 호출
    //    try
    //    {
    //        MinigameName gameNameTemp = (MinigameName)System.Enum.Parse(typeof(MinigameName), minigameName);
    //        return LoadMiniGame(gameNameTemp, reward, entryPlayer);
    //    }
    //    catch
    //    {
    //        Debug.LogError("error :: 존재하지 않는 미니게임 -> " + minigameName);
    //        Debug.Break();
    //        return null;
    //    }
    //}
    ///// <summary>
    ///// 미니게임 호출
    ///// </summary>
    ///// <param name="minigameName"></param>
    ///// <param name="entryPlayer"></param>
    //public AsyncOperation LoadMiniGame(MinigameName minigameName, int reward, List<Player> entryPlayer)
    //{
    //    // 이미 진행중일 경우 차단
    //    if (gameName != MinigameName.None)
    //    {
    //        Debug.LogError("error :: 미니게임 중복 호출");
    //        Debug.Break();
    //        return null;
    //    }
    //    // 참가자 무효 차단
    //    if (entryPlayer == null)
    //    {
    //        Debug.LogError("error :: 미니게임 참가자 무효");
    //        Debug.Break();
    //        return null;
    //    }
    //    // 참가자 없을 경우 차단
    //    if (entryPlayer.Count <= 0)
    //    {
    //        Debug.LogError("error :: 미니게임 참가자 없음");
    //        Debug.Break();
    //        return null;
    //    }

    //    // 미니게임 설정
    //    gameName = minigameName;

    //    // 보수 설정
    //    MiniScore.reward = reward;

    //    // 참가자 설정
    //    for (int i = 0; i < entryPlayer.Count; i++)
    //    {
    //        entryPlayer[i].miniInfo.join = true;
    //    }

    //    // 미니게임 로드
    //    loader = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)minigameName);
    //    return loader;
    //}
}
