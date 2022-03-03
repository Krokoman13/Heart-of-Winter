using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;
using HeartOfWinter.Moves.Priest;

namespace HeartOfWinter.Characters.HeroCharacters
{
    public class Priest : HeroCharacter
    {
        public Priest()
        {
            maxHealth = 25;
            initiative = 7;
            heroName = Hero.Priest;

            knownMoves = new List<Move>()
            {
                new WarmingGlow(this),
                new HolyLight(this),
                new NewDawn(this)
            };
        }
    }
}