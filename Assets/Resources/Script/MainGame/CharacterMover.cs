using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    /*
     * < 미구현 요구사항 >
     * 캐릭터 액션 제어
     * 캐릭터 이동 제어
     * 캐릭터 최대 최소 좌표 제어
     */
    //


    // 장애물 목록
    public static List<int> barricade = new List<int>();

    // 액션 큐
    public Queue<Action> actionsQueue = new Queue<Action>();

    // 위치 인덱스
    int _location = -1;
    public int location { get { return _location; } /*set { _location = GameData.blockManager.indexLoop(_location, value); }*/ }

    // 이동량
    public int moveCount = 0;

    // 위치 오브젝트
    public Transform locateBlock { get { if (GameData.isMainGameScene) { if (location >= 0) return GameData.blockManager.GetBlock(location).transform; else return GameData.blockManager.startBlock; } else return null; } }



    // 이동 속도
    float moveSpeed = 1.00f;


    [SerializeField]
    float posMinY = 1.9f;           // 캐릭터 최소 높이
    [SerializeField]
    float posMaxY = 20f;            // 캐릭터 최대 높이






    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 고도 제한
        Tool.HeightLimit(transform, posMinY, posMaxY);
    }

    /// <summary>
    /// moveLocation 위치로 전진 스케줄링
    /// </summary>
    /// <param name="moveLocation">위치</param>
    void PlanMoveTo(int moveLocation)
    {
        // 이동량 계산
        int distance = moveLocation - location;

        // 전진 보정
        if (distance < 0)
            distance += GameData.blockManager.blockCount;

        // 이동 계획 요청
        PlanMoveBy(distance);
    }

    /// <summary>
    /// moveValue 만큼 이동 스케줄링
    /// </summary>
    /// <param name="moveValue"></param>
    void PlanMoveBy(int moveValue)
    {
        // 최종 목표
        int movePoint = GameData.blockManager.indexLoop(_location, moveValue);

        // 총 이동 거리(moveValue) 이내에 장애물 계산
        int counter = 0;
        int _sign = (int)Mathf.Sign(moveValue);
        for (int i = 0; i < moveValue; i += _sign)
        {
            counter += _sign;

            // 장애물 체크
            if (barricade[location + ((i + 1) * _sign)] > 0)
            {
                //스케줄링 추가 - counter만큼 칸수 지정
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, counter, moveSpeed));

                // 카운터 리셋
                counter = 0;
            }
        }
    }
}
