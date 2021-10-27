using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    // 장애물 목록
    //public static List<int> barricade = new List<int>();
    //public static List<List<DynamicObject>> barricade = new List<List<DynamicObject>>();

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


    
    // 오브젝트 습득 제어용
    Coroutine objectPickUp = null;
    List<DynamicObject> objectPickUpList = new List<DynamicObject>();
    ActionProgress objectPickUpStep = ActionProgress.Ready;



    // 위치 인덱스
    int _location = -1;
    //public int location { get { return _location; } set { _location = GameData.blockManager.indexLoop(_location, value); } }
    public int location { get { return _location; } set { _location = GameData.blockManager.indexLoop(value, 0); owner.MirrorLoaction(); } }

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
    public Transform bodyObject = null;

    // 애니메이션
    public Animator animator = null;

    // 임시용 택스쳐
    public SkinnedMeshRenderer avatarColor = null;


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

        // 액션 자동 수행
        ActionCall();
    }


    public static void AvatarOverFixAll()
    {
        // 플레이어 검색 리스트
        List<Player> fixSearch = new List<Player>(Player.allPlayer);

        // 작업 대상
        List<Player> targetList = new List<Player>();

        // 검색
        while (fixSearch.Count > 0)
        {
            // 퀵등록
            Player current = fixSearch[0];

            // 작업 대상 추가
            targetList.Clear();
            targetList.Add(current);

            // 겹침 여부
            bool isOver = false;

            // 다른플레이어와 위치 비교
            for (int i = 0; i < current.otherPlayers.Count; i++)
            {
                // 퀵등록
                Player other = current.otherPlayers[i];
                
                // 위치 중복 체크
                if (current.location == other.location)
                {
                    // 중복 플레이어 검색에서 제외
                    fixSearch.Remove(other);

                    // 작업 대상 지정
                    targetList.Add(other);

                    // 겹침 처리
                    isOver = true;
                }
                //if (current.movement.location == other.movement.location)
                //{
                //    // 중복 플레이어 검색에서 제외
                //    fixSearch.Remove(other);

                //    // 작업 대상 지정
                //    targetList.Add(other);

                //    // 겹침 처리
                //    isOver = true;
                //}
            }

            // 검색 끝난 플레이어 검색 제외
            fixSearch.Remove(current);

            // 겹쳤으면 해소 작업 호출
            //if (isOver)
                ReLocation(targetList);
        }
    }

    static void ReLocation(List<Player> playerList)
    {
        // 대상 미지정시 중단
        if (playerList == null || playerList.Count == 0)
            return;

        // 겹친 좌표
        //Vector3 crossPoint = playerList[0].movement.locateBlock.position;
        Vector3 crossPoint;

        // 스타트 블록 우회
        if (playerList[0].location == -1)
            crossPoint = BlockManager.script.startBlock.position;
        else
            crossPoint = BlockManager.script.GetBlock(playerList[0].location).transform.position;

        // 대상이 1명일 경우
        if (playerList.Count == 1)
        {
            // 원점으로 이동
            playerList[0].movement.MoveSet(crossPoint, ActTurnSpeed, true);
            return;
        }

        int count = playerList.Count;

        for (int i = 0; i < count; i++)
        {
            // 퀵 등록
            Player current = playerList[0];

            // 임시 생성
            GameObject obj = new GameObject("pos");

            // 임시 이동
            //playerList[0].movement.transform.RotateAround(crossPoint, Vector3.down, (360 / playerList.Count * i));
            float angle = 180 / count + 360 / count * -i;
            obj.transform.position = crossPoint + Vector3.forward * 2;
            obj.transform.RotateAround(crossPoint, Vector3.down, angle);

            // 좌표 추출
            Vector3 pos = obj.transform.position;
            
            // 임시 오브젝트 제거
            Destroy(obj);

            // 이동 명령
            playerList[i].movement.MoveSet(pos, ActTurnSpeed, true);
        }
    }

    /// <summary>
    /// 아바타 겹침 처리
    /// </summary>
    public void AvatarOverFix()
    {
        // 중복 플레이어 리스트
        List<Player> fixTarget = new List<Player>();
        fixTarget.Add(owner);

        // 모든 플레이어 위치 체크
        for (int i = 0; i < owner.otherPlayers.Count; i++)
        {
            // 퀵등록
            Player other = owner.otherPlayers[i];

            // 위치 중복 플레이어 확보
            if (location == other.movement.location)
                fixTarget.Add(other);
        }

        // 겹쳤으면 해소 작업 호출
        if (fixTarget.Count > 1)
            ReLocation(fixTarget);

        return;
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
        // 이동 안할경우
        if(moveValue == 0)
            actionsQueue.Enqueue(new Action(Action.ActionType.Stop, owner.location, moveSpeed));

        // 최종 목표
        int movePoint = GameData.blockManager.indexLoop(_location, moveValue);

        Debug.Log("이동 예상 :: "+moveValue + " 칸 이동하여 도착지점 = " + movePoint);

        // 총 이동 거리(moveValue) 이내에 장애물 계산
        int counter = 0;
        int _sign = (int)Mathf.Sign(moveValue);
        for (int i = 0; i != moveValue; i += _sign)
        {
            counter += _sign;

            //int loc = GameData.blockManager.indexLoop(location, ((i) * _sign));
            //int locNext = GameData.blockManager.indexLoop(location,  ((i + 1) * _sign));
            int loc = GameData.blockManager.indexLoop(location, i);
            int locNext = GameData.blockManager.indexLoop(location, i + _sign );

            // 스타트 블록 체크
            if (location == -1 && i == 0)
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
                // 체크 대상
                Player current = owner.otherPlayers[p];

                // 경로에 있을 경우
                if (current.movement.location == locNext)
                {
                    Debug.Log("플레이어 탐지 : " + counter);

                    //스케줄링 추가 - counter만큼 칸수 지정
                    actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));

                    //스케줄링 추가 - 공격 처리
                    if (Cycle.now > 5 || owner == Player.system.Monster)  
                    actionsQueue.Enqueue(new Action(Action.ActionType.Attack, locNext, moveSpeed));

                    // 카운터 리셋
                    counter = 0;

                    // 중단
                    break;
                }
            }

            // 장애물 체크
            if (DynamicObject.objectList[locNext].Count > 0)
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


            // 종료 전 체크
            if (i + _sign == moveValue)
            {
                Debug.Log("종료 탐지 : " + counter);

                // 이동 마무리
                if (counter != 0)
                {
                    //스케줄링 추가 - counter만큼 칸수 지정
                    actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));

                    // 카운터 리셋
                    counter = 0;
                }

                //스케줄링 추가 - 최종 액션 : 정지 후 정면
                actionsQueue.Enqueue(new Action(Action.ActionType.Stop, i + _sign, moveSpeed));
            }
            // 코너 체크 - 도착점이 코너일 경우 코너탐지 필요 없음
            else if (BlockManager.dynamicBlockList[locNext].isCorner)
            {
                Debug.Log("코너 탐지 : " + counter);

                //스케줄링 추가 - counter만큼 칸수 지정
                actionsQueue.Enqueue(new Action(Action.ActionType.Move, i + _sign, moveSpeed));

                // 카운터 리셋
                counter = 0;
            }
        }

        //스케줄링 추가 - 정면 보기
        //actionsQueue.Enqueue(new Action(Action.ActionType.Turn, 2, moveSpeed));
    }


    public void ActionCall()
    {
        // 액션 미수행 경우
        if (actNow.type == Action.ActionType.None)
        {
            // 잔여 액션 있음
            if (actionsQueue.Count > 0)
                GetAction();

            // 모든 액션 소진
            else
            {
                // 대기
            }
        }
        // 액션 수행
        else if (!actNow.isFinish)
        {
            // 이동 처리
            if (actNow.type == Action.ActionType.Move)
                MoveByAction(ref actNow);

            // 장애물 처리
            else if (actNow.type == Action.ActionType.Barricade)
                CheckBarricade(ref actNow);

            // 공격 처리
            else if (actNow.type == Action.ActionType.Attack)
                AttackPlayer(ref actNow);

            // 정면 회전 처리
            else if (actNow.type == Action.ActionType.Stop)
                StopByAction(ref actNow);

        }
        // 액션 종료 처리
        else //if (actNow.isFinish)
        {
            // 액션 소거
            actNow = new Action();
        }
    }


    public ref Action GetAction()
    {
        actNow = actionsQueue.Dequeue();

        Debug.Log("액션 :: " + actNow.type.ToString());

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
            Debug.Log("액션 :: 플래닝 -> 공격 목록 작성");

            // 공격 대상 스캔
            for (int i = 0; i < owner.otherPlayers.Count; i++)
                // 액션위치에 있는 다른 플레이어
                if (owner.otherPlayers[i].movement.location == loc)
                {
                    // 등록
                    attackTarget.Add(owner.otherPlayers[i]);

                    // 로그
                    Debug.Log("액션 :: 플래닝 -> 공격 목록 추가 :: " + owner.otherPlayers[i].name + "가 추가됨");
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
                        Debug.Log("액션 :: 공격 시도 -> " + owner.name + "가 " + attackTarget[i].name + "를 공격");
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
            Debug.Log("액션 :: 오브젝트 습득 -> 위치 = " + act.count);

            // 오브젝트 : 초기화
            objectPickUpList.Clear();
            objectPickUpStep = ActionProgress.Ready;


            //// 아이템 : 초기화
            //itemList.Clear();
            //isPickingUpItem = false;
            //itemFinish = false;

            //// 이벤트 : 초기화
            //eventList.Clear();
            //isPickingUpEvent = false;
            //eventFinish = false;

            // 스킵
            act.progress = ActionProgress.Start;
        }
        else if (act.progress == ActionProgress.Start)
        {
            // 초기화
            objectPickUpList.Clear();

            // 위치의 오브젝트 목록 가져오기
            List<DynamicObject> localBarricade = DynamicObject.objectList[loc];

            // 오브젝트 : 리스트 작성
            for (int i = 0; i < localBarricade.Count; i++)
            {
                DynamicObject obj = localBarricade[i];

                // 획득 조건 충족
                if (obj.CheckCondition(owner))
                {
                    // 획득 대기열 추가
                    objectPickUpList.Add(obj);

                    // 로그
                    Debug.Log(string.Format("액션 :: {0} 오브젝트 -> {1}의 습득 목록 추가", obj.type, obj.transform.name));

                    continue;
                }
                else
                    Debug.Log(string.Format("액션 :: {0} 오브젝트 -> {1}의 습득 자격 미달", obj.type, obj.transform.name));

            }

            // 스킵
            act.progress = ActionProgress.Working;
        }
        else if (act.progress == ActionProgress.Working)
        {
            // 오브젝트 : 습득
            if (objectPickUpStep == ActionProgress.Ready)
            objectPickUp = StartCoroutine(GetObject());


            // 완료 체크
            //if (itemFinish && eventFinish)
            if (objectPickUpStep == ActionProgress.Finish)
            {
                // 스킵
                objectPickUpStep = ActionProgress.Ready;
                act.progress = ActionProgress.Finish;
            }
        }
        else if (act.progress == ActionProgress.Finish)
        {
            // 종료 처리
            act.isFinish = true;
        }
    }

    void SetObjectPickUpList()
    {
        // 초기화
        objectPickUpList.Clear();


    }

    /// <summary>
    /// 오브젝트 획득 리스트 전체 획득
    /// </summary>
    /// <returns></returns>
    IEnumerator GetObject()
    {
        // 습득 시작
        objectPickUpStep = ActionProgress.Start;

        // 전부 획득
        while (objectPickUpList.Count > 0)
        {
            // 개별 습득 대기
            yield return GetObject(objectPickUpList[0]);
        }

        // 습득 종료 처리
        objectPickUpStep = ActionProgress.Finish;
    }

    /// <summary>
    /// 오브젝트 단일 습득
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    IEnumerator GetObject(DynamicObject current)
    {
        // 습득중 처리
        objectPickUpStep = ActionProgress.Working;

        // 아이템일 경우
        if (current.type == DynamicObject.Type.Item)
        {
            DynamicItem di = (DynamicItem)current;

            Debug.Log("액션 :: 아이템 오브젝트 습득 => " + di.item.index);

            // 획득
            di.GetItem(owner);

            // 연출 명령
            yield return null;
        }

        // 아이템일 경우
        else if (current.type == DynamicObject.Type.Event)
        {
            DynamicEvent de = (DynamicEvent)current;

            Debug.Log("액션 :: 이벤트 오브젝트 습득 => " + de.iocEvent.index);

            // 획득
            //de.GetEvent(owner, de.location);
            yield return de.GetEvent(owner);

            // 연출 명령
            yield return null;
        }

        // 습득 리스트에서 제거
        objectPickUpList.Remove(current);

        // 습득중 완료 처리
        objectPickUpStep = ActionProgress.Start;
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
            if (act.count != 0)
                MoveSet(
                    BlockManager.dynamicBlockList[BlockManager.script.indexLoop(location, act.count)].transform.position,
                    act.speed,
                    false
                    );


            // 임시 이동값 설정
            owner.location = location + act.count;
            AvatarOverFixAll();

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
                //MoveSet(transform.position, ActTurnSpeed, true);

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




    public void StopByAction(ref Action act)
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
            //if (isBusy)
            //    return;

            // 좌표 설정 및 이동
            //MoveSet(transform.position, ActTurnSpeed, true);

            // 정지
            //StopCoroutine(moveCoroutine);

            // 스킵
            act.progress = ActionProgress.Working;
        }
        else if (act.progress == ActionProgress.Working)
        {
            // 완료 체크
            //if (!isBusy)
            {
                // 좌표 변경
                //location +=  owner.dice.valueTotal;
                location = owner.location;

                // 이동좌표 리셋
                movePoint = locateBlock.position;

                // 겹침 정렬
                CharacterMover.AvatarOverFixAll();

                // 스킵
                act.progress = ActionProgress.Finish;
            }
        }
        else if (act.progress == ActionProgress.Finish)
        {
            // 걷기 정지
            if (owner.type != Player.Type.System)
                animator.SetFloat("Speed", 0f);

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
        else
            Debug.LogWarning("error :: 이미 액션 수행중 -> " + actNow.type);
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
        else
            Debug.LogWarning("error :: 이미 액션 수행중 -> " + actNow.type);
    }

    /// <summary>
    /// 현재 액션과 정지(마지막) 액션 사이의 모든 액션 제거
    /// 주의 :: 즉시 멈추지 않음
    /// </summary>
    public void MoveStop()
    {
        Debug.Log("액션 중단 :: (" + transform.name + ") 에서 요청됨 -> " + owner.location + " = " + location);

        //int diceTemp = owner.location - location;
        int diceTemp = owner.dice.valueTotal - (owner.location - location);
        Debug.Log("액션 중단 :: 잔여 주사위 = " + diceTemp);
        owner.dice.SetValueTotal(diceTemp);

        // 스킵 대상 없을 경우
        if (actNow.type == Action.ActionType.Stop || actionsQueue.Count == 0)
            return;
        else
        {
            // 스킵 대상 없을 때 까지 제거
            while (actionsQueue.Peek().type != Action.ActionType.Stop)
            {
                // 제거
                //    Action.ActionType aType = actionsQueue.Dequeue().type;
                //    Debug.LogError("액션 제거 :: " + aType);
                Debug.Log("액션 제거 :: " + actionsQueue.Dequeue().type);
            }

            //// 액션 전체 제거
            //while (actionsQueue.Count > 0)
            //{
            //    // 제거
            //    Action.ActionType aType = actionsQueue.Dequeue().type;

            //    Debug.LogError("액션 제거 :: " + aType);
            //}



            //// 정지
            //if (moveCoroutine != null)
            //{
            //    //MoveByAction(ref actNow);
            //    Debug.LogError("액션 중단 :: " + actNow.type);
            //    StopCoroutine(moveCoroutine);
            //}

            //// 좌표 변경
            //location = owner.location;

            //// 이동좌표 리셋
            //movePoint = locateBlock.position;

            //// 겹침 정렬
            //CharacterMover.AvatarOverFixAll();

            //// 강제 호출
            //GetAction();
            ////StopByAction(ref actNow);

            //// 테스트용 멈춤============
            //actNow.type = Action.ActionType.Idle;

            // 좌표 변경
            location = owner.location;

            // 걷기 정지
            if (owner.type != Player.Type.System)
                animator.SetFloat("Speed", 0f);
        }
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
        yield return ActTurnPoint(pos, speed);

        // 목표로 이동
        yield return ActMovePoint(pos, speed);

        // 정면 바라보기
        if (isTurnAfterMove)
            yield return ActTurnFornt(speed);
        
        isBusy = false;

        // 걷기 정지
        if (owner.type != Player.Type.System)
            animator.SetFloat("Speed", 0f);
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

        // 좌표 저장
        float dist = Vector3.Distance(transform.position, posY);
        float distNow;
        while ( (distNow = Vector3.Distance(transform.position, posY)) > 0.1f )
        {
            //transform.position = Vector3.Lerp(transform.position, posY, Time.deltaTime * speed);

            // 프레임 목표점 계산
            Vector3 targetPos = Vector3.Lerp(transform.position, posY, Time.deltaTime * speed);
                        
            // 부호 추출
            Vector3 signPos = new Vector3(Mathf.Sign(targetPos.x - transform.position.x), 0, Mathf.Sign(targetPos.z - transform.position.z));
            // 최대치 계산
            Vector3 limitePos = transform.position + signPos * speed / 5.00f * 0.5f;


            bool isCorner = actionsQueue.Count > 0 && actionsQueue.Peek().type == Action.ActionType.Move; //&& dist / 2 < distNow;
            // 코너 감속 차단
            if (isCorner)
            {
                // x축 최소치 보정
                targetPos.x = limitePos.x;
                targetPos.z = limitePos.z;
            }
            else
            {
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
            }


            // x축 초과 보정
            if (Mathf.Abs(posY.x - transform.position.x) < Mathf.Abs(targetPos.x - transform.position.x))
            {
                //Debug.Log("x축 보정됨");
                targetPos.x = posY.x;
            }
            // z축 초과 보정
            if (Mathf.Abs(posY.z - transform.position.z) < Mathf.Abs(limitePos.z - transform.position.z))
            {
                //Debug.Log("z축 보정됨");
                targetPos.z = posY.z;
            }


            // 이동 처리
            transform.position = targetPos;

            // 속도 반영
            if (owner.type != Player.Type.System)
                animator.SetFloat("Speed", speed);
            //float newSpeed = animator.GetFloat("Speed") + speed * Time.deltaTime;
            //if (newSpeed > speed)
            //    animator.SetFloat("Speed", speed);
            //else
            //    animator.SetFloat("Speed", newSpeed);

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

        // Y값 제외된 좌표
        Vector3 posXZ = new Vector3(pos.x, bodyObject.position.y, pos.z);

        // 좌표 차이값
        Vector3 posDifXZ = posXZ - bodyObject.position;

        // 방향값
        Quaternion look = Quaternion.LookRotation(posDifXZ.normalized);

        float lookY = Mathf.Abs(look.y);
        while (lookY - Mathf.Abs(bodyObject.rotation.y) > 0.1f)
        {
            bodyObject.rotation = Quaternion.Lerp(
                bodyObject.rotation,
                look,
                Time.deltaTime * speed
                );

            yield return null;
        }

        // 회전 보정
        bodyObject.rotation = look;
    }

    /// <summary>
    /// 정면을 향해 회전
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    IEnumerator ActTurnFornt(float speed)
    {
        // 정면 보기
        Quaternion reRot = Quaternion.Euler(0, 180, 0);
        float reRotY = Mathf.Abs(reRot.y);
        while (reRotY - Mathf.Abs(bodyObject.rotation.y) > 0.1f)
        {
            bodyObject.rotation = Quaternion.Lerp(
                bodyObject.rotation,
                reRot,
                Time.deltaTime * speed
                );

            yield return null;
        }

        // 회전 보정
        bodyObject.rotation = reRot;
    }


    public void GotoJail()
    {
        Debug.Log("액션 :: 감옥으로 이동 -> " + transform.name);

        // 잔여 액션 제거
        actionsQueue.Clear();
        actNow = new Action();

        // 행동제한 부여
        owner.stunCount = 3;

        // 걷기 정지
        if (owner.type != Player.Type.System)
            animator.SetFloat("Speed", 0f);

        // 위치 변경
        _location = -1;
        owner.location = -1;

        // 겹침 해소
        AvatarOverFix();

        // 위치 변경
        owner.isDead = true;

        // 좌표 확보
        Vector3 jailPos = GameData.blockManager.startBlock.position;

        // 기존 이동 중단
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        // 이동
        moveCoroutine = StartCoroutine(ActFly(jailPos, 2f, true));

        // 턴 마무리
        if(owner == Turn.now)
        {
            Turn.turnAction = Turn.TurnAction.Ending;
            Turn.actionProgress = ActionProgress.Start;
        }
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
        isBusy = true;

        //float height = transform.position.y;

        //// 상승
        //yield return ActFlyPoint(transform.position + Vector3.up * 5, speed);

        //// 좌표 지정
        //Vector3 flyPoint = pos;
        //flyPoint.y = transform.position.y;

        //// 이동
        //yield return ActMovePoint(flyPoint, speed * 5f);

        //// 좌표 지정
        //flyPoint.y = height;

        //// 하강
        //yield return ActFlyPoint(flyPoint, speed);


        // 하강
        yield return ActTleport(pos, speed);


        // 정면 바라보기
        if (isTurnAfterMove)
            yield return ActTurnFornt(speed);

        isBusy = false;
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


    public IEnumerator Tleport(int blockIndex, float speed)
    {
        // 이동
        if (blockIndex == -1)
            yield return ActTleport(BlockManager.script.startBlock.transform.position, speed);
        else
            yield return ActTleport(BlockManager.script.GetBlock(blockIndex).transform.position, speed);

        // 이동 반영
        location = blockIndex;
        owner.location = blockIndex;

        // 정렬
        AvatarOverFixAll();
    }

    /// <summary>
    /// 한바퀴 돌고 즉시 위치이동
    /// </summary>
    /// <param name="pos">이동 지점</param>
    /// <param name="speed">속도</param>
    /// <returns></returns>
    IEnumerator ActTleport(Vector3 pos, float speed)
    {
        isBusy = true;

        // 누적 시간
        float elapsedTime = 0f;

        // 스케일 백업
        Vector3 sclBack = transform.localScale;
        Vector3 scl = sclBack;

        // 딜레이
        WaitForSeconds waiter = new WaitForSeconds(0.5f);
        yield return waiter;


        // 준비 - 굵기 0으로 수렴
        while (transform.localScale.x > 0f)
        {
            // 시간 누적
            elapsedTime += Time.deltaTime * speed;

            // 회전
            transform.Rotate(Vector3.down * elapsedTime);

            // 굵기 계산
            scl.x -= elapsedTime;
            scl.z -= elapsedTime;

            // 굵기 반영
            transform.localScale = scl;

            yield return null;
        }


        // 준비 완료 확인
        scl.x = 0f;
        scl.z = 0f;
        transform.localScale = scl;

        // 이동
        transform.position = pos;
        yield return null;
        //yield return ActFlyPoint(pos, speed);


        // 딜레이
        waiter = new WaitForSeconds(0.5f);
        yield return waiter;

        // 누적시간 리셋
        elapsedTime = 0f;


        // 착지 - 굵기 복구
        while (transform.localScale.x < sclBack.x)
        {
            // 시간 누적
            elapsedTime += Time.deltaTime * speed;

            // 회전
            transform.Rotate(Vector3.down * elapsedTime);

            // 굵기 계산
            scl.x += elapsedTime;
            scl.z += elapsedTime;

            // 굵기 반영
            transform.localScale = scl;

            yield return null;
        }


        // 착지 완료 확인
        transform.rotation = Quaternion.identity;
        transform.localScale = sclBack;


        // 정면 바라보기
        //if (isTurnAfterMove)
        //    yield return ActTurnFornt(speed));
        yield return ActTurnFornt(speed);

        // 딜레이
        waiter = new WaitForSeconds(1f);
        yield return waiter;

        isBusy = false;
    }
}
