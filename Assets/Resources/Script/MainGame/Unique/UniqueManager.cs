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
        world_01_02 = 2,
        world_01_03 = 3,
    }

    // 퀵등록
    public static UniqueManager script = null;

    // 효과 이름
    [SerializeField]
    Text nameText = null;

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
        int _index = (int)index;

        // 메시지 박스 셋팅
        nameText.text = Unique.table[_index].name;
        info.text = Unique.table[_index].info;

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
        yield return Effect(currentPlayer, index);


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

        Unique temp = Unique.table[(int)index];

        switch (index)
        {
            case Mapcode.world_01_01:
                // 플러스 블록과 마이너스 블록의 코인 변동치를 1 증가
                while (isWork)
                {
                    BlockWork.plusBlockPlus += temp.value;
                    BlockWork.minusBlockPlus += temp.value;
                    isWork = false;

                    yield return null;
                }
                break;
            case Mapcode.world_01_02:
                // 플러스 블록과 마이너스 블록의 코인 변동치를 2배 증가
                while (isWork)
                {
                    BlockWork.plusBlockMultiple *= temp.value;
                    BlockWork.minusBlockMultiple *= temp.value;
                    isWork = false;

                    yield return null;
                }
                break;
            case Mapcode.world_01_03:
                // 없음 - 추가시 테이블 먼저 추가해야함
                while (isWork)
                {                    
                    yield return null;
                }
                break;
        }

        // 종료 판정
        BlockWork.isEnd = true;
    }

}
