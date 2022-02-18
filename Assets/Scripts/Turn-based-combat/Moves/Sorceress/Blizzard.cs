using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Sorcceress
{
    public class Blizzard : StandardAttackMove
    {
        public Blizzard(Character caster, float power) : base(caster, power, "Blizzard", 3)
        {
            SetCooldown(3);
        }
    }
}
