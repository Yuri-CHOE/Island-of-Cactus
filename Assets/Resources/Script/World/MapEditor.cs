using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace UnityEngine.MapEditor
{
    public class MapEditor
    {


        /// <summary>
        /// 클릭한 오브젝트 가져오기
        /// </summary>
        public static GameObject Targeting()
        {
            // 클릭 체크
            if (!Input.GetMouseButtonUp(0))
                return null;

            // UI 클릭 예외처리
            if (EventSystem.current.currentSelectedGameObject != null)
                return null;


            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            GameObject clickObj = null;

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                clickObj = hit.transform.gameObject;
                Debug.Log(clickObj.name);
            }

            return clickObj;
        }

    }

}