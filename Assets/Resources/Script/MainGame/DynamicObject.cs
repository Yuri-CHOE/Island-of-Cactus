using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    // 위치 (블록 인덱스)
    public int location = -2;

    // 수량
    public int count = 0;

    // 사용 준비
    public bool isReady = false;


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
        CharacterMover.barricade[loc].Add(this);
        Debug.LogWarning(string.Format("바리케이트 :: {0} 칸에 추가됨(총 수량 => {1})", location, CharacterMover.barricade[loc].Count));
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
        if (!CharacterMover.barricade[loc].Contains(this))
        {
            Debug.LogError("error : 등록되지 않은 DynamicObject 제거 => RemoveBarricade()");
            Debug.Break();
            return;
        }

        // 장애물 등록
        CharacterMover.barricade[loc].Remove(this);
    }
}
