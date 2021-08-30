using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DropTable
{
    // 레어도(드랍률) 리스트
    public List<int> rare;

    

    public int totalRare { get { int sum = 0; for (int i = 0; i < rare.Count; i++) sum += rare[i]; return sum; } }

    public int Drop()
    {
        if (rare == null)
            return -1;

        if (rare.Count > 0)
        {
            // 랜덤 값 가져오기
            int randomValue = Random.Range(0, totalRare);

            Debug.LogWarning("드랍 테이블(총 "+ rare.Count + ") :: 0 ~ " + totalRare + " 중 선택됨 -> " + randomValue);

            int result = 0;
            for (int i = 0; i < rare.Count; i++)
            {
                // 연산 중단
                if (randomValue < rare[i])
                    break;

                // 랜덤 값 연산
                randomValue -= rare[i];

                // 결과 인덱스 다음으로
                result++;
            }

            return result;

        }
        else
            return -1;
    }

    public List<int> Drop(int count)
    {
        // 결과물
        List<int> result = new List<int>();

        // 리스트 입력 안되면 중단
        if (rare.Count == 0)
            return result;


        // 인덱스 리스트 초기화
        List<int> index = new List<int>();

        // 인덱스 리스트 생성
        for (int i = 0; i < rare.Count; i++)
            index.Add(i);

        // 반복 추출
        for (int i = 0; i < count; i++)
        {
            // 가공된 인덱스
            int select = Drop();

            // 원본의 인덱스 확보
            result.Add(index[select]);

            // 제외 처리
            //rare.RemoveAt(select);
            index.RemoveAt(select);
        }

        return result;
    }
}
