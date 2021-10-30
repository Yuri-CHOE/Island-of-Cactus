using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Android;

public class PermissionChecker : MonoBehaviour
{
    public enum PermissionType
    {
        None,
        Read,
        Write,
    }

    //public static bool canRead = false;
    public static bool canRead { get { return Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead); } }
    //public static bool canWrite = false;
    public static bool canWrite { get { return Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite); } }

    // ��ȭ����
    public UnityEngine.UI.Text notice = null;

    // Ÿ�� ����
    PermissionType targetPermission = PermissionType.None;
        

    void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //// �ȵ���̵� �¾�
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    // ����� �б� ����
        //    if (!canRead))
        //    {
        //        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        //    }

        //    // ����� ���� ����
        //    if (!canWrite))
        //    {
        //        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        //    }
        //}

    }

    public void CallReadPermissionNotice()
    {
        Debug.LogWarning("�ȵ���̵� :: �б� ���� �ʿ�");

        // �ؽ�Ʈ ǥ��
        notice.text = "����� �б� ���� �ʿ�";

        // ����
        targetPermission = PermissionType.Read;
        //Permission.RequestUserPermission(Permission.ExternalStorageRead);

        // ȣ��
        gameObject.SetActive(true);
    }

    public void CallWritePermissionNotice()
    {
        Debug.LogWarning("�ȵ���̵� :: ���� ���� �ʿ�");

        // �ؽ�Ʈ ǥ��
        notice.text = "����� ���� ���� �ʿ�";

        // ����
        targetPermission = PermissionType.Write;
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);

        // ȣ��
        gameObject.SetActive(true);
    }

    public void BtnApply()
    {
        if(targetPermission == PermissionType.Read)
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            Debug.LogWarning("�ȵ���̵� :: �б� ���� ��û��");
        }
        else if (targetPermission == PermissionType.Write)
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            Debug.LogWarning("�ȵ���̵� :: ���� ���� ��û��");
        }
    }
}
