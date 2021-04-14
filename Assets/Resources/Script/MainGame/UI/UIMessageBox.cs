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
        // 열려있는 메시지 박스 비활성화
    }

    // 번호연동시켜서 각 버튼 프리셋 만드는 함수 필요
    // 미구현

    void buttonSetting(int activeNum)
    {
        // 지정된 번호를 사용하여 각 버튼 프리셋 제어
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
