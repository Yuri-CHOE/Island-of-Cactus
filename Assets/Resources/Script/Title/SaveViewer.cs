using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveViewer : MonoBehaviour
{
    [SerializeField]
    ObjectSwitch selector = null;

    [SerializeField]
    Button btnContinue = null;


    [SerializeField]
    Text cycleNow = null;
    [SerializeField]
    Text cycleGoal = null;

    [SerializeField]
    List<SaveViewerPlayer> player = new List<SaveViewerPlayer>();

    // �ӽ� �ε��� ���
    GameMode.Mode mode = GameMode.Mode.None;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SetUp()
    {
        // �ߺ� �ε� ����
        if (GameData.gameMode == mode)
            return;

        GameSaver.CodeLoad();

        // ���̺� ���� ���� ��� ����Ī
        if (GameSaver.scInfo == null  ||  GameSaver.scPlayers.Count < 1)
        {
            Debug.LogWarning("���̺� ��� :: ���̺� ���� ����");
            btnContinue.interactable = false;
            selector.setUp(0);

            return;
        }
        else
        {
            Debug.LogWarning("���̺� ��� :: ���̺� ���� �߰�");
            btnContinue.interactable = true;
            selector.setUp(1);
        }

        cycleNow.text =  GameSaver.scInfo[2];
        cycleGoal.text = GameSaver.scInfo[3];


        for (int i = 0; i < player.Count; i++)
        {
            player[i].SetUp();
        }

        // �ε� ��� ���
        mode = GameData.gameMode;
    }
}
