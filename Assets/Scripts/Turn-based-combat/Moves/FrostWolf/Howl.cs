using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.FrostWolf
{
    public class Howl : Move
    {
        public Howl(Character caster, float power) : base(caster, power, "Howl")
        {
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
