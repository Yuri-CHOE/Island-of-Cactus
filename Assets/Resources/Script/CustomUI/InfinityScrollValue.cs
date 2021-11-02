using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class InfinityScrollValue : MonoBehaviour
    {
        public InfinityScroll mother = null;

        public RectTransform rectTransform = null;

        public Text type = null;
        public Text name = null;
        public string url = null;

        int _index = 0;
        public int index { get { return _index; } /*set { _index = value; RePos(); }*/ }

        public int dataIndex = 0;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetIndex(int __index)
        {
            _index = __index;
        }
        public void Setup(IocReference data, int _dataIndex)
        {
            dataIndex = _dataIndex;

            Setup(data);
        }
        public void Setup(IocReference data)
        {
            Setup(data.type.ToString(), data.name, data.url);
        }
        void Setup(string _type, string _name, string _url)
        {
            type.text = _type;
            name.text = _name;
            url = _url;
        }

        public void WebLink()
        {
            if (url == null)
                return;

            Tool.WebLink(url);
        }

        public void RePos()
        {
            rectTransform.localPosition = new Vector3(
                rectTransform.localPosition.x,
                mother.GetPosY(dataIndex),
                0
                );
        }

        public void Refresh()
        {
            Setup(IocReference.table[dataIndex]);
        }


    }
}
