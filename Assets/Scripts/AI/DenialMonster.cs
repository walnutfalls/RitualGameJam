using Assets.Scripts.Rose;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.AI
{
    [RequireComponent(typeof(Rigidbody2D))]
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


        #region Editor Variables
        public ParticleSystem particleSystem;
        public Animator animator;
        public float stalkDistance = 7.0f;
        public float stalkDIstanceVariance = 3.0f;
        public float moveSpeed = 5.0f;
        public GameObject attackCone;
        public float attackTime = 3.0f;
        public int maxRepositions = 3;
        #endregion


        private RoseController _rose;
        private Rigidbody2D _rigidBody2D;
        private int _repositionsSoFar;

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


        private void Awake()
        {
            _rose = FindObjectOfType<RoseController>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            State = DenialMonsterState.Stalking;
        }

              

        #region Coroutines
        IEnumerator Attack()
        {
            _repositionsSoFar++;

            if (_repositionsSoFar > maxRepositions)
            {
                _repositionsSoFar = 0;
                State = DenialMonsterState.Hiding;
            }
            else
            {
                _rigidBody2D.velocity = Vector2.zero;              

                attackCone.transform.MatchUpVector((_rose.transform.position - particleSystem.transform.position).normalized);
                attackCone.SetActive(true);


                float startTime = Time.time;
                float lastTime = startTime;
                float timeElapsed = 0;

                while(timeElapsed < attackTime)
                {
                    float dt = Time.time - lastTime;
                    lastTime = Time.time;
                    timeElapsed += dt;


                    attackCone.transform.MatchUpVector((_rose.transform.position - particleSystem.transform.position).normalized);
                    yield return null;
                }
               
                attackCone.SetActive(false);

                State = DenialMonsterState.Repositioning;
            }
        }

        IEnumerator Reposition()
        {
            if (_repositionsSoFar > maxRepositions)
            {
                _repositionsSoFar = 0;
                State = DenialMonsterState.Hiding;
            }

            float x = Random.Range(-1, 1);
            Vector2 ran = new Vector2(x, 1.0f);

            var mag = stalkDistance + Random.Range(-stalkDIstanceVariance / 2, stalkDIstanceVariance / 2); //TODO: optimize
            float startTime = Time.time;
            float lastTime = startTime;
            float timeElapsed = 0;

            Vector2 targ = (Vector2)_rose.transform.position + ran * mag;

            while (State == DenialMonsterState.Repositioning && timeElapsed < 5.0f && Vector2.Distance(transform.position, targ) > 0.5f)
            {
                float dt = Time.time - lastTime;
                timeElapsed += dt;
                lastTime = Time.time;

                targ = (Vector2)_rose.transform.position + ran * mag;
                Vector2 vel = (targ - (Vector2)transform.position).normalized * moveSpeed;

                _rigidBody2D.velocity = vel;

                yield return null;
            }
                        
            State = DenialMonsterState.Attacking;
        }

        IEnumerator Hide()
        {
            particleSystem.Stop(true);
            particleSystem.gameObject.SetActive(false);
            attackCone.SetActive(false);


            animator.SetBool("IsLooking", false);
            yield return new WaitForSeconds(1.25f);

            animator.SetBool("Visible", false);
            yield return new WaitForSeconds(1.25f);          
            

            float x = Random.Range(_rose.transform.position.x - 100.0f, _rose.transform.position.x + 100.0f);
            float y = 200.0f;
            transform.position = new Vector3(x, y, transform.position.z);

            yield return new WaitForSeconds(2.25f);

            State = DenialMonsterState.Stalking;
        }

        IEnumerator Stalk()
        {
            attackCone.SetActive(false);

            float x = Random.Range(-1, 1);
            Vector2 ran = new Vector2(x, 1.0f);

            var mag = stalkDistance + Random.Range(-stalkDIstanceVariance / 2, stalkDIstanceVariance / 2); //TODO: optimize

            while (Vector2.Distance(transform.position, _rose.transform.position) > mag + 5.0f)
            {                
                Vector2 targ = (Vector2)_rose.transform.position + ran * mag;
                Vector2 vel = (targ - (Vector2)transform.position).normalized * moveSpeed;

                Debug.DrawLine(transform.position, targ, Color.red);


                _rigidBody2D.velocity = vel;

                yield return null;                
            }

            _rigidBody2D.velocity = Vector2.zero;

            State = DenialMonsterState.Showing;
        }

        IEnumerator Show()
        {
            particleSystem.gameObject.SetActive(true);
            particleSystem.Play(true);

            animator.SetBool("Visible", true);
            yield return new WaitForSeconds(1.25f);

            animator.SetBool("IsLooking", true);
            yield return new WaitForSeconds(1.25f);

            State = DenialMonsterState.Attacking;
        }
        #endregion
    }
}
