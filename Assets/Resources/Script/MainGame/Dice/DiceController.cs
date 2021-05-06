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

    // 주사위 정보
    public Dice dice { get { return GameData.turn.now.dice; } }

    // 주사위 주인
    public Player owner { get { return GameData.turn.now; } }

    // 최소 높이
    [SerializeField]
    float minHeight = 1.9f;

    // 최대 높이
    [SerializeField]
    float maxHeight = 50.0f;

    // 최대 높이
    [SerializeField]
    Vector3 diceDistance = new Vector3(0, 1, -2);

    // 회전 속도
    [Header("spin")]
    float _rotSpeed = 1.00f;
    float rotSpeed { get { return _rotSpeed + rotAccel; } }
    [SerializeField]
    float rotAccel = 0.00f;

    // 상승 하강 속도
    [Header("rise")]
    float _posSpeed = 1.00f;
    float posSpeed { get { return _posSpeed + posAccel; } }
    float posAccel = 0.00f;


    [Header("Action")]
    // 액션 종류
    public DiceAction action = DiceAction.Wait;
    // 액션 진행도
    [SerializeField]
    ActionProgress actionProgress = ActionProgress.Ready;
    bool isTimmerWork = false;
    float elapsedTime = 0.00f;


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
        test();
        ActUpdate();
        
    }

    void test()
    {
        if (!testRun)
            return;

        Debug.Log("테스트 요청됨");
        testRun = false;

        CallDice(testObject);

    }


    void ActUpdate()
    {
        if(action == DiceAction.Wait)
        {
            if (actionProgress == ActionProgress.Ready)
            {

            }
            else if (actionProgress == ActionProgress.Start)
            {

            }
            else if (actionProgress == ActionProgress.Working)
            {

            }
            else if (actionProgress == ActionProgress.Finish)
            {

            }
            return;
        }
        else if (action == DiceAction.Hovering)
        {
            if (actionProgress == ActionProgress.Ready)
            {
                // 주사위 페이드 인 구현??
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

                // 최소, 최대 높이 보정
                HeightFix();

                // 꾹 눌렀을때
                if (Input.GetMouseButton(0))
                {
                    // 가속도
                    if (rotAccel < 100f)
                        rotAccel += Time.deltaTime * 5.0f + rotAccel * Time.deltaTime * 0.5f;
                    else if (rotAccel > 100f)
                        rotAccel = 100f;
                }

                // 클릭 종료될 때
                if (Input.GetMouseButtonUp(0))
                    actionProgress = ActionProgress.Finish;
            }
            else if (actionProgress == ActionProgress.Finish)
            {
                // 스킵
                actionProgress = ActionProgress.Ready;
                action = DiceAction.Rising;
            }
            return;
        }
        else if (action == DiceAction.Rising)
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
                // 상승 처리
            }
            else if (actionProgress == ActionProgress.Finish)
            {

            }
            return;
        }
        else if (action == DiceAction.Spinning)
        {
            if (actionProgress == ActionProgress.Ready)
            {

            }
            else if (actionProgress == ActionProgress.Start)
            {

            }
            else if (actionProgress == ActionProgress.Working)
            {

            }
            else if (actionProgress == ActionProgress.Finish)
            {

            }
            return;
        }
        else if (action == DiceAction.Landing)
        {
            if (actionProgress == ActionProgress.Ready)
            {

            }
            else if (actionProgress == ActionProgress.Start)
            {

            }
            else if (actionProgress == ActionProgress.Working)
            {

            }
            else if (actionProgress == ActionProgress.Finish)
            {

            }
            return;
        }
        else if (action == DiceAction.Finish)
        {
            if (actionProgress == ActionProgress.Ready)
            {

            }
            else if (actionProgress == ActionProgress.Start)
            {

            }
            else if (actionProgress == ActionProgress.Working)
            {

            }
            else if (actionProgress == ActionProgress.Finish)
            {

            }
            return;
        }
    }





    
    /// <summary>
    /// 대기 액션 강제 수행
    /// </summary>
    public void ResetDice()
    {
        action = DiceAction.Wait;
        actionProgress = ActionProgress.Ready;

        // 타이머 리셋
        isTimmerWork = false;

        // 주사위 부착 해제
        transform.parent = transform.root;

        // 안보이도록 땅속에 숨김
        transform.position = new Vector3(0, -10, 0) ;

        // 액션상태 초기화
        action = DiceAction.Wait;

        // 오브젝트 비활성
        transform.GetComponent<MeshRenderer>().enabled = false;
        Debug.Log(transform.GetComponent<MeshRenderer>().enabled);
        //gameObject.SetActive(false);


    }


    public void CallDice(Transform obj) { CallDice(obj, diceDistance); }
    public void CallDice(Transform obj, Vector3 distance)
    {
        // 주사위 사용중이면 중단
        if (action != DiceAction.Wait)
            return;

        action = DiceAction.Hovering;
        actionProgress = ActionProgress.Start;

        // 오브젝트 이동
        Vector3 pos = new Vector3(obj.position.x, obj.position.y, obj.position.z);
        pos += distance;
        transform.position = pos;
        Debug.Log(pos.ToString());
        Debug.Log(transform.position.ToString());


        // 주사위 부착
        //transform.parent = obj;

        // 오브젝트 활성
        transform.GetComponent<MeshRenderer>().enabled = true;
        Debug.Log(transform.GetComponent<MeshRenderer>().enabled);
        //gameObject.SetActive(true);

    }






    /// <summary>
    /// 주사위 고도 제한
    /// </summary>
    void HeightFix()
    {
        // 최소값 제한
        if (transform.position.y < minHeight)
        {
            transform.position = new Vector3(
                transform.position.x,
                minHeight,
                transform.position.z
                );
            Debug.Log("최소값 보정");
            Debug.Log(transform.position.ToString());
        }

        // 최대값 제한
        if (transform.position.y > maxHeight)
        {
            transform.position = new Vector3(
                    transform.position.x,
                    maxHeight,
                    transform.position.z
                    );
            Debug.Log("최대값 보정");
            Debug.Log(transform.position.ToString());
        }
    }
}
