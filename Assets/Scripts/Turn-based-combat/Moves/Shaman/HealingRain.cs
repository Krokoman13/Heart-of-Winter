using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;
using HeartOfWinter.Characters.MonsterCharacters;

namespace HeartOfWinter.Moves.Shaman
{
    public class HealingRain : StandardHealMove
    {
        public HealingRain(Character caster) : base(caster, 2, 5, "Healing Rain", 3, 3)
        {
        }

        public override string description
        {
            get
            {
                return "Heals all allies for " + _power + "-" + _maxPower + " hit points";
            }
        }
    }
}
