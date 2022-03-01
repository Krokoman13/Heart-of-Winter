using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;

namespace HeartOfWinter.Characters.MonsterCharacters
{
    public class Draugr : MonsterCharacter
    {
        public Draugr()
        {
            maxHealth = 35;
            initiative = 3;

            knownMoves = new List<Move>()
            {
                new StandardHealMove(this, 6, "RenewedVigor", 3, 1),
                new StandardAttackMove(this, 6, "FrozenArmaments")
            };
        }
    }
}
