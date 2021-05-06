using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace UnityEngine.UI
{
    // No need for Refresh() when using Set()
    // The first operation must be manually Refresh()

    [AddComponentMenu("UI/Custom/ObjectSwitch", 0)]
    [RequireComponent(typeof(RectTransform))]
    public class ObjectSwitch : UIBehaviour
    {
        [SerializeField]
        int activeSwitchNum = 0;
        int activeSwitchNumMirror = 0;

        [SerializeField]
        List<GameObject> objectList;


        // Start is called before the first frame update
        protected override void Start()
        {
            Refresh();
        }

        void Update()
        {
            if (activeSwitchNum != activeSwitchNumMirror)
                Refresh();
        }



        void turnOn(GameObject obj)
        {
            if (obj.activeSelf != true)
                obj.SetActive(true);
        }

        void turnOff(GameObject obj)
        {
            if (obj.activeSelf != false)
                obj.SetActive(false);
        }

        void turnOffAll()
        {
            for (int i = 0; i < objectList.Count; i++)
                objectList[i].SetActive(false);
        }


        /// <summary>
        /// Turn off all switches
        /// </summary>
        public void Shutdown()
        {
            turnOffAll();
        }

        void checkOutOfRange()
        {
            if (activeSwitchNum >= objectList.Count && activeSwitchNum < 0)
            {
                Debug.Log("Error :: Out of range (0~" + (objectList.Count - 1).ToString() + ") -> " + activeSwitchNum.ToString());
                activeSwitchNum = 0;
            }
        }

        bool checkOutOfRange(int objectNumber)
        {
            if (objectNumber >= objectList.Count && objectNumber < 0)
            {
                Debug.Log("Error :: Out of range (0~" + (objectList.Count - 1).ToString() + ") -> " + objectNumber.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// Do refresh
        /// </summary>
        public void Refresh()
        {
            checkOutOfRange();
            turnOffAll();

            if (activeSwitchNum < 0)
                activeSwitchNum = 0;

            turnOn(objectList[activeSwitchNum]);

            activeSwitchNum = activeSwitchNumMirror;
        }

        public int count()
        {
            return objectList.Count;
        }

        /// <summary>
        /// Return activate gmaeobject
        /// </summary>
        public GameObject GetObject()
        {
            return objectList[activeSwitchNum];
        }

        /// <summary>
        /// Return gmaeobject by number
        /// </summary>
        public GameObject GetObject(int objectNumber)
        {
            if (!checkOutOfRange(objectNumber))
                return null;
            return objectList[objectNumber];
        }

        /// <summary>
        /// Return active switch number
        /// </summary>
        public int Get()
        {
            return activeSwitchNum;
        }

        /// <summary>
        /// Do only "gmaeobject of switchNumber" active
        /// </summary>
        public void setUp(int switchNum)
        {
            if (switchNum >= objectList.Count)
                return;

            activeSwitchNum = switchNum;
            Refresh();
        }
    }

}