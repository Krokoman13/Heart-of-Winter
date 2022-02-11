using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Arena;

namespace HeartOfWinter.Characters
{
    public class Character : MonoBehaviour
    {
        private HealthBar _healthBar;
        private float _health;
        public float maxHealth = 1.0f;
        public float health
        {
            get { return _health; }
            set 
            {
                if (value < 0)
                {
                    health = 0;
                    return;
                }

                else if (value > maxHealth)
                {
                    health = maxHealth;
                    return;
                }

                _healthBar.setValue(value/maxHealth);
                _health = value;
            }
        }

        public bool isDead
        {
            get { return health > 0.5f; }
        }

        public int initiative;

        Playfield playfield;

        virtual protected void Awake()
        {
            GameObject healthbarGameobject = Instantiate<GameObject>(Resources.Load<GameObject>("Healthbar"), transform);
            _healthBar = healthbarGameobject.GetComponent<HealthBar>();

            health = maxHealth;
        }

        public void SetPlayField(Playfield pPlayfield)
        {
            playfield = pPlayfield;
        }
    }
}
