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

    // �ε� Ŀư
    CanvasGroup curtain = null;

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
    bool _isFadeFinish = false;
    public bool isFadeFinish { get { return _isFadeFinish; } }


    // �񵿱� �ε� ����
    AsyncOperation ao;

    // �ε��� ��
    [SerializeField]
    string nextScene = null;


    private void Awake()
    {
        curtain = gameObject.GetComponent<CanvasGroup>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RrefreshGuage());
        Work();
    }

    void FixedUpdate()
    {
        // ���̵� �۵�
        if (isFadeActive)
            Fade();
    }

    // Update is called once per frame
    void Update()
    {

        // �� �ε� �Ϸ� ����
        if (!isFadeActive && ao != null)
            if (ao.progress >= 0.9f) // 90% �̻� �Ϸ�� ��� => allowSceneActivation : true ���¿��� �ε� �ִ�ġ�� 90%�̹Ƿ� isDone ��� �Ұ���
            {
                // �ε�ȭ�� ��Ȱ��
                //transform.parent.gameObject.SetActive(false);

                // ���� �� Ȱ��ȭ
                ao.allowSceneActivation = true;

                // �ε�ȭ�� ��Ȱ��
                //gameObject.SetActive(false);

            }


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

        curtain.alpha = 0.0f;
        isFadeIn = true;
        isFadeActive = true;
    }

    public void LoadFinish()
    {
        // ���̵� �ƿ� ó��
        curtain.alpha = 1.0f;
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
        //// �ε�ȭ�� Ȱ��ȭ
        //gameObject.SetActive(true);

        //// ���� �� ĵ���� �켱���� ���
        //transform.parent.GetComponent<Canvas>().sortingOrder += 1;

        //// �� ������Ʈ �ı� ����
        //DontDestroyOnLoad(transform.root);

        //// �ε� ����
        //ao = SceneManager.LoadSceneAsync(sceneName);

        //// �ε� �Ϸ�� �ڵ� ����ȯ ��Ȱ��
        //ao.allowSceneActivation = false;

        //// �ε� ���� ����
        //LoadBefore();

        // �ε� ���� �� ���� ó��
        LoadAsync(SceneManager.LoadSceneAsync(sceneName));
    }
    /// <summary>
    /// �ܺο��� �ε��� ���� ���ڷ� �Ѱܹ޾� ����ó��
    /// (�ε� ��� ����)
    /// </summary>
    /// <param name="loadSceneAsync"></param>
    void LoadAsync(AsyncOperation loadSceneAsync)
    {
        // �ε�ȭ�� Ȱ��ȭ
        gameObject.SetActive(true);

        // ���� �� ĵ���� �켱���� ���
        transform.parent.GetComponent<Canvas>().sortingOrder += 1;

        // �� ������Ʈ �ı� ����
        DontDestroyOnLoad(transform.root);

        // �ε� ����
        ao = loadSceneAsync;

        // �ε� �Ϸ�� �ڵ� ����ȯ ��Ȱ��
        ao.allowSceneActivation = false;

        // �ε� ���� ����
        LoadBefore();
    }


    public void LoadAsyncMiniGame(Minigame minigame, int reward, List<Player> entryPlayer)
    {
        // �̹� �������� ��� ����
        if (MiniGameManager.minigameNow != null)
        {
            Debug.LogError("error :: �̴ϰ��� �ߺ� ȣ��");
            Debug.Break();
            return;
        }
        // ������ ��ȿ ����
        if (entryPlayer == null)
        {
            Debug.LogError("error :: �̴ϰ��� ������ ��ȿ");
            Debug.Break();
            return;
        }
        // ������ ���� ��� ����
        if (entryPlayer.Count <= 0)
        {
            Debug.LogError("error :: �̴ϰ��� ������ ����");
            Debug.Break();
            return;
        }

        Debug.LogWarning(string.Format("�̴ϰ��� :: ȣ��� -> [{0}] {1}",minigame.index, minigame.name));

        // �̴ϰ��� ����
        MiniGameManager.minigameNow = minigame;

        // ���� ����
        MiniScore.reward = reward;

        // ������ ����
        for (int i = 0; i < entryPlayer.Count; i++)
        {
            entryPlayer[i].miniInfo.join = true;
        }

        // �̴ϰ��� �ε�
        LoadAsync(SceneManager.LoadSceneAsync(minigame.sceneNum));
    }

    /// <summary>
    /// ���̵� �۵�
    /// StartCoroutine() ���� ȣ���Ұ�
    /// </summary>
    /// <returns></returns>
    void Fade()
    {
        // ���̵� ��
        if (isFadeIn)
        {
            // ���̵� ó��
            if (curtain.alpha < 1.0f)
            {
                curtain.alpha += Time.deltaTime * 2;
            }
            // ���̵� �Ϸ�
            else
            {
                curtain.alpha = 1.0f;
                curtain.blocksRaycasts = true;
                isFadeActive = false;
                _isFadeFinish = true;
                Debug.Log("fade in done");
            }
        }

        // ���̵� �ƿ�
        else
        {
            // ���̵� ó��
            if (curtain.alpha > 0.0f)
            {
                curtain.alpha -= Time.deltaTime;
            }
            // ���̵� �Ϸ�
            else
            {
                curtain.alpha = 0.0f;
                curtain.blocksRaycasts = false;
                isFadeActive = false;
                _isFadeFinish = true;
                Debug.Log("fade out done");
            }
        }
    }

    /// <summary>
    /// ��� LoadingManager ��ũ��Ʈ�� ã�� �� ��ũ��Ʈ�� �ߺ��˻� �Ͽ� DontDestroyOnLoad�� ��ȣ�� ���� �� �ı�
    /// </summary>
    void DestroyOld()
    {
        // �ε� ������Ʈ Ȯ��
        LoadingManager[] obj = FindObjectsOfType<LoadingManager>();
        for (int i = 0; i < obj.Length; i++)
        {
            // �� ��ũ��Ʈ�� ���� ������Ʈ���� üũ
            if (obj[i].transform != transform)
            {
                // �ٸ���� DontDestroyOnLoad�� ��ȣ�� ���������� �Ǵ�
                // ����ó��
                Destroy(obj[i].transform.root.gameObject);
                return;
            }
        }
    }


    /// <summary>
    /// �ε� �۾�
    /// </summary>
    void Work()
    {
        // ���� �� ����
        DestroyOld();

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
        workCount++;

        // ���� �÷ο� ����
        GameData.gameFlow = GameMaster.Flow.Wait;
        

        // ���� ������ �ε�
        // = �� ���еǴ� ��ⵥ���Ͱ��/User/UserData.iocdata ������ ���纻(true)���� ���� ����(false)�ϰ� �о��
        UserData.file = new CSVReader("User", "UserData.iocdata", true, false, '=');
        UserData.SetUp();
        workCount += UserData.file.table.Count;
        
        // ĳ���� ���̺�
        Character.SetUp();
        workCount+= Character.table.Count;

        // ������ ���̺�
        Item.SetUp();
        workCount += Item.table.Count;

        // �̺�Ʈ ���̺�
        IocEvent.SetUp();
        workCount += IocEvent.table.Count;

        // ��Ű�ڽ� ���̺�
        LuckyBox.SetUp();
        workCount += LuckyBox.table.Count;

        // ����ũ ���̺�
        Unique.SetUp();
        workCount += Unique.table.Count;

        // �̴ϰ��� ���̺�
        Minigame.SetUp();
        workCount += Minigame.table.Count;
    }

    void Work_MainGame()
    {
        {
            /*
            // �۾� ��ǥ�� ����
            workMax = 10000;

            // ���� ���� ����
            WorldManager wm = GameObject.Find("World").GetComponent<WorldManager>();
            workCount++;

            // ī�޶� �Ѱ� ����
            wm.cameraManager.controller.SetCameraLimit(WorldManager.worldFile[0]);
            workCount+=6;

            // ��ŸƮ ��� ����
            wm.blockManager.SetStartBlock(WorldManager.worldFile[1]);

            // ���� ����
            wm.groundManager.BuildByString(WorldManager.worldFile[2]);
            workCount++;

            // ��� ����
            wm.blockManager.BuildByString(WorldManager.worldFile[3]);
            workCount++;

            // ��Ĺ� ����
            wm.decorManager.BuildByString(WorldManager.worldFile[4]);
            workCount++;

            // ��ֹ� �ʱ�ȭ
            // ================ ���� ���� �ʿ� : �̷��� �� ���� �ε������� ��ֹ� ���� ���ư�
            // �� �ذ�� : �ε��Ҷ� ���� �ε� ������ ���ΰ��� �÷ο� �ʱ�ȭ�������� ó���Ұ�
            // �� �ذ�� : �׳� ������,�̺�Ʈ ������Ʈ ����Ʈ �о �籸���Ұ�
            // �ʱ�ȭ �ȉ��� ��� �ʱ�ȭ
            if(DynamicObject.objectList.Count == 0)
            {
                for (int i = 0; i < wm.blockManager.blockCount; i++)
                    DynamicObject.objectList.Add(new List<DynamicObject>());
            }
            // �̹� �ʱ�ȭ �� ��� �����
            else
            {
                // ������ �����
                ItemManager.ReCreateAll();

                // �̺�Ʈ �����
                EventManager.ReCreateAll();
            }
            */
        }

        // �۾� ��ǥ�� ����
        workMax = 10000;

        // ���� ���� ����
        WorldManager wm = GameObject.Find("World").GetComponent<WorldManager>();
        workCount++;

        // ī�޶� �Ѱ� ����
        wm.cameraManager.controller.SetCameraLimit(WorldManager.worldFile[0]);
        workCount += 6;

        // ��ŸƮ ��� ����
        wm.blockManager.SetStartBlock(WorldManager.worldFile[1]);
        workCount++;

        // ���� ����
        wm.groundManager.BuildByString(WorldManager.worldFile[2]);
        workCount++;

        // ��� ����        
        wm.blockManager.BuildByString(WorldManager.worldFile[3]);
        workCount++;

        // ��Ĺ� ����
        wm.decorManager.BuildByString(WorldManager.worldFile[4]);
        workCount++;

        // �ڳ� ����
        wm.blockManager.SetCorner();
        workCount += wm.blockManager.blockCount;

        // ��� ����Ʈ ���
        wm.blockManager.SetDynamicBlockList();
        workCount += wm.blockManager.blockCount;

        // ù �ε�
        if (GameMaster.flowCopy == GameMaster.Flow.Wait)
        {
            Debug.LogWarning("�ε� :: ������Ʈ ����Ʈ �ʱ�ȭ��");

            // ��ֹ� �ʱ�ȭ
            for (int i = 0; i < wm.blockManager.blockCount; i++)
                DynamicObject.objectList.Add(new List<DynamicObject>());
        }
        // �ٽ� �ε�
        else
        {
            // ��ֹ� �����
            {
                Debug.LogWarning(string.Format(
                    "�ε� :: ������Ʈ ����� ��û��\n�ջ� : {0}\n������ : {1}\n�̺�Ʈ : {2}",
                    ItemManager.itemObjectList.Count + EventManager.eventObjectList.Count, 
                    ItemManager.itemObjectList.Count, 
                    EventManager.eventObjectList.Count
                    ) );

                // ������ �����
                ItemManager.ReCreateAll(true);

                // �̺�Ʈ �����
                EventManager.ReCreateAll(true);
            }

            GameData.gameFlow = GameMaster.Flow.Wait;
        }
    }
}
