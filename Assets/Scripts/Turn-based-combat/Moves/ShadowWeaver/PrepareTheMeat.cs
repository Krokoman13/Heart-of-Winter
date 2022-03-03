using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.ShadowWeaver
{
    public class PrepareTheMeat : Move
    {
        public PrepareTheMeat(Character caster) : base(caster, 0.5f, 0.4f, "Prepare the Meat")
        {
            setCooldown(1);

            _amountOfTargets = 1;
            targetsNPCs = true;
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.damageModifier = fullPower;
                target.damageModifierDuration = 2;
            }
        }
    }
}
