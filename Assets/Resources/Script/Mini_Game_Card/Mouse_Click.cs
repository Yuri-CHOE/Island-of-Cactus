using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouse_Click : MonoBehaviour, IPointerClickHandler
{
    public CheckCard check;
    public Card_Setting card_Setting;
    public int i = 0, x = 0, y = 0;
    string nameObject, number;
    Animator animator;

    void Awake()
    {
        check = GameObject.Find("Game").GetComponent<CheckCard>();
        nameObject = this.gameObject.name;
        card_Setting = GameObject.Find(name).GetComponent<Card_Setting>();
        animator = GetComponent<Animator>();
        animator.Play("aniTouch");
    }

    void Update()
    {
        AnimationUpdate();
    }

    void AnimationUpdate()
    {
        if (i == 1)
        {
            animator.SetBool("isClick", true);
            set_num();
        }
        if (i == 2)
        {
            animator.SetBool("isFront", false);
            animator.SetBool("isBack", true);
        }
        if (i == 3)
        {
            animator.SetBool("isBack", false);
            animator.SetBool("isClick", false);
            animator.SetBool("isFront", true);
            i = 1;
            set_num();
        }
        if (i == 4)
        {
            y = 1;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (y != 1)
            {
                if (i == 0)
                {
                    number = card_Setting.num2;
                    x = 1;
                }
                if (i == 2)
                {
                    x = 3;
                }
                i = x;
            }

        }

    }

    public void set_num()
    {
        if (nameObject != check.name1)
        {
            if (check.x == 1)
            {
                check.num1_set(number, nameObject);
                check.x = 2;
            }
            else
            {
                check.num2_set(number, nameObject);
                check.x = 3;
            }
        }

    }
}
