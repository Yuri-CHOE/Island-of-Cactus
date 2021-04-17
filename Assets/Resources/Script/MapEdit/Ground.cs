using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.MapEditor;
using UnityEngine.EventSystems;


namespace UnityEngine.MapEditor
{
    [AddComponentMenu("Custom/MapEditor/Ground", 0)]
    public class Ground : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> prefab = new List<GameObject>();    // 반복 생성할 프리팹

        [SerializeField]
        int prefabSelect;          // 반복 생성할 프리팹 번호

        [SerializeField]
        Vector3 loopCount;          // 방향 별 반복 횟수

        [SerializeField]
        Vector3 startPoint;         // 생성 기준점

        [SerializeField]
        Vector3 intervalSize;       // 생성 간격


        void Start()
        {
            prefab[2].GetComponent<GroundEditor>().Init();
            Destroy(prefab[2].GetComponent<GroundEditor>());
            CreateGround();
        }


        // 맵 배치 기능
        void CreateTile(Transform obj, int h, int v, int d)
        {
            Vector3 v3 = new Vector3(
                startPoint.x + (intervalSize.x * obj.lossyScale.x * h),
                startPoint.y + (intervalSize.y * obj.lossyScale.y * v),
                startPoint.z + (intervalSize.z * obj.lossyScale.z * d)
                );
            //obj.localEulerAngles = new Vector3(
            //    obj.localEulerAngles.x,
            //    90 * Random.Range(1, 5),
            //    obj.localEulerAngles.z
            //    );
            Instantiate(obj.gameObject, v3, obj.rotation)
                .transform.SetParent(transform);

            Debug.Log(string.Format("create :: {0}, {1}, {2}", v3.x, v3.y, v3.z));
        }

        void CreateGround()
        {
            if (loopCount.y <= 0)
                loopCount = new Vector3(loopCount.x, 1, loopCount.z);

            for (int yy = 0; yy < loopCount.y; yy++)
            {
                for (int zz = 0; zz < loopCount.z; zz++)
                {
                    for (int xx = 0; xx < loopCount.x; xx++)
                    {
                        CreateTile(prefab[prefabSelect].transform, xx, yy, zz);
                    }
                }
            }

            // 목록용 오브젝트 제거
            for (int i = 0; i < prefab.Count; i++)
                Destroy(prefab[i]);
        }

    }

}

