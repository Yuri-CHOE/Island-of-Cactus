using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static List<DynamicItem> itemObjectList = new List<DynamicItem>();

    [Header("itemObject")]
    // 복사할 프리팹
    [SerializeField]
    GameObject itemPrefab = null;

    // 아이템 사용 UI
    [Header("itemUseMessegeBox")]
    public ItemSlot selected = null;
    public Player target = null;
    public Transform itemUseBox = null;
    public Text nameText = null;
    public Text infoText = null;
    public Button btnUse = null;

    [Header("itemObject")]
    // 관련 프리팹
    public GameObject itemSlotPrefab = null;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void CallItemUseBox()
    {
        MessageBox mb = GameData.gameMaster.messageBox;

        // 메시지 박스 닫기
        if (!mb.gameObject.activeSelf)
            mb.PopUp(0);

        // 호출
        itemUseBox.gameObject.SetActive(true);
    }

    public void CloseItemUseBox()
    {
        //MessageBox mb = GameData.gameMaster.messageBox;
        MessageBox mb = GameMaster.script.messageBox;

        // 메시지 박스 임시모드일 경우 닫기
        if (mb.pageSwitch.objectList[0].activeSelf)
            mb.PopUp(MessageBox.Type.Close);

        // 비활성
        ResetItemUseBox();
    }

    public void ResetItemUseBox()
    {
        // 비활성
        itemUseBox.gameObject.SetActive(false);

        // 타겟팅 UI 비활성
        GameData.gameMaster.playerSelecter[0].gameObject.SetActive(false);

    }

    /// <summary>
    /// PlayerInfoUI를 바탕으로 playerSelecter를 활성화하며 자신의 것만 비활성
    /// </summary>
    public void CallPlayerSelecter()
    {
        List<PlayerInfoUI> piuil = GameData.gameMaster.playerInfoUI;
        for (int i = 0; i < piuil.Count; i++)
        {
            // 선택 버튼 전체 활성화
            GameData.gameMaster.playerSelecter[i + 1].gameObject.SetActive(true);

            // 자신 비활성
            if (piuil[i].owner == Player.me)
            {
                GameData.gameMaster.playerSelecter[i + 1].gameObject.SetActive(false);
                continue;
            }

            // 존재하지 않는 플레이어 비활성
            if (piuil[i].owner == null)
            {
                GameData.gameMaster.playerSelecter[i + 1].gameObject.SetActive(false);
                continue;
            }

            // 사망자 비활성
            if (piuil[i].owner.isDead)
            {
                GameData.gameMaster.playerSelecter[i + 1].gameObject.SetActive(false);
                continue;
            }
        }

        // UI 활설
        GameData.gameMaster.playerSelecter[0].gameObject.SetActive(true);
    }

    public void BtnUse()
    {
        // 아이템 사용 판정
        GameMaster.useItemOrder = true;

        // UI 전체 비활성
        GameData.gameMaster.playerSelecter[0].gameObject.SetActive(false);

        // 메시지 박스 닫기
        GameMaster.script.messageBox.PopUp(MessageBox.Type.Close);
    }

    /// <summary>
    /// UI를 통해 선택된 아이템을 사용
    /// 필요시 타겟팅 UI 호출됨
    /// </summary>
    public void ItemUseByUI() { ItemUseByUI(selected.owner); }
    /// <summary>
    /// UI를 통해 선택된 아이템을 특정 대상에게 사용
    /// </summary>
    /// <param name="targetPlayer_Or_null"></param>
    public void ItemUseByUI(Player targetPlayer_Or_null)
    {
        // UI 활성화
        BtnUse();

        // 아이템 사용
        ItemUse(selected, targetPlayer_Or_null);

        // 선택 초기화
        selected = null;

        // 메인스트림 탈출 - 취소
        GameMaster.useItemOrder = false;
    }


    /// <summary>
    /// 특정 아이템을 사용
    /// 필요시 타겟팅 UI 호출됨
    /// </summary>
    /// <param name="_slot"></param>
    public void ItemUse(ItemSlot _slot)
    {
        // 타겟팅 형 아이템
        if (_slot.item.type == Item.Type.Target)
        {
            // 타겟팅 UI 호출
            CallPlayerSelecter();

            // 메인스트림 탈출 - 취소
            GameMaster.useItemOrder = false;

            return;
        }

        // 아이템 사용
        ItemUse(_slot, _slot.owner);
    }
    /// <summary>
    /// 특정 아이템을 특정 대상에게 사용
    /// </summary>
    /// <param name="_slot"></param>
    /// <param name="targetPlayer_Or_null"></param>
    public void ItemUse(ItemSlot _slot, Player targetPlayer_Or_null)
    {
        Debug.Log(string.Format("item use :: 아이템 = {0}   대상 = {1}", _slot.item.index, _slot.owner.name));

        // 개수 차감
        _slot.count--;

        // UI 비활성
        //GameData.gameMaster.playerSelecter[0].gameObject.SetActive(false);

        // 아이템 사용 요청
        //Item.Effect(_slot.item, targetPlayer_Or_null);
        // ItemUse가 버튼으로 호출되기 때문에 여기서 중단 거는 방법 찾아야할듯
        // ㄴ 아이템 매니저에서 static 으로 bool값 만들어서 아이템 이펙트에서 제어하고 메인스트림에서 bool값으로 중단처리 해야할듯
        StartCoroutine(_slot.item.Effect(targetPlayer_Or_null));

        // 아이템 제거
        Player.me.RemoveItem(_slot);
    }


    /// <summary>
    /// 특정 블록에 아이템 생성
    /// </summary>
    /// <param name="blockIndex">특정 블록의 인덱스 값</param>
    DynamicItem Create(int blockIndex)
    {
        // 생성할 좌표
        Vector3 pos = GameData.blockManager.GetBlock(blockIndex).transform.position;

        // y축 보정
        pos.y = 2.5f;

        // 아이템 오브젝트 생성
        Transform obj = Instantiate(itemPrefab, pos, Quaternion.identity ,transform).transform;

        // 결과물
        DynamicItem result = obj.GetComponent<DynamicItem>();

        // 위치 설정
        result.location = blockIndex;


        return result;
    }

    /// <summary>
    /// 특정 블록에 아이템 생성 후 초기화
    /// </summary>
    /// <param name="blockIndex">특정 블록의 인덱스값</param>
    /// <param name="itemSlot">초기화 값</param>
    public void CreateItemObject(int blockIndex, ItemSlot itemSlot)
    {
        CreateItemObject(
            blockIndex,
            itemSlot.item.index,
            itemSlot.count
            //itemSlot.icon.sprite
            );
    }
    /// <summary>
    /// 특정 블록에 아이템 생성 후 초기화
    /// </summary>
    /// <param name="blockIndex">생성 위치 블록의 인덱스값</param>
    /// <param name="itemIndex">초기화 값 : 아이템 인덱스</param>
    /// <param name="_count">초기화 값 : 수량</param>
    /// <param name="_icon">초기화 값 : 아이콘 리소스</param>
    //public DynamicItem CreateItemObject(int blockIndex, int itemIndex, int _count, Sprite _icon)
    public DynamicItem CreateItemObject(int blockIndex, int itemIndex, int _count )
    {
        Debug.LogWarning("아이템 생성 :: " + blockIndex + " 에서 생성됨");

        // 아이템 오브젝트 생성 후 스크립트 확보
        DynamicItem dItem = Create(blockIndex);

        // 아이템 셋팅
        //dItem.SetUp(itemIndex, _count, _icon);
        dItem.SetUp(itemIndex, _count);


        // 목록에 추가
        itemObjectList.Add(dItem);

        // 장애물 등록
        dItem.CreateBarricade();



        return dItem;
    }


    public static void ReCreateAll() { ReCreateAll(false); }
    public static void ReCreateAll(bool isDeleted)
    {
        Debug.LogError("아이템 오브젝트 :: 재생성 요청됨 => 총 "+itemObjectList.Count);

        // 백업
        List<DynamicItem> temp = itemObjectList;

        // 초기화
        itemObjectList = new List<DynamicItem>();

        // 반복 재생성
        for (int i = 0; i < temp.Count; i++)
        {
            DynamicItem dTemp = temp[i];

            // 리스트 및 장애물 제거
            //dTemp.Remove();
            dTemp.RemoveBarricade();

            // 생성
            DynamicItem dNew =
            GameMaster.script.itemManager.CreateItemObject(
                dTemp.location,
                dTemp.item.index,
                dTemp.count
                //dTemp.icon
                );

            Tool.ThrowParabola(dNew.transform, dNew.transform.position, 2f, 1f);

            // 제거
            if (!isDeleted)
                Destroy(dTemp.transform);
        }
    }




    public void Tester(string blockIndex_itemIndex_value)
    {
        int blockIndex = 7;
        int itemIndex = 1;
        int value = 1;

        string[] str = blockIndex_itemIndex_value.Split('_');
        int.TryParse(str[0], out blockIndex);
        if(str.Length > 1)
        int.TryParse(str[1], out itemIndex);
        if (str.Length > 2)
            int.TryParse(str[2], out value);

        DynamicItem di =
        CreateItemObject(
            blockIndex,
            itemIndex,
            value
            //ItemSlot.LoadIcon(Item.table[itemIndex])
            );

        Tool.ThrowParabola(di.transform, di.transform.position, 2f, 1f);
    }


    /// <summary>
    /// 아이템 슬롯 생성
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <param name="_count"></param>
    /// <returns></returns>
    public ItemSlot CreateItemSlot(Item _item, int _count)
    {
        // 생성할 좌표
        Vector3 pos = GameData.blockManager.startBlock.position;

        // y축 보정
        pos.y = -5f;

        // 아이템 오브젝트 생성
        ItemSlot result = Instantiate(itemSlotPrefab, pos, Quaternion.identity, GameMaster.script.transform).GetComponent<ItemSlot>();

        result.item = _item;
        result.count = _count;

        return result;
    }
}
