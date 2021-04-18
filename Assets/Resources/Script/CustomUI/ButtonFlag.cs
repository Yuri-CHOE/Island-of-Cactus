using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace UnityEngine.UI
{
    // No need for Refresh() when using Set()
    // The first operation must be manually Refresh()

    [AddComponentMenu("UI/Custom/ButtonFlag", 0)]
    [RequireComponent(typeof(RectTransform))]
    public class ButtonFlag : UIBehaviour
    {
        // ��ư �۵� ��ũ��Ʈ ����� ������ ����� �ӽ� ��ũ��Ʈ

        [SerializeField]
        bool isOn = false;      // Ȱ�� or ��Ȱ��
        bool Mirror = false;    // ��ũ��Ʈ ���� ����


        /// <summary>
        /// ������ OnClick()�� ����� �Լ�
        /// Ȱ��ȭ ����: ����, ��ũ��Ʈ ����: �ȵ� ���·� ����
        /// </summary>
        public void Clicked()
        {
            setToggle();
        }

        /// <summary>
        /// Ȱ��ȭ ����: ����, ��ũ��Ʈ ����: �ȵ� ���·� ����
        /// </summary>
        public void setToggle()
        {
            isOn = !isOn;
            MirrorSync(false);
        }


        /// <summary>
        /// Ȱ��ȭ ����: Ȱ��, ��ũ��Ʈ ����: �ȵ� ���·� ����
        /// </summary>
        public void setOn( )
        {
            isOn = true;
            MirrorSync(false);
        }


        /// <summary>
        /// Ȱ��ȭ ����: ��Ȱ��, ��ũ��Ʈ ����: �ȵ� ���·� ����
        /// </summary>
        public void setOff()
        {
            isOn = false;
            MirrorSync(false);
        }
        

        /// <summary>
        /// ��ũ��Ʈ ���� ���� ���� (true=�����)
        /// </summary>
        public void MirrorSync(bool isMirrorSync)
        {
            if (isMirrorSync)
                Mirror = isOn;
            else
                Mirror = !isOn;
        }


        /// <summary>
        /// ��ũ��Ʈ ���࿩�� ����
        /// </summary>
        public bool isRun()
        {
            if(isOn == Mirror)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Ȱ��ȭ ���� ����
        /// </summary>
        public bool getFlag()
        {
            return isOn;
        }
    }

}