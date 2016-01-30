using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Rose
{
    [RequireComponent(typeof(Health))]
    class RoseController : MonoBehaviour
    {
        private const string UseButton = "Use";

        #region Pickup Readius
        public float pickupRadius = 4.0f;
        public LayerMask flowers;

        #endregion

        private bool _isUsePressed;
        private Health _health;


        private void Awake()
        {
            _health = GetComponent<Health>();
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
        }

        private void PickUpFlower(Flower flower)
        {
            flower.PlaySound();
            Destroy(flower.gameObject);
            _health.HealthPoints += 10.0f;
            // ...
        }
    }
}
