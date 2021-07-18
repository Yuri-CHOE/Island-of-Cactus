using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    // 장애물 목록
    public static List<List<DynamicObject>> objectList = new List<List<DynamicObject>>();

    public enum Type
    {
        None,
        Item,
        Event,
    }

    // 오브젝트 타입
    public Type type = Type.None;


    // 위치 (블록 인덱스)
    public int location = -2;

    // 수량
    public int count = 0;

    // 사용 준비
    public bool isReady = false;



    public virtual bool CheckCondition(Player current)
    {
        Debug.LogError("error :: 획득 조건 체크 실패 -> DynamicObject");

        return false;
    }



    /// <summary>
    /// 장애물 생성
    /// </summary>
    public void CreateBarricade()
    {
        // 준비 미달시 중단
        if (!isReady)
        {
            Debug.LogError("error : 셋업되지 않은 DynamicObject => CreateBarricade()");
            Debug.Break();
            return;
        }

        // 유효범위 필터링
        int loc = GameData.blockManager.indexLoop(location,0);

        // 장애물 등록
        objectList[loc].Add(this);
        Debug.LogWarning(string.Format("바리케이트 :: {0} 칸에 추가됨(총 수량 => {1})", location, objectList[loc].Count));
    }


    /// <summary>
    /// 장애물 제거
    /// </summary>
    public void RemoveBarricade()
    {
        // 준비 미달시 중단
        if (!isReady)
        {
            Debug.LogError("error : 셋업되지 않은 DynamicObject => CreateBarricade()");
            Debug.Break();
            return;
        }

        // 유효범위 필터링
        int loc = GameData.blockManager.indexLoop(location, 0);

        // 등록되지 않은 경우 중단
        if (!objectList[loc].Contains(this))
        {
            Debug.LogError("error : 등록되지 않은 DynamicObject 제거 => RemoveBarricade()");
            Debug.Break();
            return;
        }

        // 장애물 등록
        objectList[loc].Remove(this);
    }
}
