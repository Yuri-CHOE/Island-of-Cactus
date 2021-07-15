using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static List<DynamicEvent> eventObjectList = new List<DynamicEvent>();

    [Header("eventObject")]
    // 복사할 프리팹
    [SerializeField]
    GameObject eventPrefab = null;

    // 이벤트 작동 UI
    [Header("EventMessegeBox")]
    //public IocEvent selected = null;
    public Transform eventBox = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    //public void CallMessageBox()
    //{
    //    MessageBox mb = GameData.gameMaster.messageBox;

    //    // 메시지 박스 임시모드
    //    if (!mb.gameObject.activeSelf)
    //        mb.PopUp(0);

    //    // 호출
    //    eventBox.gameObject.SetActive(true);
    //}

    //public void CloseMessageBox()
    //{
    //    MessageBox mb = GameData.gameMaster.messageBox;

    //    // 메시지 박스 임시모드일 경우 닫기
    //    if (mb.pageSwitch.objectList[0].activeSelf)
    //        mb.PopUp(MessageBox.Type.Close);

    //    // 비활성
    //    eventBox.gameObject.SetActive(false);
    //}



    /// <summary>
    /// 특정 블록에 아이템 생성
    /// </summary>
    /// <param name="blockIndex">특정 블록의 인덱스 값</param>
    DynamicEvent Create(int blockIndex)
    {
        // 생성할 좌표
        Vector3 pos = GameData.blockManager.GetBlock(blockIndex).transform.position;

        // y축 보정
        pos.y = 2.5f;

        // 이벤트 오브젝트 생성
        Transform obj = Instantiate(eventPrefab, pos, Quaternion.identity, transform).transform;


        return obj.GetComponent<DynamicEvent>();
    }

    /// <summary>
    /// 특정 블록에 이벤트 생성 후 초기화
    /// </summary>
    /// <param name="blockIndex">생성 위치 블록의 인덱스값</param>
    /// <param name="eventIndex">초기화 값 : 이벤트 인덱스</param>
    /// <param name="_count">초기화 값 : 수량</param>
    /// <param name="creator">초기화 값 : 이벤트 생성자</param>
    public DynamicEvent CreateItemObject(int blockIndex, int eventIndex, int _count, Player creator)
    {
        Debug.LogError("아이템 생성 :: " + blockIndex + " 에서 생성됨");

        // 아이템 오브젝트 생성 후 스크립트 확보
        DynamicEvent dEvent = Create(blockIndex);

        // 아이템 셋팅
        dEvent.SetUp(eventIndex, _count, creator);


        // 목록에 추가
        eventObjectList.Add(dEvent);

        return dEvent;
    }

    public void Tester(int eventID)
    {
        CreateItemObject(
            GameData.player.me.movement.location, 
            eventID, 
            1,
            GameData.player.me
            );
    }
}
