using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;

namespace HeartOfWinter.Characters.MonsterCharacters
{
    public class Wendigo : MonsterCharacter
    {
        public Wendigo()
        {
            maxHealth = 20;
            initiative = 9;

            knownMoves = new List<Move>()
            {
                new StandardHealMove(this, 7, "EternalHunger", 1, 1),
		//Eternal hunger should only heal himself, and also damage an enemy at the same time (vampirism lifesteal, all of that) but i can't use small brain to figure out.//
                
		new StandardAttackMove(this, 10, "Rending Claws", 1)
            };
        }
    }
}
