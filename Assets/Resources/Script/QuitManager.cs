using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuitManager : MonoBehaviour
{
    public static Coroutine quitSequence = null;

    [SerializeField] Transform box = null;

    [SerializeField] InputAction escape  = null;
    //[SerializeField] InputActionMap escapeMap  = null;


    void Start()
    {
        //escapeMap.Enable();
        escape.Enable();

        // 화면 꺼짐 방지
        //Screen.sleepTimeout = 300;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }


    void Update()
    {
        //if (escape.triggered)
        if (escape.triggered)
            Call();
    }

    public void Call()
    {
        // 박스 호출
        box.gameObject.SetActive(true);
        Debug.Log("디버그 :: esc 감지");
    }



    public void Quit()
    {
        if(quitSequence != null)
        {
            Debug.LogWarning("종료 :: 프로그램 종료 취소 -> 이미 수행중");
            return;
        }

        Debug.Log("종료 :: 프로그램 종료 요청됨");

        // 종료 절차 수행
        quitSequence = StartCoroutine(QuitSequence());
    }

    IEnumerator QuitSequence()
    {
        Debug.Log("종료 :: 프로그램 종료 절차 시작됨");
        yield return null;

        // 저장 대기
        while (GameSaveStream.saveControl != null)
        {
            Debug.Log("종료 :: 게임 세이브 대기중");
            yield return null;
        }

        Debug.Log("종료 :: 프로그램 종료 절차 완료됨");

        // 종료
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        // 저장 대기
        if (CheckQuit())
        {
            // 저장 취소
            Application.CancelQuit();

            // 저장 대기후 종료
            Quit();
        }
    }

    public static bool CheckQuit()
    {
        // 저장 대기
        if (GameSaveStream.saveControl != null)
        {
            Debug.LogWarning("종료 :: 게임 세이브 대기중");
            return false;
        }

        return true;
    }
}
