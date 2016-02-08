using Assets.Scripts.AI;
using UnityEngine;

namespace Assets
{
    public class RoseDamage : MonoBehaviour
    {
        public float damage = 50.0f;
        public float radius = 10.0f;
        public LayerMask enemies;
        public ParticleSystem effect;
      
        public void Attack()
        {
            effect.Emit(20);

            var targets = Physics2D.OverlapCircleAll(transform.position, radius, enemies);

            Debug.DrawLine(transform.position, radius * Vector2.right + (Vector2)transform.position, Color.red, 3.0f);
            Debug.DrawLine(transform.position, radius * Vector2.up + (Vector2)transform.position, Color.red, 3.0f);
            Debug.DrawLine(transform.position, radius * Vector2.left + (Vector2)transform.position, Color.red, 3.0f);
            Debug.DrawLine(transform.position, radius * Vector2.down + (Vector2)transform.position, Color.red, 3.0f);

            foreach (var t in targets)
            {
                t.GetComponent<Health>().HealthPoints -= damage;
                t.transform.parent.GetComponent<DenialMonster>().State = DenialMonster.DenialMonsterState.Disabled;
            }
        }       
    }
}
