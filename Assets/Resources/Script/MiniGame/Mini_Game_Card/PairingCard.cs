using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PairingCard : MonoBehaviour, IPointerClickHandler
{
    // 카드 관리 스크립트
    public CardManager cm = null;


    // 카드 인덱스
    public int Index = -1;

    // 카드 숫자
    public int cardNum = 0;

    // 카드 숫자 텍스트
    public UnityEngine.UI.Text numText = null;

    // 카드 애니메이터
    public Animator animator = null;


    // 만료 여부
    bool isOpen = false;




    //public CheckCard check = null;
    //public Card_Setting card_Setting = null;
    //public int i = 0, x = 0, y = 0;

    //string nameObject, number;

    //[SerializeField]
    //Animator animator = null;

    void Awake()
    {
        //check = GameObject.Find("Game").GetComponent<CheckCard>();
        //nameObject = this.gameObject.name;
        //card_Setting = GameObject.Find(name).GetComponent<Card_Setting>();
        //animator = GetComponent<Animator>();
        animator.Play("aniTouch");
    }

    void Update()
    {
        //AnimationUpdate();
    }

    /// <summary>
    /// 공개 처리
    /// </summary>
    public void SetAniStateOpen()
    {
        animator.SetTrigger("isClicked");
    }

    /// <summary>
    /// 체크 완료 처리
    /// isRelease 값에 따라 복구 or 영구 공개 처리됨
    /// </summary>
    public void SetAniStateCheckFinish()
    {
        animator.SetTrigger("isCheckFinish");
    }

    /// <summary>
    /// 영구 공개 처리
    /// </summary>
    public void SetAniStateRelease()
    {
        animator.SetBool("isRelease", true);
        isOpen = true;
    }
    
    /// <summary>
    /// 클릭 인식
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 공개 처리된 카드가 아닐 경우
            if (!isOpen)
            {
                cm.PairSelect(this);
            }
        }
    }

    //void AnimationUpdate()
    //{
    //    if (i == 1)
    //    {
    //        animator.SetBool("isClick", true);
    //        set_num();
    //    }
    //    else if (i == 2)
    //    {
    //        animator.SetBool("isFront", false);
    //        animator.SetBool("isBack", true);
    //    }
    //    else if (i == 3)
    //    {
    //        animator.SetBool("isBack", false);
    //        animator.SetBool("isClick", false);
    //        animator.SetBool("isFront", true);
    //        i = 1;
    //        set_num();
    //    }
    //    else if (i == 4)
    //    {
    //        y = 1;
    //    }
    //}

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (eventData.button == PointerEventData.InputButton.Left)
    //    {
    //        // idel 상태가 아닐 경우
    //        if (y != 1)
    //        {
    //            if (i == 0)
    //            {
    //                number = card_Setting.num2;
    //                x = 1;
    //            }
    //            else if (i == 2)
    //            {
    //                x = 3;
    //            }
    //            i = x;
    //        }

    //    }

    //}

    //public void set_num()
    //{
    //    if (nameObject != check.name1)
    //    {
    //        if (check.x == 1)
    //        {
    //            check.num1_set(number, nameObject);
    //            check.x = 2;
    //        }
    //        else
    //        {
    //            check.num2_set(number, nameObject);
    //            check.x = 3;
    //        }
    //    }

    //}
}
