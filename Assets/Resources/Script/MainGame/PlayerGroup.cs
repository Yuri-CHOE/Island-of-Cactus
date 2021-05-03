using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroup
{
    public class PlayerGroupSystem
    {
        public Player Starter = new Player(Player.Type.System, 1, true, "Starter");
        public Player Minigame = new Player(Player.Type.System, 1, true, "Minigame");
        public Player MinigameEnder = new Player(Player.Type.System, 1, true, "MinigameEnder");
        public Player Ender = new Player(Player.Type.System, 1, true, "Ender");
    }

    // �ý��� �÷��̾�
    public PlayerGroupSystem system = new PlayerGroupSystem();

    // �÷��̾� �ڽ�
    public Player me = null;

    // p1~4 �÷��̾�
    public Player player_1 = null;
    public Player player_2 = null;
    public Player player_3 = null;
    public Player player_4 = null;

    // �� ������ �÷��̾�
    //public Player isTurn = null;
}
