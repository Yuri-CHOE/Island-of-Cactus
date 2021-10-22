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

        // 결과물
        DynamicEvent result = obj.GetComponent<DynamicEvent>();

        // 위치 설정
        result.location = blockIndex;


        return result;
    }

    /// <summary>
    /// 특정 블록에 이벤트 생성 후 초기화
    /// </summary>
    /// <param name="blockIndex">생성 위치 블록의 인덱스값</param>
    /// <param name="eventIndex">초기화 값 : 이벤트 인덱스</param>
    /// <param name="_count">초기화 값 : 수량</param>
    /// <param name="creator">초기화 값 : 이벤트 생성자</param>
    public DynamicEvent CreateEventObject(int blockIndex, int eventIndex, int _count, Player creator)
    {
        // 이벤트 오브젝트 생성 후 스크립트 확보
        DynamicEvent dEvent = Create(blockIndex);

        // 이벤트 셋팅
        dEvent.SetUp(eventIndex, _count, creator);

        Debug.LogWarning("이벤트 생성 :: [" + dEvent.iocEvent.name + "] 이벤트가 " + blockIndex + " 에서 생성됨");


        // 목록에 추가
        eventObjectList.Add(dEvent);

        // 장애물 등록
        dEvent.CreateBarricade();

        return dEvent;
    }


    public static void ReCreateAll() { ReCreateAll(false); }
    public static void ReCreateAll(bool isDeleted)
    {
        Debug.LogError("이벤트 오브젝트 :: 재생성 요청됨 => 총 " + eventObjectList.Count);

        // 백업
        List<DynamicEvent> temp = eventObjectList;

        // 초기화
        eventObjectList = new List<DynamicEvent>();

        // 반복 재생성
        for (int i = 0; i < temp.Count; i++)
        {
            DynamicEvent dTemp = temp[i];

            // 리스트 및 장애물 제거
            //dTemp.Remove();
            dTemp.RemoveBarricade();

            // 생성
            GameMaster.script.eventManager.CreateEventObject(
                dTemp.location,
                dTemp.iocEvent.index,
                dTemp.count,
                dTemp.creator
                );

            // 제거
            if (!isDeleted)
                Destroy(dTemp.transform);            
        }
    }









    public void Tester(string blockIndex_eventID)
    {
        int blockIndex = 7;
        int eventID = 1;

        string[] str = blockIndex_eventID.Split('_');
        int.TryParse(str[0], out blockIndex);
        if (str.Length > 1)
            int.TryParse(str[1], out eventID);

        DynamicEvent de =
        CreateEventObject(
            blockIndex,
            eventID, 
            1,
            Player.me
            );
    }
}
