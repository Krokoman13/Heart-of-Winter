using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Priest
{
    public class PrepareTheMeat : Move
    {
        public PrepareTheMeat(Character caster, float power) : base(caster, power, "Prepare the Meat")
        {
            setCooldown(1);

            _amountOfTargets = 1;
            targetsNPCs = true;
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.damageModifier = power;
                target.damageModifierDuration = 1;
            }
        }
    }
}
