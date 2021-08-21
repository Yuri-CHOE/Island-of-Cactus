using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    // �� ���
    public static MiniGameManager script = null;
    
    /// <summary>
    /// �̴ϰ��� �÷��̾� �Ŵ���
    /// </summary>
    public MiniPlayerManager mpm = null;

    // Ŀư ������Ʈ
    [SerializeField] CanvasGroup curtain = null;

    // �غ� �ؽ�Ʈ
    [SerializeField] UnityEngine.UI.Text readyText = null;


    /// <summary>
    /// �̴ϰ��� ���� �Ŵ���
    /// </summary>
    public CardManager manager = null;

    // ���� ����
    public int scoreRiseValue = 10;


    // ���� ���� ����
    public bool isGameStart = false;



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
        StartCoroutine(Tool.CanvasFade(curtain, false, 2f));
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
        StartCoroutine(Tool.CanvasFade(curtain, true, 2f));

        // ��� ����
        mpm.SetRanking();

        // ���ΰ��� �ε�
        StartCoroutine(DelayedScoreScene());
    }

    IEnumerator DelayedScoreScene()
    {
        // Ŀư ���
        while (curtain.alpha != 1)
            yield return null;


        // �� �̵�
        // �ӽ� ���� ========== ���� �� �۾� �Ϸ� �� �ش� �� �̸����� �ٲܰ�
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main_game");
    }
}
