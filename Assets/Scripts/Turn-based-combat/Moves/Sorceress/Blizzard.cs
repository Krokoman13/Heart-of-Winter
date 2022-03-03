using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Sorcceress
{
    public class Blizzard : StandardAttackMove
    {
        public Blizzard(Character caster) : base(caster, 5f, 7f, "Blizzard", 3)
        {
            setCooldown(3);
        }
    }
}
