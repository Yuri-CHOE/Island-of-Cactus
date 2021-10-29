using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    GameObject dialog = null;

    [SerializeField]
    bool colorSwitch;

    AsyncOperation ao;

    bool isLoadFinish = false;
    bool isTableReady = false;
    
    [SerializeField]
    List<Sprite> iconTable = new List<Sprite>();

    [Header("Data Table")]
    [SerializeField] UnityEditor.DefaultAsset userData = null;
    [SerializeField] TextAsset userDataT = null;

    void Awake()
    {
        //CSVReader.basicPath = Application.dataPath + "/Resources/Data";
        //CSVReader.copyPath = Application.persistentDataPath + "/Data";

        // 아이콘 캐싱
        Item.iconTable = iconTable;

        //// 안드로이드 셋업
        //if(Application.platform == RuntimePlatform.Android)
        //{
        //    // 저장소 읽기 권한
        //    if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.ExternalStorageRead))
        //    {
        //        UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.ExternalStorageRead);
        //    }

        //    // 저장소 쓰기 권한
        //    if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.ExternalStorageWrite))
        //    {
        //        UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.ExternalStorageWrite);
        //    }
        //}
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

        //// 테이블 로드
        //StartCoroutine(TableLoad());
        //Item.SetUp();
        //Character.SetUp();
        //text.text = "wait...";
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

        //if (CustomInput.GetPoint())
        //{
        //    text.text = string.Format("isTableReady={0}, 로드={1}", isTableReady, ao.progress);

        //    Debug.LogError(string.Format("touchCount={0}, pos={1}", 
        //        UnityEngine.EventSystems.EventSystem.current.currentInputModule.input.touchCount, 
        //        UnityEngine.EventSystems.EventSystem.current.currentInputModule.input.GetTouch(0).position.ToString()
        //        ));
        //}
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

        //try
        //{
        //    //text.text = string.Format("isTableReady={0}, 로드={1}", isTableReady, ao.progress);
        //    Debug.LogError(string.Format("isTableReady={0}, 로드={1}, 상태={2}", isTableReady, ao.progress, (ao.progress >= 0.9f).ToString() ));
        //    Debug.LogError(string.Format("테이블 :: 유저={0} 캐릭터={1}, 아이템={2}, 이벤트={3}, 럭키박스={4}, 유니크={5}, 미니={6}", (UserData.file!=null), Character.table.Count, Item.table.Count, IocEvent.table.Count, LuckyBox.table.Count, Unique.table.Count, Minigame.table.Count));

        //    Debug.LogError(string.Format("permissionR={0}, permissionW={1}", 
        //        UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.ExternalStorageRead),
        //        UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.ExternalStorageWrite)
        //        ));
        //}
        //catch { }

    }
    
    IEnumerator SetUp()
    {
        yield return PermissionCheck();

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

        // 유저 데이터 로드
        // = 로 구분되는 기기데이터경로/User/UserData.iocdata 파일을 복사본(true)으로 저장 가능(false)하게 읽어옴
        UserData.file = new CSVReader("User", "UserData.iocdata", true, false, '=');
        UserData.SetUp();
        yield return null;

        // 캐릭터 테이블
        Character.SetUp();
        yield return null;

        // 아이템 테이블
        Item.SetUp();
        yield return null;

        // 이벤트 테이블
        IocEvent.SetUp();
        yield return null;

        // 럭키박스 테이블
        LuckyBox.SetUp();
        yield return null;

        // 유니크 테이블
        Unique.SetUp();
        yield return null;

        // 미니게임 테이블
        Minigame.SetUp();
        yield return null;

        // 완료 처리
        isTableReady = true;
    }
}
