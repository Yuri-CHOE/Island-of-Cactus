using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.MessageBox;

public class MessageBox : MonoBehaviour
{
    public enum Type
    {
        Close = -1,
        Message,
        Setting,
        Itemshop,
        LuckyBox,
        ShortCut,
        UniqueBlock,
        Event,
        ItemUse,
    }

    [SerializeField]
    Image m_box;
    [SerializeField]
    GameObject Innerbox;
    public GameObject npcSpace;

    public Text messageTitle = null;
    public Text messageInfo = null;

    [Header("button")]
    [SerializeField]
    Button btnEmpty = null;
    [SerializeField]
    Button btnNameChange = null;
    [SerializeField]
    Button btnLanguage = null;
    [SerializeField]
    Button btnTitle = null;
    [SerializeField]
    Button btnBuy = null;
    [SerializeField]
    Button btnUse = null;
    [SerializeField]
    Button btnClose = null;

    //public ObjectMultiSwitch btnSwitch;
    public ObjectSwitch pageSwitch;

    List<MessageBoxRule> ruleList = new List<MessageBoxRule>();

    [Header("rule")]
    [SerializeField]
    MessageBoxRule ruleMessage = null;
    [SerializeField]
    MessageBoxRule ruleSetting = null;
    [SerializeField]
    MessageBoxRule ruleItemshop = null;
    [SerializeField]
    MessageBoxRule ruleLuckyBox = null;
    [SerializeField]
    MessageBoxRule ruleShortCut = null;
    [SerializeField]
    MessageBoxRule ruleUniqueBlock = null;
    [SerializeField]
    MessageBoxRule ruleEvent = null;


    //List<int[]> mBoxPreset = new List<int[]>();

    //[SerializeField]
    //const int btnMax =6;

    //[Header("preset")]

    //[SerializeField]
    //bool[] preMessage = new bool[btnMax];    // 메시지박스 모드

    //[SerializeField]
    //bool[] preSetting = new bool[btnMax];    // 환경설정 모드

    //[SerializeField]
    //bool[] preItemshop = new bool[btnMax];   // 아이템 상점 모드

    //[SerializeField]
    //bool[] preLuckyBox = new bool[btnMax];   // 럭키 박스 모드

    //[SerializeField]
    //bool[] preShortCut = new bool[btnMax];   // 숏컷 질문 모드

    //[SerializeField]
    //bool[] preUniqueBlock = new bool[btnMax];   // 유니크 블록 모드

    //[SerializeField]
    //bool[] preEvent = new bool[btnMax];   // 이벤트 모드


    // Start is called before the first frame update
    void Start()
    {
        // 프리셋 초기화
        ruleList.Clear();

        // 프리셋 등록
        ruleList.Add(ruleMessage);
        ruleList.Add(ruleSetting);
        ruleList.Add(ruleItemshop);
        ruleList.Add(ruleLuckyBox);
        ruleList.Add(ruleShortCut);
        ruleList.Add(ruleUniqueBlock);
        ruleList.Add(ruleEvent);

        //addPreset(preMessage, 0);
        //addPreset(preSetting, 1);
        //addPreset(preItemshop, 2);
        //addPreset(preLuckyBox, 3);
        //addPreset(preShortCut, 4);
        //addPreset(preUniqueBlock, 5);
        //addPreset(preEvent, 6);

        // 비활성
        PopUp(Type.Close);
    }

    // Update is called once per frame
    void Update()
    {
        //tempSet();   // 테스트 작동용
    }


    //void tempSet()
    //{
    //    if (runBtn == true)
    //    {
    //        Debug.Log("메시지 박스 작동 감지");
    //        if (runID == -1)
    //        {
    //            close();
    //            Debug.Log("메시지 박스 닫음");
    //        }
    //        else if (runID >= mBoxPreset.Count)
    //        {
    //            close();
    //            Debug.Log("Out of range -> 메시지 박스 닫음");
    //        }
    //        else
    //        {
    //            runBtn = false;
    //            // 번호에 맞는 프리셋 popUp()
    //            PopUp(  pageSwitch.GetObject(runID).GetComponent<MessageBoxRule>(), runID   );
    //            Debug.Log("메시지 박스 열기 : " + runID);
    //        }
    //    }
    //}
    // 테스트용 끝


    /// <summary>
    /// 열려있는 메시지 박스 비활성화
    /// </summary>
    public void Close()
    {
        // 메시지 박스 GameObject 비활성
        gameObject.SetActive(false);

        // 독립형 박스 비활성
        //if(GameMaster.script.itemManager.itemUseBox.gameObject.activeSelf)
        //    GameMaster.script.itemManager.CloseItemUseBox();
            GameMaster.script.itemManager.ResetItemUseBox();
    }


    void open()
    {
        // 메시지 박스 GameObject 활성
        gameObject.SetActive(true);
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="btnPreset"></param>
    ///// <param name="pageNum"></param>
    //void addPreset(bool[] btnPreset, int pageNum)
    //{
    //    // 버튼 목록 수량 체크
    //    if (!btnSwitch.checkCountObject(btnPreset.Length))
    //        return;

    //    // 페이지 수량 체크
    //    if (pageNum > pageSwitch.count())
    //    {
    //        Debug.Log("error :: page over (Max:" + pageSwitch.count() + ") -> " + pageNum);
    //        return;
    //    }

    //    // 페이지 룰 누락시 자동 생성
    //    if (pageSwitch.GetObject(pageNum).GetComponent<MessageBoxRule>() == null)
    //        pageSwitch.GetObject(pageNum).AddComponent<MessageBoxRule>();

    //    mBoxPreset.Add(new int[] { btnSwitch.countPreset(), pageNum });
    //    btnSwitch.addPreset(btnPreset);
    //    Debug.Log("Add : " + pageNum);
    //}

    //void callPreset(int Num)
    //{
    //    // 지정된 번호를 사용하여 각 버튼 프리셋 제어

    //    // 지정 안된 프리셋 요청시 중단
    //    if (Num >= mBoxPreset.Count)
    //    {
    //        Debug.Log("error :: Out of Range (0~" + (mBoxPreset.Count-1) + ") -> " + Num);
    //        return;
    //    }

    //    Debug.Log("test :: [" + mBoxPreset[Num][0] + "] [" + mBoxPreset[Num][1] + "]");

    //    // 버튼 설정
    //    btnSwitch.setUp(mBoxPreset[Num][0]);

    //    // 페이지 설정
    //    pageSwitch.setUp(mBoxPreset[Num][1]);

    //    // 오브젝트 활성화
    //    open();
    //}
    void callPreset(int ruleListIndex)
    {
        // 지정된 번호를 사용하여 각 버튼 프리셋 제어

        // 지정 안된 프리셋 요청시 중단
        if (ruleListIndex >= ruleList.Count)
        {
            Debug.Log("error :: Out of Range (0~" + (ruleList.Count - 1) + ") -> " + ruleListIndex);
            return;
        }

        // 버튼 설정
        BtnSetUp(ruleList[ruleListIndex]);

        // 페이지 설정
        pageSwitch.setUp(ruleListIndex);

        // 오브젝트 활성화
        open();
    }


    void BtnSetUp(MessageBoxRule rule)
    {
        btnEmpty.gameObject.SetActive(rule.useEmpty);
        btnNameChange.gameObject.SetActive(rule.useNameChange);
        btnLanguage.gameObject.SetActive(rule.useLanguage);
        btnTitle.gameObject.SetActive(rule.useTitle);
        btnBuy.gameObject.SetActive(rule.useBuy);
        btnUse.gameObject.SetActive(rule.useUse);
        btnClose.gameObject.SetActive(rule.useClose);
    }

    /// <summary>
    /// 메시지 박스를 프리셋을 사용하여 출력
    /// </summary>
    /// <param name="isBoxUse">바깥 box 제어</param>
    /// <param name="isInnerBoxUse">안쪽 box 제어</param>
    /// <param name="isNpcSpaceUse">좌측 하단 NPC용 버튼 여백 확보</param>
    /// <param name="mBoxPresetNum">메시지 박스 프리셋 번호</param>
    public void PopUp(bool isBoxUse, bool isInnerBoxUse, bool isNpcSpaceUse, int ruleListIndex)
    {
        // 음수값이면 모든 메시지 박스 닫기
        if (ruleListIndex < 0)
        {
            Close();
            return;
        }

        m_box.enabled = isBoxUse;
        Innerbox.SetActive(isInnerBoxUse);
        npcSpace.SetActive(isNpcSpaceUse);
        callPreset(ruleListIndex);
    }
    public void PopUp(MessageBoxRule rule, int ruleListIndex) { PopUp(rule.isBoxUse, rule.isInnerBoxUse, rule.isNpcSpaceUse, ruleListIndex); }
    public void PopUp(bool isBoxUse, bool isInnerBoxUse, bool isNpcSpaceUse, Type mBoxPresetType) { PopUp(isBoxUse, isInnerBoxUse, isNpcSpaceUse, (int)mBoxPresetType); }
    public void PopUp(Type mBoxPresetType)
    {
        if (mBoxPresetType == Type.Close)
        {
            Close(); return;
        }
        int num = (int)mBoxPresetType;

        PopUp(
            ruleList[num].isBoxUse, 
            ruleList[num].isInnerBoxUse, 
            ruleList[num].isNpcSpaceUse, 
            num);
    }
    public void PopUpText(string _title, string _info)
    {
        int num = 0;

        messageTitle.text = _title;
        messageInfo.text = _info;

        PopUp(
            ruleList[num].isBoxUse,
            ruleList[num].isInnerBoxUse,
            ruleList[num].isNpcSpaceUse,
            num);


    }




    public void Out()
    {
        // 메시지 박스 초기화 및 닫기
        PopUp(Type.Close);

        // 종료 판정
        //BlockWork.isEnd = true;
        BlockWork.isEnd = ruleList[pageSwitch.now].blockWorkEndWithClose;
    }

}
