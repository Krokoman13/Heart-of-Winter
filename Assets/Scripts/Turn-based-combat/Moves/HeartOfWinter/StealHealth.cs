using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Heart
{
    public class StealHealth : Move
    {
        public StealHealth(Character caster) : base(caster, 2f, "StealHealth")
        {
            _amountOfTargets = 3;
            targetsPCs = true;
            setCooldown(2);
        }

        protected override void execute()
        {
            float amount = 0;

            foreach (Character target in targets)
            {
                amount += -target.ModifyHealth(-fullPower * caster.damageModifier);
            }

            caster.ModifyHealth(amount);
        }

        protected override void step()
        {
            ready = true;
        }
    }
}
