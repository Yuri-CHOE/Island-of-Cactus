using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    public enum DiceAction
    {
        Wait,
        Hovering,
        Rising,
        Spinning,
        Landing,
        Finish,
    }
    [SerializeField]
    CameraManager cm = null;

    // 주사위 정보
    public Dice dice = null;
    //public Dice diceTurnPlayer { get { return GameData.turn.now.dice; } }

    // 주사위 주인
    public Player owner = null;
    //public Player owner { get { return GameData.turn.now; } }

    // 소유자 확인
    public bool isMyDice { get { return owner == GameData.player.me; } }

    // 최소 높이
    [SerializeField]
    float minHeight = 1.9f;

    // 최대 높이
    [SerializeField]
    float maxHeight = 30.0f;

    // 최대 높이
    [SerializeField]
    Vector3 diceDistance = new Vector3(0, 3.1f, -2);

    // 회전 속도
    [Header("spin")]
    float _rotSpeed = 1.00f;
    float rotSpeed { get { return _rotSpeed * rotAccel; } }
    [SerializeField]
    float rotAccelMax = 100f;
    [SerializeField]
    float rotAccel = 0.00f;

    // 상승 하강 속도
    [Header("rise")]
    float posSpeed = 5.00f;
    float timeLimit = 3.00f;


    // 반응 제어
    public bool isInputBlock { get { return GameData.worldManager.cameraManager.controller.isFreeMode; } }

    // 주사위 눈 별 각도값
    Quaternion eye1 = Quaternion.Euler(-90, 0, 0);
    Quaternion eye2 = Quaternion.Euler(0, 0, 0);
    Quaternion eye3 = Quaternion.Euler(0, 0, 90);
    Quaternion eye4 = Quaternion.Euler(0, 0, -90);
    Quaternion eye5 = Quaternion.Euler(0, 0, 180);
    Quaternion eye6 = Quaternion.Euler(90, 0, 0);


    [Header("Action")]
    // 액션 종류
    public DiceAction action = DiceAction.Wait;
    // 액션 진행도
    public ActionProgress actionProgress = ActionProgress.Ready;
    float elapsedTime = 0.00f;
    public bool isTimeCountWork = false;


    // AI 제어
    public Coroutine AIWork = null;
    // 진행 인풋
    public bool doForceClick = false;
    public bool doForceClickUp = false;
    // 진행 아웃풋
    public bool isFree { get { return action == DiceAction.Wait && actionProgress == ActionProgress.Ready; } }
    public bool isBusy { get { return !isFree && !isFinish; } }
    public bool isFinish { get { return action == DiceAction.Finish && actionProgress == ActionProgress.Finish; } }



    [Header("TestTool")]
    [SerializeField]
    bool testRun = false;
    [SerializeField]
    Transform testObject;



    // Start is called before the first frame update
    void Start()
    {
        ResetDice();
    }

    // Update is called once per frame
    void Update()
    {
        ActUpdate();
        test();

        if(owner != null)
        {
            // 수동 조작시 시간제한 해제
            if (isTimeCountWork && !owner.isAutoPlay)
                isTimeCountWork = false;
        }
    }

    void test()
    {
        if (!testRun)
            return;

        Debug.Log("테스트 요청됨");
        testRun = false;

        if (GameData.player.me == null)
            GameData.player.me = new Player(Player.Type.User, 1, false, "테스트");

        CallDice(GameData.player.me, testObject);

    }

    void ActUpdate()
    {
        if(action == DiceAction.Wait)
        {
            if (actionProgress == ActionProgress.Ready)
            {
                isTimeCountWork = true;

                // AI 관련 초기화
                if(owner != null)
                    owner.ai.mainGame.dice.Ready();
                doForceClick = false;
                doForceClickUp = false;

                // 스킵
                //actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // 스킵
                actionProgress = ActionProgress.Working;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                // 시점 변경
                cm.CamMoveTo(owner.avatar.transform, CameraManager.CamAngle.Middle);

                // 스킵
                actionProgress = ActionProgress.Finish;
            }
            else if (actionProgress == ActionProgress.Finish)
            {

                // 스킵
                actionProgress = ActionProgress.Ready;
                action = DiceAction.Hovering;
            }
            return;
        }
        else if (action == DiceAction.Hovering)
        {
            if (actionProgress == ActionProgress.Ready)
            {
                // 회전 가속 초기화
                rotAccel = 1.0f;

                // 주사위 페이드 인 구현 예정 ===============

                // 스킵
                actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {            
                // 스킵
                actionProgress = ActionProgress.Working;
                return;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                // 오토 플레이
                if (owner.isAutoPlay)
                {
                    // AI 초도 작동
                    if (!owner.ai.mainGame.dice.isStart)
                    {
                        //AI 활성화
                        owner.ai.mainGame.dice.Work();
                    }
                }

                // 입력 가능 상태 및 UI 클릭 아닐경우 ================= 주인 인식해서 제어권 통제해야함
                if (!isInputBlock && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
                {
                    // 꾹 눌렀을때
                    if (Input.GetMouseButton(0) || doForceClick)
                    {
                        // 가속도
                        if (rotAccel < rotAccelMax)
                            rotAccel += 0.1f+ Time.deltaTime * 5.0f + rotAccel * Time.deltaTime * 0.5f;
                        else if (rotAccel > rotAccelMax)
                            rotAccel = rotAccelMax;
                    }
                    // 클릭 종료될 때
                    else if (Input.GetMouseButtonUp(0) || doForceClickUp)
                    {
                        //Debug.Break();
                        actionProgress = ActionProgress.Finish;
                    }
                }

                // 최소, 최대 높이 보정
                Tool.HeightLimit(transform, minHeight, maxHeight);

                // 스핀
                Tool.Spin(transform, rotSpeed);


                // 시간 제한
                if (elapsedTime > 15.0f)
                {
                    actionProgress = ActionProgress.Finish;

                    Debug.Log("호버링 타임 오버");
                }

                // 시간 카운트
                if(isTimeCountWork)
                    elapsedTime += Time.deltaTime;
            }
            else if (actionProgress == ActionProgress.Finish)
            {
                // 시간 초기화
                elapsedTime = 0f;

                // 스킵
                actionProgress = ActionProgress.Ready;
                action = DiceAction.Rising;
            }
            return;
        }
        else if (action == DiceAction.Rising)
        {
            // 스핀
            Tool.Spin(transform, rotSpeed);

            // 회전 가속도 -  한계치까지
            if (rotAccel < rotAccelMax)
                rotAccel += 1f + Time.deltaTime * 5.0f + rotAccel * Time.deltaTime * 0.5f;
            else if (rotAccel > rotAccelMax)
                rotAccel = rotAccelMax;

            if (actionProgress == ActionProgress.Ready)
            {
                // 중력 활성화
                transform.GetComponent<Rigidbody>().useGravity = true;

                // 스킵
                actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // 시점 변경
                cm.CamMoveTo(owner.avatar.transform, CameraManager.CamAngle.Top);

                // 상승 처리
                transform.GetComponent<Rigidbody>().isKinematic = false;
                transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 50f * transform.GetComponent<Rigidbody>().mass, ForceMode.Impulse);

                // 좌표 저장
                //_posSpeed = transform.position.y;

                // 스킵
                actionProgress = ActionProgress.Working;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                // 상승 인식
                if (transform.GetComponent<Rigidbody>().velocity.y > 0)
                {
                    // 상승 마감
                    actionProgress = ActionProgress.Finish;
                }
            }
            else if (actionProgress == ActionProgress.Finish)
            {
                // 스킵
                actionProgress = ActionProgress.Ready;
                action = DiceAction.Spinning;
            }
            return;
        }
        else if (action == DiceAction.Spinning)
        {
            // 스핀
            Tool.Spin(transform, rotSpeed);

            // 회전 가속도 -  한계치까지
            if (rotAccel < rotAccelMax)
                rotAccel += 1f + Time.deltaTime * 5.0f + rotAccel * Time.deltaTime * 0.5f;
            else if (rotAccel > rotAccelMax)
                rotAccel = rotAccelMax;

            if (actionProgress == ActionProgress.Ready)
            {
                // 스킵
                actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // 주사위 값 설정
                dice.Rolling();
                Debug.Log("주사위 값 :: " + dice.value);

                // 스킵
                actionProgress = ActionProgress.Working;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                // 가속 중지하고 다음으로
                if (rotAccel == rotAccelMax)
                {
                    actionProgress = ActionProgress.Finish;
                }

            }
            else if (actionProgress == ActionProgress.Finish)
            {
                // 스킵
                actionProgress = ActionProgress.Ready;
                action = DiceAction.Landing;
            }
            return;
        }
        else if (action == DiceAction.Landing)
        {
            if (actionProgress == ActionProgress.Ready)
            {
                // 스킵
                actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // 스킵
                actionProgress = ActionProgress.Working;
            }
            else if (actionProgress == ActionProgress.Working)
            {
                // 스핀
                Tool.Spin(transform, rotSpeed);

                // 회전 감속
                if (rotAccel > 0.0f)
                    rotAccel -= Time.deltaTime * 5.0f - rotAccel * Time.deltaTime * 0.5f;
                else if (rotAccel < 0.0f)
                    rotAccel = 0.0f;

                // 하강 인식
                if (transform.GetComponent<Rigidbody>().velocity.y < 0)
                {
                    if (transform.position.y <= minHeight)
                    {
                        // 중력 비활성화
                        transform.GetComponent<Rigidbody>().isKinematic = true;
                        transform.GetComponent<Rigidbody>().useGravity = false;

                        // 최소, 최대 높이 보정
                        Tool.HeightLimit(transform, minHeight, maxHeight);

                        // 하강 마감
                        actionProgress = ActionProgress.Finish;
                    }
                }

            }
            else if (actionProgress == ActionProgress.Finish)
            {
                // 스킵
                actionProgress = ActionProgress.Ready;
                action = DiceAction.Finish;
            }
            return;
        }
        else if (action == DiceAction.Finish)
        {
            if (actionProgress == ActionProgress.Ready)
            {
                // 시간 카운터 리셋
                elapsedTime = 0f;

                // 스킵
                actionProgress = ActionProgress.Start;
            }
            else if (actionProgress == ActionProgress.Start)
            {
                // 눈에 따른 각도 지정
                Quaternion lastRot;
                if (dice.value == 1)
                    lastRot = eye1;
                else if (dice.value == 2)
                    lastRot = eye2;
                else if (dice.value == 3)
                    lastRot = eye3;
                else if (dice.value == 4)
                    lastRot = eye4;
                else if (dice.value == 5)
                    lastRot = eye5;
                else //(dice.value == 6)
                    lastRot = eye6;

                // 최종 회전 처리
                if (
                    Mathf.Abs(transform.rotation.x) != Mathf.Abs(lastRot.x) &&
                    Mathf.Abs(transform.rotation.y) != Mathf.Abs(lastRot.y) &&
                    Mathf.Abs(transform.rotation.z) != Mathf.Abs(lastRot.z) &&
                    Mathf.Abs(transform.rotation.w) != Mathf.Abs(lastRot.w)
                    )
                {
                    // 회전량 계산 (선형 보간)
                    transform.rotation = Quaternion.Lerp(transform.rotation, lastRot, Time.deltaTime * Mathf.Abs(_rotSpeed -rotAccel));
                }
                // 최동 회전 완료시 스킵
                else
                {
                    // 주사위 개수 차감
                    dice.count--;
                    Debug.Log("주사위 개수 :: -1 =>" + dice.count);

                    // 보정
                    transform.rotation = lastRot;

                    actionProgress = ActionProgress.Working;
                }
            }
            else if (actionProgress == ActionProgress.Working)
            {
                if (dice.value == 1)
                    Tool.SpinZ(transform, _rotSpeed);
                else if (dice.value == 2)
                    Tool.SpinY(transform, _rotSpeed);
                else if (dice.value == 3)
                    Tool.SpinY(transform, _rotSpeed);
                else if (dice.value == 4)
                    Tool.SpinY(transform, _rotSpeed);
                else if (dice.value == 5)
                    Tool.SpinY(transform, _rotSpeed);
                else // (dice.value == 6)
                    Tool.SpinZ(transform, _rotSpeed);

                // 시간 경과 시 넘김
                if (elapsedTime > 3.0f)
                {
                    elapsedTime = 0f;
                    actionProgress = ActionProgress.Finish;
                }

                // 시간 카운트
                elapsedTime += Time.deltaTime;

            }
            else if (actionProgress == ActionProgress.Finish)
            {
            }
            return;
        }
    }




    
    /// <summary>
    /// 강제 초기화 수행
    /// </summary>
    public void ResetDice()
    {
        // 주사위 소유권 초기화
        owner = null;
        dice = null;

        // 액션상태 초기화
        action = DiceAction.Wait;
        actionProgress = ActionProgress.Ready;

        // AI 관련 초기화
        if (owner != null)
            owner.ai.mainGame.dice.Ready();
        doForceClick = false;
        doForceClickUp = false;

        // 시간 관련값 리셋
        rotAccel = 0f;
        elapsedTime = 0f;

        // 중력 비활성화
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<Rigidbody>().useGravity = false;

        // 주사위 부착 해제
        transform.parent = transform.root;

        // 안보이도록 땅속에 숨김
        transform.position = new Vector3(0, -10, 0) ;
        
        // 오브젝트 비활성
        transform.GetComponent<MeshRenderer>().enabled = false;
        //gameObject.SetActive(false);


    }


    public void CallDice(Player __owner, Transform obj) {  CallDice(__owner, obj, diceDistance); }
    public void CallDice(Player __owner, Transform obj, Vector3 distance)
    {
        // 주사위 사용중이면 중단
        if (action != DiceAction.Wait)
            if (actionProgress != ActionProgress.Ready)
                return;

        // 주사위 소유권자 지정
        owner = __owner;
        dice = owner.dice;
        Debug.LogWarning("주사위 :: 소유자 = " + owner.name);

        // 주사위 개수가 부족하면 중단
        if (dice.count < 1)
        {
            Debug.Log("주사위 개수 부족 :: " + dice.count);

            // 액션 설정
            action = DiceAction.Finish;
            actionProgress = ActionProgress.Working;

            //Debug.Break();
            return;
        }

        // 굴림 시작
        dice.isRolling = true;

        // 액션 설정
        action = DiceAction.Wait;
        actionProgress = ActionProgress.Start;

        // 오브젝트 이동
        Vector3 pos = new Vector3(obj.position.x, obj.position.y, obj.position.z);
        pos += distance;
        transform.position = pos;


        // 주사위 부착
        //transform.parent = obj;

        // 오브젝트 활성
        transform.GetComponent<MeshRenderer>().enabled = true;
        //gameObject.SetActive(true);


    }

    /// <summary>
    ///  주사위의 눈을 가져오고 초기화 진행
    /// </summary>
    /// <returns></returns>
    public int UseDice()
    {
        int result = 0;

        // AI 관련 초기화
        owner.ai.mainGame.dice.Ready();
        doForceClick = false;
        doForceClickUp = false;

        // 주사위 남았으면 추가 진행
        if (dice.count > 1)
        {
            Debug.Log("주사위 굴림 :: 다시 시작");

            // 다시 처음으로 초기화
            action = DiceAction.Wait;
            actionProgress = ActionProgress.Start;

            // 시간 관련값 리셋
            rotAccel = 0f;
            elapsedTime = 0f;
        }
        else
        {
            Debug.Log("주사위 굴림 :: 종료");

            // 굴림 종료
            dice.isRolling = false;
            dice.isRolled = true;

            // 결과값 백업
            result = dice.value;

            // 초기화 시작
            ResetDice();

            // 시점 변경
            cm.CamMoveTo(cm.controller.cam, CameraManager.CamAngle.Top);
        }


        return result;
    }


    /// <summary>
    /// AI 작동
    /// </summary>
    public void RunAI(IEnumerator script)
    {
        AIWork =
                // 작업 수행
                StartCoroutine(
                    script
                    );
    }



    /// <summary>
    /// 주사위 고도 제한
    /// </summary>
    //void HeightFix()
    //{
    //    // 최소값 제한
    //    if (transform.position.y < minHeight)
    //    {
    //        transform.position = new Vector3(
    //            transform.position.x,
    //            minHeight,
    //            transform.position.z
    //            );
    //        Debug.Log("최소값 보정");
    //        Debug.Log(transform.position.ToString());
    //    }

    //    // 최대값 제한
    //    if (transform.position.y > maxHeight)
    //    {
    //        transform.position = new Vector3(
    //                transform.position.x,
    //                maxHeight,
    //                transform.position.z
    //                );
    //        Debug.Log("최대값 보정");
    //        Debug.Log(transform.position.ToString());
    //    }
    //}
}
