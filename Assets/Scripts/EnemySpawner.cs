using Assets.Scripts.GameStates;
using GameStateManagement;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject denial;

        void Start()
        {
            FightingState.Instance.OnEnter += () =>
            {
                StartCoroutine(SpawnGuys(5));
            };
        }

        IEnumerator SpawnGuys(int num)
        {
            WaitForSeconds wait = new WaitForSeconds(5.0f);

            while (num > 0)
            {
                Instantiate(denial).transform.position = transform.position; ;
                num--;
                yield return wait;
            }
        }
    }
}
