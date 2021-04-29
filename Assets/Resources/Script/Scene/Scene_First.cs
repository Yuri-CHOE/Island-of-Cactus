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

    // Start is called before the first frame update
    void Start()
    {
        // ��� �ε� ����
        ao = SceneManager.LoadSceneAsync(nextScene);

        // �ڵ� �Ѿ ����
        ao.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Clicked()
    {
        if (!ao.isDone)
            text.text = "wait...";

        // �ڵ� �Ѿ Ȱ��
        ao.allowSceneActivation = true;
    }

}
