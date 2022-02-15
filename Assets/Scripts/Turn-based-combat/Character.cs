using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Arena;
using HeartOfWinter.Moves;

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
            protected set 
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

        SpriteRenderer outlineRenderer;
        GameObject outlineChild;

        public int initiative;

        protected Playfield playfield;

        public List<Move> knownMoves;
        public Move currentMove = null;

        public bool outLine
        {
            set 
            {
                outlineChild.SetActive(value);
                if (value) outlineRenderer.color = new Color(119, 251, 255, 255)/255.0f;
            }

            get { return outlineChild.activeSelf; }
        }

        bool _selected;

        public bool selected
        {
            get 
            {
                return _selected;
            }

            set 
            {
                _selected = value;
                if (value) outlineRenderer.color = new Color(255, 246, 97, 255) / 255.0f;
                else outlineRenderer.color = new Color(119, 251, 255, 255) / 255.0f;
            }
        }

        virtual protected void Awake()
        {
            GameObject healthbarGameobject = Instantiate<GameObject>(Resources.Load<GameObject>("Healthbar"), transform);
            _healthBar = healthbarGameobject.GetComponent<HealthBar>();

            health = maxHealth;

            gameObject.AddComponent<BoxCollider2D>();

            outlineChild = new GameObject();
            outlineChild.transform.SetParent(transform);
            outlineChild.transform.localPosition = new Vector3();
            outlineRenderer = outlineChild.AddComponent<SpriteRenderer>();
            outlineRenderer.sprite = Resources.Load<Sprite>("Outline");
            outlineChild.SetActive(false);
        }

        public float RemoveHealth(float amount)
        {
            health = health - amount;
            return amount;
        }

        public void SetPlayField(Playfield pPlayfield)
        {
            playfield = pPlayfield;
        }
    }
}
