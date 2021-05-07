using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomController : MonoBehaviour
{
    /*
     * < 미구현 요구사항 >
     * 캐릭터 액션 제어
     * 캐릭터 이동 제어
     * 캐릭터 최대 최소 좌표 제어
     */

        


    [SerializeField]
    float posMinY = 1.9f;           // 캐릭터 최소 높이
    [SerializeField]
    float posMaxY = 20f;            // 캐릭터 최대 높이




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 고도 제한
        Tool.HeightLimit(transform, posMinY, posMaxY);
    }
}
