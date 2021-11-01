using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UnityEditor;

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

    // ������ ��ϱ�
    [SerializeField] UserNameSetter userNameSetter = null;

    [SerializeField]
    bool colorSwitch;

    AsyncOperation ao;

    bool isLoadFinish = false;
    bool isTableReady = false;
    
    [SerializeField]
    List<Sprite> iconTable = new List<Sprite>();


    [Header("Data Table")]
    [SerializeField] TextAsset charData = null;
    [SerializeField] TextAsset charDataLocal = null;

    [SerializeField] TextAsset itemData = null;
    [SerializeField] TextAsset itemDataLocal = null;

    [SerializeField] TextAsset eventData = null;
    [SerializeField] TextAsset eventDataLocal = null;

    [SerializeField] TextAsset luckyData = null;
    [SerializeField] TextAsset luckyDataLocal = null;

    [SerializeField] TextAsset uniqueData = null;
    [SerializeField] TextAsset uniqueDataLocal = null;

    [SerializeField] TextAsset minigameData = null;
    [SerializeField] TextAsset minigameDataLocal = null;

    [SerializeField] List<TextAsset> mapFile = new List<TextAsset>();
    


    void Awake()
    {
        // ������ ĳ��
        Item.iconTable = iconTable;

        // ������ ��ũ
        WorldManager.worldAsset = mapFile;
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
    }
    
    IEnumerator SetUp()
    {
        // �ȵ���̵� ����
        if (Application.platform == RuntimePlatform.Android)
        {
            // ���� ��û
            yield return PermissionCheck();
        }

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

        if(!UserData.checkSetup)
            UserData.SetUp();

        yield return SetUserName();

        // ĳ���� ���̺�
        Character.SetUp(charData, charDataLocal);
        yield return null;

        // ������ ���̺�
        Item.SetUp(itemData, itemDataLocal);
        yield return null;

        // �̺�Ʈ ���̺�
        IocEvent.SetUp(eventData, eventDataLocal);
        yield return null;

        // ��Ű�ڽ� ���̺�
        LuckyBox.SetUp(luckyData, luckyDataLocal);
        yield return null;

        // ����ũ ���̺�
        Unique.SetUp(uniqueData, uniqueDataLocal);
        yield return null;

        // �̴ϰ��� ���̺�
        Minigame.SetUp(minigameData, minigameDataLocal);
        yield return null;

        // �Ϸ� ó��
        isTableReady = true;
    }

    IEnumerator SetUserName()
    {
        Debug.Log("���� ������ :: ���� �̸� ����");

        if (UserData.userName == UserData.defaultName)
            userNameSetter.gameObject.SetActive(true);
        else
            Debug.Log("���� ������ :: ���� �̸� Ȯ�ε� -> " + UserData.userName);

        yield return null;
    }
}
