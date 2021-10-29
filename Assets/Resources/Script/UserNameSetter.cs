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
    //    // 저장된 유저명 정보 있으면 비활성화
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
        // 이름 저장
        UserData.userName = inputField.text;
    }
}
