using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
