using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Priest
{
    public class WarmingGlow : StandardHealMove
    {
        public WarmingGlow(Character caster, float power) : base(caster, power, "Warming Glow", 1)
        {
        }
    }
}
