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

        private bool _startedSpawining;

        public GameObject denial;

       void Start()
        {
            _startedSpawining = false;

            FightingState.Instance.OnEnter += () => {
                if(!_startedSpawining)
                    StartCoroutine(SpawnGuys(5));

                _startedSpawining = true;
            };
        }

        IEnumerator SpawnGuys(int num)
        {
            WaitForSeconds wait = new WaitForSeconds(5.0f);

            while(num > 0)
            {
                Instantiate(denial).transform.position = transform.position; ;
                num--;
                yield return wait;
            }
        }
    }
}
