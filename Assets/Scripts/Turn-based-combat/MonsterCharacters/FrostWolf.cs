using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;

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
                //new StandardBuffMove(this, 30, "Howl", 1)//
                new StandardAttackMove(this, 5, "FrostBite", 1)
            };
        }
    }
}
