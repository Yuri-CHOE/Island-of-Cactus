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
    bool _isFadeFinish = false;
    public bool isFadeFinish { get { return _isFadeFinish; } }


    // 비동기 로딩 도구
    AsyncOperation ao;

    // 로드할 씬
    [SerializeField]
    string nextScene = null;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RrefreshGuage());
        Work();
    }

    void FixedUpdate()
    {
        // 페이드 작동
        if (isFadeActive)
            Fade();
    }

    // Update is called once per frame
    void Update()
    {

        // 씬 로딩 완료 연출
        if (!isFadeActive && ao != null)
            if (ao.progress >= 0.9f) // 90% 이상 완료된 경우 => allowSceneActivation : true 상태에서 로딩 최대치는 90%이므로 isDone 사용 불가능
            {
                // 로딩화면 비활성
                //transform.parent.gameObject.SetActive(false);

                // 다음 씬 활성화
                ao.allowSceneActivation = true;

                // 로딩화면 비활성
                //gameObject.SetActive(false);

            }


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
        // 로딩화면 활성화
        gameObject.SetActive(true);

        // 현재 씬 캔버스 우선순위 상승
        transform.parent.GetComponent<Canvas>().sortingOrder += 1;

        // 본 오브젝트 파괴 방지
        DontDestroyOnLoad(transform.root);

        // 로딩 시작
        ao = SceneManager.LoadSceneAsync(sceneName);

        // 로딩 완료시 자동 씬전환 비활성
        ao.allowSceneActivation = false;

        // 로딩 시작 연출
        LoadBefore();
    }

    /// <summary>
    /// 페이드 작동
    /// StartCoroutine() 으로 호출할것
    /// </summary>
    /// <returns></returns>
    void Fade()
    {
        // 페이드 인
        if (isFadeIn)
        {
            // 페이드 처리
            if (gameObject.GetComponent<CanvasGroup>().alpha < 1.0f)
            {
                gameObject.GetComponent<CanvasGroup>().alpha += Time.deltaTime * 2;
            }
            // 페이드 완료
            else
            {
                gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
                gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
                isFadeActive = false;
                _isFadeFinish = true;
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
                gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                isFadeActive = false;
                _isFadeFinish = true;
                Debug.Log("fade out done");
            }
        }
    }

    /// <summary>
    /// 모든 LoadingManager 스크립트를 찾아 본 스크립트와 중복검사 하여 DontDestroyOnLoad로 보호된 이전 씬 파괴
    /// </summary>
    void DestroyOld()
    {
        // 로딩 오브젝트 확보
        LoadingManager[] obj = FindObjectsOfType<LoadingManager>();
        for (int i = 0; i < obj.Length; i++)
        {
            // 본 스크립트와 같은 오브젝트인지 체크
            if (obj[i].transform != transform)
            {
                // 다를경우 DontDestroyOnLoad로 보호된 이전씬으로 판단
                // 삭제처리
                Destroy(obj[i].transform.root.gameObject);
                return;
            }
        }
    }


    /// <summary>
    /// 로딩 작업
    /// </summary>
    void Work()
    {
        // 이전 씬 삭제
        DestroyOld();

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
        workCount++;


        // 유저 데이터 로드
        // = 로 구분되는 기기데이터경로/User/UserData.iocdata 파일을 복사본(true)으로 저장 가능(false)하게 읽어옴
        UserData.file = new CSVReader("User", "UserData.iocdata", true, false, '=');
        UserData.SetUp();
        workCount += UserData.file.table.Count;
        
        // 캐릭터 테이블
        Character.SetUp();
        workCount+= Character.table.Count;

        // 아이템 테이블
        Item.SetUp();
        workCount += Item.table.Count;

        // 럭키박스 테이블
        LuckyBox.SetUp();
        workCount += LuckyBox.table.Count;
    }

    void Work_MainGame()
    {
        // 작업 목표량 설정
        workMax = 10000;

        // 월드 빌드 시작
        WorldManager wm = GameObject.Find("World").GetComponent<WorldManager>();
        workCount++;

        // 카메라 한계 설정
        wm.cameraManager.controller.SetCameraLimit(WorldManager.worldFile[0]);
        workCount+=6;

        // 스타트 블록 설정
        wm.blockManager.SetStartBlock(WorldManager.worldFile[1]);

        // 지형 빌드
        wm.groundManager.BuildByString(WorldManager.worldFile[2]);
        workCount++;

        // 블록 빌드
        wm.blockManager.BuildByString(WorldManager.worldFile[3]);
        workCount++;

        // 장식물 빌드
        wm.decorManager.BuildByString(WorldManager.worldFile[4]);
        workCount++;

        // 장애물 초기화
        // ================ 버그 수정 필요 : 이러면 씬 새로 로딩했을때 장애물 전부 날아감
        // ㄴ 해결법 : 로딩할때 말고 로딩 끝나고 메인게임 플로우 초기화구간에서 처리할것
        for (int i = 0; i < wm.blockManager.blockCount; i++)
            CharacterMover.barricade.Add(0);
    }
}
