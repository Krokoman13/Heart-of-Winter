using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;
using HeartOfWinter.Moves.Priest;

namespace HeartOfWinter.Characters.HeroCharacters
{
    public class ShadowWeaver : HeroCharacter
    {
        public ShadowWeaver()
        {
            maxHealth = 35;
            initiative = 10;
            heroName = Hero.ShadowWeaver;

            knownMoves = new List<Move>()
            {
                new Feast(this, 4f),
                new PrepareTheMeat(this, 0.5f),
                new VisionOfTheAbyss(this, 2f)
            };
        }
    }
}