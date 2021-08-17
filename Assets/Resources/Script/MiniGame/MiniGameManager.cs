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

    /// <summary>
    /// �̴ϰ��� ���� �Ŵ���
    /// </summary>
    public CardManager manager = null;

    // ���� ����
    public int scoreRiseValue = 10;



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

    public void Ending(GameObject endingCurtain)
    {
        if (endingCurtain != null)
        {
            endingCurtain.SetActive(true);

            CanvasGroup cg = endingCurtain.GetComponent<CanvasGroup>();

            if(cg != null)
            Tool.CanvasFade(cg, false, 2f);
        }

        // ��� ����
        mpm.SetRanking();

        // ���ΰ��� �ε�
        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main_game");
    }
}
