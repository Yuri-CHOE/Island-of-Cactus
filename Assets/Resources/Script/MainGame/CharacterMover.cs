using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    // 장애물 목록
    public static List<int> barricade = new List<int>();

    // 액션 큐
    public Queue<Action> actionsQueue = new Queue<Action>();

    // 현재 액션
    public Action actNow = new Action();


    public static float ActTurnSpeed = 5f;


    // 목표 이동 좌표
    Vector3 movePoint = Vector3.zero;

    // 이동 제어용
    Coroutine moveCoroutine = null;
    public bool isBusy = false;



    // 위치 인덱스
    int _location = -1;
    //public int location { get { return _location; } set { _location = GameData.blockManager.indexLoop(_location, value); } }
    public int location { get { return _location; } set { _location = GameData.blockManager.indexLoop(value, 0); } }

    // 이동량
    public int moveCount = 0;

    // 현재 위치 기반 오브젝트
    public Transform locateBlock { get { if (GameData.isMainGameScene) { if (location >= 0) return GameData.blockManager.GetBlock(location).transform; else return GameData.blockManager.startBlock; } else return null; } }



    // 이동 속도
    float moveSpeed = 1.00f;


    [SerializeField]
    float posMinY = 1.9f;           // 캐릭터 최소 높이
    [SerializeField]
    float posMaxY = 20f;            // 캐릭터 최대 높이


    // 회전용 하위 오브젝트
    [SerializeField]
    Transform bodyObject = null;



    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        //// 완료된 이동좌표 리셋
        //if (movePoint != Vector3.zero && moveCoroutine == null)
        //    movePoint = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // 고도 제한
        Tool.HeightLimit(bodyObject, posMinY - transform.position.y, posMaxY - transform.position.y);
    }


    /// <summary>
    /// 캐릭터 겹침 처리
    /// </summary>
    public void AvatarOverFix()
    {

        // 중복 플레이어 리스트
        List<Player> fixTarget = new List<Player>();

        // 모든 플레이어 위치 체크
        for (int i = 0; i < GameData.player.allPlayer.Count; i++)
        {
            // 본인 제외
            //if (GameData.player.allPlayer[i].avatar == transform)
            //    continue;

            // 위치 중복 체크
            if (location == GameData.player.allPlayer[i].avatar.GetComponent<CharacterMover>().location)
                // 중복 플레이어 확보
                fixTarget.Add(GameData.player.allPlayer[i]);
        }

        // 겹쳤다 떠나도 포메이션 그대로 =========== 큰 문제는 아님

        // 겹친 장소
        Vector3 corssPoint = new Vector3();
        if (location == -1)     // 스타트 블록
            corssPoint = BlockManager.script.startBlock.transform.position;
        else                    // 그 외
            corssPoint = BlockManager.script.GetBlock(location).transform.position;

        // 4명 중복
        if (fixTarget.Count >= 4)
        {
            fixTarget[0].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(-2, 0, 2), ActTurnSpeed, true);
            fixTarget[1].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(2, 0, 2), ActTurnSpeed, true);
            fixTarget[2].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(2, 0, -2), ActTurnSpeed, true);
            fixTarget[3].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(-2, 0, -2), ActTurnSpeed, true);

            //fixTarget[0].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[0].avatar.transform.position + new Vector3(-2, 0, 2), ActTurnSpeed, true);
            //fixTarget[1].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[1].avatar.transform.position + new Vector3(2, 0, 2), ActTurnSpeed, true);
            //fixTarget[2].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[2].avatar.transform.position + new Vector3(2, 0, -2), ActTurnSpeed, true);
            //fixTarget[3].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[3].avatar.transform.position + new Vector3(-2, 0, -2), ActTurnSpeed, true);
        }
        // 3명 중복
        else if (fixTarget.Count == 3)
        {
            fixTarget[0].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(-2, 0, 2), ActTurnSpeed, true);
            fixTarget[1].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(2, 0, 2), ActTurnSpeed, true);
            fixTarget[2].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(0, 0, -2), ActTurnSpeed, true);

            //fixTarget[0].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[0].avatar.transform.position + new Vector3(-2, 0, 2), ActTurnSpeed, true);
            //fixTarget[1].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[1].avatar.transform.position + new Vector3(2, 0, 2), ActTurnSpeed, true);
            //fixTarget[2].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[2].avatar.transform.position + new Vector3(0, 0, -2), ActTurnSpeed, true);
        }
        // 2명 중복
        else if (fixTarget.Count == 2)
        {
            fixTarget[0].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(-2, 0, 0), ActTurnSpeed, true);
            fixTarget[1].avatar.GetComponent<CharacterMover>().MoveSet(corssPoint + new Vector3(+2, 0, 0), ActTurnSpeed, true);

            //fixTarget[0].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[0].avatar.transform.position + new Vector3(-2, 0, 0), ActTurnSpeed, true);
            //fixTarget[1].avatar.GetComponent<CharacterMover>().MoveSet(fixTarget[1].avatar.transform.position + new Vector3(+2, 0, 0), ActTurnSpeed, true);
        }
        // 중복 없음
        else if (fixTarget.Count == 1)
        {

        }
    }

    /// <summary>
    /// 특정 위치로 전진 스케줄링
    /// </summary>
    /// <param name="moveLocation">위치</param>
    public void PlanMoveTo(int moveLocation)
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
    /// 값 만큼 이동 스케줄링
    /// </summary>
    /// <param name="moveValue"></param>
    public void PlanMoveBy(int moveValue)
    {
        // 최종 목표
        int movePoint = GameData.blockManager.indexLoop(_location, moveValue);

        Debug.LogError(moveValue + " = " + movePoint);

        // 총 이동 거리(moveValue) 이내에 장애물 계산
        int counter = 0;
        int _sign = (int)Mathf.Sign(moveValue);
        for (int i = 0; i != moveValue; i += _sign)
        {
            counter += _sign;

            int loc = GameData.blockManager.indexLoop(location, ((i) * _sign));
            int locNext = GameData.blockManager.indexLoop(location,  ((i + 1) * _sign));


            // 스타트 블록 체크
            if(location == -1 && i == 0)
            {
                Debug.Log("스타트 블록 탐지 : " + counter);

                //스케줄링 추가 - counter만큼 칸수 지정
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));

                //스케줄링 추가 - 회전 액션 추가
                //actionsQueue.Enqueue(new Action(Action.ActionType.Turn, GameData.blockManager.GetBlock(locNext).GetComponent<DynamicBlock>().GetDirection(), moveSpeed));

                // 카운터 리셋
                counter = 0;
            }

            // 장애물 체크
            if (barricade[locNext] > 0)
            {
                Debug.Log("장애물 탐지 : " + counter);

                //스케줄링 추가 - counter만큼 칸수 지정
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));
                //actionsQueue.Enqueue(new Action(Action.ActionType.Move, counter, moveSpeed));

                //스케줄링 추가 - 정지 추가
                //actionsQueue.Enqueue(new Action(Action.ActionType.Stop, 1, moveSpeed));

                // 카운터 리셋
                counter = 0;
            }

            // 코너 체크
            if (
            GameData.blockManager.GetBlock(locNext).GetComponent<DynamicBlock>().GetDirection()
            !=
            GameData.blockManager.GetBlock(GameData.blockManager.indexLoop(locNext, - 1)).GetComponent<DynamicBlock>().GetDirection()
            // 스타트블록에서 시작하고 탐색점이 스타트 블록 혹은 그 다음 칸일 경우 제외 처리
            && !(location == -1 && (loc == 0 || locNext == 0))
            )
            {
                Debug.Log("코너 탐지 : " + counter);

                //스케줄링 추가 - counter만큼 칸수 지정
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i+ _sign, moveSpeed));
                //actionsQueue.Enqueue(new Action(Action.ActionType.Move, counter, moveSpeed));

                //스케줄링 추가 - 회전 액션 추가
                //actionsQueue.Enqueue(new Action(Action.ActionType.Turn, GameData.blockManager.GetBlock(locNext).GetComponent<DynamicBlock>().GetDirection(), moveSpeed));

                // 카운터 리셋
                counter = 0;
            }

            // 종료 전 체크
            if (counter > 0 && (i + _sign == moveValue))
            {
                Debug.Log("종료 탐지 : " + counter);

                //스케줄링 추가 - counter만큼 칸수 지정
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i+ _sign, moveSpeed));
                //actionsQueue.Enqueue(new Action(Action.ActionType.Move, counter, moveSpeed));

                // 카운터 리셋
                counter = 0;

                break;
            }

            Debug.Log(counter + " , " + (i + _sign == moveValue));
        }

        //스케줄링 추가 - 정면 보기
        //actionsQueue.Enqueue(new Action(Action.ActionType.Turn, 2, moveSpeed));
    }


    public ref Action GetAction()
    {
        actNow = actionsQueue.Dequeue();
        return ref actNow;
    }



    public void MoveByAction(ref Action act)
    {

        if (act.progress == ActionProgress.Ready)
        {
            // 각종 초기화

            // 스킵
            act.progress = ActionProgress.Start;
        }
        else if (act.progress == ActionProgress.Start)
        {
            // 이미 이동중일 경우 대기
            if (isBusy)
                return;

            // 좌표 설정 및 이동
            if (act.count > 0)
                MoveSet(
                    GameData.blockManager.GetBlock(GameData.blockManager.indexLoop(location, act.count)).transform.position,
                    act.speed,
                    false
                    );

            // 스킵
            act.progress = ActionProgress.Working;
        }
        else if (act.progress == ActionProgress.Working)
        {

            // 완료 체크
            if (!isBusy)
            {
                // 이동좌표 리셋
                movePoint = Vector3.zero;

                // 정면 보기
                MoveSet(transform.position, ActTurnSpeed, true);

                // 스킵
                act.progress = ActionProgress.Finish;
            }
        }
        else if (act.progress == ActionProgress.Finish)
        {
            // 종료 처리
            act.isFinish = true;
        }
    }


    /// <summary>
    /// 입력된 좌표를 저장 후 코루틴으로 이동 처리
    /// </summary>
    /// <param name="pos">목적지</param>
    /// <param name="speed">속도</param>
    public void MoveSet(Vector3 pos, float speed, bool isTurnAfterMove)
    {
        Debug.Log("이동 설정 :: (" + transform.name + ") -> " + pos);

        movePoint = pos;

        if (!isBusy)
            moveCoroutine = StartCoroutine(ActMove(movePoint, speed, isTurnAfterMove));
    }

    /// <summary>
    /// 입력된 좌표만큼 추가 후 코루틴으로 이동 처리
    /// </summary>
    /// <param name="pos">목적지</param>
    /// <param name="speed">속도</param>
    public void MoveAdd(Vector3 pos, float speed, bool isTurnAfterMove)
    {
        Debug.Log("이동 변경 :: (" + transform.name + ") -> " + pos);

        movePoint += pos;

        if (!isBusy)
            moveCoroutine = StartCoroutine(ActMove(movePoint, speed, isTurnAfterMove));
    }

    /// <summary>
    /// 구조 : 목표 바라보기 -> 이동 -> 정면 바라보기(옵션)
    /// </summary>
    /// <param name="pos">목표</param>
    /// <param name="speed">속도</param>
    /// <returns></returns>
    IEnumerator ActMove(Vector3 pos, float speed, bool isTurnAfterMove)
    {
        isBusy = true;

        // 목표 바라보기
        yield return StartCoroutine(ActTurnPoint(pos, speed));

        // 목표로 이동
        yield return StartCoroutine(ActMovePoint(pos, speed));

        // 정면 바라보기
        if (isTurnAfterMove)
            yield return StartCoroutine(ActTurnFornt(speed));

        // 구 코드 백업
        {
            //// 목표 바라보기
            //Vector3 dir = transform.position;
            //dir.y = pos.y - transform.position.y;
            //float elapsedTime = 0.000f;
            //while (Quaternion.LookRotation(dir.normalized).y / transform.rotation.y > 0.1f)
            //{
            //    elapsedTime += Time.deltaTime;
            //    // 회전
            //    transform.rotation = Quaternion.Lerp(
            //        transform.rotation,
            //        Quaternion.LookRotation(dir.normalized),
            //        elapsedTime * speed
            //        );

            //    yield return null;
            //}
            //// 회전 보정
            //transform.rotation = Quaternion.Lerp(
            //    transform.rotation,
            //    Quaternion.LookRotation(dir.normalized),
            //    1f
            //    );


            //// 목표로 이동
            //Vector3 posY = new Vector3(pos.x, transform.position.y, pos.z);
            //while (Vector3.Distance(transform.position, posY) > 0.1f)
            //{
            //    transform.position = Vector3.Lerp(transform.position, posY, Time.deltaTime * speed);

            //    yield return null;
            //}
            //// 값 보정
            //transform.position = posY;
        }
        
        isBusy = false;
    }

    /// <summary>
    /// 특정 좌표를 향해 이동
    /// </summary>
    /// <param name="pos">특정 좌표</param>
    /// <param name="speed"></param>
    /// <returns></returns>
    IEnumerator ActMovePoint(Vector3 pos, float speed)
    {
        // 목표로 이동
        Tool.HeightLimit(bodyObject, posMinY - transform.position.y, posMaxY - transform.position.y);
        Vector3 posY = new Vector3(pos.x, transform.position.y, pos.z);
        while (Vector3.Distance(transform.position, posY) > 0.1f)
        {
            //transform.position = Vector3.Lerp(transform.position, posY, Time.deltaTime * speed);

            // 프레임 목표점 계산
            Vector3 targetPos = Vector3.Lerp(transform.position, posY, Time.deltaTime * speed);
                        
            // 부호 추출
            Vector3 signPos = new Vector3(Mathf.Sign(targetPos.x - transform.position.x), 0, Mathf.Sign(targetPos.z - transform.position.z));
            // 최대치 계산
            Vector3 limitePos = transform.position + signPos * speed / 5.00f;



            // x축 최대치 보정
            if (Mathf.Abs(targetPos.x - transform.position.x) > Mathf.Abs(limitePos.x - transform.position.x))
            {
                Debug.Log("x축 보정됨");
                targetPos.x = limitePos.x;
            }
            // z축 최대치 보정
            if (Mathf.Abs(targetPos.z - transform.position.z) > Mathf.Abs(limitePos.z - transform.position.z))
            {
                Debug.Log("z축 보정됨");
                targetPos.z = limitePos.z;
            }

            // 이동 처리
            transform.position = targetPos;

            yield return null;
        }
        // 값 보정
        transform.position = posY;
    }

    /// <summary>
    /// 특정 좌표를 향해 회전
    /// </summary>
    /// <param name="pos">특정 좌표</param>
    /// <param name="speed"></param>
    /// <returns></returns>
    IEnumerator ActTurnPoint(Vector3 pos, float speed)
    {
        // 목표 바라보기
        Tool.HeightLimit(bodyObject, posMinY - transform.position.y, posMaxY - transform.position.y);
        Vector3 posY = new Vector3(pos.x, bodyObject.position.y, pos.z);
        float elapsedTime = 0.000f;
        //while (Mathf.Abs(Quaternion.LookRotation((posY - bodyObject.position).normalized).y) - Mathf.Abs(bodyObject.rotation.y) > 0.1f)
        while (Mathf.Abs(Quaternion.LookRotation((bodyObject.position- posY).normalized).y) - Mathf.Abs(bodyObject.rotation.y) > 0.1f)
        {
            elapsedTime += Time.deltaTime;
            // 회전
            bodyObject.rotation = Quaternion.Lerp(
                bodyObject.rotation,
                //Quaternion.LookRotation((posY - bodyObject.position).normalized),
                Quaternion.LookRotation((bodyObject.position - posY).normalized),
                elapsedTime * speed
                );

            yield return null;
        }
        // 회전 보정
        bodyObject.rotation = Quaternion.Lerp(
            bodyObject.rotation,
            //Quaternion.LookRotation((posY - bodyObject.position).normalized),
            Quaternion.LookRotation((bodyObject.position- posY).normalized),
            1f
            );
    }

    /// <summary>
    /// 정면을 향해 회전
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    IEnumerator ActTurnFornt(float speed)
    {
        // 정면 보기
        //Vector3 dir = bodyObject.position;
        //dir.z += 1f;
        while (Quaternion.LookRotation(Vector3.forward.normalized).y / bodyObject.rotation.y > 0.1f)
        {
            bodyObject.rotation = Quaternion.Lerp(
                bodyObject.rotation,
                Quaternion.LookRotation(Vector3.forward.normalized),
                Time.deltaTime * speed
                );

            yield return null;
        }
        //while (Quaternion.LookRotation((dir - bodyObject.position).normalized).y / bodyObject.rotation.y > 0.1f)
        //{
        //    bodyObject.rotation = Quaternion.Lerp(
        //        bodyObject.rotation,
        //        Quaternion.LookRotation((dir - bodyObject.position).normalized),
        //        Time.deltaTime * speed
        //        );

        //    yield return null;
        //}
        // 회전 보정
        bodyObject.rotation = Quaternion.Lerp(
            bodyObject.rotation,
            Quaternion.LookRotation(Vector3.forward.normalized),
            1f
            );
        //bodyObject.rotation = Quaternion.Lerp(
        //    bodyObject.rotation,
        //    Quaternion.LookRotation((dir - bodyObject.position).normalized),
        //    1f
        //    );
    }
}
