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
        /// 대분류 드롭다운 새로고침
        /// </summary>
        void SetDropdown()
        {
            // 초기화
            category.options.Clear();

            // 목록 추가
            for (int i = 0; i < groundManager.groundList.Count; i++)
                category.options.Add(
                    new Dropdown.OptionData(
                        groundManager.groundList[i].name.ToString(),
                        null)
                    );

            // 기본값 지정
            category.value = 0;


            // 새로고침
            category.RefreshShownValue();

        }
        

        /// <summary>
        /// 버튼 UI 입력 체크 기능
        /// </summary>
        void CheckBtn()
        {
            // + 버튼 (오브젝트 추가)
            if (btnAdd.getFlag())
            {
                Debug.Log("생성 요청함");
                btnAdd.setOff();

                groundManager.Create(
                    category.value,
                    Vector3.zero,
                    Vector3.zero,
                    Vector3.one
                    );
            }

            // R 버튼 (새로고침)
            if (btnRefresh.getFlag())
            {
                btnRefresh.setOff();
            }

            // D 버튼 (삭제)
            if (btnDelete.getFlag())
            {
                btnDelete.setOff();
            }

            // C 버튼 (코드(string)화)
            if (btnCode.getFlag())
            {
                btnCode.setOff();
                buildOutput = groundManager.GetBuildCode();
            }

            // B 버튼 (코드(string)로 빌드)
            if (btnBuild.getFlag())
            {
                btnBuild.setOff();
                groundManager.BuildByString(buildInput);
            }

        }
    }
}