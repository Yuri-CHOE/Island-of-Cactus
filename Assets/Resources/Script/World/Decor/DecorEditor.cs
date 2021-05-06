using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.MapEditor
{
    public class DecorEditor : MonoBehaviour
    {
        [Header("Decor Script")]
        [SerializeField]
        DecorManager decorManager;
        

        [Header("UI")]
        [SerializeField]
        Dropdown category1;
        [SerializeField]
        Dropdown category2;
        [SerializeField]
        ButtonFlag btnAdd;
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


        [Header("Build")]
        [SerializeField]
        string buildInput;
        [TextArea()]
        [SerializeField]
        string buildOutput;

        public List<GameObject> test;

        // Start is called before the first frame update
        void Start()
        {
            SetDropdown();
        }

        // Update is called once per frame
        void Update()
        {
            RefreshTarget(Tool.Targeting());
            //RefreshDropdown();

            CheckBtn();
        }



        /// <summary>
        /// 대분류 드롭다운 새로고침
        /// </summary>
        void SetDropdown()
        {
            // 초기화
            category1.options.Clear();

            // 목록 추가
            for (int i = 1; i < DecorType.GetTypeCount(); i++)
                category1.options.Add(
                    new Dropdown.OptionData(
                        ((DecorType.Type)i).ToString(),
                        null)
                    );

            // 기본값 지정
            category1.value = 0;

            // 새로고침
            category1.RefreshShownValue();

            SetDropdown((DecorType.Type)(category1.value + 1));
        }


        /// <summary>
        /// 소분류 드롭다운 새로고침
        /// </summary>
        void SetDropdown(DecorType.Type dt)
        {
            // 초기화
            category2.options.Clear();

            // 목록 추가
            for (int i = 0; i < decorManager.GetDecorList(dt).Count; i++)
                category2.options.Add(
                    new Dropdown.OptionData(
                        decorManager.GetDecorList(dt)[i].name,
                        null)
                    );

            // 새로고침
            category2.RefreshShownValue();
        }


        /// <summary>
        /// 드롭다운 갱신
        /// </summary>
        public void RefreshDropdown()
        {
            // 소분류 갱신
            //if (category2.options[0].text != decorManager.GetDecorList(category1.value + 1)[0].name)
            //    SetDropdown((DecorType.Type)(category1.value + 1));
            SetDropdown((DecorType.Type)(category1.value + 1));
        }


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
            if (decorManager.CheckObjectList(obj))
                target = obj;
            else
                target = null;


            // 텍스트 변경
            if (target == null)
            {
                targetName.text = "is not decor";
                return;
            }
            else
                targetName.text = target.name;
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

                float size = Random.Range(1.50f, 2.01f);

                decorManager.Create(
                    category1.value + 1,
                    category2.value,
                    new Vector3(
                        Camera.main.transform.position.x,
                        0f,
                        Camera.main.transform.position.z + 41f),
                    new Vector3(
                        Random.Range(-5.0f, 5.1f),
                        Random.Range(0, 360),
                        Random.Range(-5.0f, 5.1f)),
                    new Vector3(
                        size * Random.Range(0.90f, 1.21f),
                        size * Random.Range(0.90f, 1.21f),
                        size * Random.Range(0.90f, 1.21f))
                    ).gameObject.AddComponent<CapsuleCollider>();
            }

            // R 버튼 (새로고침)
            if (btnRefresh.getFlag())
            {
                btnRefresh.setOff();
                decorManager.RefreshAllObject();
            }

            // D 버튼 (삭제)
            if (btnDelete.getFlag())
            {
                btnDelete.setOff();
                decorManager.DeleteObject(target);
                decorManager.RefreshAllObject();
                target = null;
            }

            // C 버튼 (코드(string)화)
            if (btnCode.getFlag())
            {
                btnCode.setOff();
                buildOutput = decorManager.GetBuildCode();
            }

            // B 버튼 (코드(string)로 빌드)
            if (btnBuild.getFlag())
            {
                btnBuild.setOff();
                decorManager.BuildByString(buildInput);
            }

        }

        public GameObject GetTarget()
        {
            return target;
        }
    }
}