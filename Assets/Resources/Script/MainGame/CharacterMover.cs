using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    // 장애물 목록
    public static List<int> barricade = new List<int>();

    // 플레이어
    public Player owner = null;

    // 액션 큐
    public Queue<Action> actionsQueue = new Queue<Action>();

    // 현재 액션
    public Action actNow = new Action();


    // 회전 속도
    public static float ActTurnSpeed = 5f;


    // 목표 이동 좌표
    Vector3 movePoint = Vector3.zero;

    // 이동 제어용
    Coroutine moveCoroutine = null;
    public bool isBusy = false;



    // 공격 리스트
    List<Player> attackTarget = new List<Player>();
    bool attackNow = false;


    // 아이템 습득 리스트
    List<DynamicItem> itemList = new List<DynamicItem>();

    // 아이템 습득 제어용
    public bool isPickingUpItem = false;
    bool itemFinish = false;

    bool eventFinish = false;



    // 위치 인덱스
    int _location = -1;
    //public int location { get { return _location; } set { _location = GameData.blockManager.indexLoop(_location, value); } }
    public int location { get { return _location; } set { _location = GameData.blockManager.indexLoop(value, 0); } }

    // 이동량
    public int moveCount = 0;

    // 현재 위치 기반 오브젝트
    public Transform locateBlock { get { if (GameData.isMainGameScene) { if (location >= 0) return GameData.blockManager.GetBlock(location).transform; else return GameData.blockManager.startBlock; } else return null; } }



    // 이동 속도
    float moveSpeed = 2.00f;


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

        Debug.LogWarning("이동 예상 :: "+moveValue + " 칸 이동하여 도착지점 = " + movePoint);

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

            // 플레이어 체크
            for (int p = 0; p < owner.otherPlayers.Count; p++)
            {
                // 공격 최소 사이클 제한
                if (GameData.cycle.now < 5)
                    break;
                   

                // 체크 대상
                Player current = owner.otherPlayers[p];

                // 경로에 있을 경우
                if (current.movement.location == locNext)
                {
                    Debug.Log("플레이어 탐지 : " + counter);

                    //스케줄링 추가 - counter만큼 칸수 지정
                    actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));

                    //스케줄링 추가 - 공격 처리
                    actionsQueue.Enqueue(new Action(Action.ActionType.Attack, locNext, moveSpeed));

                    // 카운터 리셋
                    counter = 0;

                    // 중단
                    break;
                }
            }


            // 장애물 체크
            if (barricade[locNext] > 0)
            {
                Debug.Log("장애물 탐지 : " + counter);

                //스케줄링 추가 - counter만큼 칸수 지정
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));
                //actionsQueue.Enqueue(new Action(Action.ActionType.Move, counter, moveSpeed));

                //스케줄링 추가 - 장애물 처리 추가
                actionsQueue.Enqueue(new Action(Action.ActionType.Barricade, locNext, moveSpeed));

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



    /// <summary>
    /// 공격 액션을 사용한 공격
    /// </summary>
    /// <param name="act"></param>
    public void AttackPlayer(ref Action act)
    {
        // 위치
        int loc = act.count;

        if (act.progress == ActionProgress.Ready)
        {
            // 각종 초기화
            attackTarget.Clear();
            attackNow = false;

            // 스킵
            act.progress = ActionProgress.Start;
        }
        else if (act.progress == ActionProgress.Start)
        {
            Debug.LogWarning("공격 목록 작성");

            // 공격 대상 스캔
            for (int i = 0; i < owner.otherPlayers.Count; i++)
                // 액션위치에 있는 다른 플레이어
                if (owner.otherPlayers[i].movement.location == loc)
                {
                    // 등록
                    attackTarget.Add(owner.otherPlayers[i]);

                    // 로그
                    Debug.LogWarning("공격 목록 추가 :: " + owner.otherPlayers[i].name + "가 추가됨");
                }
                //else Debug.LogWarning("공격 대상 아님 :: " + owner.otherPlayers[i].name);

            // 스킵
            act.progress = ActionProgress.Working;
        }
        else if (act.progress == ActionProgress.Working)
        {
            // 공격중이 아닐때
            if (!attackNow)
            {
                // 공격 대상이 있을 경우
                if ( attackTarget.Count > 0)
                {
                    // 공격 처리
                    attackNow = true;
                    
                    // 대상들에게 일괄 공격
                    for (int i = 0; i < attackTarget.Count; i++)
                    {
                        // 공격
                        owner.Attack(attackTarget[i]);

                        // 로그
                        Debug.LogWarning("공격 시도 :: " + owner.name + "가 " + attackTarget[i].name + "를 공격");
                    }
                    // 임시 : 아래의 종료판정으로 옮길것
                    attackTarget.Clear();
                    attackNow = false;
                }
                else
                    // 공격 종료시 스킵
                    act.progress = ActionProgress.Finish;
            }
            // 공격중일때
            else
            {
                // 공격 종료 판정
                // ================조건 변경 할것 => 애니메이션 상태 이용
                if (false)
                    attackNow = false;
            }
        }
        else if (act.progress == ActionProgress.Finish)
        {

            // 종료 처리
            act.isFinish = true;
        }
    }


    public void CheckBarricade(ref Action act)
    {
        // 위치
        int loc = act.count;

        if (act.progress == ActionProgress.Ready)
        {
            Debug.LogWarning("아이템 오브젝트 :: 액션 위치 = " + act.count);

            // 아이템 : 초기화
            itemList.Clear();
            isPickingUpItem = false;
            itemFinish = false;

            // 이벤트 : 초기화
            // 이벤트 리스트 클리어 미구현==========
            // 이벤트 작동중 bool값 미구현============
            eventFinish = false;

            // 스킵
            act.progress = ActionProgress.Start;
        }
        else if (act.progress == ActionProgress.Start)
        {
            // 아이템 : 리스트 작성
            for (int i = 0; i < ItemManager.itemObjectList.Count; i++)
            {
                // 위치가 일치할 경우
                if (ItemManager.itemObjectList[i].location == loc)
                {
                    // 인벤토리가 남았을 경우
                    if (owner.inventoryCount < Player.inventoryMax)
                    {
                        // 획득 대기열 추가
                        itemList.Add(ItemManager.itemObjectList[i]);

                        // 획득
                        //ItemManager.itemObjectList[i].GetItem(owner);

                        // 로그
                        Debug.LogWarning("아이템 오브젝트 :: 습득 목록 추가 => " + ItemManager.itemObjectList[i].item.index);
                    }
                    else
                        break;
                }
            }

            // 이벤트 : 리스트 작성

            // 스킵
            act.progress = ActionProgress.Working;
        }
        else if (act.progress == ActionProgress.Working)
        {

            // 아이템 : 습득
            {
                if (itemList.Count > 0)
                {
                    // 습득중이지 않을때
                    if (!isPickingUpItem)
                    {
                        // 획득중 처리
                        isPickingUpItem = true;

                        // 습득 연출
                        // 미구현
                    }
                    else
                    {

                        // ==========연출 종료를 조건으로 하위 묶을것

                        Debug.LogWarning("아이템 오브젝트 :: 습득 => " + itemList[0].item.index);

                        // 획득
                        itemList[0].GetItem(owner);

                        // 리스트에서 제거
                        itemList.RemoveAt(0);

                        // 획득 종료 처리 
                        isPickingUpItem = false;
                    }
                }
                // 습득 목록 비었을때
                else
                {
                    //습득중이지 않을때
                    if (!isPickingUpItem)
                        itemFinish = true;
                }
            }

            // 이벤트 : 작동
            {
                // 미구현=========================
                // 임시 처리
                eventFinish = true;
            }

            // 완료 체크
            if (itemFinish && eventFinish)
                // 스킵
                act.progress = ActionProgress.Finish;
        }
        else if (act.progress == ActionProgress.Finish)
        {
            // 종료 처리
            act.isFinish = true;
        }
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
                // 버그 발생! =============== 이동 한번 할때마다 정면 바라봄 => 정면보기 액션 플랜 독립시킬것
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
            Vector3 limitePos = transform.position + signPos * speed / 5.00f * 0.5f;



            // x축 최대치 보정
            if (Mathf.Abs(targetPos.x - transform.position.x) > Mathf.Abs(limitePos.x - transform.position.x))
            {
                //Debug.Log("x축 보정됨");
                targetPos.x = limitePos.x;
            }
            // z축 최대치 보정
            if (Mathf.Abs(targetPos.z - transform.position.z) > Mathf.Abs(limitePos.z - transform.position.z))
            {
                //Debug.Log("z축 보정됨");
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


    public void GotoJail()
    {
        Debug.LogWarning("감옥으로 이동 :: " + transform.name);

        // 행동제한 부여
        owner.stunCount = 3;

        // 위치 변경
        _location = -1;

        // 위치 변경
        owner.isDead = true;

        // 좌표 확보
        Vector3 jailPos = GameData.blockManager.startBlock.position;

        // 기존 이동 중단
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        // 이동
        moveCoroutine = StartCoroutine(ActFly(jailPos, 2f, true));
    }

    /// <summary>
    /// 공중으로 상승하여 이동 후 하강
    /// </summary>
    /// <param name="pos">착지 지점</param>
    /// <param name="speed">속도</param>
    /// <param name="isTurnAfterMove">이동 후 정면</param>
    /// <returns></returns>
    IEnumerator ActFly(Vector3 pos, float speed, bool isTurnAfterMove)
    {
        float height = transform.position.y;

        // 상승
        yield return StartCoroutine(ActFlyPoint(transform.position + Vector3.up * 5, speed));

        // 좌표 지정
        Vector3 flyPoint = pos;
        flyPoint.y = transform.position.y;

        // 이동
        yield return StartCoroutine(ActMovePoint(flyPoint, speed * 5f));

        // 좌표 지정
        flyPoint.y = height;

        // 하강
        yield return StartCoroutine(ActFlyPoint(flyPoint, speed));

        // 정면 바라보기
        if (isTurnAfterMove)
            yield return StartCoroutine(ActTurnFornt(speed));

    }

    /// <summary>
    /// 특정 좌표를 향해 이동
    /// </summary>
    /// <param name="pos">특정 좌표</param>
    /// <param name="speed"></param>
    /// <returns></returns>
    IEnumerator ActFlyPoint(Vector3 pos, float speed)
    {
        // 목표로 이동
        Tool.HeightLimit(bodyObject, posMinY - transform.position.y, posMaxY - transform.position.y);
        while (Vector3.Distance(transform.position, pos) > 0.1f)
        {
            //transform.position = Vector3.Lerp(transform.position, posY, Time.deltaTime * speed);

            // 프레임 목표점 계산
            Vector3 targetPos = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed);

            // 부호 추출
            Vector3 signPos = new Vector3(Mathf.Sign(targetPos.x - transform.position.x), 0, Mathf.Sign(targetPos.z - transform.position.z));
            // 최대치 계산
            Vector3 limitePos = transform.position + signPos * speed / 5.00f * 0.5f;



            // x축 최대치 보정
            if (Mathf.Abs(targetPos.x - transform.position.x) > Mathf.Abs(limitePos.x - transform.position.x))
            {
                //Debug.Log("x축 보정됨");
                targetPos.x = limitePos.x;
            }
            // z축 최대치 보정
            if (Mathf.Abs(targetPos.z - transform.position.z) > Mathf.Abs(limitePos.z - transform.position.z))
            {
                //Debug.Log("z축 보정됨");
                targetPos.z = limitePos.z;
            }

            // 이동 처리
            transform.position = targetPos;

            yield return null;
        }
        // 값 보정
        transform.position = pos;
    }
}
