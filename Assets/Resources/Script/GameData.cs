using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{

    //// �ʵ� �ӽñ���======================

    // ���Ӹ��
    static GameMode.Mode _gameMode = GameMode.Mode.None;
    public static GameMode.Mode gameMode { get { return _gameMode; } }

    // �÷��� ȸ�� - ���� ������ ���̺� �о�ð�==============
    static int _playCount = -1;
    public static int playCount { get { return _playCount; } }

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
        _playCount = -1;
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

    public static void SetpPlayCount(int __playCount)
    {
        _playCount = __playCount;
    }

    public static void SetWorldFileName(string __worldFileName)
    {
        _worldFileName = __worldFileName;
    }
}
