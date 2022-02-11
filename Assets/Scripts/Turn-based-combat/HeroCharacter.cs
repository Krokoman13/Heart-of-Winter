using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeartOfWinter.Characters.HeroCharacters
{
    public enum Hero { Sorceress = 0, Priest = 1, ShadowWeaver = 2, Other = 3, NULL = default };

    public class HeroCharacter : Character
    {
        bool isMine = false;

        MovesSelector movesSelector;

        public Hero heroName;

        public void SetMovesSelector(MovesSelector pMovesSelector)
        {
            movesSelector = pMovesSelector;
        }

        public void setMine()
        {
            isMine = true;
        }

        protected void Start()
        {
            if (isMine) gameObject.AddComponent<BoxCollider2D>();
        }
    }
}
