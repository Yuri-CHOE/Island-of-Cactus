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
        /// ������ ������Ʈ ���� ��ȯ
        /// </summary>
        public int count()
        {
            return objects.Count;
        }

        /// <summary>
        /// ��ϵ� ������ ���� ��ȯ
        /// </summary>
        public int countPreset()
        {
            return (preset.Count);
        }


        /// <summary>
        /// ������ ����Ʈ �ʱ�ȭ
        /// </summary>
        public void clear()
        {
            preset.Clear();
        }

        /// <summary>
        /// �Է��� ������Ʈ ��ȣ�� ��ȿ��
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
        /// �Է��� ������ ��ȣ�� ��ȿ��
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
        /// ���ο� ������ �߰� (��Ȯ�� ������Ʈ ������ �迭 �ʿ�)
        /// </summary>
        public void addPreset(bool[] bArr)
        {
            if (checkCountObject(bArr.Length))
                preset.Add(bArr);
            else
                Debug.Log("fail :: preset add");

        }


        /// <summary>
        /// ������ ����
        /// </summary>
        public void setUp(int number)
        {
            if (!checkCountObject(preset[number].Length))
                return;

            if (!checkCountPreset(number))
                return;

            // ������Ʈ �� On, Off ����
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].SetActive(preset[number][i]);
                Debug.Log("objects :: " + i + " -> " + objects[i].activeSelf.ToString() + " = " + preset[number][i].ToString());
            }

            Debug.Log("Excute :: Button list setup -> " + number);
        }


    }
}



