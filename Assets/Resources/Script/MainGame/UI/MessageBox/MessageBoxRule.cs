using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace UnityEngine.UI.MessageBox
{
    public class MessageBoxRule : UIBehaviour
    {
        public bool isBoxUse = true;
        public bool isInnerBoxUse = true;
        public bool isNpcSpaceUse = false;

        public bool blockWorkEndWithClose = false;

        [Header("empty")]
        //public Button btnEmpty = null;
        public bool useEmpty = false;

        [Header("nameChange")]
        //public Button btnNameChange = null;
        public bool useNameChange = false;

        [Header("language")]
        //public Button btnLanguage = null;
        public bool useLanguage = false;

        [Header("title")]
        //public Button btnTitle = null;
        public bool useTitle = false;

        [Header("buy")]
        //public Button btnBuy = null;
        public bool useBuy = false;

        [Header("use")]
        //public Button btnUse = null;
        public bool useUse = false;

        [Header("close")]
        //public Button btnClose = null;
        public bool useClose = true;


    }
}
