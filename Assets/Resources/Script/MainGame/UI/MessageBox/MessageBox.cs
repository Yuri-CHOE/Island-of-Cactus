using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.MessageBox;

public class MessageBox : MonoBehaviour
{
    public enum Type
    {
        Message,
        Setting,
        Itemshop,
        LuckyBox,
    }

    [SerializeField]
    Image m_box;
    [SerializeField]
    GameObject Innerbox;
    [SerializeField]
    GameObject npcSpace;
        
    public ObjectMultiSwitch btnSwitch;
    public ObjectSwitch pageSwitch;

    List<int[]> mBoxPreset = new List<int[]>();

    [SerializeField]
    const int btnMax = 5;

    [SerializeField]
    bool[] preMessage = new bool[btnMax];    // 메시지박스 모드

    [SerializeField]
    bool[] preSetting = new bool[btnMax];    // 환경설정 모드

    [SerializeField]
    bool[] preItemshop = new bool[btnMax];   // 아이템 상점 모드

    [SerializeField]
    bool[] preLuckyBox = new bool[btnMax];   // 럭키 박스 모드


    // Start is called before the first frame update
    void Start()
    {
        // 메시지 박스 프리셋 등록
        addPreset(preMessage, 0);
        addPreset(preSetting, 1);
        addPreset(preItemshop, 2);
        addPreset(preLuckyBox, 3);
    }

    // Update is called once per frame
    void Update()
    {
        tempSet();   // 테스트 작동용
    }


    // 테스트용 시작
    [SerializeField]
    bool runBtn = false;
    [SerializeField]
    int runID = 0;
    void tempSet()
    {
        if (runBtn == true)
        {
            Debug.Log("메시지 박스 작동 감지");
            if (runID == -1)
            {
                close();
                Debug.Log("메시지 박스 닫음");
            }
            else if (runID >= mBoxPreset.Count)
            {
                close();
                Debug.Log("Out of range -> 메시지 박스 닫음");
            }
            else
            {
                runBtn = false;
                // 번호에 맞는 프리셋 popUp()
                PopUp(  pageSwitch.GetObject(runID).GetComponent<MessageBoxRule>(), runID   );
                Debug.Log("메시지 박스 열기 : " + runID);
            }
        }
    }
    // 테스트용 끝


    /// <summary>
    /// 열려있는 메시지 박스 비활성화
    /// </summary>
    public void close()
    {
        // 메시지 박스 GameObject 비활성
        gameObject.SetActive(false);
    }


    void open()
    {
        // 메시지 박스 GameObject 활성
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="btnPreset"></param>
    /// <param name="pageNum"></param>
    void addPreset(bool[] btnPreset, int pageNum)
    {
        // 버튼 목록 수량 체크
        if (!btnSwitch.checkCountObject(btnPreset.Length))
            return;

        // 페이지 수량 체크
        if (pageNum > pageSwitch.count())
        {
            Debug.Log("error :: page over (Max:" + pageSwitch.count() + ") -> " + pageNum);
            return;
        }

        // 페이지 룰 누락시 자동 생성
        if (pageSwitch.GetObject(pageNum).GetComponent<MessageBoxRule>() == null)
            pageSwitch.GetObject(pageNum).AddComponent<MessageBoxRule>();

        mBoxPreset.Add(new int[] { btnSwitch.countPreset(), pageNum });
        btnSwitch.addPreset(btnPreset);
        Debug.Log("Add : " + pageNum);
    }

    void callPreset(int Num)
    {
        // 지정된 번호를 사용하여 각 버튼 프리셋 제어

        // 지정 안된 프리셋 요청시 중단
        if (Num >= mBoxPreset.Count)
        {
            Debug.Log("error :: Out of Range (0~" + (mBoxPreset.Count-1) + ") -> " + Num);
            return;
        }

        Debug.Log("test :: [" + mBoxPreset[Num][0] + "] [" + mBoxPreset[Num][1] + "]");

        // 버튼 설정
        btnSwitch.setUp(mBoxPreset[Num][0]);

        // 페이지 설정
        pageSwitch.setUp(mBoxPreset[Num][1]);

        // 오브젝트 활성화
        open();
    }

    /// <summary>
    /// 메시지 박스를 프리셋을 사용하여 출력
    /// </summary>
    /// <param name="isBoxUse">바깥 box 제어</param>
    /// <param name="isInnerBoxUse">안쪽 box 제어</param>
    /// <param name="isNpcSpaceUse">좌측 하단 NPC용 버튼 여백 확보</param>
    /// <param name="mBoxPresetNum">메시지 박스 프리셋 번호</param>
    public void PopUp(bool isBoxUse, bool isInnerBoxUse, bool isNpcSpaceUse, int mBoxPresetNum)
    {
        // 음수값이면 모든 메시지 박스 닫기
        if (mBoxPresetNum < 0)
        {
            close();
            return;
        }

        // 테스트 비활성
        runBtn = false;

        m_box.enabled = isBoxUse;
        Innerbox.SetActive(isInnerBoxUse);
        npcSpace.SetActive(isNpcSpaceUse);
        callPreset(mBoxPresetNum);
    }
    public void PopUp(MessageBoxRule rule, int mBoxPresetNum) { PopUp(rule.isBoxUse, rule.isInnerBoxUse, rule.isNpcSpaceUse, mBoxPresetNum); }
    public void PopUp(int mBoxPresetNum) { PopUp(true, true, false, mBoxPresetNum); }
    public void PopUp(bool isBoxUse, bool isInnerBoxUse, bool isNpcSpaceUse, Type mBoxPresetType) { PopUp(isBoxUse, isInnerBoxUse, isNpcSpaceUse, (int)mBoxPresetType); }
    public void PopUp(Type mBoxPresetType) { PopUp(true, true, false, (int)mBoxPresetType); }
}
