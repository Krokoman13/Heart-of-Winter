using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Priest
{
    public class NewDawn : Move
    {
        public NewDawn(Character caster) : base(caster, 1.5f, 1.6f, "New Dawn")
        {
            setCooldown(4);

            _amountOfTargets = 1;
            targetsPCs = true;
        }

        public override string description
        {
            get
            {
                return "Target ally deals " + (_power - 1f)*100f + "extra damge for 2 rounds";
            }
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
