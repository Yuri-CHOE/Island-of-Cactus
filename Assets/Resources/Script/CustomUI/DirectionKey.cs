using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DirectionKey : UIBehaviour
{
    [SerializeField]
    GameObject btnL;
    [SerializeField]
    GameObject btnR;
    [SerializeField]
    GameObject btnU;
    [SerializeField]
    GameObject btnD;

    [SerializeField]
    MoveDirection moveDirection = MoveDirection.None;       // 방향 인터럽트

    // Update is called once per frame
    void Update()
    {
        CheckFlag();
    }


    /// <summary>
    /// 버튼 UI에서 방향값 획득
    /// ButtonFlag 컴포넌트 연동
    /// </summary>
    /// <returns></returns>
    void CheckFlag()
    {
        ButtonFlag L = btnL.GetComponent<ButtonFlag>();
        ButtonFlag R = btnR.GetComponent<ButtonFlag>();
        ButtonFlag U = btnU.GetComponent<ButtonFlag>();
        ButtonFlag D = btnD.GetComponent<ButtonFlag>();
        

        if (L.getFlag())
            moveDirection = MoveDirection.Left;
        else if (R.getFlag())
            moveDirection = MoveDirection.Right;
        else if (U.getFlag())
            moveDirection = MoveDirection.Up;
        else if (D.getFlag())
            moveDirection = MoveDirection.Down;
        else
            moveDirection = MoveDirection.None;

        L.setOff();
        R.setOff();
        U.setOff();
        D.setOff();

        L.MirrorSync(true);
        R.MirrorSync(true);
        U.MirrorSync(true);
        D.MirrorSync(true);
    }

    public MoveDirection GetDirection()
    {
        // 리턴값 백업
        MoveDirection md = moveDirection;

        // 초기화
        moveDirection = MoveDirection.None;

        return md;
    }
}
