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

    /// <summary>
    /// 미니게임 메인 매니저
    /// </summary>
    public CardManager manager = null;

    // 점수 배율
    public int scoreRiseValue = 10;



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

    public void Ending(GameObject endingCurtain)
    {
        if (endingCurtain != null)
        {
            endingCurtain.SetActive(true);

            CanvasGroup cg = endingCurtain.GetComponent<CanvasGroup>();

            if(cg != null)
            Tool.CanvasFade(cg, false, 2f);
        }

        // 등수 산정
        mpm.SetRanking();

        // 메인게임 로딩
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main_game");
    }
}
