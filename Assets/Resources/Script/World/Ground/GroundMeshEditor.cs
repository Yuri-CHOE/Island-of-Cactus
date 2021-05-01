using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UnityEngine.MapEditor
{
    [AddComponentMenu("Custom/MapEditor/GroundMeshEditor", 0)]
    public class GroundMeshEditor : MonoBehaviour
    {
        // 인게임에서 3D 지형 모델링 생성하는 스크립트


        // 제작용
        [SerializeField]
        int x, z;
        [SerializeField]
        bool isActive = false;
        [SerializeField]
        bool isReset = false;



        [SerializeField]
        MeshFilter meshFilter;

        [SerializeField]
        MeshRenderer meshRenderer;



        public List<Vector3> meshPoint;

        [SerializeField]
        List<Vector3> meshTriangles;

        List<int> triangles = new List<int>();






        void Start()
        {
            //QuickBuild(x, z);
            //QuickTriangles(x, z);
        }

        void Update()
        {
            //QuickSide(x,z);
            //CreateMesh();
        }

        public void Init()
        {
            QuickBuild(x, z);
            QuickTriangles(x, z);
            QuickSide(x, z);
            CreateMesh();

            Destroy(transform.GetComponent<GroundMeshEditor>());
        }


        // xz바둑판, y = 0 으로 초기화
        void QuickBuild(int xx, int zz)
        {
            if (!isReset)
                return;

            meshPoint.Clear();


            for (int zzz = -zz / 2; zzz < zz / 2 + 1; zzz++)
            {
                for (int xxx = -xx / 2; xxx < xx / 2 + 1; xxx++)
                {
                    meshPoint.Add(new Vector3(xxx, 0, zzz));
                    //meshPoint[zzz * x + xxx] = new Vector3(xxx, 0, zzz);
                    Debug.Log("좌표설정 = " + xxx + ", " + "0, " + zzz);
                }
            }
        }

        void QuickSide(int xx, int zz)
        {
            if (xx < 0)
                return;
            if (zz < 0)
                return;

            for (int xxx = 0; xxx < xx; xxx++)
            {
                if(meshPoint[xxx].x != meshPoint[xxx + xx * (zz - 1)].x || meshPoint[xxx].z != meshPoint[xxx + xx * (zz - 1)].z)
                    meshPoint[xxx + xx * (zz - 1)] = new Vector3(
                        meshPoint[xxx + xx * (zz - 1)].x, 
                        meshPoint[xxx].y, 
                        meshPoint[xxx + xx * (zz - 1)].z
                        );
            }

            for (int zzz = 0; zzz < zz; zzz++)
            {
                if (meshPoint[zzz*xx].x != meshPoint[zzz * xx + (xx-1)].x || meshPoint[zzz * xx].z != meshPoint[zzz * xx + (xx - 1)].z)
                    meshPoint[zzz * xx + (xx - 1)] = new Vector3(
                        meshPoint[zzz * xx + (xx - 1)].x, 
                        meshPoint[zzz * xx].y, 
                        meshPoint[zzz * xx + (xx - 1)].z
                        );
            }
        }

        void CreateMesh()
        {
            if (!isActive)
                return;

            Mesh myMesh = new Mesh();

            myMesh.vertices = meshPoint.ToArray();

            ConvertTriangles();
            myMesh.triangles = triangles.ToArray();

            meshFilter.mesh = myMesh;

            gameObject.AddComponent<MeshCollider>().convex = true;

            isActive = false;
        }

        void ConvertTriangles()
        {
            for(int i = 0; i < meshTriangles.Count; i++)
            {
                Debug.Log("삼각형 = " + (int)meshTriangles[i].x + ", " + meshTriangles[i].y + ", " + meshTriangles[i].z);

                triangles.Add((int)meshTriangles[i].x);
                triangles.Add((int)meshTriangles[i].y);
                triangles.Add((int)meshTriangles[i].z);
                Debug.Log("삼각형 = end");
            }
        }


        // 자동 삼각형 분할
        void QuickTriangles(int xx, int zz)
        {
            meshTriangles.Clear();

            bool type = true;

            for (int zzz = 0; zzz < zz-1; zzz++)
            {
                for (int xxx = 0; xxx < xx - 1; xxx++)
                {
                    if (type)
                    {
                        meshTriangles.Add(new Vector3(
                            (zzz * xx + xxx),
                            (zzz * xx + xxx) + xx,
                            (zzz * xx + xxx) + 1
                            ));
                        meshTriangles.Add(new Vector3(
                            (zzz * xx + xxx) + 1,
                            (zzz * xx + xxx) + xx,
                            (zzz * xx + xxx) + xx + 1
                            ));
                    }
                    else
                    {
                        meshTriangles.Add(new Vector3(
                            (zzz * xx + xxx),
                            (zzz * xx + xxx) + xx,
                            (zzz * xx + xxx) + xx + 1
                            ));
                        meshTriangles.Add(new Vector3(
                            (zzz * xx + xxx) + xx + 1,
                            (zzz * xx + xxx) + 1,
                            (zzz * xx + xxx)
                            ));
                    }
                    type = !type;
                }
                type = !type;
            }
        }

    }

}



