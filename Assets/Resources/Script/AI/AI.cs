using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI.MainGame;

public abstract class AI
{
    public Player owner = null;

    // AI 세부값
    public AIProperty element = AIProperty.New();

    // AI 작동방식
    public abstract void Work();
}

public class AIGroup
{
    // 메인게임 AI
    public MainGameAI mainGame = null;
    public class MainGameAI
    {
        public AI dice = null;


        // 생성자
        protected MainGameAI() { }
        public MainGameAI(Player _owner)
        {
            dice = new DiceAI(_owner);
        }
    }

    /// <summary>
    /// 사용 금지
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
        // AI - 주사위
        public class DiceAI : AI
        {
            public override void Work()
            {

            }

            /// <summary>
            /// 사용 금지
            /// </summary>
            protected DiceAI() { }
            public DiceAI(Player _owner) { owner = _owner; }
        }
    }
}