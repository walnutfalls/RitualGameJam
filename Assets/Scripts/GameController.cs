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



            FightingState.Instance.OnEnter += () =>
            {
                GetComponent<AudioSource>().Stop();
            };
        }
    }
}
