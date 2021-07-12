using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI;
using CustomAI.MainGame;

public class AIGroup
{
    // ���ΰ��� AI
    public MainGameAI mainGame = null;
    public class MainGameAI
    {
        public AI dice = new DiceAI();


        // ������
        protected MainGameAI() { }
        public MainGameAI(Player _owner)
        {
            dice.owner = _owner;
        }
    }

    /// <summary>
    /// ��� ����
    /// </summary>
    protected AIGroup() { }
    public AIGroup(Player _owner)
    {
        mainGame = new MainGameAI(_owner);
    }
}