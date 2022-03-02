using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Priest
{
    public class HolyLight : StandardAttackMove
    {
        public HolyLight(Character caster) : base(caster, 1, 3, "Holy Light", 1)
        {
            setCooldown(2);
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.ModifyHealth(-fullPower * caster.damageModifier);
                target.SetStunned();
            }
        }
    }
}
