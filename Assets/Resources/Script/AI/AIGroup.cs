using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAI;
using CustomAI.MainGame;

public class AIGroup
{
    // 메인게임 AI
    public MainGameAI mainGame = null;
    public class MainGameAI
    {
        public AI dice = new DiceAI();


        // 생성자
        protected MainGameAI() { }
        public MainGameAI(Player _owner)
        {
            dice.owner = _owner;
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