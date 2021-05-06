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
        /// ��з� ��Ӵٿ� ���ΰ�ħ
        /// </summary>
        void SetDropdown()
        {
            // �ʱ�ȭ
            category1.options.Clear();

            // ��� �߰�
            for (int i = 1; i < DecorType.GetTypeCount(); i++)
                category1.options.Add(
                    new Dropdown.OptionData(
                        ((DecorType.Type)i).ToString(),
                        null)
                    );

            // �⺻�� ����
            category1.value = 0;

            // ���ΰ�ħ
            category1.RefreshShownValue();

            SetDropdown((DecorType.Type)(category1.value + 1));
        }


        /// <summary>
        /// �Һз� ��Ӵٿ� ���ΰ�ħ
        /// </summary>
        void SetDropdown(DecorType.Type dt)
        {
            // �ʱ�ȭ
            category2.options.Clear();

            // ��� �߰�
            for (int i = 0; i < decorManager.GetDecorList(dt).Count; i++)
                category2.options.Add(
                    new Dropdown.OptionData(
                        decorManager.GetDecorList(dt)[i].name,
                        null)
                    );

            // ���ΰ�ħ
            category2.RefreshShownValue();
        }


        /// <summary>
        /// ��Ӵٿ� ����
        /// </summary>
        public void RefreshDropdown()
        {
            // �Һз� ����
            //if (category2.options[0].text != decorManager.GetDecorList(category1.value + 1)[0].name)
            //    SetDropdown((DecorType.Type)(category1.value + 1));
            SetDropdown((DecorType.Type)(category1.value + 1));
        }


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
            if (decorManager.CheckObjectList(obj))
                target = obj;
            else
                target = null;


            // �ؽ�Ʈ ����
            if (target == null)
            {
                targetName.text = "is not decor";
                return;
            }
            else
                targetName.text = target.name;
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

            // R ��ư (���ΰ�ħ)
            if (btnRefresh.getFlag())
            {
                btnRefresh.setOff();
                decorManager.RefreshAllObject();
            }

            // D ��ư (����)
            if (btnDelete.getFlag())
            {
                btnDelete.setOff();
                decorManager.DeleteObject(target);
                decorManager.RefreshAllObject();
                target = null;
            }

            // C ��ư (�ڵ�(string)ȭ)
            if (btnCode.getFlag())
            {
                btnCode.setOff();
                buildOutput = decorManager.GetBuildCode();
            }

            // B ��ư (�ڵ�(string)�� ����)
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