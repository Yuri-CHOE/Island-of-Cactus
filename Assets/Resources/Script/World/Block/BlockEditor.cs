using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace UnityEngine.MapEditor
{
    public class BlockEditor : MonoBehaviour
    {
        [Header("Block Script")]
        [SerializeField]
        BlockManager blockManager;

        [Header("CrossKey")]
        [SerializeField]
        DirectionKey dkCopy;
        [SerializeField]
        DirectionKey dkMove;


        [Header("UI")]
        [SerializeField]
        Text targetName;
        [SerializeField]
        ButtonFlag btnRefresh;
        [SerializeField]
        ButtonFlag btnDelete;
        [SerializeField]
        ButtonFlag btnCode;
        [SerializeField]
        ButtonFlag btnBuild;

        GameObject target;


        [Header("Option")]
        [Tooltip("Distance of block to block")]
        [SerializeField]
        Vector3 gridSpace;


        [Header("Build")]
        [SerializeField]
        string buildInput;
        [TextArea()]
        [SerializeField]
        string buildOutput;



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            RefreshTarget(Tool.Targeting());
            BtnCopyOnOFF();
            MoveCamera(dkMove.GetDirection());
            Copy(dkCopy.GetDirection(), BlockType.TypeDetail.plus);
            CheckBtn();
        }


        /// <summary>
        /// 클릭한 오브젝트 가져오기
        /// </summary>
        //GameObject Targeting()
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


        /// <summary>
        /// 타겟 정보 새로고침
        /// </summary>
        /// <param name="obj">클릭한 블록 오브젝트</param>
        void RefreshTarget(GameObject obj)
        {
            // 오브젝트가 아닐 경우
            if (obj == null)
                return;

            // 대상이 블록이 아닐경우
            if (blockManager.CheckObjectList(obj))
                target = obj;
            else
                target = null;


            // 텍스트 변경
            if (target == null)
            {
                targetName.text = "is not block";
                return;
            }
            else
                targetName.text = target.name;
        }


        /// <summary>
        /// 방향 지정 복사)
        /// </summary>
        /// <param name="direction">결과물 복사 방향</param>
        /// <param name="td">결과물 블록 타입</param>
        void Copy(MoveDirection direction, BlockType.TypeDetail td)
        {
            if (direction == MoveDirection.None)
                return;

            Tool.Targeting();

            // 복사 대상이 없을 경우
            if (target == null)
                return;

            // 위치 및 회전값 설정
            Vector3 pos = target.transform.position;

            if (direction == MoveDirection.Left)
                pos.x -= gridSpace.x;
            else if (direction == MoveDirection.Right)
                pos.x += gridSpace.x;
            else if (direction == MoveDirection.Up)
                pos.z += gridSpace.z;
            else if (direction == MoveDirection.Down)
                pos.z -= gridSpace.z;


            // 타겟 방향 재설정
            int yRot = (int)direction - 1;
            target.GetComponent<DynamicBlock>().ReDirection(yRot);

            // 타겟 재설정 및 생성
            target = blockManager.Create(pos, yRot + 2, td).gameObject;

            // 네임박스 재설정
            targetName.text = target.name;

            // 카메라 이동
            MoveCamera(direction);
            MoveCamera(direction);
        }

        /// <summary>
        /// 일정 방향만큼 카메라 이동
        /// </summary>
        /// <param name="direction">카메라 이동 방향</param>
        void MoveCamera(MoveDirection direction)
        {
            if (direction == MoveDirection.None)
                return;

            if (direction == MoveDirection.Left)
            {
                Camera.main.transform.position = new Vector3(
                    Camera.main.transform.position.x - (gridSpace.x / 2),
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z
                    );
            }
            else if (direction == MoveDirection.Right)
            {
                Camera.main.transform.position = new Vector3(
                    Camera.main.transform.position.x + (gridSpace.x / 2),
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z
                    );
            }
            else if (direction == MoveDirection.Up)
            {
                Camera.main.transform.position = new Vector3(
                    Camera.main.transform.position.x,
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z + (gridSpace.z / 2)
                    );
            }
            else if (direction == MoveDirection.Down)
            {
                Camera.main.transform.position = new Vector3(
                    Camera.main.transform.position.x,
                    Camera.main.transform.position.y,
                    Camera.main.transform.position.z - (gridSpace.z / 2)
                    );
            }
        }


        /// <summary>
        /// 복사버튼 활성화 체크
        /// </summary>
        void BtnCopyOnOFF()
        {
            if (target == null)
                dkCopy.gameObject.SetActive(false);
            else
                dkCopy.gameObject.SetActive(true);
        }


        /// <summary>
        /// 버튼 UI 입력 체크 기능
        /// </summary>
        void CheckBtn()
        {
            // R 버튼 (새로고침)
            if (btnRefresh.getFlag())
            {
                btnRefresh.setOff();
                blockManager.RefreshAllObject();
            }

            // D 버튼 (삭제)
            if (btnDelete.getFlag())
            {
                btnDelete.setOff();
                blockManager.DeleteObject(target);
                target = null;
            }

            // C 버튼 (코드(string)화)
            if (btnCode.getFlag())
            {
                btnCode.setOff();
                buildOutput = blockManager.GetBuildCode();
            }

            // B 버튼 (코드(string)로 빌드)
            if (btnBuild.getFlag())
            {
                btnBuild.setOff();
                blockManager.BuildByString(buildInput);
            }

        }

        public GameObject GetTarget()
        {
            return target;
        }
    }
}