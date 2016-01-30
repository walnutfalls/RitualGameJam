using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Extensions;
using UnityEngine.UI;


namespace Assets.Scripts
{     
    public class HealthBarDisplay : MonoBehaviour
    {
        public enum HealthbarLocation
        {
            Top,
            Left,
            Bottom,
            Right
        }

        public enum DecreaseDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        public DecreaseDirection decreaseDirection = DecreaseDirection.Left;

        public float minHealthBarDimension = 0.01f;
        public float healthbarMargin = 0.01f;

      
        private GameObject _healthBarGreen;
        public Health health;

        public void Awake()
        {
            health.HealthChangedListeners += UpdateHealthDisplay;
            _healthBarGreen = transform.Find("HealthBackground/HealthBarRed/HealthBarGreen").gameObject;
        }

        public void Update()
        {

        }


        public void UpdateHealthDisplay(float hp, float hpMax)
        {
            RectTransform hbTrans = _healthBarGreen.GetComponent<RectTransform>();
            float hbW = hbTrans.GetWidth();
            float hbH = hbTrans.GetHeight();

            float displacement = 0.0f;

            switch (decreaseDirection)
            {
                case DecreaseDirection.Up:
                    displacement = hbH - (hp / hpMax) * hbH;
                    hbTrans.localPosition = new Vector3(0, -displacement, 0);
                    break;
                case DecreaseDirection.Right:
                    displacement = hbW * (1.0f - (hp / hpMax));
                    hbTrans.localPosition = new Vector3(displacement, 0, 0);
                    break;
                case DecreaseDirection.Down:
                    displacement = hbH - (hp / hpMax) * hbH;
                    hbTrans.localPosition = new Vector3(0, displacement, 0);
                    break;
                case DecreaseDirection.Left:
                    displacement = hbW * (1.0f - (hp / hpMax));
                    hbTrans.localPosition = new Vector3(-displacement, 0, 0);
                    break;
            }
        }
    }
}