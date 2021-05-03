using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene_Title : MonoBehaviour
{
    [SerializeField]
    List<Transform> characterList = new List<Transform>();

    // ������ ĳ���� �ε���
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
        GameData.SetPlayerMe(new Player(Player.Type.User, _selected, false, "���� �̸� �������� ���� �� ��ü�Ұ�========"));
    }

    public static void SetWorldFileName(string __worldFileName)
    {
        // ���ϸ� �Է�
        GameData.SetWorldFileName(__worldFileName);

        // �ڵ� ��������
        WorldManager.BuildWorld(GameData.worldFileName);
    }
}