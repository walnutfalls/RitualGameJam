using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public delegate void OnKillHandler();
public delegate void OnHealthChanged(float hp, float hpMax);

public class Health : MonoBehaviour
{        
    public event OnKillHandler EntityKilledListeners;
    public event OnHealthChanged HealthChangedListeners;

    public float initialHp = 100.0f;
    public float maxHp = 100.0f;


    private float _healthPoints;
    public float HealthPoints
    {
        get
        {
            return _healthPoints;
        }
        set
        {
            if (_healthPoints <= 0 && value <= 0)
                return;

            if (_healthPoints > 0 && value <= 0 && EntityKilledListeners != null)
            {
                _healthPoints = value;
                EntityKilledListeners();
                return;
            }

            _healthPoints = value;
          
            if (HealthChangedListeners != null)
                HealthChangedListeners(_healthPoints, maxHp);                
        }
    }

    void Start()
    {
        HealthPoints = initialHp;                
    }   
}
