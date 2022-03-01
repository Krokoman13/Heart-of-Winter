using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;
using HeartOfWinter.Moves.Wendigo;

namespace HeartOfWinter.Characters.MonsterCharacters
{
    public class Wendigo : MonsterCharacter
    {
        public Wendigo()
        {
            maxHealth = 20;
            initiative = 5;

            knownMoves = new List<Move>()
            {
                new EternalHunger(this, 5), 
		        new StandardAttackMove(this, 10, "Rending Claws", 1)
            };
        }
    }
}
