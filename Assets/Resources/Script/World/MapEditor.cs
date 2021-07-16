using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace UnityEngine.MapEditor
{
    public class MapEditor : MonoBehaviour
    {
        // 월드 오브젝트
        public Transform world;


        [Header("ManagerScript")]
        [SerializeField]
        WorldManager wm;
        [SerializeField]
        CameraController cm;
        [SerializeField]
        GroundManager gm;
        [SerializeField]
        BlockManager bm;
        [SerializeField]
        DecorManager dm;


        [Header("EditorScript")]
        [SerializeField]
        GroundEditor groundEditor;
        [SerializeField]
        BlockEditor blockEditor;
        [SerializeField]
        DecorEditor decorEditor;



        [Header("Build")]
        [SerializeField]
        string buildInput;
        [TextArea()]
        [SerializeField]
        string buildOutput;
        [SerializeField]
        InputField fileName;
        string emptyText = "File name is empty";




        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }




        ///// <summary>
        ///// 클릭한 오브젝트 가져오기
        ///// </summary>
        //public static GameObject Targeting()
        //{
        //    // 클릭 체크
        //    if (!Input.GetMouseButtonUp(0))
        //        return null;

        //    // UI 클릭 예외처리
        //    if (EventSystem.current.currentSelectedGameObject != null)
        //        return null;


        //    RaycastHit hit = new RaycastHit();
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    GameObject clickObj = null;

        //    if (Physics.Raycast(ray.origin, ray.direction, out hit))
        //    {
        //        clickObj = hit.transform.gameObject;
        //        Debug.Log(clickObj.name);
        //    }

        //    return clickObj;
        //}


        public void CheckFileName()
        {
            CheckFileNameEmpty();
        }

        bool CheckFileNameEmpty()
        {
            // 잘못된 파일명 필터링
            fileName.text = fileName.text.Trim(System.IO.Path.GetInvalidFileNameChars());

            // 파일명 미입력 또는 공백만 있을 경우
            if (fileName.text == null || fileName.text == "" || fileName.text == " ")
            {
                fileName.text = emptyText;
                return false;
            }
            else
                return true;
        }


        /// <summary>
        /// 코드를 출력하여 지정된 파일명으로 저장
        /// </summary>
        public void MapFileSave()
        {
            // 파일명 미입력시 중단
            if (!CheckFileNameEmpty())
                return;

            // 파일명 오류 문자일 경우 중단            
            if (fileName.text == emptyText)
                return;

            // 코드 갱신
            GetBuildCode();

            // 저장
            CSVReader.SaveNew(WorldManager.subRoot, fileName.text + WorldManager.extension, false, false, buildOutput );
        }


        /// <summary>
        /// buildOutput 에 코드 출력
        /// </summary>
        void GetBuildCode()
        {
            StringBuilder sb = new StringBuilder();

            sb
                // 카메라 한계선 정보
                .Append(cm.camLimitPos1.x)
                .Append(',')
                .Append(cm.camLimitPos1.y)
                .Append(',')
                .Append(cm.camLimitPos1.z)
                .Append('|')
                .Append(cm.camLimitPos2.x)
                .Append(',')
                .Append(cm.camLimitPos2.y)
                .Append(',')
                .Append(cm.camLimitPos2.z)
                .Append('$')

                // 스타트 블록 정보
                .Append(bm.startBlock.position.x)           
                .Append(',')
                .Append(bm.startBlock.position.y)
                .Append(',')
                .Append(bm.startBlock.position.z)
                .Append(',')
                .Append(bm.startBlock.rotation.eulerAngles.x)
                .Append(',')
                .Append(bm.startBlock.rotation.eulerAngles.y)
                .Append(',')
                .Append(bm.startBlock.rotation.eulerAngles.z)
                .Append(',')
                .Append(bm.startBlock.localScale.x)
                .Append(',')
                .Append(bm.startBlock.localScale.y)
                .Append(',')
                .Append(bm.startBlock.localScale.z)
                .Append('$')

                // 지형 코드
                .Append(gm.GetBuildCode())
                .Append('$')
                // 블록 코드
                .Append(bm.GetBuildCode())
                .Append('$')

                // 장식물 코드
                .Append(dm.GetBuildCode())
                .Append(WorldManager.tableSplit)

                // 종료 문자
                .Append(WorldManager.endSplit)
                ;

            buildOutput = sb.ToString();
        }
    }

}