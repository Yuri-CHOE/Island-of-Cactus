using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserNameSetter : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.InputField inputField = null;
    [SerializeField]
    UnityEngine.UI.Button btnApply = null;

    bool isReady { get { return inputField.text.Length > 0; } }


    //void Awake()
    //{
    //    // ����� ������ ���� ������ ��Ȱ��ȭ
    //    if (PlayerPrefs.HasKey(UserData.userNameKey))
    //        gameObject.SetActive(false);
    //}

    void Update()
    {

    }

    public void RefreshBtnApply()
    {
        if (isReady)
            btnApply.interactable = true;
        else
            btnApply.interactable = false;
    }


    public void BtnApply()
    {
        // �̸� ����
        UserData.userName = inputField.text;
    }
}
