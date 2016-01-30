using UnityEngine;

namespace Assets.Scripts
{
    public class FlowerSpawner : MonoBehaviour
    {
        #region Editor Variables
        public Transform leftLimit;
        public Transform rightLimit;
        public float fromHeight = 200.0f;

        public GameObject[] flowers;
        public float flowerStepMin = 3.0f;
        public float flowerStepMax = 10.0f;
        public LayerMask ground;
        #endregion

        private void Start()
        {
            GenerateFlowers();
        }


        public void GenerateFlowers()
        {
            float x = leftLimit.position.x;

            while(x < rightLimit.position.x)
            {
                x += Random.Range(flowerStepMin, flowerStepMax);
                Vector2? pos = GetFlowerPos(x);

                if(pos.HasValue)
                {
                    GameObject randomFlower = flowers[Random.Range(0, flowers.Length)];
                    Instantiate(randomFlower, pos.Value, Quaternion.identity);
                }
            }

        }

        private Vector2? GetFlowerPos(float x)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, fromHeight), -Vector2.up, float.MaxValue, ground);
            Debug.DrawRay(new Vector2(x, fromHeight), -Vector2.up, Color.red, 1000);
            if (hit.collider != null)
            {
                return hit.point;
            }

            return null;
        }
    }
}
