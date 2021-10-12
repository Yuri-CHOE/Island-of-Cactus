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
    //bool[] preMessage = new bool[btnMax];    // �޽����ڽ� ���

    //[SerializeField]
    //bool[] preSetting = new bool[btnMax];    // ȯ�漳�� ���

    //[SerializeField]
    //bool[] preItemshop = new bool[btnMax];   // ������ ���� ���

    //[SerializeField]
    //bool[] preLuckyBox = new bool[btnMax];   // ��Ű �ڽ� ���

    //[SerializeField]
    //bool[] preShortCut = new bool[btnMax];   // ���� ���� ���

    //[SerializeField]
    //bool[] preUniqueBlock = new bool[btnMax];   // ����ũ ��� ���

    //[SerializeField]
    //bool[] preEvent = new bool[btnMax];   // �̺�Ʈ ���


    // Start is called before the first frame update
    void Start()
    {
        // ������ �ʱ�ȭ
        ruleList.Clear();

        // ������ ���
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

        // ��Ȱ��
        PopUp(Type.Close);
    }

    // Update is called once per frame
    void Update()
    {
        //tempSet();   // �׽�Ʈ �۵���
    }


    //void tempSet()
    //{
    //    if (runBtn == true)
    //    {
    //        Debug.Log("�޽��� �ڽ� �۵� ����");
    //        if (runID == -1)
    //        {
    //            close();
    //            Debug.Log("�޽��� �ڽ� ����");
    //        }
    //        else if (runID >= mBoxPreset.Count)
    //        {
    //            close();
    //            Debug.Log("Out of range -> �޽��� �ڽ� ����");
    //        }
    //        else
    //        {
    //            runBtn = false;
    //            // ��ȣ�� �´� ������ popUp()
    //            PopUp(  pageSwitch.GetObject(runID).GetComponent<MessageBoxRule>(), runID   );
    //            Debug.Log("�޽��� �ڽ� ���� : " + runID);
    //        }
    //    }
    //}
    // �׽�Ʈ�� ��


    /// <summary>
    /// �����ִ� �޽��� �ڽ� ��Ȱ��ȭ
    /// </summary>
    public void Close()
    {
        // �޽��� �ڽ� GameObject ��Ȱ��
        gameObject.SetActive(false);

        // ������ �ڽ� ��Ȱ��
        //if(GameMaster.script.itemManager.itemUseBox.gameObject.activeSelf)
        //    GameMaster.script.itemManager.CloseItemUseBox();
            GameMaster.script.itemManager.ResetItemUseBox();
    }


    void open()
    {
        // �޽��� �ڽ� GameObject Ȱ��
        gameObject.SetActive(true);
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="btnPreset"></param>
    ///// <param name="pageNum"></param>
    //void addPreset(bool[] btnPreset, int pageNum)
    //{
    //    // ��ư ��� ���� üũ
    //    if (!btnSwitch.checkCountObject(btnPreset.Length))
    //        return;

    //    // ������ ���� üũ
    //    if (pageNum > pageSwitch.count())
    //    {
    //        Debug.Log("error :: page over (Max:" + pageSwitch.count() + ") -> " + pageNum);
    //        return;
    //    }

    //    // ������ �� ������ �ڵ� ����
    //    if (pageSwitch.GetObject(pageNum).GetComponent<MessageBoxRule>() == null)
    //        pageSwitch.GetObject(pageNum).AddComponent<MessageBoxRule>();

    //    mBoxPreset.Add(new int[] { btnSwitch.countPreset(), pageNum });
    //    btnSwitch.addPreset(btnPreset);
    //    Debug.Log("Add : " + pageNum);
    //}

    //void callPreset(int Num)
    //{
    //    // ������ ��ȣ�� ����Ͽ� �� ��ư ������ ����

    //    // ���� �ȵ� ������ ��û�� �ߴ�
    //    if (Num >= mBoxPreset.Count)
    //    {
    //        Debug.Log("error :: Out of Range (0~" + (mBoxPreset.Count-1) + ") -> " + Num);
    //        return;
    //    }

    //    Debug.Log("test :: [" + mBoxPreset[Num][0] + "] [" + mBoxPreset[Num][1] + "]");

    //    // ��ư ����
    //    btnSwitch.setUp(mBoxPreset[Num][0]);

    //    // ������ ����
    //    pageSwitch.setUp(mBoxPreset[Num][1]);

    //    // ������Ʈ Ȱ��ȭ
    //    open();
    //}
    void callPreset(int ruleListIndex)
    {
        // ������ ��ȣ�� ����Ͽ� �� ��ư ������ ����

        // ���� �ȵ� ������ ��û�� �ߴ�
        if (ruleListIndex >= ruleList.Count)
        {
            Debug.Log("error :: Out of Range (0~" + (ruleList.Count - 1) + ") -> " + ruleListIndex);
            return;
        }

        // ��ư ����
        BtnSetUp(ruleList[ruleListIndex]);

        // ������ ����
        pageSwitch.setUp(ruleListIndex);

        // ������Ʈ Ȱ��ȭ
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
    /// �޽��� �ڽ��� �������� ����Ͽ� ���
    /// </summary>
    /// <param name="isBoxUse">�ٱ� box ����</param>
    /// <param name="isInnerBoxUse">���� box ����</param>
    /// <param name="isNpcSpaceUse">���� �ϴ� NPC�� ��ư ���� Ȯ��</param>
    /// <param name="mBoxPresetNum">�޽��� �ڽ� ������ ��ȣ</param>
    public void PopUp(bool isBoxUse, bool isInnerBoxUse, bool isNpcSpaceUse, int ruleListIndex)
    {
        // �������̸� ��� �޽��� �ڽ� �ݱ�
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
        // �޽��� �ڽ� �ʱ�ȭ �� �ݱ�
        PopUp(Type.Close);

        // ���� ����
        //BlockWork.isEnd = true;
        BlockWork.isEnd = ruleList[pageSwitch.now].blockWorkEndWithClose;
    }

}
