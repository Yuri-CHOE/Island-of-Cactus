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


    // 자유모드 제어 스크립트
    public CameraController controller = null;

    // 자유모드 토글 버튼
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

        // 자유 모드 아닐경우 중단
        if (!controller.isFreeMode)
            return;

        // 목표 앵글 백업
        CamPoint camPointTemp = camPoint;

        // 좌표 싱크
        controller.cam.position = obj.GetComponent<PlayerInfoUI>().owner.avatar.transform.position;
        Debug.LogWarning(controller.cam.position);

        // 앵글 전환
        CamMoveTo(controller.cam, CamAngle.Top);

        // 목표 백업 가져오기
        camPoint = camPointTemp;

    }

    /// <summary>
    /// 카메라 자유 모드 진입
    /// </summary>
    public void CamFree()
    {
        // 목표 앵글 백업
        CamPoint camPointTemp = camPoint;

        // 좌표 싱크
        controller.cam.position = Camera.main.transform.parent.position;

        // 카메라 탈착
        Camera.main.transform.SetParent(controller.cam);

        // 앵글 전환
        //CamMoveTo(controller.cam, CamAngle.Top);
        CamMove(CamAngle.Top);

        // 목표 백업 가져오기
        camPoint = camPointTemp;

        Debug.Log("카메라 :: 자유 모드");
    }

    public void CamFreeStartPoint()
    {
        CamFree();
        controller.cam.position = BlockManager.script.startBlock.position;
    }


    /// <summary>
    /// 카메라 자유 모드 해제 시 복귀
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
    /// obj 에게 카메라 이동
    /// </summary>
    /// <param name="obj">타겟</param>
    /// <param name="_camAngle">적용할 앵글</param>
    public void CamMoveTo(Transform obj, CamAngle _camAngle)
    {
        // 사용 중단
        if (false)
        {
            // 메인 시점 움직임 가능처리
            if (obj == controller.cam)
                controller.isFreeMode = true;
            // 그 외 시점 움직임 불가능 처리
            else
                controller.isFreeMode = false;

            // 버튼, 자유모드 싱크
            LockBtn.isOn = !(controller.isFreeMode);
        }

        // 카메라 목표 저장
        camPoint = new CamPoint(obj, _camAngle);

        // 이동 적용
        if (LockBtn.isOn)
        {
            // 계층구조 변경
            Camera.main.transform.SetParent(obj);

            // 이동 명령
            CamMove(_camAngle);
        }

    }

    void CamMove(CamAngle _camAngle)
    {
        // 위치 및 각도 확보
        Vector3 anglePos = GetAnglePos(_camAngle);
        Vector3 angleRot = GetAngleRot(_camAngle);

        //anglePos = anglePos + obj.position;
        //angleRot = angleRot + obj.rotation.eulerAngles;

        // 이전 요청 중지
        if (coroutinePos != null)
            StopCoroutine(coroutinePos);
        if (coroutineRot != null)
            StopCoroutine(coroutineRot);

        // 이동 및 회전
        coroutinePos = StartCoroutine(ChangePos(Camera.main.transform, anglePos));
        coroutineRot = StartCoroutine(ChangeRot(Camera.main.transform, angleRot));

        Debug.Log("카메라 :: 각도 변경 -> " + _camAngle);
    }

    /// <summary>
    ///  앵글타입에 따른 사전에 정해진 포지션 반환
    /// </summary>
    /// <param name="_camAngle">앵글 타입</param>
    /// <returns></returns>
    Vector3 GetAnglePos(CamAngle _camAngle)
    {
        Vector3 anglePos = new Vector3();

        // 상단 앵글
        if (_camAngle == CamAngle.Top)
        {
            anglePos.x = 0;
            anglePos.y = 45;
            anglePos.z = -30;
        }
        // 하단 앵글
        else if (_camAngle == CamAngle.Middle)
        {
            anglePos.x = 0;
            anglePos.y = 10;
            anglePos.z = -25;
        }
        // 하단 앵글
        else if (_camAngle == CamAngle.Low)
        {
            anglePos.x = 0;
            anglePos.y = 10;
            anglePos.z = -25;
        }

        return anglePos;

    }

    /// <summary>
    ///  앵글타입에 따른 사전에 정해진 포지션 반환
    /// </summary>
    /// <param name="_camAngle">앵글 타입</param>
    /// <returns></returns>
    Vector3 GetAngleRot(CamAngle _camAngle)
    {
        Vector3 angleRot = new Vector3();

        // 상단 앵글
        if (_camAngle == CamAngle.Top)
        {
            angleRot.x = 50;
            angleRot.y = 0;
            angleRot.z = 0;
        }
        // 하단 앵글
        else if (_camAngle == CamAngle.Middle)
        {
            angleRot.x = 10;
            angleRot.y = 0;
            angleRot.z = 0;
        }
        // 하단 앵글
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

        // 보정
        target.localPosition = pos;
    }

    IEnumerator ChangeRot(Transform target, Vector3 rot)
    {
        while (Mathf.Abs(Vector3.Distance(target.rotation.eulerAngles, rot)) > 0.1f)
        {

            target.rotation = Quaternion.Lerp(target.rotation, Quaternion.Euler(rot), Time.deltaTime * controller.camSpeed);

            yield return null;
        }

        // 보정
        target.rotation = Quaternion.Euler(rot);
    }

    public void CameraLockToggle()
    {
        // 자유 모드 아닐경우
        if (LockBtn.isOn)
        {
            Debug.LogWarning(LockBtn.isOn);

            // 앵글 변환
            CamReturn();

            // 모드 적용
            controller.isFreeMode = false;
        }
        // 자유 모드
        else
        {
            Debug.LogWarning(LockBtn.isOn);

            // 앵글 변환
            CamFree();
            //CamMoveTo(controller.cam, CamAngle.Top);

            // 모드 적용
            controller.isFreeMode = true;
        }
    }
}
