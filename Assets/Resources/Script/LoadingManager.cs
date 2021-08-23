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

    // 로딩 커튼
    CanvasGroup curtain = null;

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


    private void Awake()
    {
        curtain = gameObject.GetComponent<CanvasGroup>();
    }
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

        curtain.alpha = 0.0f;
        isFadeIn = true;
        isFadeActive = true;
    }

    public void LoadFinish()
    {
        // 페이드 아웃 처리
        curtain.alpha = 1.0f;
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
        //// 로딩화면 활성화
        //gameObject.SetActive(true);

        //// 현재 씬 캔버스 우선순위 상승
        //transform.parent.GetComponent<Canvas>().sortingOrder += 1;

        //// 본 오브젝트 파괴 방지
        //DontDestroyOnLoad(transform.root);

        //// 로딩 시작
        //ao = SceneManager.LoadSceneAsync(sceneName);

        //// 로딩 완료시 자동 씬전환 비활성
        //ao.allowSceneActivation = false;

        //// 로딩 시작 연출
        //LoadBefore();

        // 로딩 시작 및 연출 처리
        LoadAsync(SceneManager.LoadSceneAsync(sceneName));
    }
    /// <summary>
    /// 외부에서 로딩한 씬을 인자로 넘겨받아 연출처리
    /// (로딩 기능 없음)
    /// </summary>
    /// <param name="loadSceneAsync"></param>
    void LoadAsync(AsyncOperation loadSceneAsync)
    {
        // 로딩화면 활성화
        gameObject.SetActive(true);

        // 현재 씬 캔버스 우선순위 상승
        transform.parent.GetComponent<Canvas>().sortingOrder += 1;

        // 본 오브젝트 파괴 방지
        DontDestroyOnLoad(transform.root);

        // 로딩 시작
        ao = loadSceneAsync;

        // 로딩 완료시 자동 씬전환 비활성
        ao.allowSceneActivation = false;

        // 로딩 시작 연출
        LoadBefore();
    }


    public void LoadAsyncMiniGame(Minigame minigame, int reward, List<Player> entryPlayer)
    {
        // 이미 진행중일 경우 차단
        if (MiniGameManager.minigameNow != null)
        {
            Debug.LogError("error :: 미니게임 중복 호출");
            Debug.Break();
            return;
        }
        // 참가자 무효 차단
        if (entryPlayer == null)
        {
            Debug.LogError("error :: 미니게임 참가자 무효");
            Debug.Break();
            return;
        }
        // 참가자 없을 경우 차단
        if (entryPlayer.Count <= 0)
        {
            Debug.LogError("error :: 미니게임 참가자 없음");
            Debug.Break();
            return;
        }

        Debug.LogWarning(string.Format("미니게임 :: 호출됨 -> [{0}] {1}",minigame.index, minigame.name));

        // 미니게임 설정
        MiniGameManager.minigameNow = minigame;

        // 보수 설정
        MiniScore.reward = reward;

        // 참가자 설정
        for (int i = 0; i < entryPlayer.Count; i++)
        {
            entryPlayer[i].miniInfo.join = true;
        }

        // 미니게임 로드
        LoadAsync(SceneManager.LoadSceneAsync(minigame.sceneNum));
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
            if (curtain.alpha < 1.0f)
            {
                curtain.alpha += Time.deltaTime * 2;
            }
            // 페이드 완료
            else
            {
                curtain.alpha = 1.0f;
                curtain.blocksRaycasts = true;
                isFadeActive = false;
                _isFadeFinish = true;
                Debug.Log("fade in done");
            }
        }

        // 페이드 아웃
        else
        {
            // 페이드 처리
            if (curtain.alpha > 0.0f)
            {
                curtain.alpha -= Time.deltaTime;
            }
            // 페이드 완료
            else
            {
                curtain.alpha = 0.0f;
                curtain.blocksRaycasts = false;
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

        // 게임 플로우 리셋
        GameData.gameFlow = GameMaster.Flow.Wait;
        

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

        // 이벤트 테이블
        IocEvent.SetUp();
        workCount += IocEvent.table.Count;

        // 럭키박스 테이블
        LuckyBox.SetUp();
        workCount += LuckyBox.table.Count;

        // 유니크 테이블
        Unique.SetUp();
        workCount += Unique.table.Count;

        // 미니게임 테이블
        Minigame.SetUp();
        workCount += Minigame.table.Count;
    }

    void Work_MainGame()
    {
        {
            /*
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
            // ㄴ 해결법 : 그냥 아이템,이벤트 오브젝트 리스트 읽어서 재구성할것
            // 초기화 안됬을 경우 초기화
            if(DynamicObject.objectList.Count == 0)
            {
                for (int i = 0; i < wm.blockManager.blockCount; i++)
                    DynamicObject.objectList.Add(new List<DynamicObject>());
            }
            // 이미 초기화 된 경우 재생성
            else
            {
                // 아이템 재생성
                ItemManager.ReCreateAll();

                // 이벤트 재생성
                EventManager.ReCreateAll();
            }
            */
        }

        // 작업 목표량 설정
        workMax = 10000;

        // 월드 빌드 시작
        WorldManager wm = GameObject.Find("World").GetComponent<WorldManager>();
        workCount++;

        // 카메라 한계 설정
        wm.cameraManager.controller.SetCameraLimit(WorldManager.worldFile[0]);
        workCount += 6;

        // 스타트 블록 설정
        wm.blockManager.SetStartBlock(WorldManager.worldFile[1]);
        workCount++;

        // 지형 빌드
        wm.groundManager.BuildByString(WorldManager.worldFile[2]);
        workCount++;

        // 블록 빌드        
        wm.blockManager.BuildByString(WorldManager.worldFile[3]);
        workCount++;

        // 장식물 빌드
        wm.decorManager.BuildByString(WorldManager.worldFile[4]);
        workCount++;

        // 코너 셋팅
        wm.blockManager.SetCorner();
        workCount += wm.blockManager.blockCount;

        // 블록 리스트 등록
        wm.blockManager.SetDynamicBlockList();
        workCount += wm.blockManager.blockCount;

        // 첫 로드
        if (GameMaster.flowCopy == GameMaster.Flow.Wait)
        {
            Debug.LogWarning("로딩 :: 오브젝트 리스트 초기화됨");

            // 장애물 초기화
            for (int i = 0; i < wm.blockManager.blockCount; i++)
                DynamicObject.objectList.Add(new List<DynamicObject>());
        }
        // 다시 로드
        else
        {
            // 장애물 재생성
            {
                Debug.LogWarning(string.Format(
                    "로딩 :: 오브젝트 재생성 요청됨\n합산 : {0}\n아이템 : {1}\n이벤트 : {2}",
                    ItemManager.itemObjectList.Count + EventManager.eventObjectList.Count, 
                    ItemManager.itemObjectList.Count, 
                    EventManager.eventObjectList.Count
                    ) );

                // 아이템 재생성
                ItemManager.ReCreateAll(true);

                // 이벤트 재생성
                EventManager.ReCreateAll(true);
            }

            GameData.gameFlow = GameMaster.Flow.Wait;
        }
    }
}
