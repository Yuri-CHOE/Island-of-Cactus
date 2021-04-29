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

    // Start is called before the first frame update
    void Start()
    {
        // 즉시 로딩 시작
        ao = SceneManager.LoadSceneAsync(nextScene);

        // 자동 넘어감 금지
        ao.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Clicked()
    {
        if (!ao.isDone)
            text.text = "wait...";

        // 자동 넘어감 활성
        ao.allowSceneActivation = true;
    }

}
