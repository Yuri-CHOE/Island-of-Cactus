using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.MapEditor
{
    public class GroundEditor : MonoBehaviour
    {
        [Header("Ground Script")]
        [SerializeField]
        GroundManager groundManager;

        [Header("UI")]
        [SerializeField]
        Dropdown category;
        [SerializeField]
        ButtonFlag btnAdd;
        [SerializeField]
        ButtonFlag btnRefresh;
        [SerializeField]
        ButtonFlag btnDelete;
        [SerializeField]
        ButtonFlag btnCode;
        [SerializeField]
        ButtonFlag btnBuild;




        [Header("Build")]
        [SerializeField]
        string buildInput;
        [TextArea()]
        [SerializeField]
        string buildOutput;



        // Start is called before the first frame update
        void Start()
        {
            SetDropdown();
        }

        // Update is called once per frame
        void Update()
        {
            CheckBtn();

        }



        /// <summary>
        /// ��з� ��Ӵٿ� ���ΰ�ħ
        /// </summary>
        void SetDropdown()
        {
            // �ʱ�ȭ
            category.options.Clear();

            // ��� �߰�
            for (int i = 0; i < groundManager.groundList.Count; i++)
                category.options.Add(
                    new Dropdown.OptionData(
                        groundManager.groundList[i].name.ToString(),
                        null)
                    );

            // �⺻�� ����
            category.value = 0;


            // ���ΰ�ħ
            category.RefreshShownValue();

        }
        

        /// <summary>
        /// ��ư UI �Է� üũ ���
        /// </summary>
        void CheckBtn()
        {
            // + ��ư (������Ʈ �߰�)
            if (btnAdd.getFlag())
            {
                Debug.Log("���� ��û��");
                btnAdd.setOff();

                groundManager.Create(
                    category.value,
                    Vector3.zero,
                    Vector3.zero,
                    Vector3.one
                    );
            }

            // R ��ư (���ΰ�ħ)
            if (btnRefresh.getFlag())
            {
                btnRefresh.setOff();
            }

            // D ��ư (����)
            if (btnDelete.getFlag())
            {
                btnDelete.setOff();
            }

            // C ��ư (�ڵ�(string)ȭ)
            if (btnCode.getFlag())
            {
                btnCode.setOff();
                buildOutput = groundManager.GetBuildCode();
            }

            // B ��ư (�ڵ�(string)�� ����)
            if (btnBuild.getFlag())
            {
                btnBuild.setOff();
                groundManager.BuildByString(buildInput);
            }

        }
    }
}