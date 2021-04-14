using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace UnityEngine.UI
{    
    [AddComponentMenu("UI/Custom/ObjectMultiSwitch", 0)]
    [RequireComponent(typeof(RectTransform))]

    public class ObjectMultiSwitch : UIBehaviour
    {
        [SerializeField]
        List<GameObject> objects;

        List<bool[]> preset = new List<bool[]>();
        

        /// <summary>
        /// 제어할 오브젝트 수량 반환
        /// </summary>
        public int count()
        {
            return objects.Count;
        }

        /// <summary>
        /// 등록된 프리셋 수량 반환
        /// </summary>
        public int countPreset()
        {
            return (preset.Count);
        }


        /// <summary>
        /// 프리셋 리스트 초기화
        /// </summary>
        public void clear()
        {
            preset.Clear();
        }

        /// <summary>
        /// 입력한 오브젝트 번호의 유효성
        /// </summary>
        public bool checkCountObject(int num)
        {
            if (num != objects.Count || num < 0)
            {
                Debug.Log("Error :: Out of Range ( button : 0~" + (objects.Count - 1) + ") -> input : " + num);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 입력한 프리셋 번호의 유효성
        /// </summary>
        public bool checkCountPreset(int num)
        {
            if (num >= preset.Count || num < 0)
            {
                Debug.Log("Error :: Out of Range ( button : 0~" + (preset.Count - 1) + ") -> input : " + num);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 새로운 프리셋 추가 (정확한 오브젝트 수량의 배열 필요)
        /// </summary>
        public void addPreset(bool[] bArr)
        {
            if (checkCountObject(bArr.Length))
                preset.Add(bArr);
            else
                Debug.Log("fail :: preset add");

        }


        /// <summary>
        /// 프리셋 적용
        /// </summary>
        public void setUp(int number)
        {
            if (!checkCountObject(preset[number].Length))
                return;

            if (!checkCountPreset(number))
                return;

            // 오브젝트 별 On, Off 설정
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].SetActive(preset[number][i]);
                Debug.Log("objects :: " + i + " -> " + objects[i].activeSelf.ToString() + " = " + preset[number][i].ToString());
            }

            Debug.Log("Excute :: Button list setup -> " + number);
        }


    }
}



