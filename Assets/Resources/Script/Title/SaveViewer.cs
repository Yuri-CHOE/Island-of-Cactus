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

    // 임시 로드한 모드
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
        // 중복 로드 방지
        if (GameData.gameMode == mode)
            return;

        GameSaver.CodeLoad();

        // 세이브 파일 없을 경우 스위칭
        if (GameSaver.scInfo == null  ||  GameSaver.scPlayers.Count < 1)
        {
            Debug.LogWarning("세이브 뷰어 :: 세이브 파일 없음");
            btnContinue.interactable = false;
            selector.setUp(0);

            return;
        }
        else
        {
            Debug.LogWarning("세이브 뷰어 :: 세이브 파일 발견");
            btnContinue.interactable = true;
            selector.setUp(1);
        }

        cycleNow.text =  GameSaver.scInfo[2];
        cycleGoal.text = GameSaver.scInfo[3];


        for (int i = 0; i < player.Count; i++)
        {
            player[i].SetUp();
        }

        // 로드 대상 기록
        mode = GameData.gameMode;
    }
}
