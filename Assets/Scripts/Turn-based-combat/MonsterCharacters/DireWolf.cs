using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;

namespace HeartOfWinter.Characters.MonsterCharacters
{
    public class DireWolf : MonsterCharacter
    {
        public DireWolf()
        {
            maxHealth = 25;
            initiative = 7;

            knownMoves = new List<Move>()
            {
                new StandardAttackMove(this, 6, "PrimalSavagery"),
                new StandardAttackMove(this, 4, "Bloodlust", 3)
            };
        }
    }
}
