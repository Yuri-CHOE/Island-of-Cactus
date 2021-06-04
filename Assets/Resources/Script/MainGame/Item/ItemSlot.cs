using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour
{
    // 빈 슬롯 파악
    public bool isEmpty {get { return item == null; } }

    // 아이템
    public Item item = null;
    Item itemMirror = null;


    // 아이템 개수
    public int count = 0;

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

    /// <summary>
    ///  아이템에 맞게 갱신
    /// </summary>
    public void Refresh()
    {
        // 아이콘 로드
        icon.sprite = LoadIcon(item);

        // 싱크
        itemMirror = item;
    }

    /// <summary>
    /// 아이콘 로드
    /// </summary>
    public static Sprite LoadIcon(Item _item)
    {
        // 아이콘 로드
        Debug.Log(@"Data/Item/icon/item" + _item.index.ToString("D4"));
        Sprite temp = Resources.Load<Sprite>(@"Data/Item/icon/item" + _item.index.ToString("D4"));

        // 이미지 유효 검사
        if (temp == null)
        {
            // 기본 아이콘 대체 처리
            Debug.Log(@"Data/Item/icon/item0000");
            temp = Resources.Load<Sprite>(@"Data/Item/icon/item0000");
        }

        //// 최종 실패 처리
        //if (temp == null)
        //    Debug.Log("로드 실패 :: Data/Item/icon/item0000");
        //// 아이콘 리턴
        //else
        //    _icon.sprite = temp;


        // 아이콘 리턴
        if (temp != null)
            return temp;

        // 최종 실패 처리
        Debug.Log("로드 실패 :: Data/Item/icon/item0000");
        return null;
    }

    /// <summary>
    /// 슬롯 눌렀을 경우 처리
    /// </summary>
    public void Clicked()
    {
        /// 미구현===========
        /// 누르면 메시지 박스로 아이템 정보 및 닫기버튼 출력 + 자신의 턴, 자신의 아이템일 경우 사용버튼 출력
    }

}
