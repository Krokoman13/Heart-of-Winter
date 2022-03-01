using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;
using HeartOfWinter.Moves.FrostWolf;

namespace HeartOfWinter.Characters.MonsterCharacters
{
    public class FrostWolf : MonsterCharacter
    {
        public FrostWolf()
        {
            maxHealth = 20;
            initiative = 4;

            knownMoves = new List<Move>()
            {
                new Howl(this, 1.3f),
                new StandardAttackMove(this, 5, "FrostBite", 1)
            };
        }
    }
}
