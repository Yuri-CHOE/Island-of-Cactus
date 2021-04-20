using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCameraMover : MonoBehaviour
{
    // [SerializeField] 는 보안상 public 을 사용할 수 없지만 엔진 스크립트 컴포넌트에는 표시되도록 합니다.


    public bool isInputBlock = false;

    [SerializeField]
    float camSpeed = 7.5f;      // 카메라 이동 속도

    [SerializeField]
    Vector3 mouseMove = new Vector3(0f, 0f, 0f);        // 좌표 계산값 저장용
    Vector2 mouseNow = new Vector2(0f, 0f);             // 새로운 좌표
    Vector2 mouseBefore = new Vector2(0f, 0f);          // 이미 이동 계산된 좌표
    Vector3 sensitivityFix = new Vector3(1f, 0f, 2f);   // 비율 보정
    [SerializeField]
    Vector3 sensitivity = new Vector3(5f, 0f, 5f);      // 설정 가능한 감도

    [SerializeField]
    Vector3 distance;

    //[SerializeField]
    //Vector2 nowPos;

    //[SerializeField]
    //Vector2 prePos;

    //[SerializeField]
    //Vector3 movePos;

    // Update is called once per frame
    void Update()
    {
        CheckMove();

        // 터치를 감지할 경우 무조건 터치스크린이 필요합니다 (아마도 이부분때문에 외부 개발킷)
        // 터치는 기본적으로 마우스 L클릭과 같습니다
        // 따라서 궂이 터치 코딩 할 필요 없이 아닌 마우스 코딩 하는게 편합니다
        //if (Input.touchCount == 1)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        prePos = touch.position - touch.deltaPosition;
        //    }
        //    else if (touch.phase == TouchPhase.Moved)
        //    {
        //        nowPos = touch.position - touch.deltaPosition;
        //        movePos = (Vector3)(prePos - nowPos) * Speed;
        //        camera.transform.Translate(movePos);
        //        prePos = touch.position - touch.deltaPositon;
        //    }
        //    else if (touch.phase == TouchPhase.Ended)
        //    {

        //    }
        //}
    }

    void FixedUpdate()
    {
        MoveCamera();
    }


    /// <summary>
    /// 프레임 단위로 드래그 거리를 확보하여 최종 이동값에 반영
    /// </summary>
    void CheckMove()
    {
        // 인풋 차단상태일 경우 리턴
        if (isInputBlock)
            return;

        // 클릭상태 아닐 경우 처리
        if (!Input.GetMouseButton(0))
        {
            mouseNow = Vector2.zero;
            mouseBefore = Vector2.zero;
            return;
        }

        // 마우스 왼쪽 키다운 시 원점 셋팅
        if(Input.GetMouseButtonDown(0))
            mouseNow = Input.mousePosition;

        // 계산 완료 좌표 확보
        mouseBefore = mouseNow;

        // 반영 안된 좌표 
        mouseNow = Input.mousePosition;

        // 카메라 높이 보정값
        float camY = transform.position.y;

        // 갱신값 계산
        Vector3 movePlus = new Vector3(
            (mouseBefore.x - mouseNow.x) / camY * sensitivity.x,
            0f,
            (mouseBefore.y - mouseNow.y) / camY * sensitivity.z
            );

        // 갱신값 남은 이동값에 반영
        mouseMove = mouseMove + movePlus;
    }


    /// <summary>
    /// 프레임별 카메라 이동 계산 및 이동
    /// </summary>
    void MoveCamera()
    {
        // 이동 불필요시 리턴
        if (mouseMove == Vector3.zero)
            return;

        // 이동할 거리 계산 (선형 보간)
        distance = Vector3.Lerp(Vector3.zero, mouseMove, Time.deltaTime * camSpeed);

        // 남은 이동값 감소
        mouseMove = mouseMove - distance;

        // 이동처리
        transform.position = transform.position + distance;

        // 이동할 거리값 리셋
        distance = Vector3.zero;
        float reset = 0.0001f;
        if (mouseMove.x > -reset && mouseMove.x < reset)
            mouseMove.x = 0f;
        if (mouseMove.z > -reset && mouseMove.z < reset)
            mouseMove.z = 0f;
    }
    
}
