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

    [SerializeField]
    bool colorSwitch;

    AsyncOperation ao;

    bool isLoadFinish = false;
    bool isTableReady = false;

    // Start is called before the first frame update
    void Start()
    {
        // ��� �ε� ����
        ao = SceneManager.LoadSceneAsync(nextScene);

        // �ڵ� �Ѿ ����
        ao.allowSceneActivation = false;

        // ���̺� �ε�
        StartCoroutine(TableLoad());
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

    }


    public void Clicked()
    {
        // �ѱ��
        if (isTableReady)
            if (isLoadFinish)
                ao.allowSceneActivation = true;
    }

    IEnumerator TableLoad()
    {
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
