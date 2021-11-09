using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemUnit
{
    // 아이템
    Item _item = null;
    public Item item { get { return _item; } set { if (value == null) Clear(); else effect = value.effect; _item = value; } }

    // 효과
    public IocEffect effect = IocEffect.New();
    
    // 아이템 개수
    //public int count = 0;
    public int count { get { return effect.count; } set { effect.SetCount(value); } }

    public bool isEmpty { get { return item == null || item == Item.empty; } }


    void Change(Item newItemOrNull)
    {
        if (newItemOrNull == null || newItemOrNull.index == 0) Clear();
        else { _item = newItemOrNull; effect = newItemOrNull.effect; }
    }

    void Clear()
    {
        _item = null;
        effect = IocEffect.New();
    }
}

public class ItemSlot : MonoBehaviour
{
    //public static operator ==(ItemSlot s1, ItemSlot s2) => ();
    //public static operator !=(ItemSlot s1, ItemSlot s2) => ();

    // 빈 슬롯 파악
    public bool isEmpty {get { return item == null; } }

    // 소유자
    public Player owner = null;

    // 아이템
    ItemUnit itemUnit = null;
    public Item item {
        get {
            if (itemUnit == null)
                return null;
            else
                return itemUnit.item;
        }
        set
        {
            // 다른 아이템일 경우
            //if (value == null || itemUnit.item != value)
            if (itemUnit.item != value)
            {
                // 아이템 교체
                itemUnit.item = value;

                // 아이콘 새로고침
                if (itemUnit.item != null)
                    icon.sprite = item.GetIcon();
                else
                    icon.sprite = Item.empty.GetIcon();
            }
            else
                Debug.LogWarning("아이콘 :: 인벤토리 아이콘 갱신 거부됨 -> 동일한 아이템");
        }
    }

    // 아이콘
    public Image icon = null;

    // 수량
    public int count { get { return itemUnit.count; } set { itemUnit.count = value; } }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //// 아이템 변경시 자동 새로고침
        //if (item != itemMirror)
        //{

        //    Refresh();
        //}
    }

    //public void Clear()
    //{
    //    //item = Item.table[0];
    //    //item = null;
    //    _item = null;
    //    //count = 0;
    //    //effect = IocEffect.New();

    //    Refresh();
    //}

    ///// <summary>
    /////  아이템에 맞게 갱신
    ///// </summary>
    //public void Refresh()
    //{
    //    // 아이콘 로드
    //    if (item == null)
    //    {
    //        // 아이콘 변경
    //        icon.sprite = LoadIcon(Item.empty);

    //        // 개수 제거
    //        count = 0;

    //        // 효과 제거
    //        effect = IocEffect.New();
    //    }
    //    else
    //    {
    //        // 아이콘 변경
    //        icon.sprite = item.GetIcon();

    //        // 효과 복사
    //        effect = item.effect;
    //    }

    //    // 싱크
    //    itemMirror = item;
    //}

    //public void CopyByMirror(ItemSlot mirror)
    //{
    //    Debug.LogWarning("아이템 계승 :: " + mirror.item.name + " -> " + mirror.count);

    //    item = mirror.item;
    //    count = mirror.count;
    //    effect = mirror.effect;
    //}

    public void SetUp(Player _owner, ItemUnit _itemUnit)
    {
        owner = _owner;
        itemUnit = _itemUnit;

        // 아이콘 새로고침
        //icon.sprite = item.GetIcon();
    }

    /// <summary>
    /// 슬롯 눌렀을 경우 처리
    /// </summary>
    public void Clicked()
    {
        // 없는 아이템 중단
        if (item == null)
            return;

        if (count <= 0)
            return;

        // UI 셋팅
        GameMaster.script.itemManager.selected = this;
        Debug.Log("아이템 :: 상세보기 => " + GameData.gameMaster.itemManager.selected.item.index);
        GameMaster.script.itemManager.nameText.text = item.name;
        GameMaster.script.itemManager.infoText.text = item.info;

        // UI 호출
        GameMaster.script.itemManager.CallItemUseBox();

        // 사용 버튼 비활성
        GameMaster.script.itemManager.btnUse.interactable = false;

        // 소유권 없을 시 사용 버튼 비활성
        if (transform.parent.name == "item") // 플레이어 정보 UI 오브젝트일 경우
        {
            PlayerInfoUI piui = transform.parent.parent.parent.parent.GetComponent<PlayerInfoUI>();

            if (piui.owner == Player.me) // 사용자가 아이템 소유권자일 경우
            {
                if (Turn.now == Player.me)    // 사용자가 턴 진행중일 경우
                {
                    if (Player.me.dice.valueTotal == 0)    // 주사위 아직 안굴렷을 경우
                    {
                        // 사용 버튼 활성
                        GameData.gameMaster.itemManager.btnUse.interactable = true;
                    }
                }

            }


        }
    }

    public bool CheckUnit(ItemUnit _itemUnit)
    {
        return (itemUnit == _itemUnit);
    }

    public void Take(ItemSlot target)
    {
        Debug.Log("아이템 계승 :: " + target.item.name + " -> " + target.count);

        item = target.item;
        target.item = null;
    }

    public ItemUnit GetUnit()
    {
        return itemUnit;
    }

}
