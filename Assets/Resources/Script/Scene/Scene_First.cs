using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_First : MonoBehaviour
{
    // ���� �� �̸�
    [SerializeField]
    string nextScene = "Title";

    // ��ġ ���� �ؽ�Ʈ
    [SerializeField]
    Text text;

    // �ȵ���̵� ���� üũ��
    [SerializeField] PermissionChecker permissionChecker = null;
    GameObject dialog = null;

    [SerializeField]
    bool colorSwitch;

    AsyncOperation ao;

    bool isLoadFinish = false;
    bool isTableReady = false;
    
    [SerializeField]
    List<Sprite> iconTable = new List<Sprite>();

    [Header("Data Table")]
    [SerializeField] UnityEditor.DefaultAsset userData = null;
    [SerializeField] TextAsset userDataT = null;

    void Awake()
    {
        //CSVReader.basicPath = Application.dataPath + "/Resources/Data";
        //CSVReader.copyPath = Application.persistentDataPath + "/Data";

        // ������ ĳ��
        Item.iconTable = iconTable;

        //// �ȵ���̵� �¾�
        //if(Application.platform == RuntimePlatform.Android)
        //{
        //    // ����� �б� ����
        //    if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.ExternalStorageRead))
        //    {
        //        UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.ExternalStorageRead);
        //    }

        //    // ����� ���� ����
        //    if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.ExternalStorageWrite))
        //    {
        //        UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.ExternalStorageWrite);
        //    }
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        // ȯ�漳�� �ε�
        Preferences.Load();

        // ��� �ε� ����
        ao = SceneManager.LoadSceneAsync(nextScene);

        // �ڵ� �Ѿ ����
        ao.allowSceneActivation = false;

        // �¾� - �۹̼�, ���̺�
        StartCoroutine(SetUp());

        //// ���̺� �ε�
        //StartCoroutine(TableLoad());
        //Item.SetUp();
        //Character.SetUp();
        //text.text = "wait...";
    }

    // Update is called once per frame
    void Update()
    {
        // �ε� ���� üũ
        if (ao.progress >= 0.9f)
        {
            isLoadFinish = true;

            // ���̺� ���� üũ
            if (isTableReady)
            {
                text.text = "Touch Anywhere";
            }
        }

        //if (CustomInput.GetPoint())
        //{
        //    text.text = string.Format("isTableReady={0}, �ε�={1}", isTableReady, ao.progress);

        //    Debug.LogError(string.Format("touchCount={0}, pos={1}", 
        //        UnityEngine.EventSystems.EventSystem.current.currentInputModule.input.touchCount, 
        //        UnityEngine.EventSystems.EventSystem.current.currentInputModule.input.GetTouch(0).position.ToString()
        //        ));
        //}
    }


    public void Clicked()
    {
        // �ѱ��
        if (isTableReady)
            if (isLoadFinish)
            {
                ao.allowSceneActivation = true;

                // �߷� ����
                Physics.gravity = Physics.gravity * 10;
            }

        //try
        //{
        //    //text.text = string.Format("isTableReady={0}, �ε�={1}", isTableReady, ao.progress);
        //    Debug.LogError(string.Format("isTableReady={0}, �ε�={1}, ����={2}", isTableReady, ao.progress, (ao.progress >= 0.9f).ToString() ));
        //    Debug.LogError(string.Format("���̺� :: ����={0} ĳ����={1}, ������={2}, �̺�Ʈ={3}, ��Ű�ڽ�={4}, ����ũ={5}, �̴�={6}", (UserData.file!=null), Character.table.Count, Item.table.Count, IocEvent.table.Count, LuckyBox.table.Count, Unique.table.Count, Minigame.table.Count));

        //    Debug.LogError(string.Format("permissionR={0}, permissionW={1}", 
        //        UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.ExternalStorageRead),
        //        UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.ExternalStorageWrite)
        //        ));
        //}
        //catch { }

    }
    
    IEnumerator SetUp()
    {
        yield return PermissionCheck();

        yield return TableLoad();
    }

    IEnumerator PermissionCheck()
    {
        Debug.Log("���� :: �б� ���� Ȯ����");
        while (!PermissionChecker.canRead)
        {
            // ���� ���������� ȣ��
            if (!permissionChecker.gameObject.activeSelf)
                permissionChecker.CallReadPermissionNotice();

            yield return null;
        }

        Debug.Log("���� :: ���� ���� Ȯ����");
        while (!PermissionChecker.canWrite)
        {
            // ���� ���������� ȣ��
            if (!permissionChecker.gameObject.activeSelf)
                permissionChecker.CallWritePermissionNotice();

            yield return null;
        }

        Debug.Log("���� :: ��� �ʿ� ���� Ȯ�ε�");
    }

    IEnumerator TableLoad()
    {
        Debug.Log("���̺� :: ��ü ������ ���̺� �¾�");

        // ���� ������ �ε�
        // = �� ���еǴ� ��ⵥ���Ͱ��/User/UserData.iocdata ������ ���纻(true)���� ���� ����(false)�ϰ� �о��
        UserData.file = new CSVReader("User", "UserData.iocdata", true, false, '=');
        UserData.SetUp();
        yield return null;

        // ĳ���� ���̺�
        Character.SetUp();
        yield return null;

        // ������ ���̺�
        Item.SetUp();
        yield return null;

        // �̺�Ʈ ���̺�
        IocEvent.SetUp();
        yield return null;

        // ��Ű�ڽ� ���̺�
        LuckyBox.SetUp();
        yield return null;

        // ����ũ ���̺�
        Unique.SetUp();
        yield return null;

        // �̴ϰ��� ���̺�
        Minigame.SetUp();
        yield return null;

        // �Ϸ� ó��
        isTableReady = true;
    }
}
