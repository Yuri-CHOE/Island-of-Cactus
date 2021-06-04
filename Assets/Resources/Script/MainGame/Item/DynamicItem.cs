using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicItem : MonoBehaviour
{
    // 아이템 오브젝트
    public Transform itemObject = null;

    // 아이콘 오브젝트
    public SpriteRenderer iconObject = null;


    // 아이템 위치
    public int location = -1;
    

    // 아이템 정보  =====  (아이템 주인, 획득 대상 등을 추가해야할 경우 여기에 추가)
    public Item item = null;
    public int count = 0;
    public Sprite icon = null;

    // 사용 준비
    public bool isReady = false;


    // 회전 동작 여부
    public bool doSpin = true;      
    // 회전 정보
    Coroutine coroutineRot = null;
    bool isSpin = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerStay(Collider target)
    {
        // 길과 충돌할 경우
        if (target.transform.name == "road")
        {
            // 충돌 거부 기능 비활성
            transform.GetComponent<BoxCollider>().isTrigger = false;

            // 관성 제거
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // 배치 보정
            transform.position = GameData.blockManager.GetBlock(location).transform.position;

            // 종료 판정
            BlockWork.isEnd = true;
            
            // 회전 시작
            Spin(true);
        }

    }

    //void OnTriggerExit(Collider target)
    ////
    //{
    //    // 캐릭터와 충돌에서 벗어난 경우
    //    if (target.transform.parent.name == "Character")
    //        transform.GetComponent<BoxCollider>().isTrigger = false;
    //}

    /// <summary>
    /// 오브젝트 셋팅
    /// </summary>
    /// <param name="itemIndex">아이템 인덱스</param>
    /// <param name="_count"> 수량</param>
    /// <param name="_icon">아이콘</param>
    public void SetUp(int itemIndex, int _count, Sprite _icon)
    {
        // 아이템 참조 설정
        item = Item.table[itemIndex];

        // 아이템 개수 설정
        count = _count;

        // 아이템 아이콘 설정
        icon = _icon;
        iconObject.sprite = icon;


        // 준비 완료
        isReady = true;
    }
    /// <summary>
    /// 아이템 슬롯으로 오브젝트 셋팅
    /// </summary>
    /// <param name="itemSlot">아이템 슬롯</param>
    public void SetUp(ItemSlot itemSlot)
    {
        // 아이템 참조 설정
        item = itemSlot.item;

        // 아이템 개수 설정
        count = itemSlot.count;

        // 아이템 아이콘 설정
        icon = itemSlot.icon.sprite;
        iconObject.sprite = icon;


        // 준비 완료
        isReady = true;
    }


    /// <summary>
    /// 회전 사용 설정
    /// </summary>
    /// <param name="_isOn">사용 여부</param>
    public void Spin(bool _isOn)
    {
        // 초기화 안됬을 경우 중단
        if (!isReady)
        {
            Debug.LogError("사용 오류 :: 아이템 오브젝트 초기화 안된 상태로 회전 요청됨");
            Debug.Break();
            return;
        }

        doSpin = _isOn;

        // 회전 시작
        if (doSpin)
        {
            // 이미 회전중이 아닐때
            if (!isSpin)
            {
                // 회전중 처리
                isSpin = true;


                // 회전 액션 초기화
                if(coroutineRot != null)
                    StopCoroutine(coroutineRot);

                // 회전 액션 시작
                coroutineRot = StartCoroutine(AcrTurn());
            }
        }
        // 회전 종료
        else
        {
            // 이미 회전중 일때
            if (isSpin)
            {
                // 회전 안함 처리
                isSpin = false;

                // 회전 액션 초기화
                StopCoroutine(coroutineRot);
            }


        }
    }

    IEnumerator AcrTurn()
    {
        // 회전 작동
        while (doSpin)
        {
            Tool.SpinY(itemObject, 3f);

            yield return null;
        }
    }


}
