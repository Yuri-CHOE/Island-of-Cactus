using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MainGame;

public abstract class AI
{
    public Player owner = null;

    // AI ���ΰ�
    public AIProperty element = AIProperty.New();

    // AI �۵����
    public abstract void Work();
}

public class AIGroup
{
    // ���ΰ��� AI
    public MainGameAI mainGame = null;
    public class MainGameAI
    {
        public AI dice = null;


        // ������
        protected MainGameAI() { }
        public MainGameAI(Player _owner)
        {
            dice = new DiceAI(_owner);
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

namespace CustomAI
{
    namespace MainGame
    {
        // AI - �ֻ���
        public class DiceAI : AI
        {
            public override void Work()
            {

            }

            /// <summary>
            /// ��� ����
            /// </summary>
            protected DiceAI() { }
            public DiceAI(Player _owner) { owner = _owner; }
        }
    }
}