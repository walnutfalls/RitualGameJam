using UnityEngine;


namespace Assets.Scripts
{
    public class AttackCone : MonoBehaviour
    {
        public float damage = 4;
        public ParticleSystem particleSystem;

        private float _timeOfLastDamage = 0;
        private float _cooldown = 0.4f;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 8)
            {
                if (Time.time - _timeOfLastDamage > _cooldown)
                {
                    other.gameObject.GetComponent<Health>().HealthPoints -= damage;
                    _timeOfLastDamage = Time.time;
                }
            }
        }

        public void OnEnable()
        {
            particleSystem.Play(true);  
        }

        public void OnDisable()
        {
            particleSystem.Stop(true);
        }
    }
}
