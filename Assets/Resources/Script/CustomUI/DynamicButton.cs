using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DynamicButton : UIBehaviour
{
    public enum ButtonList
    {
        back,
        setting,
        bgm,
        effectSound,
    }

    public ButtonList btnType = ButtonList.back;
    public Image BackgroundImg;

    [SerializeField]
    GameObject iconBack;
    [SerializeField]
    GameObject iconSetting;
    [SerializeField]
    GameObject iconBGM;
    [SerializeField]
    GameObject iconEffect;

    // Start is called before the first frame update
    protected override void Start()
    {
        btnActivate();
    }

    void btnActivate()
    {
        switch (btnType)
        {
            case ButtonList.back:
                //Destroy(iconBack);
                Destroy(iconSetting);
                Destroy(iconBGM);
                Destroy(iconEffect);                
                break;

            case ButtonList.setting:
                Destroy(iconBack);
                //Destroy(iconSetting);
                Destroy(iconBGM);
                Destroy(iconEffect);
                break;

            case ButtonList.bgm:
                Destroy(iconBack);
                Destroy(iconSetting);
                //Destroy(iconBGM);
                Destroy(iconEffect);
                break;

            case ButtonList.effectSound:
                Destroy(iconBack);
                Destroy(iconSetting);
                Destroy(iconBGM);
                //Destroy(iconEffect);
                break;
        }
    }
}
