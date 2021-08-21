using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    // 퀵 등록
    public static MiniGameManager script = null;
    
    /// <summary>
    /// 미니게임 플레이어 매니저
    /// </summary>
    public MiniPlayerManager mpm = null;

    // 커튼 오브젝트
    [SerializeField] CanvasGroup curtain = null;

    // 준비 텍스트
    [SerializeField] UnityEngine.UI.Text readyText = null;


    /// <summary>
    /// 미니게임 메인 매니저
    /// </summary>
    public CardManager manager = null;

    // 점수 배율
    public int scoreRiseValue = 10;


    // 게임 시작 여부
    public bool isGameStart = false;



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
        StartCoroutine(Tool.CanvasFade(curtain, false, 2f));
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
        StartCoroutine(Tool.CanvasFade(curtain, true, 2f));

        // 등수 산정
        mpm.SetRanking();

        // 메인게임 로딩
        StartCoroutine(DelayedScoreScene());
    }

    IEnumerator DelayedScoreScene()
    {
        // 커튼 대기
        while (curtain.alpha != 1)
            yield return null;


        // 씬 이동
        // 임시 구현 ========== 정산 씬 작업 완료 시 해당 씬 이름으로 바꿀것
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main_game");
    }
}
