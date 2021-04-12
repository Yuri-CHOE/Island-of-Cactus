using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    // No need for Refresh() when using Set()
    // The first operation must be manually Refresh()

    [AddComponentMenu("UI/Custom/ObjectSwitch", 0)]
    [RequireComponent(typeof(RectTransform))]
    public class ObjectSwitch : UIBehaviour
    {
        [Tooltip("Is the active switch number")]
        [SerializeField]
        private int ActiveSwitch = 0;

        public GameObject[] targetObjects;


        protected override void Start()
        {
            Set(ActiveSwitch);
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
            for (int i = 0; i < targetObjects.Length; i++)
                turnOff(targetObjects[i]);
        }
        
        void checkOutOfRange()
        {
            if (ActiveSwitch > targetObjects.Length && ActiveSwitch < 0)
            {
                ActiveSwitch = 0;
                Debug.Log(string.Format("error :: out of range(\"{0}\" from 0~{1})", ActiveSwitch.ToString(), targetObjects.Length.ToString()));
                return;
            }
        }


        /// <summary>
        /// Do refresh
        /// </summary>
        public void Refresh()
        {
            checkOutOfRange();
            turnOffAll();
            turnOn(targetObjects[ActiveSwitch]);
        }


        /// <summary>
        /// Turn off all switches
        /// </summary>
        public void Shutdown()
        {
            turnOffAll();
        }


        /// <summary>
        /// Do only "gmaeobject of switchNumber" active
        /// </summary>
        public void Set(int switchNumber)
        {
            if (switchNumber < 0)
            {
                turnOffAll();
                return;
            }
            else
                ActiveSwitch = switchNumber;

            checkOutOfRange();
            Refresh();
        }

        /// <summary>
        /// Return active switch number
        /// </summary>
        public int Get()
        {
            return ActiveSwitch;
        }

        /// <summary>
        /// Return activate gmaeobject
        /// </summary>
        public GameObject GetObject()
        {
            return targetObjects[ActiveSwitch];
        }
    }
}
