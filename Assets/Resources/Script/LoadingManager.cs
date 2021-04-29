using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    // �ε� ������
    [SerializeField]
    Image guage;

    // �Ϸ� ����
    [SerializeField]
    int workCount = 0;
    public int workMax = 1;
    bool isLoadComplete = false;

    // �ϷῩ��
    bool _isFinish = false;
    public bool isFinish { get { return _isFinish; } }

    // �ε�UI ���̵�
    bool isFadeIn = true;
    bool isFadeActive = false;



    // �񵿱� �ε� ����
    AsyncOperation ao;

    [SerializeField]
    string nextScene = null;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RrefreshGuage());
        Work();
    }

    // Update is called once per frame
    void Update()
    {
        // ���̵� �۵�
        if (isFadeActive)
            Fade();

        // �� �ε� �Ϸ� ����
        if (!isFadeActive && ao != null)
            if (ao.isDone)
                ao.allowSceneActivation = true;


        // ���� �ε� �ܿ� �Ųٱ�
        if (workCount < workMax && isLoadComplete)
            workCount += (int)(Time.deltaTime*3000);

        // ���� �ε� �Ϸ�
        if (isFinish) { }
    }


    /// <summary>
    /// �ε� ���൵ ����
    /// </summary>
    float GetProgress()
    {
        // �ִ�ġ ����
        if (workCount >= workMax)
            workCount = workMax;

        return (float)workCount / (float)workMax; 
    }

    /// <summary>
    /// �ε� ������ ����
    /// StartCoroutine() ���� ȣ���Ұ�
    /// </summary>
    IEnumerator RrefreshGuage()
    {
        while (!isFinish)
        {
            //Debug.Log(GetProgress());
            guage.fillAmount = GetProgress();
            CheckFinish();
            yield return null;
        }
    }

    /// <summary>
    /// �ε� �Ϸ� Ȯ��
    /// </summary>
    void CheckFinish()
    {
        // 1.00f �̻��̸� �Ϸ�ó��
        if (GetProgress() >= 1.00f)
        {
            _isFinish = true;
            Debug.Log("Dynamic Loading :: Finished");

            LoadFinish();
        }
    }

    public void LoadBefore()
    {
        // �ε� ������ ��Ȱ��
        guage.gameObject.SetActive(false);

        gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
        isFadeIn = true;
        isFadeActive = true;
    }

    public void LoadFinish()
    {
        // ���̵� �ƿ� ó��
        gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
        isFadeIn = false;
        isFadeActive = true;
    }

    public void LoadAsync()
    {
        // �ε� ����
        LoadAsync(nextScene);
    }
    public void LoadAsync(string sceneName)
    {
        // �ε� ����
        ao = SceneManager.LoadSceneAsync(sceneName);

        // �ε� ���� ����
        LoadBefore();
    }

    void Fade()
    {
        // ���̵� ��
        if (isFadeIn)
        {
            // ���̵� ó��
            if (gameObject.GetComponent<CanvasGroup>().alpha < 1.0f)
            {
                gameObject.GetComponent<CanvasGroup>().alpha += Time.deltaTime;
            }
            // ���̵� �Ϸ�
            else
            {
                gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
                isFadeActive = false;
                Debug.Log("fade in done");
            }
        }

        // ���̵� �ƿ�
        else
        {
            // ���̵� ó��
            if (gameObject.GetComponent<CanvasGroup>().alpha > 0.0f)
            {
                gameObject.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
            }
            // ���̵� �Ϸ�
            else
            {
                gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
                isFadeActive = false;
                Debug.Log("fade out done");
            }
        }
    }

    /// <summary>
    /// �ε� �۾�
    /// </summary>
    void Work()
    {
        // ���� ���� �ε��۾�
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0: // First

                break;
            case 1: // Title
                Work_Title();
                break;
            case 2: // Main_game
                Work_MainGame();
                break;
        }

        // �ε� ����
        Debug.Log("�ε� �Ϸ� :: "+SceneManager.GetActiveScene().name + "=> �۾� �� : " + workCount);
        isLoadComplete = true;
        //workCount = workMax;
    }

    void Work_Title()
    {
        // �۾� ��ǥ�� ����
        workMax = 10000;

        gameObject.SetActive(false);

        Item.SetUp();
        workCount++;
    }

    void Work_MainGame()
    {
        // �۾� ��ǥ�� ����
        workMax = 10000;

        Item.SetUp();
        workCount++;
    }
}
