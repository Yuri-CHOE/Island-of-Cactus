using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Tool
{
    public static List<int> RandomNotCross(int minInclusive, int maxExclusive, int requiredCount)
    {
        // 부적절한 범위 차단
        if (minInclusive >= maxExclusive)
            return null;

        // 부적절한 요구량 차단
        if (maxExclusive - minInclusive <= requiredCount)
            return null;

        // 랜덤 픽 테이블
        List<int> table = new List<int>();
        for (int i = minInclusive; i < maxExclusive; i++)
            table.Add(i);

        // 결과물 저장용
        List<int> result = new List<int>();

        // 요구 수량만큼 랜덤 호출
        for (int i = 0; i < requiredCount; i++)
        {
            // 랜덤 픽 테이블에서 랜덤한 인덱스 호출
            int randIndex = Random.Range(0, table.Count);

            // 인덱스로 랜덤 픽
            result.Add(table[randIndex]);

            // 픽된 값 제외
            table.RemoveAt(randIndex);
        }

        // 결과물 개수 확인 => 불일치 케이스 사전 차단중
        //while(result.Count < requiredCount)
        //    result.Add(0);

        return result;
    }


    /// <summary>
    /// 클릭한 오브젝트 가져오기
    /// </summary>
    public static GameObject Targeting()
    {
        // 클릭 체크
        if (!Input.GetMouseButtonUp(0))
            return null;

        // UI 클릭 예외처리
        if (EventSystem.current.currentSelectedGameObject != null)
            return null;


        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        GameObject clickObj = null;

        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            clickObj = hit.transform.gameObject;
            Debug.Log(clickObj.name);
        }

        return clickObj;
    }

    /// <summary>
    /// 회전속도 1의 3축 랜덤 스핀
    /// </summary>
    /// <param name="obj"></param>
    public static void Spin(Transform obj) { Spin(obj, 1.0f); }
    /// <summary>
    /// 3축 랜덤 스핀
    /// </summary>
    /// <param name="obj"></param>
    public static void Spin(Transform obj, float speed)
    {
        // 각도값 양수화
        Quaternion rot = Quaternion.Euler(
            obj.rotation.eulerAngles.x + 360f,
            obj.rotation.eulerAngles.y + 360f,
            obj.rotation.eulerAngles.z + 360f
            );

        Quaternion spin = Quaternion.Euler(
            rot.eulerAngles.x + Random.Range(144, 170),
            rot.eulerAngles.y + Random.Range(144, 170),
            rot.eulerAngles.z + Random.Range(144, 170)
            );

        // 회전량 계산 (선형 보간)
        obj.rotation = Quaternion.Lerp(rot, spin, Time.deltaTime * speed);

        ////Quaternion spin = Quaternion.Euler(
        //    obj.rotation.eulerAngles.x + Random.Range(144, 170),
        //    obj.rotation.eulerAngles.y + Random.Range(144, 170),
        //    obj.rotation.eulerAngles.z + Random.Range(144, 170)
        //    );

        //// 회전량 계산 (선형 보간)
        //obj.rotation = Quaternion.Lerp(obj.rotation, spin, Time.deltaTime * speed);
    }
    public static void SpinX(Transform obj, float speed)
    {
        // 각도값 양수화
        Quaternion rot = Quaternion.Euler(
            obj.rotation.eulerAngles.x + 360f,
            obj.rotation.eulerAngles.y,
            obj.rotation.eulerAngles.z
            );

        Quaternion spin = Quaternion.Euler(
            rot.eulerAngles.x + Random.Range(144, 170),
            rot.eulerAngles.y,
            rot.eulerAngles.z
            );

        // 회전량 계산 (선형 보간)
        obj.rotation = Quaternion.Lerp(rot, spin, Time.deltaTime * speed);
    }
    public static void SpinY(Transform obj, float speed)
    {
        // 각도값 양수화
        Quaternion rot = Quaternion.Euler(
            obj.rotation.eulerAngles.x,
            obj.rotation.eulerAngles.y + 360f,
            obj.rotation.eulerAngles.z
            );

        Quaternion spin = Quaternion.Euler(
            rot.eulerAngles.x,
            rot.eulerAngles.y + Random.Range(144, 170),
            rot.eulerAngles.z
            );

        // 회전량 계산 (선형 보간)
        obj.rotation = Quaternion.Lerp(rot, spin, Time.deltaTime * speed);
    }
    public static void SpinZ(Transform obj, float speed)
    {
        // 각도값 양수화
        Quaternion rot = Quaternion.Euler(
            obj.rotation.eulerAngles.x,
            obj.rotation.eulerAngles.y,
            obj.rotation.eulerAngles.z + 360f
            );

        Quaternion spin = Quaternion.Euler(
            rot.eulerAngles.x,
            rot.eulerAngles.y,
            rot.eulerAngles.z + Random.Range(144, 170)
            );

        // 회전량 계산 (선형 보간)
        obj.rotation = Quaternion.Lerp(rot, spin, Time.deltaTime * speed);
    }



    /// <summary>
    /// 고도 제한
    /// </summary>
    public static void HeightLimit(Transform target, float min, float max)
    {
        // 최소값 제한
        if (target.position.y < min)
        {
            target.position = new Vector3(
                target.position.x,
                min,
                target.position.z
                );
            Debug.Log("최소값 보정");
            Debug.Log(target.position.ToString());
        }

        // 최대값 제한
        if (target.position.y > max)
        {
            target.position = new Vector3(
                    target.position.x,
                    max,
                    target.position.z
                    );
            Debug.Log("최대값 보정 :: " + target.position.ToString());
        }
    }


    /// <summary>
    /// 페이드 처리 - 코루틴 필수
    /// </summary>
    /// <param name="canvasGroup">제어 대상</param>
    /// <param name="isFadeIn">f=페이드 아웃, t=페이드 인</param>
    /// <param name="timer">목표 시간</param>
    /// <returns></returns>
    /// </summary>
    public static IEnumerator CanvasFade(CanvasGroup canvasGroup, bool isFadeIn, float timer)
    {
        // 음수 양수 처리
        if (isFadeIn)
        {
            // 클릭 차단 기능 활성화
            canvasGroup.blocksRaycasts = true;

            while (canvasGroup.alpha < 1f)
            {
                // 페이드 처리
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.deltaTime / timer); ;

                // 보정
                if (canvasGroup.alpha > 0.9999f)
                    canvasGroup.alpha = 1f;

                yield return null;
            }
        }
        else
        {
            // 클릭 차단 기능 비활성화
            canvasGroup.blocksRaycasts = false;

            while (canvasGroup.alpha > 0f)
            {
                // 페이드 처리
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, Time.deltaTime / timer); ;

                // 보정
                if (canvasGroup.alpha < 0.0001f)
                    canvasGroup.alpha = 0f;

                yield return null;
            }
        }
    }


    /// <summary>
    /// 오브젝트를 목표 좌표로 포물선 날리기
    /// BoxCollider, Rigidbody와 중력 사용 필수
    /// </summary>
    /// <param name="obj">날릴 오브젝트</param>
    /// <param name="destination">목적지 좌표</param>
    /// <param name="height">포물선 높이</param>
    /// <param name="time">예상 시간</param>
    public static void ThrowParabola(Transform obj, Vector3 destination, float height, float time)
    {
        // 잘못된 오브젝트 중단
        if (obj == null)
            return;

        // 퀵 등록
        Rigidbody objRigidbody = obj.GetComponent<Rigidbody>();

        // Rigidbody 미사용시 중단
        if (objRigidbody == null)
            return;

        // 캐릭터 겹침 방지 충돌 해제
        obj.GetComponent<Collider>().isTrigger = true;


        // 방향 백터 계산

        // 거리 확보
        Vector3 distance = destination - obj.position;

        // 높이 제거된 거리
        Vector3 distanceXZ = new Vector3(distance.x, 0, distance.z);

        // 속도
        float spdY = height / time + (Mathf.Abs(Physics.gravity.y) * time / 2f);
        float spdXZ = distanceXZ.magnitude / time;

        // 결과물
        Vector3 result = distanceXZ.normalized * spdXZ;
        result.y = spdY;


        // 날리기
        objRigidbody.velocity = result;

        /*
         참고 : https://foo897.tistory.com/24
        */
    }







}
