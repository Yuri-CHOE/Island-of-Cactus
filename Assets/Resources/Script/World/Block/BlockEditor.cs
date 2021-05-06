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
        /// Ŭ���� ������Ʈ ��������
        /// </summary>
        //GameObject Targeting()
        //{
        //    // Ŭ�� üũ
        //    if (!Input.GetMouseButtonUp(0))
        //        return null;

        //    // UI Ŭ�� ����ó��
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
        /// Ÿ�� ���� ���ΰ�ħ
        /// </summary>
        /// <param name="obj">Ŭ���� ��� ������Ʈ</param>
        void RefreshTarget(GameObject obj)
        {
            // ������Ʈ�� �ƴ� ���
            if (obj == null)
                return;

            // ����� ����� �ƴҰ��
            if (blockManager.CheckObjectList(obj))
                target = obj;
            else
                target = null;


            // �ؽ�Ʈ ����
            if (target == null)
            {
                targetName.text = "is not block";
                return;
            }
            else
                targetName.text = target.name;
        }


        /// <summary>
        /// ���� ���� ����)
        /// </summary>
        /// <param name="direction">����� ���� ����</param>
        /// <param name="td">����� ��� Ÿ��</param>
        void Copy(MoveDirection direction, BlockType.TypeDetail td)
        {
            if (direction == MoveDirection.None)
                return;

            Tool.Targeting();

            // ���� ����� ���� ���
            if (target == null)
                return;

            // ��ġ �� ȸ���� ����
            Vector3 pos = target.transform.position;

            if (direction == MoveDirection.Left)
                pos.x -= gridSpace.x;
            else if (direction == MoveDirection.Right)
                pos.x += gridSpace.x;
            else if (direction == MoveDirection.Up)
                pos.z += gridSpace.z;
            else if (direction == MoveDirection.Down)
                pos.z -= gridSpace.z;


            // Ÿ�� ���� �缳��
            int yRot = (int)direction - 1;
            target.GetComponent<DynamicBlock>().ReDirection(yRot);

            // Ÿ�� �缳�� �� ����
            target = blockManager.Create(pos, yRot + 2, td).gameObject;

            // ���ӹڽ� �缳��
            targetName.text = target.name;

            // ī�޶� �̵�
            MoveCamera(direction);
            MoveCamera(direction);
        }

        /// <summary>
        /// ���� ���⸸ŭ ī�޶� �̵�
        /// </summary>
        /// <param name="direction">ī�޶� �̵� ����</param>
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
        /// �����ư Ȱ��ȭ üũ
        /// </summary>
        void BtnCopyOnOFF()
        {
            if (target == null)
                dkCopy.gameObject.SetActive(false);
            else
                dkCopy.gameObject.SetActive(true);
        }


        /// <summary>
        /// ��ư UI �Է� üũ ���
        /// </summary>
        void CheckBtn()
        {
            // R ��ư (���ΰ�ħ)
            if (btnRefresh.getFlag())
            {
                btnRefresh.setOff();
                blockManager.RefreshAllObject();
            }

            // D ��ư (����)
            if (btnDelete.getFlag())
            {
                btnDelete.setOff();
                blockManager.DeleteObject(target);
                target = null;
            }

            // C ��ư (�ڵ�(string)ȭ)
            if (btnCode.getFlag())
            {
                btnCode.setOff();
                buildOutput = blockManager.GetBuildCode();
            }

            // B ��ư (�ڵ�(string)�� ����)
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