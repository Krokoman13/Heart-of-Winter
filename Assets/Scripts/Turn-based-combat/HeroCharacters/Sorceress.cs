using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;

namespace HeartOfWinter.Characters.HeroCharacters
{
    public class Sorceress : HeroCharacter
    {
        public Sorceress()
        {
            heroName = Hero.Sorceress;
            knownMoves = new List<Move>() 
            {   
                new StandardAttackMove(this, 1, "PaperSwordAttack"), 
                new StandardAttackMove(this, 2, "RockSwordAttack", 2), 
                new StandardAttackMove(this, 5, "ScissorsSwordAttack") 
            };
        }
    }
}