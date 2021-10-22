using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum CamAngle
    {
        Top,
        Middle,
        Low,
    }

    public struct CamPoint
    {
        public Transform focus;
        //public Vector3 Pos;
        //public Vector3 Rot;
        public CamAngle camAngle;

        //public CamPoint(Vector3 _pos, Vector3 _rot, CamAngle _camAngle)
        //{
        //    Pos = _pos;
        //    Rot = _rot;
        //    camAngle = _camAngle;
        //}
        public CamPoint(Transform obj, CamAngle _camAngle)
        {
            focus = obj;
            //Pos = obj.position;
            //Rot = obj.rotation.eulerAngles;
            camAngle = _camAngle;
        }
        public CamPoint(GameObject obj, CamAngle _camAngle)
        {
            focus = obj.transform;
            //Pos = obj.transform.position;
            //Rot = obj.transform.rotation.eulerAngles;
            camAngle = _camAngle;
        }
    }


    // ������� ���� ��ũ��Ʈ
    public CameraController controller = null;

    // ������� ��� ��ư
    public UnityEngine.UI.Toggle LockBtn = null;

    [SerializeField]
    CamPoint camPoint = new CamPoint();


    Coroutine coroutinePos = null;
    Coroutine coroutineRot = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }



    public void CamToPlayer(Transform obj)
    {
        Debug.LogWarning(obj.GetComponent<PlayerInfoUI>().owner.avatar.transform.name);
        Debug.LogWarning(obj.GetComponent<PlayerInfoUI>().owner.avatar.transform.position);

        // ���� ��� �ƴҰ�� �ߴ�
        if (!controller.isFreeMode)
            return;

        // ��ǥ �ޱ� ���
        CamPoint camPointTemp = camPoint;

        // ��ǥ ��ũ
        controller.cam.position = obj.GetComponent<PlayerInfoUI>().owner.avatar.transform.position;
        Debug.LogWarning(controller.cam.position);

        // �ޱ� ��ȯ
        CamMoveTo(controller.cam, CamAngle.Top);

        // ��ǥ ��� ��������
        camPoint = camPointTemp;

    }

    /// <summary>
    /// ī�޶� ���� ��� ����
    /// </summary>
    public void CamFree()
    {
        // ��ǥ �ޱ� ���
        CamPoint camPointTemp = camPoint;

        // ��ǥ ��ũ
        controller.cam.position = Camera.main.transform.parent.position;

        // ī�޶� Ż��
        Camera.main.transform.SetParent(controller.cam);

        // �ޱ� ��ȯ
        //CamMoveTo(controller.cam, CamAngle.Top);
        CamMove(CamAngle.Top);

        // ��ǥ ��� ��������
        camPoint = camPointTemp;

        Debug.Log("ī�޶� :: ���� ���");
    }

    public void CamFreeStartPoint()
    {
        CamFree();
        controller.cam.position = BlockManager.script.startBlock.position;
    }


    /// <summary>
    /// ī�޶� ���� ��� ���� �� ����
    /// </summary>
    public void CamReturn()
    {
        CamMoveTo(camPoint);
    }
    public void CamMoveTo(CamPoint _camPoint)
    {
        CamMoveTo(_camPoint.focus, _camPoint.camAngle);
    }
    /// <summary>
    /// obj ���� ī�޶� �̵�
    /// </summary>
    /// <param name="obj">Ÿ��</param>
    /// <param name="_camAngle">������ �ޱ�</param>
    public void CamMoveTo(Transform obj, CamAngle _camAngle)
    {
        // ��� �ߴ�
        if (false)
        {
            // ���� ���� ������ ����ó��
            if (obj == controller.cam)
                controller.isFreeMode = true;
            // �� �� ���� ������ �Ұ��� ó��
            else
                controller.isFreeMode = false;

            // ��ư, ������� ��ũ
            LockBtn.isOn = !(controller.isFreeMode);
        }

        // ī�޶� ��ǥ ����
        camPoint = new CamPoint(obj, _camAngle);

        // �̵� ����
        if (LockBtn.isOn)
        {
            // �������� ����
            Camera.main.transform.SetParent(obj);

            // �̵� ���
            CamMove(_camAngle);
        }

    }

    void CamMove(CamAngle _camAngle)
    {
        // ��ġ �� ���� Ȯ��
        Vector3 anglePos = GetAnglePos(_camAngle);
        Vector3 angleRot = GetAngleRot(_camAngle);

        //anglePos = anglePos + obj.position;
        //angleRot = angleRot + obj.rotation.eulerAngles;

        // ���� ��û ����
        if (coroutinePos != null)
            StopCoroutine(coroutinePos);
        if (coroutineRot != null)
            StopCoroutine(coroutineRot);

        // �̵� �� ȸ��
        coroutinePos = StartCoroutine(ChangePos(Camera.main.transform, anglePos));
        coroutineRot = StartCoroutine(ChangeRot(Camera.main.transform, angleRot));

        Debug.Log("ī�޶� :: ���� ���� -> " + _camAngle);
    }

    /// <summary>
    ///  �ޱ�Ÿ�Կ� ���� ������ ������ ������ ��ȯ
    /// </summary>
    /// <param name="_camAngle">�ޱ� Ÿ��</param>
    /// <returns></returns>
    Vector3 GetAnglePos(CamAngle _camAngle)
    {
        Vector3 anglePos = new Vector3();

        // ��� �ޱ�
        if (_camAngle == CamAngle.Top)
        {
            anglePos.x = 0;
            anglePos.y = 45;
            anglePos.z = -30;
        }
        // �ϴ� �ޱ�
        else if (_camAngle == CamAngle.Middle)
        {
            anglePos.x = 0;
            anglePos.y = 10;
            anglePos.z = -25;
        }
        // �ϴ� �ޱ�
        else if (_camAngle == CamAngle.Low)
        {
            anglePos.x = 0;
            anglePos.y = 10;
            anglePos.z = -25;
        }

        return anglePos;

    }

    /// <summary>
    ///  �ޱ�Ÿ�Կ� ���� ������ ������ ������ ��ȯ
    /// </summary>
    /// <param name="_camAngle">�ޱ� Ÿ��</param>
    /// <returns></returns>
    Vector3 GetAngleRot(CamAngle _camAngle)
    {
        Vector3 angleRot = new Vector3();

        // ��� �ޱ�
        if (_camAngle == CamAngle.Top)
        {
            angleRot.x = 50;
            angleRot.y = 0;
            angleRot.z = 0;
        }
        // �ϴ� �ޱ�
        else if (_camAngle == CamAngle.Middle)
        {
            angleRot.x = 10;
            angleRot.y = 0;
            angleRot.z = 0;
        }
        // �ϴ� �ޱ�
        else if (_camAngle == CamAngle.Low)
        {
            angleRot.x = -10;
            angleRot.y = 0;
            angleRot.z = 0;
        }

        return angleRot;
    }


    IEnumerator ChangePos(Transform target, Vector3 pos)
    {
        //while (Mathf.Abs(Vector3.Distance(target.position, pos)) > 0.1f)
        //{
        //    target.position = Vector3.Lerp(target.position, pos, Time.deltaTime * controller.camSpeed);

        //    yield return null;
        //}
        while (Mathf.Abs(Vector3.Distance(target.localPosition, pos)) > 0.1f)
        {
            target.localPosition = Vector3.Lerp(target.localPosition, pos, Time.deltaTime * controller.camSpeed);

            yield return null;
        }

        // ����
        target.localPosition = pos;
    }

    IEnumerator ChangeRot(Transform target, Vector3 rot)
    {
        while (Mathf.Abs(Vector3.Distance(target.rotation.eulerAngles, rot)) > 0.1f)
        {

            target.rotation = Quaternion.Lerp(target.rotation, Quaternion.Euler(rot), Time.deltaTime * controller.camSpeed);

            yield return null;
        }

        // ����
        target.rotation = Quaternion.Euler(rot);
    }

    public void CameraLockToggle()
    {
        // ���� ��� �ƴҰ��
        if (LockBtn.isOn)
        {
            Debug.LogWarning(LockBtn.isOn);

            // �ޱ� ��ȯ
            CamReturn();

            // ��� ����
            controller.isFreeMode = false;
        }
        // ���� ���
        else
        {
            Debug.LogWarning(LockBtn.isOn);

            // �ޱ� ��ȯ
            CamFree();
            //CamMoveTo(controller.cam, CamAngle.Top);

            // ��� ����
            controller.isFreeMode = true;
        }
    }
}
