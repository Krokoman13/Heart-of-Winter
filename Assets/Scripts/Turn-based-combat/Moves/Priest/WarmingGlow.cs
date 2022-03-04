using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Priest
{
    public class WarmingGlow : StandardHealMove
    {
        public WarmingGlow(Character caster) : base(caster, 5f, 8f, "Warming Glow", 1)
        {
        }

        public override string description
        {
            get
            {
                return "Heals target ally for " + _power + "-" + _maxPower + " hit points";
            }
        }
    }
}
