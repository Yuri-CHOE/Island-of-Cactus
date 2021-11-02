using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class InfinityScroll : MonoBehaviour
    {
        // ����
        public ScrollRect scroll = null;
        //public RectTransform viewPort = null;
        //public RectTransform content = null;

        // ����
        public List<InfinityScrollValue> unit = new List<InfinityScrollValue>();
        [SerializeField] float unitPadding = 10f;
        float unitHeight = 0f;

        // ��ü ������
        //[SerializeField] List<string> dataValue = new List<string>();

        [SerializeField] int top = 0;
        int bottom { get { return top + unitCount; } }

        public int count = 0;
        public int unitCount = 0;

        private void Awake()
        {
            // �������� ����
            unitHeight = unit[0].rectTransform.rect.height;

            // ������ ���� ���
            count = IocReference.table.Count - 1;
            unitCount = unit.Count -1;

            // ���� �¾�
            for (int i = 1; i < unit.Count; i++)
            {
                // ������ ������ ��� ��Ȱ��
                if (i >= IocReference.table.Count)
                {
                    unit[i].gameObject.SetActive(false);
                    continue;
                }

                // ���� �ε��� ����
                unit[i].SetIndex(i);

                // ������ ����
                unit[i].Setup(IocReference.table[i], i);
            }

            // ���� ũ�� �缳��
            float trueHeight = unitHeight * count + unitPadding * count;
            scroll.content.sizeDelta = new Vector2(
                scroll.content.rect.width,
                trueHeight
                );

            Debug.Log("���� :: ���� ���� �缳�� -> " + trueHeight);

            // ���۽� ��Ȱ��
            transform.parent.gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public float GetPosY(int _dataIndex)
        {
            float y = unit[0].transform.localPosition.y - (unitHeight + unitPadding ) * (_dataIndex - 1);

            return y;
        }

        public void ReCalc()
        {
            try
            {
                // �ֻ�� �ε���
                // n = content.position.y / (unitHeight + unitPadding)
                top = (int)(scroll.content.localPosition.y / (unitHeight + unitPadding));


                // ���� ����
                for (int i = 1; i < unit.Count; i++)
                {
                    // �Ʒ���
                    if (unit[i].dataIndex < top)
                    {
                        if (unit[i].dataIndex + unitCount <= count)
                        {
                            unit[i].dataIndex += unitCount;
                            unit[i].RePos();
                            unit[i].Refresh();
                        }
                    }
                    // ����
                    else if (unit[i].dataIndex > bottom)
                    {
                        if (unit[i].dataIndex - unitCount > 0)
                        {
                            unit[i].dataIndex -= unitCount;
                            unit[i].RePos();
                            unit[i].Refresh();
                        }
                    }
                }
            }
            catch { }
        }
    }
}
