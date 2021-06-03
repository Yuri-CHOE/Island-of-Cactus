using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockWork
{
    // 사용중 판정
    public static bool isWork = false; 

    // 종료 판정
    public static bool isEnd = false; 

    // 아이템 띄우기
    static float liftY = 3f;

    /// <summary>
    /// 플레이어의 현재 위치를 기반으로 블록 작업 수행
    /// </summary>
    /// <param name="currentPlayer">플레이어</param>
    public static void Work(Player currentPlayer)
    {
        //Debug.Break();
        // 오류 차단
        if (currentPlayer == null)
            return;
        if (currentPlayer.avatar == null)
            return;
        if (currentPlayer.movement == null)
            return;


        // 초기화
        Clear();

        // 사용중 판정 처리
        isWork = true;


        // 위치 복사
        int location = currentPlayer.movement.location;

        // 블록 종류 파악
        BlockType.TypeDetail blockType = GetBlockType(location);

        Debug.LogError(currentPlayer.name);
        Debug.LogError(blockType);
        //Debug.Break();

        // 블록 별 기능
        if (blockType == BlockType.TypeDetail.none)
            return;

        else if (blockType == BlockType.TypeDetail.plus)
            BlockPlus(currentPlayer);
        else if (blockType == BlockType.TypeDetail.minus)
            BlockMinus(currentPlayer);

        else if (blockType == BlockType.TypeDetail.trap)
            BlockTrap(currentPlayer);
        else if (blockType == BlockType.TypeDetail.lucky)
            BlockLuckyBox(currentPlayer);


        /*
        
        => 초도 구현 완료 : --
        => 테스트 완료 : ++
        


        --none,

        --plus,
        --minus,

        boss,
        --trap,
        lucky,

        shop,
        unique,
        shortcutIn,
        shortcutOut, 
         */
    }

    public static void Clear()
    {
        // 종료 판정 초기화
        isWork = false;

        // 종료 판정 초기화
        isEnd = false;
    }


    /// <summary>
    /// 블록 타입 추출
    /// </summary>
    /// <param name="blockIndex"></param>
    /// <returns></returns>
    public static BlockType.TypeDetail GetBlockType(int blockIndex)
    {
        return  GameData.blockManager.GetBlock(blockIndex).GetComponent<DynamicBlock>().blockTypeDetail;
    }


    static void BlockPlus(Player currentPlayer)
    {
        int coinValue = 1;

        // 코인 추가
        currentPlayer.coin.Add(coinValue);

        // 종료 판정
        isEnd = true;
    }


    static void BlockMinus(Player currentPlayer)
    {
        int coinValue = 2;

        // 코인 감소
        currentPlayer.coin.subtract(coinValue);

        // 종료 판정
        isEnd = true;
    }


    static void BlockTrap(Player currentPlayer)
    {
        // 코인 수량 확보
        int coinValue = currentPlayer.coin.Value;

        // 코인 몰수
        currentPlayer.coin.subtract(coinValue);


        // 코인 아이템 인덱스
        int index = 1;


        // 오브젝트화
        DynamicItem obj = GameData.itemManager.CreateItemObject(currentPlayer.movement.location, index, coinValue, ItemSlot.LoadIcon(Item.table[index]));

        // 배치할 위치
        int loc = GameData.blockManager.indexLoop(currentPlayer.movement.location, -1);
        Debug.LogError("날릴 위치 : " + loc);
        Vector3 pos = GameData.blockManager.GetBlock(loc).transform.position;


        // 아이템 날리기
        Tool.ThrowParabola(obj.transform, pos, liftY, 1f);

        // 오브젝트 배치
        obj.location = loc;

        // 장애물 등록
        CharacterMover.barricade[loc]++;

        // 종료 판정
        isEnd = true;
    }


    static void BlockLuckyBox(Player currentPlayer)
    {
        // 럭키박스 드랍테이블
        DropTable dropTable = new DropTable();

        // 드랍테이블 셋팅
        dropTable.rare = new List<int>();
        Debug.LogError(LuckyBox.table.Count);
        for (int i = 1; i < LuckyBox.table.Count; i++)
        {
            dropTable.rare.Add(LuckyBox.table[i].rare);
            Debug.LogError("드랍 테이블 :: 추가됨 -> " + LuckyBox.table[i].rare);
        }

        // 드랍 테이블 작동 및 드랍대상 인덱스 확보
        int select = 1 + dropTable.Drop();
        Debug.LogError("럭키박스 :: 선택됨 -> "+ select);


        // 럭키박스 연출 시작
        LuckyBoxManager lbm = LuckyBoxManager.obj;

        // 강제 초기화
        lbm.ClearForced();

        // 소환
        lbm.GetLuckyBox(currentPlayer);

        // 오픈
        lbm.Open();

        // 코루틴 초기화
        if(lbm.coroutineOpen != null)
            lbm.StopCoroutine(lbm.coroutineOpen);
        // 대기, 결과 출력, 효과 적용, 종료 판정
        lbm.coroutineOpen = lbm.StartCoroutine(lbm.WaitAndResult());

        // 스트링 입력
        lbm.SetTextByIndex(select);
    }

}
