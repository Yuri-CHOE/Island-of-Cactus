using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class InfinityScroll : MonoBehaviour
    {
        // 영역
        public ScrollRect scroll = null;
        //public RectTransform viewPort = null;
        //public RectTransform content = null;

        // 유닛
        public List<InfinityScrollValue> unit = new List<InfinityScrollValue>();
        [SerializeField] float unitPadding = 10f;
        float unitHeight = 0f;

        // 전체 데이터
        //[SerializeField] List<string> dataValue = new List<string>();

        [SerializeField] int top = 0;
        int bottom { get { return top + unitCount; } }

        public int count = 0;
        public int unitCount = 0;

        private void Awake()
        {
            // 단위높이 지정
            unitHeight = unit[0].rectTransform.rect.height;

            // 데이터 수량 등록
            count = IocReference.table.Count - 1;
            unitCount = unit.Count -1;

            // 유닛 셋업
            for (int i = 1; i < unit.Count; i++)
            {
                // 데이터 부족할 경우 비활성
                if (i >= IocReference.table.Count)
                {
                    unit[i].gameObject.SetActive(false);
                    continue;
                }

                // 고유 인덱스 셋팅
                unit[i].SetIndex(i);

                // 데이터 셋팅
                unit[i].Setup(IocReference.table[i], i);
            }

            // 영역 크기 재설정
            float trueHeight = unitHeight * count + unitPadding * count;
            scroll.content.sizeDelta = new Vector2(
                scroll.content.rect.width,
                trueHeight
                );

            Debug.Log("참조 :: 영역 높이 재설정 -> " + trueHeight);

            // 시작시 비활성
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
                // 최상단 인덱스
                // n = content.position.y / (unitHeight + unitPadding)
                top = (int)(scroll.content.localPosition.y / (unitHeight + unitPadding));


                // 유닛 셋팅
                for (int i = 1; i < unit.Count; i++)
                {
                    // 아래로
                    if (unit[i].dataIndex < top)
                    {
                        if (unit[i].dataIndex + unitCount <= count)
                        {
                            unit[i].dataIndex += unitCount;
                            unit[i].RePos();
                            unit[i].Refresh();
                        }
                    }
                    // 위로
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
