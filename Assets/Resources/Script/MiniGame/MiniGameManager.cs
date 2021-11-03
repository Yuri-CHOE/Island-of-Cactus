using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MiniGame;

public class MiniGameManager : MonoBehaviour
{
    // �� ���
    public static MiniGameManager script = null;

    // ���� �������� ����
    public static Minigame minigameNow = null;

    // ���� ����
    public static ActionProgress progress = ActionProgress.Ready;

    //// �̴ϰ��� �δ�
    //static AsyncOperation loader = null;


    /// <summary>
    /// �̴ϰ��� �÷��̾� �Ŵ���
    /// </summary>
    public MiniPlayerManager mpm = null;

    // Ŀư ������Ʈ
    [SerializeField] CanvasGroup curtain = null;

    // �غ� �ؽ�Ʈ
    public UnityEngine.UI.Text readyText = null;


    /// <summary>
    /// �̴ϰ��� ���� �Ŵ���
    /// </summary>
    public CardManager manager = null;

    // ���� ����
    public int scoreRiseValue = 10;


    // ���� ���� ����
    public bool isGameStart = false;

    // ���� ������ ���� �ε�
    AsyncOperation ao = null;


    // AI ����� �����
    public static MiniAI.AnswerType answerType = MiniAI.AnswerType.none;
    public static MiniAI.Answer answer = MiniAI.Answer.none;
    public static bool isAnswerSubmit = false;



    private void Awake()
    {
        // �� ���
        script = this;

        // �ʱ� ���� ȣ��
        mpm.Init();
        manager.Init();

        // ���ΰ��� �� ����
        GameObject deleteObj = FindObjectsOfType<LoadingManager>()[0].transform.root.gameObject;
        Destroy(deleteObj);

        // Ŀư ���̵� ��
        Starting();


        // ���� ������ ���� �ε�
        ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MiniGameReporter");

        // �ε� �Ϸ�� �ڵ� ����ȯ ��Ȱ��
        ao.allowSceneActivation = false;
    }


    private void Update()
    {

    }



    public void ScoreAdd(int addCount)
    {
        //ScoreAdd(MiniPlayerManager.script.turnNow, addCount);
        ScoreAdd(mpm.turnNow, addCount);
    }
    public void ScoreAdd(Player target, int addCount)
    {
        target.miniPlayerUI.scorePlus+= addCount * scoreRiseValue;
    }

    public void Starting()
    {
        //// Ŀư Ȱ��ȭ
        //curtain.gameObject.SetActive(true);

        // Ŀư ���̵� �ƿ�
        StartCoroutine(Tool.CanvasFade(curtain, false, 1f));
    }

    public void Ready()
    {
        // �غ� ����
        //isGameStart = true;

        // �غ� ��ȯ
        Player.me.miniInfo.isReady = true;

        // �ؽ�Ʈ ����
        readyText.text = "wait for other player . . .";
    }

    public void Ending()
    {
        //// Ŀư Ȱ��ȭ
        //curtain.gameObject.SetActive(true);

        // Ŀư ���̵� ��
        StartCoroutine(Tool.CanvasFade(curtain, true, 0.5f));

        // ��� ����
        mpm.SetRanking();

        // ���� ó��
        progress = ActionProgress.Finish;

        // ����
        MiniScore.GiveRewardAll();

        // ���� �� �ε�
        StartCoroutine(DelayedScoreScene());
    }

    IEnumerator DelayedScoreScene()
    {
        // Ŀư ���
        while (curtain.alpha != 1)
            yield return null;


        //// �� �̵�
        //AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main_game");
        //ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MiniGameReporter");

        // �ε� �Ϸ�� �ڵ� ����ȯ Ȱ��
        ao.allowSceneActivation = true;
    }



    ///// <summary>
    ///// �̴ϰ��� ȣ��
    ///// </summary>
    ///// <param name="minigameName"></param>
    ///// <param name="entryPlayer"></param>
    //public AsyncOperation LoadMiniGame(string minigameName, int reward, List<Player> entryPlayer)
    //{
    //    // ȣ��
    //    try
    //    {
    //        MinigameName gameNameTemp = (MinigameName)System.Enum.Parse(typeof(MinigameName), minigameName);
    //        return LoadMiniGame(gameNameTemp, reward, entryPlayer);
    //    }
    //    catch
    //    {
    //        Debug.LogError("error :: �������� �ʴ� �̴ϰ��� -> " + minigameName);
    //        Debug.Break();
    //        return null;
    //    }
    //}
    ///// <summary>
    ///// �̴ϰ��� ȣ��
    ///// </summary>
    ///// <param name="minigameName"></param>
    ///// <param name="entryPlayer"></param>
    //public AsyncOperation LoadMiniGame(MinigameName minigameName, int reward, List<Player> entryPlayer)
    //{
    //    // �̹� �������� ��� ����
    //    if (gameName != MinigameName.None)
    //    {
    //        Debug.LogError("error :: �̴ϰ��� �ߺ� ȣ��");
    //        Debug.Break();
    //        return null;
    //    }
    //    // ������ ��ȿ ����
    //    if (entryPlayer == null)
    //    {
    //        Debug.LogError("error :: �̴ϰ��� ������ ��ȿ");
    //        Debug.Break();
    //        return null;
    //    }
    //    // ������ ���� ��� ����
    //    if (entryPlayer.Count <= 0)
    //    {
    //        Debug.LogError("error :: �̴ϰ��� ������ ����");
    //        Debug.Break();
    //        return null;
    //    }

    //    // �̴ϰ��� ����
    //    gameName = minigameName;

    //    // ���� ����
    //    MiniScore.reward = reward;

    //    // ������ ����
    //    for (int i = 0; i < entryPlayer.Count; i++)
    //    {
    //        entryPlayer[i].miniInfo.join = true;
    //    }

    //    // �̴ϰ��� �ε�
    //    loader = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)minigameName);
    //    return loader;
    //}
}
