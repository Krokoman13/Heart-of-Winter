using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Sorcceress
{
    public class IceLance : StandardAttackMove
    {
        public IceLance(Character caster, float power) : base(caster, power, "IceLance", 1)
        {
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.ModifyHealth(-power);
                target.initiative -= 2;

                if (target.initiative < 2) target.initiative = 2;
            }
        }
    }
}
