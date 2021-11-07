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

    // 노말 블록 보정치
    static public int plusBlockPlus = 0;
    static public int plusBlockMultiple = 1;
    static public int minusBlockPlus = 0;
    static public int minusBlockMultiple = 1;

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
        if (currentPlayer.movement.location == -1)
            return;


        // 초기화
        Clear();

        // 메시지 박스 강제 종료
        GameMaster.script.messageBox.Close();

        // 사용중 판정 처리
        isWork = true;


        // 위치 복사
        int location = currentPlayer.movement.location;

        // 블록 종류 파악
        BlockType.TypeDetail blockType = GetBlockType(location);

        Debug.Log("블록 기능 :: " + currentPlayer.name  +" 에 의해 작동됨 => "+ blockType );
        //Debug.Break();

        // 블록 별 기능
        if (blockType == BlockType.TypeDetail.none)
            return;

        else if (blockType == BlockType.TypeDetail.plus)
            BlockPlus(currentPlayer);
        else if (blockType == BlockType.TypeDetail.minus)
            BlockMinus(currentPlayer);

        else if (blockType == BlockType.TypeDetail.boss)
            BlockBoss(currentPlayer);
        else if (blockType == BlockType.TypeDetail.trap)
            BlockTrap(currentPlayer);
        else if (blockType == BlockType.TypeDetail.lucky)
            BlockLuckyBox(currentPlayer);

        else if (blockType == BlockType.TypeDetail.shop)
            BlockShop(currentPlayer); 
        else if (blockType == BlockType.TypeDetail.unique)
            BlockUnique(currentPlayer); 
        else if (blockType == BlockType.TypeDetail.shortcutIn)
            BlockshortcutIn(currentPlayer);
        else if (blockType == BlockType.TypeDetail.shortcutOut)
            BlockPlus(currentPlayer);


        /*
        
        => 초도 구현 완료 : --
        => 테스트 완료 : ++
        


        ++none,

        ++plus,
        ++minus,

        boss,               ============== 미구현
        ++trap,
        ++lucky,

        ++shop,
        unique,
        ++shortcutIn,
        ++shortcutOut, 
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
        int coinValue = 1 * plusBlockMultiple + plusBlockPlus;

        // 코인 추가
        currentPlayer.coin.Add(coinValue);

        // 종료 판정
        isEnd = true;
    }


    static void BlockMinus(Player currentPlayer)
    {
        int coinValue = 2 * minusBlockMultiple + minusBlockPlus;

        // 코인 감소
        currentPlayer.coin.subtract(coinValue);

        // 종료 판정
        isEnd = true;
    }


    static void BlockBoss(Player currentPlayer)
    {
        // 보상 코인
        int reward = 100;

        // 미니게임
        // 미구현==================
        Minigame mini = Minigame.RandomGame(2);

        


        // 참가자
        List<Player> entry = new List<Player>();
        entry.Add(Player.system.Monster);
        entry.Add(currentPlayer);

        // 호출
        GameMaster.script.loadingManager.LoadAsyncMiniGame(mini, reward, entry);
    }


    static void BlockTrap(Player currentPlayer)
    {
        // 코인 수량 확보
        int coinValue = currentPlayer.coin.Value;

        // 코인 없으면 중단
        if (coinValue <= 0)
        {
            // 종료 판정
            isEnd = true;

            return;
        }

        // 코인 몰수
        currentPlayer.coin.subtract(coinValue);


        // 코인 아이템 인덱스
        int index = 1;


        // 오브젝트화
        //DynamicItem obj = GameData.itemManager.CreateItemObject(currentPlayer.movement.location, index, coinValue, ItemSlot.LoadIcon(Item.table[index]));
        DynamicItem obj = GameData.itemManager.CreateItemObject(currentPlayer.movement.location, index, coinValue);

        // 배치할 위치
        int loc = GameData.blockManager.indexLoop(currentPlayer.movement.location, -1);
        Debug.Log("트랩 블록 : 날릴 위치 => " + loc);
        Vector3 pos = GameData.blockManager.GetBlock(loc).transform.position;


        // 아이템 날리기
        Tool.ThrowParabola(obj.transform, pos, liftY, 1f);

        // 오브젝트 재배치
        obj.RemoveBarricade();
        obj.location = loc;
        obj.CreateBarricade();

        // 장애물 등록
        //DynamicObject.objectList[loc]++;          // 생성시 자동 등록

        // 종료 판정
        obj.DelayedBlockWorkEnd();
    }


    static void BlockLuckyBox(Player currentPlayer)
    {
        // 럭키박스 드랍테이블
        DropTable dropTable = new DropTable();

        // 드랍테이블 셋팅
        dropTable.rare = new List<int>();
        for (int i = 1; i < LuckyBox.table.Count; i++)
        {
            dropTable.rare.Add(LuckyBox.table[i].rare);
            Debug.Log("드랍 테이블 :: 추가됨 -> " + LuckyBox.table[i].rare);
        }
        Debug.Log("드랍 테이블 :: 목록 총량 ->" + dropTable.rare.Count);

        // 드랍 테이블 작동 및 드랍대상 인덱스 확보
        int select = 1 + dropTable.Drop();
        //select = 19;
        Debug.Log("럭키박스 :: 선택됨 -> "+ select);

        // 럭키박스 연출 시작
        LuckyBoxManager lbm = LuckyBoxManager.script;

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
        lbm.coroutineOpen = lbm.StartCoroutine(lbm.WaitAndResult(LuckyBox.table[select], currentPlayer));

        // 스트링 입력
        lbm.SetTextByIndex(select);
    }


    static void BlockShop(Player currentPlayer)
    {
        // 아이템번들 드랍테이블
        DropTable dropTable = new DropTable();

        // 첫번째 아이템 인댁스 (레이블과 코인 제외 => 2)
        int first = 2;

        // 드랍테이블 셋팅
        dropTable.rare = new List<int>();
        Debug.Log("드랍 테이블 :: 목록 총량 ->" + Item.table.Count);
        for (int i = first; i < Item.table.Count; i++)
        {
            dropTable.rare.Add(Item.table[i].rare);
            Debug.Log("드랍 테이블 :: 추가됨 -> " + Item.table[i].rare);
        }


        // 드랍 테이블 작동
        List<int> select = dropTable.Drop(ItemShop.script.bundle.Count);

        // 아이템 상점 갱신
        int trueIndex;
        for (int i = 0; i < ItemShop.script.bundle.Count; i++)
        {
            trueIndex = select[i] + first;

            // 드랍 테이블 작동
            Debug.Log("아이템 상점 :: 추가됨 -> [" + trueIndex + "] " + Item.table[trueIndex].name);
             
            // 아이템 번들 등록
            ItemShop.script.SetItemBundle(i, Item.table[trueIndex]);
        }

        // UI 출력
        //GameMaster.script.messageBox.PopUp(true, true, true, MessageBox.Type.Itemshop);
        //GameMaster.script.messageBox.PopUp(MessageBox.Type.Itemshop);
        ItemShop.script.OpenShop();


        // 종료판정은 UI에서 버튼 클릭으로 처리됨

    }


    static void BlockUnique(Player currentPlayer)
    {
        // 작동
        UniqueManager.script.Active(currentPlayer);

        // 종료 판정
        //isEnd = true;
    }


    static void BlockshortcutIn(Player currentPlayer)
    {
        /*
         
        사용 질문 UI 출력
        사용 거부시 중단 및 종료판정

        사용시 비용 지불
        이동
        도착 및 종료 판정
         
         */

        // 사용 질문 UI 호출
        ShortcutManager.script.CallShortcutUI(currentPlayer);

    }

}
