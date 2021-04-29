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
        /// Ŭ���� ������Ʈ ��������
        /// </summary>
        public static GameObject Targeting()
        {
            // Ŭ�� üũ
            if (!Input.GetMouseButtonUp(0))
                return null;

            // UI Ŭ�� ����ó��
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