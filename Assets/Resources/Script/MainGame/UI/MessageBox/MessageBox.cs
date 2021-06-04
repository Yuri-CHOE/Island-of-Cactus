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
    bool[] preMessage = new bool[btnMax];    // �޽����ڽ� ���

    [SerializeField]
    bool[] preSetting = new bool[btnMax];    // ȯ�漳�� ���

    [SerializeField]
    bool[] preItemshop = new bool[btnMax];   // ������ ���� ���

    [SerializeField]
    bool[] preLuckyBox = new bool[btnMax];   // ��Ű �ڽ� ���


    // Start is called before the first frame update
    void Start()
    {
        // �޽��� �ڽ� ������ ���
        addPreset(preMessage, 0);
        addPreset(preSetting, 1);
        addPreset(preItemshop, 2);
        addPreset(preLuckyBox, 3);
    }

    // Update is called once per frame
    void Update()
    {
        tempSet();   // �׽�Ʈ �۵���
    }


    // �׽�Ʈ�� ����
    [SerializeField]
    bool runBtn = false;
    [SerializeField]
    int runID = 0;
    void tempSet()
    {
        if (runBtn == true)
        {
            Debug.Log("�޽��� �ڽ� �۵� ����");
            if (runID == -1)
            {
                close();
                Debug.Log("�޽��� �ڽ� ����");
            }
            else if (runID >= mBoxPreset.Count)
            {
                close();
                Debug.Log("Out of range -> �޽��� �ڽ� ����");
            }
            else
            {
                runBtn = false;
                // ��ȣ�� �´� ������ popUp()
                PopUp(  pageSwitch.GetObject(runID).GetComponent<MessageBoxRule>(), runID   );
                Debug.Log("�޽��� �ڽ� ���� : " + runID);
            }
        }
    }
    // �׽�Ʈ�� ��


    /// <summary>
    /// �����ִ� �޽��� �ڽ� ��Ȱ��ȭ
    /// </summary>
    public void close()
    {
        // �޽��� �ڽ� GameObject ��Ȱ��
        gameObject.SetActive(false);
    }


    void open()
    {
        // �޽��� �ڽ� GameObject Ȱ��
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="btnPreset"></param>
    /// <param name="pageNum"></param>
    void addPreset(bool[] btnPreset, int pageNum)
    {
        // ��ư ��� ���� üũ
        if (!btnSwitch.checkCountObject(btnPreset.Length))
            return;

        // ������ ���� üũ
        if (pageNum > pageSwitch.count())
        {
            Debug.Log("error :: page over (Max:" + pageSwitch.count() + ") -> " + pageNum);
            return;
        }

        // ������ �� ������ �ڵ� ����
        if (pageSwitch.GetObject(pageNum).GetComponent<MessageBoxRule>() == null)
            pageSwitch.GetObject(pageNum).AddComponent<MessageBoxRule>();

        mBoxPreset.Add(new int[] { btnSwitch.countPreset(), pageNum });
        btnSwitch.addPreset(btnPreset);
        Debug.Log("Add : " + pageNum);
    }

    void callPreset(int Num)
    {
        // ������ ��ȣ�� ����Ͽ� �� ��ư ������ ����

        // ���� �ȵ� ������ ��û�� �ߴ�
        if (Num >= mBoxPreset.Count)
        {
            Debug.Log("error :: Out of Range (0~" + (mBoxPreset.Count-1) + ") -> " + Num);
            return;
        }

        Debug.Log("test :: [" + mBoxPreset[Num][0] + "] [" + mBoxPreset[Num][1] + "]");

        // ��ư ����
        btnSwitch.setUp(mBoxPreset[Num][0]);

        // ������ ����
        pageSwitch.setUp(mBoxPreset[Num][1]);

        // ������Ʈ Ȱ��ȭ
        open();
    }

    /// <summary>
    /// �޽��� �ڽ��� �������� ����Ͽ� ���
    /// </summary>
    /// <param name="isBoxUse">�ٱ� box ����</param>
    /// <param name="isInnerBoxUse">���� box ����</param>
    /// <param name="isNpcSpaceUse">���� �ϴ� NPC�� ��ư ���� Ȯ��</param>
    /// <param name="mBoxPresetNum">�޽��� �ڽ� ������ ��ȣ</param>
    public void PopUp(bool isBoxUse, bool isInnerBoxUse, bool isNpcSpaceUse, int mBoxPresetNum)
    {
        // �������̸� ��� �޽��� �ڽ� �ݱ�
        if (mBoxPresetNum < 0)
        {
            close();
            return;
        }

        // �׽�Ʈ ��Ȱ��
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
