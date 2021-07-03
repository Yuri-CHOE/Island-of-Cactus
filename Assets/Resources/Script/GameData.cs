using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    // ��ũ��Ʈ ���� �˻�
    // �Ź� �˻����� ���� �� ��ũ��Ʈ Awake()���� ��Ͻ��Ѽ� ����ϴ°� ȿ�� ������====================
    public static bool isMainGameScene { get { if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Main_game") return true; else return false; } }
    public static LoadingManager loadingManager { get { return GameObject.FindObjectOfType<LoadingManager>(); ; } }
    public static WorldManager worldManager { get { if (isMainGameScene) return GameObject.FindObjectOfType<WorldManager>(); else return null; } }
    public static GroundManager groundManager { get { if (isMainGameScene) return worldManager.groundManager; else return null; } }
    public static BlockManager blockManager { get { if (isMainGameScene) return worldManager.blockManager; else return null; } }
    public static DecorManager decorManager { get { if (isMainGameScene) return worldManager.decorManager; else return null; } }
    public static GameMaster gameMaster { get { if (isMainGameScene) return GameObject.FindObjectOfType<GameMaster>(); else return null; } }
    public static ItemManager itemManager { get { if (isMainGameScene) return gameMaster.itemManager; else return null; } }



    // ���Ӹ��
    static GameMode.Mode _gameMode = GameMode.Mode.None;
    public static GameMode.Mode gameMode { get { return _gameMode; } }

    // ������ ����
    static string _worldFileName = null;
    public static string worldFileName { get { return _worldFileName; } }

    // �÷��̾�
    static PlayerGroup _player = new PlayerGroup();
    public static PlayerGroup player { get { return _player; } }

    // ����Ŭ
    static Cycle _cycle = new Cycle();
    public static Cycle cycle { get { return _cycle; } }

    // ��
    static Turn _turn = new Turn();
    public static Turn turn { get { return _turn; } }

    // ���� �÷ο�
    public static GameMaster.Flow gameFlow = GameMaster.Flow.Wait;


    /// <summary>
    /// ���� ������ �ʱ�ȭ
    /// </summary>
    public static void Clear()
    {
        _gameMode = GameMode.Mode.None;
        _worldFileName = null;
        _player = new PlayerGroup();
        _cycle = new Cycle();
        _turn = new Turn();
    }

    public static void SetGameMode(GameMode.Mode __gameMode)
    {
        _gameMode = __gameMode;
    }

    public static void SetGameMode(int gameMode)
    {
        SetGameMode((GameMode.Mode)gameMode);
    }

    public static void SetWorldFileName(string __worldFileName)
    {
        _worldFileName = __worldFileName;
    }
}
