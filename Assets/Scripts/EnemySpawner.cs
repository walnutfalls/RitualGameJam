using Assets.Scripts.GameStates;
using GameStateManagement;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        private bool _switchedToFighting;
        private bool _switchedToNonFighting;

        void Update()
        {
            if(GameStateManager.Instance.CurrentGameState.Is<FightingState>() && !_switchedToFighting)
            {
                _switchedToFighting = true;
                _switchedToNonFighting = false;

                StartCoroutine(SpawnEnemies());
            }
            else if(!GameStateManager.Instance.CurrentGameState.Is<FightingState>() && !_switchedToNonFighting)
            {
                _switchedToFighting = false;
                _switchedToNonFighting = true;
                StopCoroutine(SpawnEnemies());
            }
        }

        IEnumerator SpawnEnemies()
        {
            while(!_switchedToNonFighting)
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
