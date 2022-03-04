using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;
using HeartOfWinter.Moves.Shaman;

namespace HeartOfWinter.Characters.HeroCharacters
{
    public class Shaman : HeroCharacter
    {
        public Shaman()
        {
            maxHealth = 35;
            initiative = 10;
            heroName = Hero.Shaman;

            knownMoves = new List<Move>()
            {
                new LightningStrike(this),
                new StormShield(this),
                new HealingRain(this)
            };
        }
    }
}