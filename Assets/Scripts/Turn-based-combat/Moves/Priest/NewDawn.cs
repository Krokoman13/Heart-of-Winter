using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Priest
{
    public class NewDawn : Move
    {
        public NewDawn(Character caster, float power) : base(caster, power, "New Dawn")
        {
            setCooldown(4);

            _amountOfTargets = 1;
            targetsPCs = true;
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.damageModifier = power;
                target.damageModifierDuration = 2;
            }
        }
    }
}
