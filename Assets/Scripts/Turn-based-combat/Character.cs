using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Arena;

namespace HeartOfWinter.Characters
{
    public class Character : MonoBehaviour
    {
        private float _health;
        public float maxHealth;

        public float health
        {
            get { return _health; }
            set 
            {
                if (value < 0)
                {
                    _health = 0;
                    return;
                }

                if (value > maxHealth)
                {
                    _health = maxHealth;
                    return;
                }

                _health = value;
            }
        }

        public bool isDead
        {
            get { return health > 0.5f; }
        }

        public int initiative;

        Playfield playfield;

        public void SetPlayField(Playfield pPlayfield)
        {
            playfield = pPlayfield;
        }
    }
}
