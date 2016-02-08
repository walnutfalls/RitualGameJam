using Assets.Scripts.GameStates;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        public Transform Rose;
        public Collider2D gateTrigger;

        void Start()
        {
            GameStateManagement.GameStateManager.Instance.TransitionToGameState<FlowerPickingState>();

            FightingState.Instance.OnEnter += OnFightingStateEnter;
        }

        void OnDestroy()
        {
            FightingState.Instance.OnEnter -= OnFightingStateEnter;
        }

        private void OnFightingStateEnter()
        {
            GetComponent<AudioSource>().Stop();
        }
    }
}
