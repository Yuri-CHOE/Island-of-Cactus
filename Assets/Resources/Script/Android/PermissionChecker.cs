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

    // 대화상자
    public UnityEngine.UI.Text notice = null;

    // 타겟 권한
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
        //// 안드로이드 셋업
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    // 저장소 읽기 권한
        //    if (!canRead))
        //    {
        //        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        //    }

        //    // 저장소 쓰기 권한
        //    if (!canWrite))
        //    {
        //        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        //    }
        //}

    }

    public void CallReadPermissionNotice()
    {
        Debug.LogWarning("안드로이드 :: 읽기 권한 필요");

        // 텍스트 표시
        notice.text = "저장소 읽기 권한 필요";

        // 셋팅
        targetPermission = PermissionType.Read;
        //Permission.RequestUserPermission(Permission.ExternalStorageRead);

        // 호출
        gameObject.SetActive(true);
    }

    public void CallWritePermissionNotice()
    {
        Debug.LogWarning("안드로이드 :: 쓰기 권한 필요");

        // 텍스트 표시
        notice.text = "저장소 쓰기 권한 필요";

        // 셋팅
        targetPermission = PermissionType.Write;
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);

        // 호출
        gameObject.SetActive(true);
    }

    public void BtnApply()
    {
        if(targetPermission == PermissionType.Read)
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            Debug.LogWarning("안드로이드 :: 읽기 권한 요청됨");
        }
        else if (targetPermission == PermissionType.Write)
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            Debug.LogWarning("안드로이드 :: 쓰기 권한 요청됨");
        }
    }
}
