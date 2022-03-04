using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;
using HeartOfWinter.Characters.MonsterCharacters;

namespace HeartOfWinter.Moves.Shaman
{
    public class LightningStrike : StandardAttackMove
    {
        public LightningStrike(Character caster) : base(caster, 6, 11, "Lightning Strike")
        {
        }

        public override string description
        {
            get
            {
                return "Deals " + _power + "-" + _maxPower + " damage to one enemy";
            }
        }
    }
}
