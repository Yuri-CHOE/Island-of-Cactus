using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static List<DynamicItem> itemObjectList = new List<DynamicItem>();

    // 복사할 프리팹
    [SerializeField]
    GameObject itemPrefab = null;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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


        return obj.GetComponent<DynamicItem>();
    }

    /// <summary>
    /// 특정 블록에 아이템 생성 후 초기화
    /// </summary>
    /// <param name="blockIndex">특정 블록의 인덱스값</param>
    /// <param name="itemSlot">초기화 값</param>
    public void CreateItemObject(int blockIndex, ItemSlot itemSlot)
    {
        // 아이템 오브젝트 생성 후 스크립트 확보
        DynamicItem dItem = Create(blockIndex);

        // 아이템 셋팅
        dItem.SetUp(itemSlot);


        // 목록에 추가
        itemObjectList.Add(dItem);
    }
    /// <summary>
    /// 특정 블록에 아이템 생성 후 초기화
    /// </summary>
    /// <param name="blockIndex">생성 위치 블록의 인덱스값</param>
    /// <param name="itemIndex">초기화 값 : 아이템 인덱스</param>
    /// <param name="_count">초기화 값 : 수량</param>
    /// <param name="_icon">초기화 값 : 아이콘 리소스</param>
    public DynamicItem CreateItemObject(int blockIndex, int itemIndex, int _count, Sprite _icon)
    {
        Debug.LogError("아이템 생성 :: " + blockIndex + " 에서 생성됨");

        // 아이템 오브젝트 생성 후 스크립트 확보
        DynamicItem dItem = Create(blockIndex);

        // 아이템 셋팅
        dItem.SetUp(itemIndex, _count, _icon);


        // 목록에 추가
        itemObjectList.Add(dItem);

        return dItem;
    }
}
