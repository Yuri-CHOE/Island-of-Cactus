using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCameraMover : MonoBehaviour
{
    // [SerializeField] �� ���Ȼ� public �� ����� �� ������ ���� ��ũ��Ʈ ������Ʈ���� ǥ�õǵ��� �մϴ�.


    public bool isInputBlock = false;

    [SerializeField]
    float camSpeed = 7.5f;      // ī�޶� �̵� �ӵ�

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

    // Update is called once per frame
    void Update()
    {
        CheckMove();

        // ��ġ�� ������ ��� ������ ��ġ��ũ���� �ʿ��մϴ� (�Ƹ��� �̺κж����� �ܺ� ����Ŷ)
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
        if (isInputBlock)
            return;

        // Ŭ������ �ƴ� ��� ó��
        if (!Input.GetMouseButton(0))
        {
            mouseNow = Vector2.zero;
            mouseBefore = Vector2.zero;
            return;
        }

        // ���콺 ���� Ű�ٿ� �� ���� ����
        if(Input.GetMouseButtonDown(0))
            mouseNow = Input.mousePosition;

        // ��� �Ϸ� ��ǥ Ȯ��
        mouseBefore = mouseNow;

        // �ݿ� �ȵ� ��ǥ 
        mouseNow = Input.mousePosition;

        // ī�޶� ���� ������
        float camY = transform.position.y;

        // ���Ű� ���
        Vector3 movePlus = new Vector3(
            (mouseBefore.x - mouseNow.x) / camY * sensitivity.x,
            0f,
            (mouseBefore.y - mouseNow.y) / camY * sensitivity.z
            );

        // ���Ű� ���� �̵����� �ݿ�
        mouseMove = mouseMove + movePlus;
    }


    /// <summary>
    /// �����Ӻ� ī�޶� �̵� ��� �� �̵�
    /// </summary>
    void MoveCamera()
    {
        // �̵� ���ʿ�� ����
        if (mouseMove == Vector3.zero)
            return;

        // �̵��� �Ÿ� ��� (���� ����)
        distance = Vector3.Lerp(Vector3.zero, mouseMove, Time.deltaTime * camSpeed);

        // ���� �̵��� ����
        mouseMove = mouseMove - distance;

        // �̵�ó��
        transform.position = transform.position + distance;

        // �̵��� �Ÿ��� ����
        distance = Vector3.zero;
        float reset = 0.0001f;
        if (mouseMove.x > -reset && mouseMove.x < reset)
            mouseMove.x = 0f;
        if (mouseMove.z > -reset && mouseMove.z < reset)
            mouseMove.z = 0f;
    }
    
}
