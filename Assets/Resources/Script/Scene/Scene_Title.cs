using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene_Title : MonoBehaviour
{
    [SerializeField]
    List<Transform> characterList = new List<Transform>();


    [SerializeField]
    Text userName = null;


    // 선택한 캐릭터 인덱스
    int _selected = 0;
    public int selected { get { return _selected; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 유저 이름 갱신
        if (UserData.userName != null)
            userName.text = UserData.userName;
        
    }

    public void Refresh()
    {
        for (int i = 0; i < characterList.Count; i++)
            if (characterList[i].GetComponent<Toggle>().isOn)
            {
                _selected = i + 1;
                Debug.Log("선택된 캐릭터 인덱스 : " + selected);
                return;
            }
    }



    public static void SetGameMode(int gameMode)
    {
        GameData.SetGameMode((GameMode.Mode)gameMode);
    }

    public void SetPlayerMe()
    {
        Refresh();
        GameData.player.me = new Player(Player.Type.User, selected, false, UserData.userName);
    }

    public static void SetWorldFileName(string __worldFileName)
    {
        // 파일명 입력
        GameData.SetWorldFileName(__worldFileName);

        // 코드 가져오기
        WorldManager.BuildWorld(GameData.worldFileName);
    }

    public void Tester()
    {
        UserData.SaveData();
    }
}
