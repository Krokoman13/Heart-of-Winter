using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Priest
{
    public class WarmingGlow : StandardHealMove
    {
        public WarmingGlow(Character caster) : base(caster, 5f, 6f, "Warming Glow", 1)
        {
        }
    }
}
