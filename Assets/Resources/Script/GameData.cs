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

    public static void SetPlayer(Player __player1, Player __player2, Player __player3, Player __player4)
    {
        _player.player_1 = __player1;
        _player.player_2 = __player2;
        _player.player_3 = __player3;
        _player.player_4 = __player4;
    }
    public static void SetPlayerMe(Player _me)
    {
        _player.me = _me;
    }
    public static void SetPlayer1(Player __player1)
    {
        _player.player_1 = __player1;
    }
    public static void SetPlayer2(Player __player2)
    {
        _player.player_2 = __player2;
    }
    public static void SetPlayer3(Player __player3)
    {
        _player.player_3 = __player3;
    }
    public static void SetPlayer4(Player __player4)
    {
        _player.player_4 = __player4;
    }
}
