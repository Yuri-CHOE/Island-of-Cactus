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

    [SerializeField]
    bool colorSwitch;

    AsyncOperation ao;

    bool isLoadFinish = false;
    bool isTableReady = false;

    // Start is called before the first frame update
    void Start()
    {
        // 즉시 로딩 시작
        ao = SceneManager.LoadSceneAsync(nextScene);

        // 자동 넘어감 금지
        ao.allowSceneActivation = false;

        // 테이블 로드
        StartCoroutine(TableLoad());
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

    }


    public void Clicked()
    {
        // 넘기기
        if (isTableReady)
            if (isLoadFinish)
                ao.allowSceneActivation = true;
    }

    IEnumerator TableLoad()
    {
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
