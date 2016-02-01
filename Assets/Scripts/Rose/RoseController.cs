using UnityEngine;
using System.Linq;
using System.Collections;

namespace Assets.Scripts.Rose
{
    [RequireComponent(typeof(Health))]
    class RoseController : MonoBehaviour
    {
        private const string UseButton = "Use";

        #region Pickup Readius
        public float pickupRadius = 8.0f;
        public LayerMask flowers;
        public float attackCooldown = 1.0f;
        public GameObject damage;
        #endregion

        private bool _isUsePressed;
        private Health _health;
        private bool _canAttack;


        private void Awake()
        {
            _health = GetComponent<Health>();
            _canAttack = true;

            _health.EntityKilledListeners += () =>
            {
                Application.LoadLevel(0);
            };
        }

        private void Update()
        {
            if(Input.GetAxis(UseButton) > 0 && !_isUsePressed)
            {
                _isUsePressed = true;

                //try to pick up flower
                var colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius, flowers);

                Debug.DrawLine(transform.position, transform.position + Vector3.left * pickupRadius, Color.blue);

                if (colliders.Any())
                {

                    Collider2D minDistCol = colliders.ElementAt(0);
                    float minDist = Vector2.Distance(colliders.ElementAt(0).transform.position, transform.position);
                    foreach (var col in colliders)
                    {
                        float dist = Vector2.Distance(col.transform.position, transform.position);
                        if(dist < minDist)
                        {
                            minDistCol = col;
                            minDist = dist;
                        }
                    }

                    PickUpFlower(minDistCol.gameObject.GetComponent<Flower>());
                }
            }
            else
            {
                _isUsePressed = false;
            }

            if(Input.GetAxis("Fire1") > 0 && _canAttack)
            {
                GetComponent<Animator>().SetTrigger("attack");
                damage.SetActive(true);
                StartCoroutine(AttackCooldown());
            }
        }

        private void PickUpFlower(Flower flower)
        {
            flower.PlaySound();
            Destroy(flower.gameObject);
            _health.HealthPoints += 10.0f;
            // ...
        }

        IEnumerator AttackCooldown()
        {
            _canAttack = false;
            yield return new WaitForSeconds(attackCooldown);
            damage.SetActive(false);
            _canAttack = true;
        }
    }
}
