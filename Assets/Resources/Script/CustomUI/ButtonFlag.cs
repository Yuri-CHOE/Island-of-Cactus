using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace UnityEngine.UI
{
    // No need for Refresh() when using Set()
    // The first operation must be manually Refresh()

    [AddComponentMenu("UI/Custom/ButtonFlag", 0)]
    [RequireComponent(typeof(RectTransform))]
    public class ButtonFlag : UIBehaviour
    {
        // 버튼 작동 스크립트 만들기 전까지 사용할 임시 스크립트

        [SerializeField]
        bool isOn = false;      // 활성 or 비활성
        bool Mirror = false;    // 스크립트 수행 여부


        /// <summary>
        /// 엔진의 OnClick()에 등록할 함수
        /// 활성화 상태: 반전, 스크립트 수행: 안됨 상태로 변경
        /// </summary>
        public void Clicked()
        {
            setToggle();
        }

        /// <summary>
        /// 활성화 상태: 반전, 스크립트 수행: 안됨 상태로 변경
        /// </summary>
        public void setToggle()
        {
            isOn = !isOn;
            MirrorSync(false);
        }


        /// <summary>
        /// 활성화 상태: 활성, 스크립트 수행: 안됨 상태로 변경
        /// </summary>
        public void setOn( )
        {
            isOn = true;
            MirrorSync(false);
        }


        /// <summary>
        /// 활성화 상태: 비활성, 스크립트 수행: 안됨 상태로 변경
        /// </summary>
        public void setOff()
        {
            isOn = false;
            MirrorSync(false);
        }
        

        /// <summary>
        /// 스크립트 수행 상태 제어 (true=수행됨)
        /// </summary>
        public void MirrorSync(bool isMirrorSync)
        {
            if (isMirrorSync)
                Mirror = isOn;
            else
                Mirror = !isOn;
        }


        /// <summary>
        /// 스크립트 수행여부 리턴
        /// </summary>
        public bool isRun()
        {
            if(isOn == Mirror)
                return true;
            else
                return false;
        }


        /// <summary>
        /// 활성화 상태 리턴
        /// </summary>
        public bool getFlag()
        {
            return isOn;
        }
    }

}