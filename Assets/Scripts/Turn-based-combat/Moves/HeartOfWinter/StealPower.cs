using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Heart
{
    public class StealPower : Move
    {
        public StealPower(Character caster) : base(caster, 1f, "DrainWarmth")
        {
            _amountOfTargets = 1;
            setCooldown(2);
            targetsPCs = true;
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.damageModifier = fullPower/2f;
                target.damageModifierDuration = 1;
                target.ModifyHealth(-0.1f);
            }

            caster.damageModifier = fullPower * 2f;
            caster.damageModifierDuration = 1;
            caster.ModifyHealth(+0.1f);
        }

        protected override void step()
        {
            ready = true;
        }
    }
}
