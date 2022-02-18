using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Priest
{
    public class HolyLight : StandardAttackMove
    {
        public HolyLight(Character caster, float power) : base(caster, power, "Holy Light", 1)
        {
            SetCooldown(2);
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.ModifyHealth(-power * caster.damageModifier);
                target.GetStunned();
            }
        }
    }
}
