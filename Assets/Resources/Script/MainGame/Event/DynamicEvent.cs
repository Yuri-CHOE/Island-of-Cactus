using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicEvent : DynamicObject
{
    // 이벤트 오브젝트
    public Transform eventObject = null;


    // 이벤트 위치
    //public int location = -2;             // 상속받은 클래스(DynamicObject)로 이전됨


    // 생성한 플레이어
    public Player creator = null;

    // 이벤트 정보  =====  (이벤트 주인, 획득 대상 등을 추가해야할 경우 여기에 추가)
    public IocEvent iocEvent = null;
    //public int count = 0;                 // 상속받은 클래스(DynamicObject)로 이전됨


    // 사용 준비
    //public bool isReady = false;          // 상속받은 클래스(DynamicObject)로 이전됨


    // 애니메이션 동작 여부
    public bool doAnimate = false;
    //// 애니메이션 정보
    //Coroutine coroutineAnimate = null;
    //bool isAnimate = false;

    DynamicEvent()
    {
        type = Type.Event;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    // 충돌한 경우
    private void OnTriggerEnter(Collider target)
    {
        // 캐릭터와 충돌할 경우
        if (target.transform.parent.name == "Character")
        {

        }
    }

    // 충돌중인 경우
    //private void OnTriggerStay(Collider target)
    //{
    //    // 캐릭터와 충돌할 경우
    //    if (target.transform.parent.name == "Character")
    //    {

    //    }

    //}

    //    // 충돌에서 벗어난 경우
    //void OnTriggerExit(Collider target)
    ////
    //{
    //    // 캐릭터와 충돌에서 벗어난 경우
    //    if (target.transform.parent.name == "Character")
    //        transform.GetComponent<BoxCollider>().isTrigger = false;
    //}




    /// <summary>
    /// 이벤트 작동 조건
    /// </summary>
    /// <param name="current">작동시킨 플레이어</param>
    public override bool CheckCondition(Player current)
    {
        return IocEvent.Condition(iocEvent, current, creator);
    }

    public void GetEvent(Player user)
    {
        // 작동
        IocEvent.Effect(iocEvent, user);

        // 목록 및 장애물 제외
        Remove();

        // 제거
        Destroy(gameObject);
    }

    /// <summary>
    /// 리스트 및 바리케이트 제거
    /// </summary>
    public void Remove()
    {
        // 목록 제외
        EventManager.eventObjectList.Remove(this);

        // 장애물 제거
        RemoveBarricade();
    }

    /// <summary>
    /// 오브젝트 셋팅
    /// </summary>
    /// <param name="index">인덱스</param>
    /// <param name="_count"> 수량</param>
    public void SetUp(int index, int _count, Player _creator)
    {
        // 아이템 참조 설정
        iocEvent = IocEvent.table[index];

        // 아이템 개수 설정
        count = _count;

        // 생성자 설정
        creator = _creator;

        // 모델 오브젝트 체크
        GameObject obj = Resources.Load<GameObject>(@"Data/Event/Event" + iocEvent.index.ToString("D4"));
        if (obj == null)
        {
            obj = Resources.Load<GameObject>(@"Data/Event/Event0000");
            Debug.Log(@"Data/Event/Event0000");
        }
        if (obj == null)
            Debug.LogError("로드 실패 :: Data/Event/Event0000");

        // 모델 오브젝트 생성 및 설정
        eventObject = Instantiate(
            obj,
            transform.position,
            Quaternion.identity,
            transform
            ).transform;


        // 준비 완료
        isReady = true;
    }

}
