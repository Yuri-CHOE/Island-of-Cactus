using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // [SerializeField] �� ���Ȼ� public �� ����� �� ������ ���� ��ũ��Ʈ ������Ʈ���� ǥ�õǵ��� �մϴ�.


    public Transform cam;                 // ������

    [SerializeField]
    Transform camLimit1;       // ���� �ϴ� �Ѱ� ��ǥ
    public Vector3 camLimitPos1 { get { return camLimit1.position; }  }

    [SerializeField]
    Transform camLimit2;       // ���� ��� �Ѱ� ��ǥ
    public Vector3 camLimitPos2 { get { return camLimit2.position;  } }

    public bool isFreeMode = false;

    [SerializeField]
    float _camSpeed = 7.5f;      // ī�޶� �̵� �ӵ�
    public float camSpeed { get { return _camSpeed; } }

    [SerializeField]
    Vector3 mouseMove = new Vector3(0f, 0f, 0f);        // ��ǥ ��갪 �����
    Vector2 mouseNow = new Vector2(0f, 0f);             // ���ο� ��ǥ
    Vector2 mouseBefore = new Vector2(0f, 0f);          // �̹� �̵� ���� ��ǥ
    Vector3 sensitivityFix = new Vector3(1f, 0f, 2f);   // ���� ����
    [SerializeField]
    Vector3 sensitivity = new Vector3(5f, 0f, 5f);      // ���� ������ ����

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
        // ���� ����
        if (cam == null) cam = Camera.main.transform.parent;

        CheckMove();

        // ��ġ�� ������ ��� ������ ��ġ��ũ���� �ʿ��մϴ�
        // ��ġ�� �⺻������ ���콺 LŬ���� �����ϴ�
        // ���� ���� ��ġ �ڵ� �� �ʿ� ���� �ƴ� ���콺 �ڵ� �ϴ°� ���մϴ�
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
    /// ������ ������ �巡�� �Ÿ��� Ȯ���Ͽ� ���� �̵����� �ݿ�
    /// </summary>
    void CheckMove()
    {
        // ��ǲ ���ܻ����� ��� ����
        if (!isFreeMode)
            return;

        // Ŭ������ �ƴ� ��� ó��
        //if (!Input.GetMouseButton(0))
        if (!CustomInput.GetPoint())
        {
            mouseNow = Vector2.zero;
            mouseBefore = Vector2.zero;
            return;
        }

        // ���콺 ���� Ű�ٿ� �� ���� ����
        //if(Input.GetMouseButtonDown(0))
            //mouseNow = Input.mousePosition;
        if (CustomInput.GetPointDown())
            mouseNow = CustomInput.GetPointPosition();

        // ��� �Ϸ� ��ǥ Ȯ��
        mouseBefore = mouseNow;

        // �ݿ� �ȵ� ��ǥ 
        //mouseNow = Input.mousePosition;
        mouseNow = CustomInput.GetPointPosition();

        // ī�޶� ���� ������
        //float camY = cam.transform.position.y;
        float camY = Camera.main.transform.position.y;

        // ���Ű� ���
        Vector3 movePlus = new Vector3(
            (mouseBefore.x - mouseNow.x) / camY * sensitivity.x,
            0f,
            (mouseBefore.y - mouseNow.y) / camY * sensitivity.z
            );

        // ���Ű� ���� �̵����� �ݿ�
        mouseMove = mouseMove + movePlus;

        // �̵� �Ѱ�ġ �ݿ�
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
    /// �����Ӻ� ī�޶� �̵� ��� �� �̵�
    /// </summary>
    void MoveCamera()
    {
        // �̵� �Ұ��� ���� �̵��Ÿ� �ʱ�ȭ �� �ߴ�
        if (!isFreeMode)
        {
            mouseMove = Vector3.zero;
            return;
        }

        // �̵� ���ʿ�� ����
        if (mouseMove == Vector3.zero)
            return;

        // �̵��� �Ÿ� ��� (���� ����)
        distance = Vector3.Lerp(Vector3.zero, mouseMove, Time.deltaTime * camSpeed);

        // ���� �̵��� ����
        mouseMove = mouseMove - distance;

        // �̵�ó��
        cam.transform.position = cam.transform.position + distance;

        // �̵��� �Ÿ��� ����
        distance = Vector3.zero;
        float reset = 0.0001f;
        if (mouseMove.x > -reset && mouseMove.x < reset)
            mouseMove.x = 0f;
        if (mouseMove.z > -reset && mouseMove.z < reset)
            mouseMove.z = 0f;
    }
    


    public void SetCameraLimit(string code)
    {
        // ������Ʈ�� ����
        List<string> _code = new List<string>();
        _code.AddRange(code.Split('|'));

        // ��ǥ�� ����
        List<string> limit1 = new List<string>();
        limit1.AddRange(code.Split(','));
        Vector3 temp1 = new Vector3();
        float.TryParse(limit1[0], out temp1.x);
        float.TryParse(limit1[1], out temp1.y);
        float.TryParse(limit1[2], out temp1.z);

        // ��ǥ�� ����
        List<string> limit2 = new List<string>();
        limit2.AddRange(code.Split(','));
        Vector3 temp2 = new Vector3();
        float.TryParse(limit2[0], out temp2.x);
        float.TryParse(limit2[1], out temp2.y);
        float.TryParse(limit2[2], out temp2.z);

    }
}
