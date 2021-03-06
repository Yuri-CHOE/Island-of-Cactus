using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniqueManager : MonoBehaviour
{
    public enum Mapcode
    {
        none,
        world_01_01 = 1,
    }

    // 퀵등록
    public static UniqueManager script = null;

    // 효과 이름
    [SerializeField]
    Text name = null;

    // 효과 정보
    [SerializeField]
    Text info = null;

    // 작동 제어
    public  Coroutine coroutineWork = null;
    public  bool isWork = false;


    void Awake()
    {
        // 퀵등록
        script = this;
    }


    public  void Active(Player currentPlayer)
    {
        Active(currentPlayer, GameData.worldFileName);
    }
    public  void Active(Player currentPlayer, string worldFileName)
    {
        // 이전 연출 중단
        if (coroutineWork != null)
            StopCoroutine(coroutineWork);

        // 맵파일 미입력시 차단
        if (worldFileName == null)
        {
            Debug.LogError("Error :: 맵파일 이름 누락");
            Debug.Break();
            return;
        }

        // 맵파일 인덱스화
        Mapcode index = Mapcode.none;
        Enum.TryParse(worldFileName.Replace(".iocw",""), out index);

        // 유효하지 않은 맵파일 입력시 차단
        if (index == Mapcode.none)
        {
            Debug.LogError("Error :: 잘못된 맵파일 이름 -> " + worldFileName);
            Debug.Break();
            return;
        }

        // 작동 시작
        isWork = true;
        coroutineWork = StartCoroutine( Work(currentPlayer, index) );
    }


    IEnumerator Work(Player currentPlayer, Mapcode index)
    {
        // 메시지 박스 셋팅
        name.text = Unique.table[(int)index].name;
        info.text = Unique.table[(int)index].info;

        // 효과 설명 출력
        MessageBox mb = GameData.gameMaster.messageBox;
        mb.PopUp(MessageBox.Type.UniqueBlock);


        // 메시지 박스 확인 대기
        while (mb.gameObject.activeSelf)
        {
            //Debug.LogWarning("유니크 블록 :: 메시지 박스 확인 대기중");
            yield return null;
        }


        // 연출 및 효과 시작
        yield return StartCoroutine(Effect(currentPlayer, index));


        // 작동 연출
        while (isWork)
        {
            //Debug.LogWarning("유니크 블록 :: 작동 연출중");
            yield return null;
        }

    }


    public IEnumerator Effect(Player currentPlayer, Mapcode index)
    {
        isWork = true;

        switch (index)
        {
            case Mapcode.world_01_01:
                // 플러스 블록과 마이너스 블록의 코인 변동치를 1 증가
                while (isWork)
                {
                    BlockWork.plusBlockValue++;
                    BlockWork.minusBlockValue++;
                    isWork = false;

                    yield return null;
                }
                break;
        }

        // 종료 판정
        BlockWork.isEnd = true;
    }

}
