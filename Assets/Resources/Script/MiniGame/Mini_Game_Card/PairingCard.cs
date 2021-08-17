using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PairingCard : MonoBehaviour, IPointerClickHandler
{
    // ī�� ���� ��ũ��Ʈ
    public CardManager cm = null;


    // ī�� �ε���
    public int Index = -1;

    // ī�� ����
    public int cardNum = 0;

    // ī�� ���� �ؽ�Ʈ
    public UnityEngine.UI.Text numText = null;

    // ī�� �ִϸ�����
    public Animator animator = null;


    // ���� ����
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
    /// ���� ó��
    /// </summary>
    public void SetAniStateOpen()
    {
        animator.SetTrigger("isClicked");
    }

    /// <summary>
    /// üũ �Ϸ� ó��
    /// isRelease ���� ���� ���� or ���� ���� ó����
    /// </summary>
    public void SetAniStateCheckFinish()
    {
        animator.SetTrigger("isCheckFinish");
    }

    /// <summary>
    /// ���� ���� ó��
    /// </summary>
    public void SetAniStateRelease()
    {
        animator.SetBool("isRelease", true);
        isOpen = true;
    }
    
    /// <summary>
    /// Ŭ�� �ν�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // ���� ó���� ī�尡 �ƴ� ���
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
    //        // idel ���°� �ƴ� ���
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
