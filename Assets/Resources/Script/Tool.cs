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
        Quaternion spin = Quaternion.Euler(
            obj.rotation.eulerAngles.x + Random.Range(144, 170),
            obj.rotation.eulerAngles.y + Random.Range(144, 170),
            obj.rotation.eulerAngles.z + Random.Range(144, 170)
            );

        // 회전량 계산 (선형 보간)
        obj.rotation = Quaternion.Lerp(obj.rotation, spin, Time.deltaTime * speed);
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


    //public static float SpeedAcceleration()
    //{

    //}
}
