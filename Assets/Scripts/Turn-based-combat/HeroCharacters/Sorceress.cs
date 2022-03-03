using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;
using HeartOfWinter.Moves.Sorcceress;

namespace HeartOfWinter.Characters.HeroCharacters
{
    public class Sorceress : HeroCharacter
    {
        public Sorceress()
        {
            maxHealth = 30;
            initiative = 6;
            heroName = Hero.Sorceress;

            knownMoves = new List<Move>()
            {
                new IceLance(this),
                new ColdSnap(this),
                new Blizzard(this) 
            };
        }
    }
}