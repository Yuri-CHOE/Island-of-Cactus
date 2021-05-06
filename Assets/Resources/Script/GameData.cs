using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{

    //// 필드 임시구성======================

    // 게임모드
    static GameMode.Mode _gameMode = GameMode.Mode.None;
    public static GameMode.Mode gameMode { get { return _gameMode; } }

    // 플레이 회차 - 유저 데이터 테이블 읽어올것==============
    static int _playCount = -1;
    public static int playCount { get { return _playCount; } }

    // 선택한 월드
    static string _worldFileName = null;
    public static string worldFileName { get { return _worldFileName; } }

    // 플레이어
    static PlayerGroup _player = new PlayerGroup();
    public static PlayerGroup player { get { return _player; } }

    // 사이클
    static Cycle _cycle = new Cycle();
    public static Cycle cycle { get { return _cycle; } }

    // 턴
    static Turn _turn = new Turn();
    public static Turn turn { get { return _turn; } }

    // 게임 플로우
    public static GameMaster.Flow gameFlow = GameMaster.Flow.Wait;


    /// <summary>
    /// 게임 데이터 초기화
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
