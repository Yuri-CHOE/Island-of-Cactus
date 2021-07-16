using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace UnityEngine.MapEditor
{
    public class MapEditor : MonoBehaviour
    {
        // ���� ������Ʈ
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
        ///// Ŭ���� ������Ʈ ��������
        ///// </summary>
        //public static GameObject Targeting()
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


        public void CheckFileName()
        {
            CheckFileNameEmpty();
        }

        bool CheckFileNameEmpty()
        {
            // �߸��� ���ϸ� ���͸�
            fileName.text = fileName.text.Trim(System.IO.Path.GetInvalidFileNameChars());

            // ���ϸ� ���Է� �Ǵ� ���鸸 ���� ���
            if (fileName.text == null || fileName.text == "" || fileName.text == " ")
            {
                fileName.text = emptyText;
                return false;
            }
            else
                return true;
        }


        /// <summary>
        /// �ڵ带 ����Ͽ� ������ ���ϸ����� ����
        /// </summary>
        public void MapFileSave()
        {
            // ���ϸ� ���Է½� �ߴ�
            if (!CheckFileNameEmpty())
                return;

            // ���ϸ� ���� ������ ��� �ߴ�            
            if (fileName.text == emptyText)
                return;

            // �ڵ� ����
            GetBuildCode();

            // ����
            CSVReader.SaveNew(WorldManager.subRoot, fileName.text + WorldManager.extension, false, false, buildOutput );
        }


        /// <summary>
        /// buildOutput �� �ڵ� ���
        /// </summary>
        void GetBuildCode()
        {
            StringBuilder sb = new StringBuilder();

            sb
                // ī�޶� �Ѱ輱 ����
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

                // ��ŸƮ ��� ����
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

                // ���� �ڵ�
                .Append(gm.GetBuildCode())
                .Append('$')
                // ��� �ڵ�
                .Append(bm.GetBuildCode())
                .Append('$')

                // ��Ĺ� �ڵ�
                .Append(dm.GetBuildCode())
                .Append(WorldManager.tableSplit)

                // ���� ����
                .Append(WorldManager.endSplit)
                ;

            buildOutput = sb.ToString();
        }
    }

}