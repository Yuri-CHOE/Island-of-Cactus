using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    // 로딩 게이지
    [SerializeField]
    Image guage;

    // 완료 정도
    [SerializeField]
    int workCount = 0;
    public int workMax = 1;
    bool isLoadComplete = false;

    // 완료여부
    bool _isFinish = false;
    public bool isFinish { get { return _isFinish; } }

    // 로딩UI 페이드
    bool isFadeIn = true;
    bool isFadeActive = false;



    // 비동기 로딩 도구
    AsyncOperation ao;

    [SerializeField]
    string nextScene = null;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RrefreshGuage());
        Work();
    }

    // Update is called once per frame
    void Update()
    {
        // 페이드 작동
        if (isFadeActive)
            Fade();

        // 씬 로딩 완료 연출
        if (!isFadeActive && ao != null)
            if (ao.isDone)
                ao.allowSceneActivation = true;


        // 동적 로딩 잔여 매꾸기
        if (workCount < workMax && isLoadComplete)
            workCount += (int)(Time.deltaTime*3000);

        // 동적 로딩 완료
        if (isFinish) { }
    }


    /// <summary>
    /// 로딩 진행도 갱신
    /// </summary>
    float GetProgress()
    {
        // 최대치 보정
        if (workCount >= workMax)
            workCount = workMax;

        return (float)workCount / (float)workMax; 
    }

    /// <summary>
    /// 로딩 게이지 갱신
    /// StartCoroutine() 으로 호출할것
    /// </summary>
    IEnumerator RrefreshGuage()
    {
        while (!isFinish)
        {
            //Debug.Log(GetProgress());
            guage.fillAmount = GetProgress();
            CheckFinish();
            yield return null;
        }
    }

    /// <summary>
    /// 로딩 완료 확인
    /// </summary>
    void CheckFinish()
    {
        // 1.00f 이상이면 완료처리
        if (GetProgress() >= 1.00f)
        {
            _isFinish = true;
            Debug.Log("Dynamic Loading :: Finished");

            LoadFinish();
        }
    }

    public void LoadBefore()
    {
        // 로딩 게이지 비활성
        guage.gameObject.SetActive(false);

        gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
        isFadeIn = true;
        isFadeActive = true;
    }

    public void LoadFinish()
    {
        // 페이드 아웃 처리
        gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
        isFadeIn = false;
        isFadeActive = true;
    }

    public void LoadAsync()
    {
        // 로딩 시작
        LoadAsync(nextScene);
    }
    public void LoadAsync(string sceneName)
    {
        // 로딩 시작
        ao = SceneManager.LoadSceneAsync(sceneName);

        // 로딩 시작 연출
        LoadBefore();
    }

    void Fade()
    {
        // 페이드 인
        if (isFadeIn)
        {
            // 페이드 처리
            if (gameObject.GetComponent<CanvasGroup>().alpha < 1.0f)
            {
                gameObject.GetComponent<CanvasGroup>().alpha += Time.deltaTime;
            }
            // 페이드 완료
            else
            {
                gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
                isFadeActive = false;
                Debug.Log("fade in done");
            }
        }

        // 페이드 아웃
        else
        {
            // 페이드 처리
            if (gameObject.GetComponent<CanvasGroup>().alpha > 0.0f)
            {
                gameObject.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
            }
            // 페이드 완료
            else
            {
                gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
                isFadeActive = false;
                Debug.Log("fade out done");
            }
        }
    }

    /// <summary>
    /// 로딩 작업
    /// </summary>
    void Work()
    {
        // 씬별 동적 로딩작업
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0: // First

                break;
            case 1: // Title
                Work_Title();
                break;
            case 2: // Main_game
                Work_MainGame();
                break;
        }

        // 로딩 종료
        Debug.Log("로딩 완료 :: "+SceneManager.GetActiveScene().name + "=> 작업 수 : " + workCount);
        isLoadComplete = true;
        //workCount = workMax;
    }

    void Work_Title()
    {
        // 작업 목표량 설정
        workMax = 10000;

        gameObject.SetActive(false);

        Item.SetUp();
        workCount++;
    }

    void Work_MainGame()
    {
        // 작업 목표량 설정
        workMax = 10000;

        Item.SetUp();
        workCount++;
    }
}
