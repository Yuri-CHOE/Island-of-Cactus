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

    // Start is called before the first frame update
    void Start()
    {
        // ��� �ε� ����
        ao = SceneManager.LoadSceneAsync(nextScene);

        // �ڵ� �Ѿ ����
        ao.allowSceneActivation = false;

        // ���̺� �ε� �Ϸ�
        //Item.SetUp();
        //Character.SetUp();
        text.text = "Press Anywhere";
        isLoadFinish = true;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Clicked()
    {
        // ��ġ ����
        if (ao.progress < 0.9f)
        {
            text.text = "wait...";
            return;
        }

        // �ڵ� �Ѿ Ȱ��
        else if (isLoadFinish)
            ao.allowSceneActivation = true;

    }

}
