using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UnityEditor;

public class Scene_First : MonoBehaviour
{
    // 다음 씬 이름
    [SerializeField]
    string nextScene = "Title";

    // 터치 지시 텍스트
    [SerializeField]
    Text text;

    // 안드로이드 권한 체크기
    [SerializeField] PermissionChecker permissionChecker = null;

    // 유저명 등록기
    [SerializeField] UserNameSetter userNameSetter = null;

    [SerializeField]
    bool colorSwitch;

    AsyncOperation ao;

    bool isLoadFinish = false;
    bool isTableReady = false;
    
    [SerializeField]
    List<Sprite> iconTable = new List<Sprite>();


    [Header("Data Table")]
    [SerializeField] TextAsset charData = null;
    [SerializeField] TextAsset charDataLocal = null;

    [SerializeField] TextAsset itemData = null;
    [SerializeField] TextAsset itemDataLocal = null;

    [SerializeField] TextAsset eventData = null;
    [SerializeField] TextAsset eventDataLocal = null;

    [SerializeField] TextAsset luckyData = null;
    [SerializeField] TextAsset luckyDataLocal = null;

    [SerializeField] TextAsset uniqueData = null;
    [SerializeField] TextAsset uniqueDataLocal = null;

    [SerializeField] TextAsset minigameData = null;
    [SerializeField] TextAsset minigameDataLocal = null;

    [SerializeField] List<TextAsset> mapFile = new List<TextAsset>();
    


    void Awake()
    {
        // 아이콘 캐싱
        Item.iconTable = iconTable;

        // 맵파일 싱크
        WorldManager.worldAsset = mapFile;
    }

    // Start is called before the first frame update
    void Start()
    {

        // 환경설정 로드
        Preferences.Load();

        // 즉시 로딩 시작
        ao = SceneManager.LoadSceneAsync(nextScene);

        // 자동 넘어감 금지
        ao.allowSceneActivation = false;

        // 셋업 - 퍼미션, 테이블
        StartCoroutine(SetUp());
    }

    // Update is called once per frame
    void Update()
    {
        // 로드 종료 체크
        if (ao.progress >= 0.9f)
        {
            isLoadFinish = true;

            // 테이블 셋팅 체크
            if (isTableReady)
            {
                text.text = "Touch Anywhere";
            }
        }

    }


    public void Clicked()
    {
        // 넘기기
        if (isTableReady)
            if (isLoadFinish)
            {
                ao.allowSceneActivation = true;

                // 중력 설정
                Physics.gravity = Physics.gravity * 10;
            }
    }
    
    IEnumerator SetUp()
    {
        // 안드로이드 셋팅
        if (Application.platform == RuntimePlatform.Android)
        {
            // 권한 요청
            yield return PermissionCheck();
        }

        yield return TableLoad();
    }

    IEnumerator PermissionCheck()
    {
        Debug.Log("권한 :: 읽기 권한 확인중");
        while (!PermissionChecker.canRead)
        {
            // 상자 닫혀있으면 호출
            if (!permissionChecker.gameObject.activeSelf)
                permissionChecker.CallReadPermissionNotice();

            yield return null;
        }

        Debug.Log("권한 :: 쓰기 권한 확인중");
        while (!PermissionChecker.canWrite)
        {
            // 상자 닫혀있으면 호출
            if (!permissionChecker.gameObject.activeSelf)
                permissionChecker.CallWritePermissionNotice();

            yield return null;
        }

        Debug.Log("권한 :: 모든 필요 권한 확인됨");
    }

    IEnumerator TableLoad()
    {
        Debug.Log("테이블 :: 전체 데이터 테이블 셋업");

        if(!UserData.checkSetup)
            UserData.SetUp();

        yield return SetUserName();

        // 캐릭터 테이블
        Character.SetUp(charData, charDataLocal);
        yield return null;

        // 아이템 테이블
        Item.SetUp(itemData, itemDataLocal);
        yield return null;

        // 이벤트 테이블
        IocEvent.SetUp(eventData, eventDataLocal);
        yield return null;

        // 럭키박스 테이블
        LuckyBox.SetUp(luckyData, luckyDataLocal);
        yield return null;

        // 유니크 테이블
        Unique.SetUp(uniqueData, uniqueDataLocal);
        yield return null;

        // 미니게임 테이블
        Minigame.SetUp(minigameData, minigameDataLocal);
        yield return null;

        // 완료 처리
        isTableReady = true;
    }

    IEnumerator SetUserName()
    {
        Debug.Log("유저 데이터 :: 유저 이름 셋팅");

        if (UserData.userName == UserData.defaultName)
            userNameSetter.gameObject.SetActive(true);
        else
            Debug.Log("유저 데이터 :: 유저 이름 확인됨 -> " + UserData.userName);

        yield return null;
    }
}
