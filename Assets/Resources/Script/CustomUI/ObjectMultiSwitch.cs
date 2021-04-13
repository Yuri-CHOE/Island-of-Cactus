using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace UnityEngine.UI
{
    // No need for Refresh() when using Set()
    // The first operation must be manually Refresh()

    [AddComponentMenu("UI/Custom/ObjectMultiSwitch", 0)]
    [RequireComponent(typeof(RectTransform))]

    public class ObjectMultiSwitch : UIBehaviour
    {
        [SerializeField]
        List<GameObject> btn;

        List<string> btnSetName = new List<string>();
        List<bool[]> btnSet = new List<bool[]>();


        // Start is called before the first frame update
        protected override void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void addBtnSet(string name, bool[] bArr)
        {
            if (bArr.Length != btn.Count || bArr.Length < 0)
            {
                Debug.Log("Error :: Out of Range ( button : 0~" + (btn.Count - 1) + ") -> Array : " + (bArr.Length - 1));
                return;
            }

            btnSetName.Add(name);
            btnSet.Add(bArr);
        }


        public void setUp(int number)
        {
            if (number >= btnSet.Count || number < 0)
            {
                Debug.Log("Error :: Out of Range ( button set Lixt : 0~" + (btnSet.Count - 1) + ") -> Array : " + number);
                return;
            }

            if(btn.Count < btnSet[number].Length)
            {
                Debug.Log("Error :: Out of Range ( button : 0~" + (btn.Count - 1) + ") -> Array : " + (btnSet[number].Length - 1));
                return;
            }

            for (int i = 0; i < btn.Count; i++)
            {
                btn[i].SetActive(btnSet[number][i]);
            }

            Debug.Log("Excute :: Button list setup -> [" + number + "]" + btnSetName[number]);
        }


        public void setUp(string name)
        {
            for (int i = 0; i < btnSetName.Count; i++)
            {
                if(btnSetName[i] == name)
                {
                    setUp(i);
                    return;
                }
            }
            
        }


    }
}



