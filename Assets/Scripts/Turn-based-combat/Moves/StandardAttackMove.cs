using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves
{
    public class StandardAttackMove : Move
    {
        public StandardAttackMove(Character caster, float power, string iconName, int amountOfTargets = 1) : base(caster, power, iconName)
        {
            _amountOfTargets = amountOfTargets;
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.RemoveHealth(power);
            }
        }
    }
}
