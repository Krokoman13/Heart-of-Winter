using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;

namespace HeartOfWinter.Characters.HeroCharacters
{
    public enum Hero { Sorceress = 0, Priest = 1, ShadowWeaver = 2, Other = 3, NULL = default };

    public class HeroCharacter : Character
    {
        bool isMine = false;

        public Hero heroName;

        public void setMine()
        {
            isMine = true;
        }

        protected override Transform findParent()
        {
            return GameObject.FindGameObjectWithTag("HeroParent").transform;
        }
    }
}
