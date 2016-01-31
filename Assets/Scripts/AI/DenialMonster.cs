using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class DenialMonster : MonoBehaviour
    {
        public enum DenialMonsterState
        {
            Attacking,
            Repositioning,
            Hiding,
            Stalking,
            Showing
        }

        #region Properties
        private DenialMonsterState _state;
        public DenialMonsterState State
        {
            get { return _state; }
            set
            {
                _state = value;
                switch (_state)
                {
                    case DenialMonsterState.Attacking:
                        StartCoroutine(Attack());
                        break;
                    case DenialMonsterState.Repositioning:
                        StartCoroutine(Reposition());
                        break;
                    case DenialMonsterState.Hiding:
                        StartCoroutine(Hide());
                        break;
                    case DenialMonsterState.Stalking:
                        StartCoroutine(Stalk());
                        break;
                    case DenialMonsterState.Showing:
                        StartCoroutine(Show());
                        break;
                    default: break;
                }
            }
        }
        #endregion


        private void start()
        {
            State = DenialMonsterState.Hiding;
        }


        #region Coroutines
        IEnumerator Attack()
        {
            return null;
        }

        IEnumerator Reposition()
        {
            return null;
        }

        IEnumerator Hide()
        {
            return null;
        }

        IEnumerator Stalk()
        {
            return null;
        }

        IEnumerator Show()
        {
            return null;
        }
        #endregion
    }
}
