using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour
{

    // 빈 슬롯 파악
    public bool isEmpty {get { return item == null; } }

    // 소유자
    public Player owner = null;

    // 아이템
    //public Item item = null;
    Item _item = null;
    public Item item { get { return _item; } set { if (value == null) Clear(); else effect = value.effect; _item = value; } }
    Item itemMirror = null;


    // 아이템 개수
    public int count = 0;

    // 효과
    public IocEffect effect = IocEffect.New();

    // 아이콘
    public Image icon = null;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 아이템 변경시 자동 새로고침
        if (item != itemMirror)
        {

            Refresh();
        }
    }

    public void Clear()
    {
        //item = Item.table[0];
        //item = null;
        _item = null;
        //count = 0;
        //effect = IocEffect.New();

        Refresh();
    }

    /// <summary>
    ///  아이템에 맞게 갱신
    /// </summary>
    public void Refresh()
    {
        // 아이콘 로드
        if (item == null)
        {
            // 아이콘 변경
            icon.sprite = LoadIcon(Item.empty);

            // 개수 제거
            count = 0;

            // 효과 제거
            effect = IocEffect.New();
        }
        else
        {
            // 아이콘 변경
            icon.sprite = LoadIcon(item);

            // 효과 복사
            effect = item.effect;
        }

        // 싱크
        itemMirror = item;
    }

    public void CopyByMirror(ItemSlot mirror)
    {
        Debug.LogWarning("아이템 계승 :: " + mirror.item.name + " -> " + mirror.count);

        item = mirror.item;
        count = mirror.count;
        effect = mirror.effect;
    }

    /// <summary>
    /// 아이콘 로드
    /// </summary>
    public static Sprite LoadIcon(Item _item)
    {
        return _item.GetIcon();

        //// 아이콘 로드
        //Debug.Log(@"Data/Item/icon/item" + _item.index.ToString("D4"));
        //Sprite temp = Resources.Load<Sprite>(@"Data/Item/icon/item" + _item.index.ToString("D4"));

        //// 이미지 유효 검사
        //if (temp == null)
        //{
        //    // 기본 아이콘 대체 처리
        //    Debug.Log(@"Data/Item/icon/item0000");
        //    temp = Resources.Load<Sprite>(@"Data/Item/icon/item0000");
        //}

        ////// 최종 실패 처리
        ////if (temp == null)
        ////    Debug.Log("로드 실패 :: Data/Item/icon/item0000");
        ////// 아이콘 리턴
        ////else
        ////    _icon.sprite = temp;


        //// 아이콘 리턴
        //if (temp != null)
        //    return temp;

        //// 최종 실패 처리
        //Debug.Log("로드 실패 :: Data/Item/icon/item0000");
        //return null;
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

        // 퀵등록
        ItemManager im = GameMaster.script.itemManager;

        // UI 셋팅
        im.selected = this;
        Debug.LogWarning("아이템 :: 상세보기 => " + GameData.gameMaster.itemManager.selected.item.index);
        im.nameText.text = item.name;
        im.infoText.text = item.info;

        // UI 호출
        im.CallItemUseBox();

        // 사용 버튼 비활성
        im.btnUse.interactable = false;

        // 소유권 없을 시 사용 버튼 비활성
        if (transform.parent.name == "item") // 플레이어 정보 UI 오브젝트일 경우
        {
            PlayerInfoUI piui = transform.parent.parent.parent.parent.GetComponent<PlayerInfoUI>();

            if (piui.owner == Player.me) // 사용자가 아이템 소유권자일 경우
            {
                if (Turn.now == Player.me)    // 사용자가 턴 진행중일 경우
                {
                    // 사용 버튼 활성
                    GameData.gameMaster.itemManager.btnUse.interactable = true;
                }

            }


        }
    }

}
