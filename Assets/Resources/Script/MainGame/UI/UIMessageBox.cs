using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageBox : MonoBehaviour
{
    public Image m_box;
    public GameObject Innerbox;

    public ObjectMultiSwitch btnSwitch;
    public ObjectSwitch papeSwitch;

    [SerializeField]
    int activeNum = 0;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    void close()
    {
        // �����ִ� �޽��� �ڽ� ��Ȱ��ȭ
    }

    // ��ȣ�������Ѽ� �� ��ư ������ ����� �Լ� �ʿ�
    // �̱���

    void buttonSetting(int activeNum)
    {
        // ������ ��ȣ�� ����Ͽ� �� ��ư ������ ����
    }
       
    public void popUp(bool isBoxUse, bool isInnerBoxUse, int activeNum)
    {
        m_box.enabled = isBoxUse;
        Innerbox.SetActive(isInnerBoxUse);
        buttonSetting(activeNum);
    }
    public void popUp(bool isInnerBoxUse, int activeNum)
    {
        popUp(true, isInnerBoxUse, activeNum);
    }
    public void popUp(int activeNum)
    {
        popUp(true, true, activeNum);
    }
}
