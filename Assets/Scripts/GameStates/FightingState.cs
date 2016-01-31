using System;
using GameStateManagement;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class FightingState : GameStateBase<FightingState>
    {
        public override void StateStaticInitialize()
        {
            Camera.main.orthographicSize += 2;
        }
    }
}
