using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;
using HeartOfWinter.Moves.ShadowWeaver;

namespace HeartOfWinter.Characters.HeroCharacters
{
    public class ShadowWeaver : HeroCharacter
    {
        public ShadowWeaver()
        {
            maxHealth = 35;
            initiative = 8;
            heroName = Hero.ShadowWeaver;

            knownMoves = new List<Move>()
            {
                new Feast(this),
                new PrepareTheMeat(this),
                new VisionOfTheAbyss(this)
            };
        }
    }
}