using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene_Title : MonoBehaviour
{
    [SerializeField]
    List<Transform> characterList = new List<Transform>();

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
        
    }

    public void Refresh()
    {
        for (int i = 0; i < characterList.Count; i++)
            if (characterList[i].GetComponent<Toggle>().isOn)
            {
                _selected = i;
                return;
            }
    }



    public static void SetGameMode(int gameMode)
    {
        GameData.SetGameMode((GameMode.Mode)gameMode);
    }

    public  void SetPlayerMe()
    {
        GameData.SetPlayerMe(new Player(Player.Type.User, _selected, false, "유저 이름 가저오기 구현 후 대체할것========"));
    }

    public static void SetWorldFileName(string __worldFileName)
    {
        // 파일명 입력
        GameData.SetWorldFileName(__worldFileName);

        // 코드 가져오기
        WorldManager.BuildWorld(GameData.worldFileName);
    }
}
