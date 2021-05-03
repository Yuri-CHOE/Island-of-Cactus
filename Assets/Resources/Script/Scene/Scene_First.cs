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

    // Start is called before the first frame update
    void Start()
    {
        // 즉시 로딩 시작
        ao = SceneManager.LoadSceneAsync(nextScene);

        // 자동 넘어감 금지
        ao.allowSceneActivation = false;

        // 테이블 로드 완료
        //Item.SetUp();
        //Character.SetUp();
        text.text = "Press Anywhere";
        isLoadFinish = true;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Clicked()
    {
        // 터치 차단
        if (ao.progress < 0.9f)
        {
            text.text = "wait...";
            return;
        }

        // 자동 넘어감 활성
        else if (isLoadFinish)
            ao.allowSceneActivation = true;

    }

}
