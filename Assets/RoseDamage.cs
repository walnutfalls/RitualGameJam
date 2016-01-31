using Assets.Scripts.AI;
using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class RoseDamage : MonoBehaviour
    {
        public float damage = 50.0f;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 9)
            {
                other.gameObject.GetComponent<Health>().HealthPoints -= damage;
                other.transform.parent.gameObject.GetComponent<Rigidbody2D>().AddForce((other.transform.position - transform.position) * 200.0f);
                other.transform.parent.GetComponent<DenialMonster>().State = DenialMonster.DenialMonsterState.Disabled;
                gameObject.SetActive(false);
            }
        }
    }
}
