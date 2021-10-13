using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // [SerializeField] 는 보안상 public 을 사용할 수 없지만 엔진 스크립트 컴포넌트에는 표시되도록 합니다.


    public Transform cam;                 // 제어대상

    [SerializeField]
    Transform camLimit1;       // 좌측 하단 한계 좌표
    public Vector3 camLimitPos1 { get { return camLimit1.position; }  }

    [SerializeField]
    Transform camLimit2;       // 우측 상단 한계 좌표
    public Vector3 camLimitPos2 { get { return camLimit2.position;  } }

    public bool isFreeMode = false;

    [SerializeField]
    float _camSpeed = 7.5f;      // 카메라 이동 속도
    public float camSpeed { get { return _camSpeed; } }

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


    void start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 누락 지정
        if (cam == null) cam = Camera.main.transform.parent;

        CheckMove();

        // 터치를 감지할 경우 무조건 터치스크린이 필요합니다
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
        if (!isFreeMode)
            return;

        // 클릭상태 아닐 경우 처리
        //if (!Input.GetMouseButton(0))
        if (!CustomInput.GetPoint())
        {
            mouseNow = Vector2.zero;
            mouseBefore = Vector2.zero;
            return;
        }

        // 마우스 왼쪽 키다운 시 원점 셋팅
        //if(Input.GetMouseButtonDown(0))
            //mouseNow = Input.mousePosition;
        if (CustomInput.GetPointDown())
            mouseNow = CustomInput.GetPointPosition();

        // 계산 완료 좌표 확보
        mouseBefore = mouseNow;

        // 반영 안된 좌표 
        //mouseNow = Input.mousePosition;
        mouseNow = CustomInput.GetPointPosition();

        // 카메라 높이 보정값
        //float camY = cam.transform.position.y;
        float camY = Camera.main.transform.position.y;

        // 갱신값 계산
        Vector3 movePlus = new Vector3(
            (mouseBefore.x - mouseNow.x) / camY * sensitivity.x,
            0f,
            (mouseBefore.y - mouseNow.y) / camY * sensitivity.z
            );

        // 갱신값 남은 이동값에 반영
        mouseMove = mouseMove + movePlus;

        // 이동 한계치 반영
        float chkX = cam.transform.position.x + mouseMove.x;
        if (chkX < camLimitPos1.x)
            mouseMove.x -= chkX - camLimitPos1.x;
        else if (chkX > camLimitPos2.x)
            mouseMove.x -= chkX - camLimitPos2.x;

        float chkZ = cam.transform.position.z + mouseMove.z;
        if (chkZ < camLimitPos1.z)
            mouseMove.z -= chkZ - camLimitPos1.z;
        else if (chkZ > camLimitPos2.z)
            mouseMove.z -= chkZ - camLimitPos2.z;
    }


    /// <summary>
    /// 프레임별 카메라 이동 계산 및 이동
    /// </summary>
    void MoveCamera()
    {
        // 이동 불가시 남은 이동거리 초기화 후 중단
        if (!isFreeMode)
        {
            mouseMove = Vector3.zero;
            return;
        }

        // 이동 불필요시 리턴
        if (mouseMove == Vector3.zero)
            return;

        // 이동할 거리 계산 (선형 보간)
        distance = Vector3.Lerp(Vector3.zero, mouseMove, Time.deltaTime * camSpeed);

        // 남은 이동값 감소
        mouseMove = mouseMove - distance;

        // 이동처리
        cam.transform.position = cam.transform.position + distance;

        // 이동할 거리값 리셋
        distance = Vector3.zero;
        float reset = 0.0001f;
        if (mouseMove.x > -reset && mouseMove.x < reset)
            mouseMove.x = 0f;
        if (mouseMove.z > -reset && mouseMove.z < reset)
            mouseMove.z = 0f;
    }
    


    public void SetCameraLimit(string code)
    {
        // 오브젝트별 분할
        List<string> _code = new List<string>();
        _code.AddRange(code.Split('|'));

        // 좌표별 분할
        List<string> limit1 = new List<string>();
        limit1.AddRange(code.Split(','));
        Vector3 temp1 = new Vector3();
        float.TryParse(limit1[0], out temp1.x);
        float.TryParse(limit1[1], out temp1.y);
        float.TryParse(limit1[2], out temp1.z);

        // 좌표별 분할
        List<string> limit2 = new List<string>();
        limit2.AddRange(code.Split(','));
        Vector3 temp2 = new Vector3();
        float.TryParse(limit2[0], out temp2.x);
        float.TryParse(limit2[1], out temp2.y);
        float.TryParse(limit2[2], out temp2.z);

    }
}
